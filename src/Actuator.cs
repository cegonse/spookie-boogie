using System;
using UnityEngine;
using System.Collections.Generic;

public class Actuator : MonoBehaviour
{
    public enum State { IDLE, RUNNING };
	
	public Furniture _type;
	public float _timeout;
	public int _useCount;
	public AudioClip _sound;
	public Sprite _broken;
    public bool _toggleable = false;
    public bool _toggled = false;
    
    private State _state;
    
    public Game _game;
    private float _timeleft;
    public string _room;
    private bool _animated;
    private bool _playing = false;

	void Awake()
	{
        _state = State.IDLE;
        _timeleft = 0;
	}

    void Start()
    {
        if (GetComponent<SpriteAnimator>() && GetComponent<SpriteAnimator>()._frames.Length > 0)
        {
            _animated = true;
        }
        else
        {
            _animated = false;
        }
    }

    void Update()
    {
        SpriteAnimator sa = GetComponent<SpriteAnimator>();
        
		if (_animated && _toggled && !sa.IsToggled())
        {
            sa.Toggle();
            sa.Reverse();
        }
        else if (_animated && !_toggled && sa.IsToggled())
        {
            sa.Toggle();
            sa.Reverse();
        }
        
		if (_state == State.RUNNING)
        {
            _timeleft -= Time.deltaTime;
			
            if (_timeleft <= 0)
            {
                _playing = false;
                _state = State.IDLE; //Action finish
                
				if (_animated)
                {
					sa.StopSpriteAnimator();
                }
            }
        }
    }

    public void OnBlow()
    {
        if (_type == Furniture.Velas)
        {
            OnAction();
        }
    }

    public void OnTremble()
    {
        if (_type == Furniture.Jarron)
        {
            OnAction();
        }
    }

    public void OnAction(bool fright = true) 
    {
		if (!fright && _type != Furniture.Puerta)
		{
			return;
		}
        if ((_useCount != 0 && _state == State.IDLE) || (_state == State.RUNNING && !fright))
        {
            _timeleft = _timeout;
            _state = State.RUNNING;
            
            List<Npc> npcs = _game.GetNpcs();
            
            if (fright)
            {
				for (int i = 0; i < npcs.Count; i++) //fright people
				{
					Npc n = npcs[i];
					
					if (n.GetCurrentRoom() == _room)
					{
						n.ChangeState(_type);
					}
				}
			}

            if (_animated)
			{
                if (_toggleable)
                {
                    _toggled = !_toggled; 
                }
				
                GetComponent<SpriteAnimator>().PlaySpriteAnimator();
            }

			if (_sound != null && !_playing && SettingsController.instance.PlaySFX()) //sound
			{
				_playing = true;
				//AudioSource[] aud = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
				AudioSource.PlayClipAtPoint(_sound, GameObject.Find("Main Camera").transform.position);
			}
			
			if (_type == Furniture.Luz)//ligths
			{
				GameObject go = GameObject.Find("Luces/"+_room);
				go.GetComponent<SpriteRenderer>().enabled = !go.GetComponent<SpriteRenderer>().enabled;
			}
			
			//Todo reproduce animation for this actuator
            if (_useCount > 0) //Only rest if non-infinite uses
            {
                _useCount--;
                
                if (_useCount == 0)
                {
					if (_broken != null)
					{
						GetComponent<SpriteRenderer>().sprite = _broken;
					}
				}
            }
        }
    }

    public void SetGame(Game game)
    {
        _game = game;
    }

    public void SetTimeout(float to)
    {
        _timeout = to;
    }

    public void SetUseCount(int uc)
    {
        _useCount = uc;
    }

    public void SetFurniture(Furniture t)
    {
        _type = t;
    }

    public Furniture GetFurniture()
    {
        return _type;
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		_room = other.gameObject.name;
	}
	
	public string GetRoom()
	{
		return _room;
	}
	
	public bool GetToggled()
	{
		return _toggled;
	}
}
