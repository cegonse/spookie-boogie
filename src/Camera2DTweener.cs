using UnityEngine;
using System.Collections.Generic;

public class Camera2DTweener : MonoBehaviour
{
	public enum State : int
	{
		Tweening = 0,
		Idle = 1
	};
	
	private State _state = State.Idle;
	
	private float _targetOrthoSize = 0f;
	private Vector3 _targetPoint;
	private float _timer = 0f;
	private float _tweenTime = 0f;
	private float _orthoSpeed = 0f;
	private float _positionSpeed = 0f;
	
	private Camera _cam = null;
	private GameObject _caller = null;
	
	void Start()
	{
		_targetPoint = new Vector3();
		_cam = GetComponent<Camera>();
	}
	
	void Update()
	{
		if (_state == State.Tweening)
		{
			_timer += Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _positionSpeed / _tweenTime * Time.deltaTime);
			_cam.orthographicSize = Mathf.MoveTowards(_cam.orthographicSize, _targetOrthoSize, _orthoSpeed / _tweenTime * Time.deltaTime);
			
			if (_timer >= _tweenTime)
			{
				_caller.SendMessage("OnCameraTweenFinished");
				_state = State.Idle;
			}
		}
	}
	
	public bool IsTweening()
	{
		return _state == State.Tweening;
	}
	
	public void TweenTo(Vector3 tar, float ortho, float time, GameObject cal)
	{
		_tweenTime = time;
		_timer = 0f;
		_targetPoint = tar;
		_targetOrthoSize = ortho;
		_caller = cal;
		
		_positionSpeed = Vector3.Distance(tar, transform.position);
		
		if (_cam.orthographicSize >= ortho)
		{
			_orthoSpeed = Mathf.Abs(_cam.orthographicSize - ortho);
		}
		else
		{
			_orthoSpeed = Mathf.Abs(ortho - _cam.orthographicSize);
		}		
		
		_state = State.Tweening;
	}
}
