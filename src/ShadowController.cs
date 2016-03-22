using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour
{
	public enum State
	{
		ShadowingIn,
		ShadowingOut,
		Idle
	}

	private SpriteRenderer[] _rend;
	private bool[] _finished;
	private State _state = State.Idle;

	void Start()
	{
		_rend = GetComponentsInChildren<SpriteRenderer>();
		_finished = new bool[_rend.Length];
		
		Color c = Color.white;
		c.a = 0;
		
		for (int i = 0; i < _rend.Length; i++)
		{
			_rend[i].color = c;
			_finished[i] = false;
		}
	}

	public void ShadowIn()
	{
		_state = State.ShadowingIn;
	}
	
	public void ShadowOut()
	{
		_state = State.ShadowingOut;
	}
	
	void Update()
	{
		if (_state == State.ShadowingIn)
		{
			for (int i = 0; i < _rend.Length; i++)
			{
				Color c = _rend[i].color;
				
				if (c.a < 1f)
				{
					c.a += Time.deltaTime;
					_rend[i].color = c;
				}
				else
				{
					_finished[i] = true;
				}
			}
			
			int reducedFinished = 0;
			
			for (int i = 0; i < _rend.Length; i++)
			{
				if (_finished[i]) reducedFinished++;
			}
			
			if (reducedFinished == _rend.Length)
			{
				for (int i = 0; i < _rend.Length; i++)
				{
					_finished[i] = false;
				}
				
				_state = State.Idle;
			}
		}
		else if (_state == State.ShadowingOut)
		{
			for (int i = 0; i < _rend.Length; i++)
			{
				Color c = _rend[i].color;
				
				if (c.a > 0f)
				{
					c.a -= Time.deltaTime;
					_rend[i].color = c;
				}
				else
				{
					_finished[i] = true;
				}
			}
			
			int reducedFinished = 0;
			
			for (int i = 0; i < _rend.Length; i++)
			{
				if (_finished[i]) reducedFinished++;
			}
			
			if (reducedFinished == _rend.Length)
			{
				for (int i = 0; i < _rend.Length; i++)
				{
					_finished[i] = false;
				}
				
				_state = State.Idle;
			}
		}
	}
}
