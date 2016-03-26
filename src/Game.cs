using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameDifficulty : int
{
	Easy,
	Medium,
	Hard,
	TimeAttack
}

public class Game : MonoBehaviour
{
	public enum ZoomState : int
	{
		ZoomedIn = 0,
		ZoomedOut,
		Zooming
	}
	
	public enum State : int
	{
		Counting = 0,
		Playing,
		Finished,
		FinishedIdle,
        WinFinish
    }

    private float _scareLevel = 0f;
    private List<Npc> _npcs;
    
    // Set from editor
    public GameObject _furnisParent;
    public Sprite[] _floorTextures;
    public Node[] _npcPath;
    public GameObject _exitWaypoint;
    
    public GameObject _alicePrefab;
    public GameObject _rupertPrefab;
    public GameObject _mindyPrefab;
    public GameObject _chadPrefab;
    
    
    public GameObject[] _furniturePrefabs;
    public float _timer = 60f;
    public float _scareDownSpeed = 3f;
    
	public Camera _mainCamera;
	public UnityEngine.UI.Image _scareSlider;
	public UnityEngine.UI.Text _timerLabel;
	public UnityEngine.UI.Text _pointsLabel;
	public UnityEngine.UI.Text _counterLabel;
	public UnityEngine.UI.Button _backButton;
	public GameDifficulty _difficulty;
	public GameObject _pauseWindow;
	public GameObject _endWindow;
	
	private string _room;
    private float _maxPoints;
    private Actuator[] _actuators;

    public AudioClip _music;
    public AudioClip _loopMusic;
	
	private ZoomState _zoomed = ZoomState.ZoomedOut;
	private ZoomState _previousZoom = ZoomState.ZoomedOut;
	private bool _paused = false;
	
	private State _state = State.Counting;
	private float _startTimer = 0f;
	private bool _win = false;
	private int _timeAttackScore = 0;
    
    void Awake()
    {
        _npcs = new List<Npc>();
        _actuators = _furnisParent.GetComponentsInChildren<Actuator>();
    }
    
    void Start()
    {
        // Spawn the three NPC characters
        SpawnNpc(_alicePrefab);
        SpawnNpc(_rupertPrefab);
        SpawnNpc(_mindyPrefab);
        SpawnNpc(_chadPrefab);
		
		string d = SettingsController.instance.GetDifficulty();
		
		if (d.Contains("Easy"))
		{
			_difficulty = GameDifficulty.Easy;
			_maxPoints = 100;
			_timer = 40f;
		}
		else if (d.Contains("Medium"))
		{
			_difficulty = GameDifficulty.Medium;
			_maxPoints = 200;
			_timer = 80f;
		}
		else if (d.Contains("Hard"))
		{
			_difficulty = GameDifficulty.Hard;
			_maxPoints = 300;
			_timer = 120f;
		}
		else
		{
			_difficulty = GameDifficulty.TimeAttack;
			_timer = 60f;
			_maxPoints = 900;
		}
		
		_timerLabel.text = ((int)_timer).ToString();
		_scareSlider.fillAmount = 0f;
		
		if (_difficulty == GameDifficulty.TimeAttack)
		{
			GetComponent<AudioSource>().clip = _loopMusic;
			_pointsLabel.text = "0";
		}
		else
		{
			GetComponent<AudioSource>().clip = _music;
			_pointsLabel.text = "0 / " + _maxPoints.ToString();
		}
		
		if (SettingsController.instance.PlayMusic())
		{
			GetComponent<AudioSource>().Play();
		}
    }
    
    private void SpawnNpc(GameObject prefab)
    {
        // Pick a random node
        int ind = Random.Range(0, _npcPath.Length);
        Vector2 spawnPoint = _npcPath[ind].transform.position;
        
        // Spawn the NPC
        GameObject npc = GameObject.Instantiate(prefab, new Vector3(spawnPoint.x, spawnPoint.y, 1f), Quaternion.identity) as GameObject;
        npc.GetComponent<Npc>().SetCurrentNode(_npcPath[ind]);
        npc.GetComponent<Npc>().SetGame(this);
		npc.GetComponent<Npc>().SetPause(true);
        _npcs.Add(npc.GetComponent<Npc>());
    }
    
