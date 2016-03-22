using UnityEngine;
using System.Collections.Generic;


public class SpriteAnimatorNpc : MonoBehaviour
{
	public float _animationTimer = 0f;
	public int _animationIndex = 0;
	
	public Sprite[] _walkUpFrames;
	public Sprite[] _walkDownFrames;
	public Sprite[] _walkSideFrames;
	
	private Game _game;
	public float _timeToNext;
    public bool _loop = true;

	private SpriteRenderer _renderer;
	
	public enum State { WalkUp, WalkDown, WalkSide };
	private State _state;
	
    public bool _isRunning = false;
	private Sprite[] s;
	
	void Awake()
	{
		s = _walkUpFrames;
		_state = State.WalkUp;
		_renderer = GetComponent<SpriteRenderer>();
	}

	void Update ()
	{
		switch (_state)
		{
			case State.WalkDown:
				s = _walkDownFrames;
				break;
			case State.WalkSide:
				s = _walkSideFrames;
				break;
			case State.WalkUp:
				s = _walkUpFrames;
				break;
			default:
				s = _walkUpFrames;
				break;
		}
		
		if (_isRunning) //Animar si >= 0
		{
			if (_timeToNext > _animationTimer)
			{
				_animationTimer += Time.deltaTime; //keep counting
			}
			else
			{
				if (_animationIndex < s.Length - 1) // If frames remaining... animate
				{
					_animationIndex++; 
				}
                else if (_loop)
                {
                    _animationIndex = 0;
                }
                else
                {
                    _isRunning = false;
                }

                _animationTimer = 0f;
                _renderer.sprite = s[_animationIndex];
			}
		}
	}
	
	public void SetGame(Game game)
	{
		_game = game;
	}

    public void PlaySpriteAnimator()
    {
        _animationIndex = 0;
        _isRunning = true;
        _loop = true;
    }
	
	public void ResumeSpriteAnimator()
	{
		_isRunning = true;
        _loop = true;
	}

    public void StopSpriteAnimator(bool force = false)
    {
        _loop = false;
		
        if (force) // Stop animation without wait until last frame
        {
            _isRunning = false;
        }
    }

    public void Reverse()
    {
        Sprite[] frames = new Sprite[s.Length];
		
        for (int i = s.Length - 1; i >= 0; i--)
        {
            frames[s.Length - i - 1] = s[i];
        }
		
        s = frames;
    }
    
    public void SetState(State s)
    {
		if(_state != s)
		{
			_state = s;
		}
	}
	
}
