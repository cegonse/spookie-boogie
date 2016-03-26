using UnityEngine;
using System.Collections.Generic;

public class Npc : MonoBehaviour
{
	
	public enum AnimState : int
	{
		Normal,
		Frightened
	}
	
	public enum LogicState : int
	{
		Stand,
		Walking
	}   
	
	private Node _currentTarget;
	private Node _lastTarget;
	private Node _desiredTarget;
	
	public string _currentRoom;
	private AnimState _animState = AnimState.Normal;
	private LogicState _logicState = LogicState.Stand;
	public Furniture[] Fear;
	public float[] FearDamage;
	public Furniture[] Courage;
	public float[] CourageDamage;
	public float _backProbability;
	public AudioClip _sound;
	private int _pointWalkCount;
	private float _waitTimer = 0f;
	
	private float _speed = 5f;
	private Game _game;
	public bool _paused = false;
	private SpriteAnimatorNpc snpc;
	
	public void Awake()
	{
		snpc = GetComponent<SpriteAnimatorNpc>();
	}
	
	public void SetGame(Game g)
	{
		_game = g;
		MoveToRandomPoint();
	}
	
	public void SetCurrentNode(Node d)
	{
		_currentTarget = d;
	}
	
	public void Update()
	{
		if (!_paused)
		{
			DoToggleDoor();
			
			Vector3 dir = _currentTarget.transform.position - transform.position;
			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			
			if (angle >= 175 && angle <= 185 || angle >= -185 && angle <= -175)
			{
				snpc.SetState(SpriteAnimatorNpc.State.WalkSide);
				GetComponent<SpriteRenderer>().flipX = false;
			}
			else if (angle >= 70 && angle <= 130)
			{
				snpc.SetState(SpriteAnimatorNpc.State.WalkUp);
			}
			else if (angle >= -5 && angle <= 5)
			{
				snpc.SetState(SpriteAnimatorNpc.State.WalkSide);
				GetComponent<SpriteRenderer>().flipX = true;
			}
			else if (angle >= -95 && angle <= -85)
			{
				snpc.SetState(SpriteAnimatorNpc.State.WalkDown);
			}

			switch (_logicState)
			{
				case LogicState.Walking : 
					Vector2 my_pos = new Vector2(transform.position.x, transform.position.y);
					Vector2 my_target_pos = new Vector2(_currentTarget.transform.position.x, _currentTarget.transform.position.y);
					
					if(Vector2.Distance(my_pos, my_target_pos) < 0.01f)
					{
						MoveToNextPoint();
					}
					else
					{
						Vector2 aux = Vector2.MoveTowards(my_pos, my_target_pos, Time.deltaTime * _speed);
						transform.position = new Vector3(aux.x, aux.y, 0);
					}
					
					break;
				
				case LogicState.Stand:
					_waitTimer -= Time.deltaTime;
					
					if(_waitTimer < 0)
					{
						if (_animState == AnimState.Frightened)
						{
							SetAnimState(AnimState.Normal);
						}
						
						_waitTimer = Random.Range(3f, 5f);
						MoveToRandomPoint();
					}
					break;
				
				default:
					break;
			}
		}
	}

	public void SetPause(bool p)
	{
		if (p)
		{
			snpc.StopSpriteAnimator();
		}
		else
		{
			snpc.ResumeSpriteAnimator();
		}
		
		_paused = p;
	}
	
	public void MoveToRandomPoint()
	{
		SpriteAnimatorNpc snpc = GetComponent<SpriteAnimatorNpc>();
		_logicState = LogicState.Walking;
		snpc.PlaySpriteAnimator();
		
		_pointWalkCount = (int) Random.Range(6, 10);
		
		if (!_lastTarget)
		{
			int random_range = _currentTarget.GetEdges().Length;
			_lastTarget = _currentTarget;
			_currentTarget = _currentTarget.GetEdges()[(int)Random.Range(0, random_range)].GetComponent<Node>();
		}
		else
		{
			if (Random.Range(0f,1f) < 0.05f)
			{
				Node aux_target = _currentTarget;
				_currentTarget = _lastTarget;
				_lastTarget = aux_target;
			}
			else
			{
				GameObject[] targets_array = _currentTarget.GetEdges().Clone() as GameObject[];
				int random_range = targets_array.Length;
				_lastTarget = _currentTarget;
				_currentTarget = targets_array[(int)Random.Range(0, random_range)].GetComponent<Node>();
			}
		} 
	}
	
	public void MoveToNextPoint()
	{
		if(_desiredTarget)
		{
			MoveToDesiredNode(_desiredTarget);

        }
		else
		{
			MoveToRandomPoint();
		}
	}
	
	private void MoveToDesiredNode(Node target)
	{
        Node n = _currentTarget.NextNodeTo(target);
        if (n == null || n == target) //Finish
        {
            SpriteAnimatorNpc snpc = GetComponent<SpriteAnimatorNpc>();
            _logicState = LogicState.Stand;
        }
        else
        {
            _currentTarget = n;
        }
        
	}

