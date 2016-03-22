using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroFader : MonoBehaviour
{
	private float _timer;
	public float _fadeTime = 1f;
	public float _logoTime = 5f;
	public float _delay = 0.2f;
	
	public string _scene;
	public GameObject _logo;
	public GameObject _logoGgj;
	public GameObject _overlay;
	public Sprite _jumble;
	public Sprite _ggj;
	public AudioClip _music;
	
	public enum State { Display, FadeIn, FadeOut, Delay };
	private int _counter;
	private State _state;
	
	
	void Start ()
	{
		_state = State.FadeIn;
		_timer = -1;
		_counter = 0;
		
		if (SettingsController.instance.PlayMusic())
		{
			AudioSource.PlayClipAtPoint(_music, transform.position);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timer -= Time.deltaTime;
		
		if(_state == State.Display) //Display logo
		{
			if(_timer < 0f)
			{
				_state = State.FadeIn; //change to FadeIn status
				_timer = _fadeTime; //Wait only fadeTime
			}
		}
		else if (_state == State.FadeOut) //FadeOut
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a = _timer / _fadeTime;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (_timer < 0f)
			{
				_state = State.Display; //Change to display logo
				_timer = _logoTime; //timeout to wait
			}
		}
		else if (_state == State.FadeIn) //FadeOut
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a = 1 - _timer / _fadeTime;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (_timer < 0f)
			{
				_state = State.FadeOut; //Change to display logo
				_timer = _fadeTime; //timeout to wait
				
				if(_counter == 0)
				{
					_logo.SetActive(true);
					_logoGgj.SetActive(false);
				}
				else if(_counter == 1)
				{
					_logo.SetActive(false);
					_logoGgj.SetActive(true);
				}
				else
				{
                    SceneManager.LoadScene(_scene);
                }
				
				_counter++;
			}
		}
		
	}
}
