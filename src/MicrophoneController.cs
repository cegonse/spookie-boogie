using UnityEngine;
using System.Collections;

public class MicrophoneController : MonoBehaviour {
    [Range(0F,100F)]
    public float _threshold; //dB

    private Game _game;
    private AudioClip _mic;

    private float[] _window;
    private float _window_mean;
    private float _dbs = 0;
    private float _reference_power = 0.0002F;

   // Use this for initialization
   IEnumerator Start()
    {
        _game = GetComponent<Game>();
        _mic = null;
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
			try
			{
				_mic = Microphone.Start(null, true, 1, 44100);
			}
			catch 
			{
				_mic = null;
			}
				
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(_mic)
        {
            UpdateWindow();
            float f = 20 * Mathf.Log10((_window_mean / _reference_power));
            if(f > _dbs)
            {
                _dbs = f;
            }
            else if (_dbs >= _threshold && _dbs >= 3)
            {
                _dbs -= 3;
            }
            else if (_dbs >= _threshold/2 && _dbs >= 2)
            {
                _dbs -= 2;
            }
            else if (_dbs >= 1)
            {
                _dbs--;
            }
            if (_dbs > _threshold)
            {
               // Debug.Log("dB: "+ _dbs);
                _game.Blow();
            }
        }   
	    
	}

    private void UpdateWindow()
    {
        int s = 256;
        int p = Microphone.GetPosition(null);
        if (p > 0)
        {

            _window = new float[s];

            _mic.GetData(_window,p - (s + 1));
            _window_mean = 0F;
            foreach (float f in _window)
            {
                _window_mean += f;
            }
            _window_mean /= _window.Length;
            if(_window_mean < _reference_power)
            {
                _window_mean = _reference_power;
            }
        }
    }
}