	private void MoveToNextRandomPoint()
	{
		_pointWalkCount--;
		
		if (_pointWalkCount > 0)
		{
			if (!_lastTarget)
			{
				int random_range = _currentTarget.GetEdges().Length;
				_lastTarget = _currentTarget;
				_currentTarget = _currentTarget.GetEdges()[(int)Random.Range(0, random_range)].GetComponent<Node>();
			}
			else
			{
				if (Random.Range(0f,1f) < _backProbability)
				{
					Node aux_target = _currentTarget;
					_currentTarget = _lastTarget;
					_lastTarget = aux_target;
				}
				else
				{
					GameObject[] targets_array = null;
					
					if (_currentTarget.GetEdges().Length != 1)
					{
						targets_array = new GameObject[_currentTarget.GetEdges().Length - 1];
						int ind = 0;
						
						for (int i = 0; i < _currentTarget.GetEdges().Length; i++)
						{
							if (_lastTarget.transform.position != _currentTarget.GetEdges()[i].transform.position)
							{
								targets_array[ind] = _currentTarget.GetEdges()[i];
								ind++;
							}
						}
					}
					else
					{
						 targets_array = _currentTarget.GetEdges().Clone() as GameObject[];
					}
					
					int random_range = targets_array.Length;
					_lastTarget = _currentTarget;
					_currentTarget = targets_array[(int)Random.Range(0, random_range)].GetComponent<Node>();
				}
			} 
		}
		else
		{
			_logicState = LogicState.Stand;
			SpriteAnimatorNpc snpc = GetComponent<SpriteAnimatorNpc>();
			snpc.StopSpriteAnimator(true);
		}
	}
	
	public void SetDesiredNode(Node n)
	{
		_desiredTarget = n;
        _logicState = LogicState.Walking;
	}
	
	public void ChangeState(Furniture f)
	{
		bool finded = false;
		
		for (int i=0; i < Fear.Length; i++)
		{
			if (Fear[i] == f)
			{
				Buu(i);
				finded = true;
				break;
			}
		}
		
		if(!finded)
		{
			for(int i=0; i < Courage.Length; i++)
			{
				if(Courage[i] == f)
				{
					Wii(i);
					break;
				}
			}
		}
		
	}
	
	private void Buu(int array_pos)
	{
		if(_sound != null)
		{
			if (SettingsController.instance.PlaySFX())
			{
				AudioSource.PlayClipAtPoint(_sound, GameObject.Find("Main Camera").transform.position);
			}
		}
		
		if(_logicState == LogicState.Stand)
		{
			MoveToRandomPoint();
		}
		
		SetAnimState(AnimState.Frightened);
		_game.AddScare(FearDamage[array_pos]);
	}
	
	private void Wii(int array_pos)
	{
		SetAnimState(AnimState.Normal);
		_game.AddScare(FearDamage[array_pos]);
	}

	public string GetCurrentRoom()
	{
		return _currentRoom;
	}
	
	public void SetAnimState(AnimState s)
	{
		_animState = s;
		
		switch(s)
		{
			case AnimState.Normal: 
				_speed = 5f;
				break;
				
			case AnimState.Frightened:
				_speed = 10f;
				break;
		}
	}
	
	public AnimState GetAnimState()
	{
		return _animState;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		_currentRoom = other.gameObject.name;
	}
	
	private void DoToggleDoor() {

        Vector3 dir = _currentTarget.transform.position - transform.position;
        Vector2 origin = transform.position + dir.normalized;
		
		bool onDoorIn = false;
		bool onDoorOut = false;
		
		Actuator actIn = null;
		Actuator actOut = null;

        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, dir, 1f);
		Debug.DrawRay(new Vector3(origin.x,origin.y,1f), dir, Color.green);
				
		for (int i = 0; i < hit.Length; i++)
		{
			GameObject go = hit[i].transform.gameObject;
			actIn = go.GetComponent<Actuator>();
			
			if(actIn)
			{
				onDoorIn = true;
				break;
			}
		}
		
		origin = transform.position - dir.normalized;
		hit = Physics2D.RaycastAll(origin, -dir, 1f);
		
		for (int i = 0; i < hit.Length; i++)
		{
			GameObject go = hit[i].transform.gameObject;
			actOut = go.GetComponent<Actuator>();
			if(actOut)
			{
				onDoorOut = true;
				break;
			}
		}
		
		if(onDoorIn && !actIn.GetToggled())
		{
			actIn.OnAction(false);
		}
		else if(onDoorOut && actOut.GetToggled() && _animState == AnimState.Frightened)
		{
			actOut.OnAction(false);
		}
			
		if (onDoorOut || onDoorIn)
		{
			Color c = GetComponent<SpriteRenderer>().color;
			c.a = 0.5f;
			GetComponent<SpriteRenderer>().color = c;
		}
		else
		{
			Color c = GetComponent<SpriteRenderer>().color;
			c.a = 1f;
			GetComponent<SpriteRenderer>().color = c;
		}
	}
    public LogicState GetLogicState()
    {
        return _logicState;
    }
}