    void Update()
    {
		if (_state == State.Counting)
		{
			_startTimer += Time.deltaTime;
			Vector3 clScale = _counterLabel.gameObject.transform.localScale;
			
			clScale.x -= Time.deltaTime;
			clScale.y -= Time.deltaTime;
			
			if (_startTimer > 0.9f && _startTimer < 1.1f)
			{
				clScale.x = 1f;
				clScale.y = 1f;
				_counterLabel.text = "2";
			}
			
			if (_startTimer > 1.9f && _startTimer < 2.1f)
			{
				clScale.x = 1f;
				clScale.y = 1f;
				_counterLabel.text = "1";
			}
			
			if (_startTimer > 2.9f && _startTimer < 3.1f)
			{
				clScale.x = 1f;
				clScale.y = 1f;
				_counterLabel.text = "Spook!";
			}
			
			if (_startTimer > 4f)
			{
				for (int i = 0; i < _npcs.Count; i++)
				{
					_npcs[i].GetComponent<Npc>().SetPause(_paused);
				}
				
				_counterLabel.gameObject.SetActive(false);
				_state = State.Playing;
			}
			
			_counterLabel.gameObject.transform.localScale = clScale;
		}
		else if (_state == State.Playing)
		{
			if (!_paused)
			{
				// Check if the player has touched an actuator
				if (Input.GetMouseButtonDown(0))
				{
					// Throw the ray
					Vector2 origin = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
					Vector2 direction = new Vector2(0.1f, 0.1f);
					RaycastHit2D[] hit = Physics2D.RaycastAll(origin, direction, 0.1f);
					
					if (_zoomed == ZoomState.ZoomedIn)
					{   
						// Get the ray origin
						if (hit != null)
						{
							for (int i = 0; i < hit.Length; i++)
							{
								GameObject go = hit[i].transform.gameObject;
								Actuator act = go.GetComponent<Actuator>();

								// If it hits on an actuator, make it react
								if (act && act.GetRoom() == _room)
								{
									act.OnAction();
								}
							}
						}
					}
					else if (_zoomed == ZoomState.ZoomedOut)
					{
						for (int i = 0; i < hit.Length; i++)
						{
							GameObject go = hit[i].transform.gameObject;

							// If it hits on an actuator, make it react
							if (go && go.gameObject.transform.parent)
							{
								if (go.gameObject.transform.parent.name.Contains("Habitaciones"))
								{
									ZoomIn(go.name,go.transform.position);
									break;
								}
							}
						}
					}
				}
				
				_timer -= Time.deltaTime;
				
				// Update scare UI bar and timer
				// @To-Do
				_scareSlider.fillAmount = _scareLevel / _maxPoints;
				_timerLabel.text = ((int)_timer).ToString();
				
				// Check if the player has lost
				if (_timer < 0 || (_difficulty != GameDifficulty.TimeAttack && _scareLevel > _maxPoints))
				{
					if (_difficulty == GameDifficulty.TimeAttack)
					{
						_timeAttackScore = (int)_scareLevel;
					}
					else
					{
						if (_scareLevel < _maxPoints)
						{
							_win = false;
						}
						else
						{
							_win = true;
						}
					}
					
					if(_win)
					{
						_state = State.WinFinish;
                        Node exitNode = _exitWaypoint.GetComponent<Node>();
                        foreach (Npc n in _npcs)
                        {
                            n.SetDesiredNode(exitNode);
                            n.SetAnimState(Npc.AnimState.Frightened);
                        }
                    }
					else
					{
						_state = State.Finished;
					}
				}
			}
		}
		else if (_state == State.Finished)
		{
			TogglePause(true);
			_endWindow.SetActive(true);
			int ownBest = SettingsController.instance.GetOwnBestScore();
			
			if (_timeAttackScore > ownBest)
			{
				SettingsController.instance.SetOwnBestScore(_timeAttackScore);
			}
			
			if (_difficulty != GameDifficulty.TimeAttack)
			{
				GameObject.Find("EndMenu/Loading").SetActive(false);
				
				if (!_win)
				{
					GameObject.Find("EndMenu/Header").GetComponent<UnityEngine.UI.Text>().text = "You lose...";
				}
				else
				{
					GameObject.Find("EndMenu/Header").GetComponent<UnityEngine.UI.Text>().text = "You win!";
				}
			}
			else
			{
				GameObject.Find("EndMenu/Points").GetComponent<UnityEngine.UI.Text>().text = 
					"Score: " + _timeAttackScore + " points (your best: " + 
					SettingsController.instance.GetOwnBestScore() + " points)";
				
				GameObject.Find("EndMenu/Header").GetComponent<UnityEngine.UI.Text>().text = "Time's up!";
			}
			
			_state = State.FinishedIdle;
		}
		else if (_state == State.WinFinish)
		{
            bool end = true;
            Node exitNode = _exitWaypoint.GetComponent<Node>();
            foreach (Npc n in _npcs)
            {
                bool is_stoped = n.GetLogicState() == Npc.LogicState.Stand;
                if(is_stoped)
                {
                    n.SetPause(true);
                }
                end &= is_stoped;
            }
            if(end)
            {
                _state = State.Finished;
            }
        }
    }
    
