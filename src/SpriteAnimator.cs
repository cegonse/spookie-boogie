using UnityEngine;
using System.Collections.Generic;

public struct AnimationFrame
{
	public float timeToNext;
	public Sprite sprite;
}

public class SpriteAnimator : MonoBehaviour
{
	public float _animationTimer = 0f;
	public int _animationIndex = 0;
	public Sprite[] _frames;
	public float _timeToNext;
    public bool _loop = true;
    public bool _toggled = false;

    private SpriteRenderer _renderer;
    private Sprite[] _selectedFrames;
    
 
    public bool _isRunning = false;

	void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();

        if(_toggled)
        {
            Reverse();
        }

        _selectedFrames = _frames;

    }

	void Update ()
	{
		if (_isRunning) //Animar si >= 0
		{
			if (_timeToNext > _animationTimer)
			{
				_animationTimer += Time.deltaTime; //keep counting
			}
			else
			{
				if (_animationIndex < _frames.Length - 1) // If frames remaining... animate
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
                _renderer.sprite = _selectedFrames[_animationIndex];
			}
		}
	}
	

    public void PlaySpriteAnimator()
    {
        _animationIndex = 0;
        _selectedFrames = _frames;
        _isRunning = true;
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
        Sprite[] frames = new Sprite[_frames.Length];
        for (int i = _frames.Length - 1; i >= 0; i--)
        {
            frames[_frames.Length - i - 1] = _frames[i];
        }
        _frames = frames;
    }

    public bool IsToggled()
    {
        return _toggled;
    }

    public void Toggle()
    {
        _toggled = !_toggled;
    }

}
