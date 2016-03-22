using UnityEngine;
using System.Collections;

public class MicrophoneController : MonoBehaviour {
    [Range(0F,10F)]
    public float _sensivility; //dB

    private Game _game;
    private AudioClip _mic;

    private float[] _window;
    private float _window_mean;
    private float[] _means;

    // Use this for initialization
    IEnumerator Start()
    {
        _means = new float[] { 0F,0F,0F};
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
	void Update () {
        if(_mic)
        {
            UpdateWindow();
            float f = 20 * Mathf.Log10((_window_mean - _means[0])*100);
            if (f > _sensivility)
            {
                Debug.Log("dB: "+ f);
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
        }
        else
        {
            _window_mean = 0;
        }

        _means[2] = _means[1];
        _means[1] = _means[0];
        _means[0] = _window_mean;

        //fir
        _means[0] += 0.3F * (_means[1] - _window_mean) + 0.1F * (_means[2] - _window_mean);

        /*foreach (float f in window)
        {
            window_desv += Mathf.Abs(f - window_mean);
        }
        window_desv /= window.Length;

        float pivot = window_mean;
        if (means_counter < size)
        {
            means_counter++;
        }
        for (int i = 0; i < means_counter; i++)
        {
            float aux = means[i];
            means[i] = pivot;
            pivot = aux;
            means_mean += means[i];
        }

        means_mean /= means_counter;
    */
    }
}