	public void OnPause()
	{
		TogglePause(!_paused);
	}
	
	public void OnRestart()
	{
		SceneManager.LoadScene("scene");
	}
	
	public void OnPauseBack()
	{
		TogglePause(!_paused);
	}
	
	private void TogglePause(bool p)
	{
		_paused = p;
			
		for (int i = 0; i < _npcs.Count; i++)
		{
			_npcs[i].GetComponent<Npc>().SetPause(_paused);
		}
		
		_pauseWindow.SetActive(_paused);
	}
	
	public void OnMenu()
	{
		SceneManager.LoadScene("menu");
	}
	
    public void SetScareLevel(float sl)
    {
        _scareLevel = sl;
    }
    
    public void AddScare(float sl)
    {
        _scareLevel += sl;
        
        if (_difficulty != GameDifficulty.TimeAttack)
        {
			_pointsLabel.text = _scareLevel.ToString() + " / " + _maxPoints.ToString();
		}
		else
		{
			_pointsLabel.text = _scareLevel.ToString();
			
			if (sl > 0)
			{
				_timer += sl*0.3f;
			}
			else
			{
				_timer += sl;
			}
		}
    }
    
    public float GetScareLevel()
    {
        return _scareLevel;
    }
    
    public void ZoomIn(string room, Vector3 pos)
    {
		pos.z = -1;
		
		// Camera zooming
        if (room.Contains("Habitacion"))
        {
            //10
            _mainCamera.gameObject.GetComponent<Camera2DTweener>().TweenTo(pos, 10f, 0.5f, this.gameObject);
        }
        else if (room.Contains("Entrada") || room.Contains("Baño"))
        {
            //5.5
            _mainCamera.gameObject.GetComponent<Camera2DTweener>().TweenTo(pos, 5.5f, 0.5f, this.gameObject);
        }
        else if (room.Contains("Pasillo"))
        {
            //17.3
            _mainCamera.gameObject.GetComponent<Camera2DTweener>().TweenTo(pos, 17.3f, 0.5f, this.gameObject);
        }
        else if (room.Contains("Salon"))
        {
            //15
            _mainCamera.gameObject.GetComponent<Camera2DTweener>().TweenTo(pos, 15f, 0.5f, this.gameObject);
        }
		
		// Room shadow tweening
		GameObject shadow = GameObject.Find("Sombras/Sombra_" + room) as GameObject;
		
		if (shadow)
		{
			ShadowController sc = shadow.GetComponent<ShadowController>();
			sc.ShadowIn();
		}
		
        _zoomed = ZoomState.Zooming;
		_room = room;
    }
    
	public void ZoomOut()
    {
		_mainCamera.gameObject.GetComponent<Camera2DTweener>().TweenTo(new Vector3(6f,-2f,-2.3f), 31f, 0.5f, this.gameObject);
		
		// Room shadow tweening
		GameObject shadow = GameObject.Find("Sombras/Sombra_" + _room) as GameObject;
		
		if (shadow)
		{
			ShadowController sc = shadow.GetComponent<ShadowController>();
			sc.ShadowOut();
		}
		
		_zoomed = ZoomState.Zooming; 
    }
	
	public void OnCameraTweenFinished()
	{
		// Changed to zoomed out
		if (_previousZoom == ZoomState.ZoomedIn)
		{
			_previousZoom = ZoomState.ZoomedOut;
			_zoomed = ZoomState.ZoomedOut;
			_backButton.gameObject.SetActive(false);
		}
		// Changed to zoomed in
		else if (_previousZoom == ZoomState.ZoomedOut)
		{
			_previousZoom = ZoomState.ZoomedIn;
			_zoomed = ZoomState.ZoomedIn;
			_backButton.gameObject.SetActive(true);
		}
	}

    public void Tremble()
    {
        foreach (Actuator act in _actuators)
        {
            if(act.GetRoom() == _room)
            {
                act.OnTremble();
            }
        }
    }

    public void Blow()
    {
        foreach (Actuator act in _actuators)
        {
            if (act.GetRoom() == _room)
            {
                act.OnBlow();
            }
        }
    }


    public List<Npc> GetNpcs()
    {
        return _npcs;
    }
    
    public Node[] GetNodes()
    {
        return _npcPath;
    }
}
