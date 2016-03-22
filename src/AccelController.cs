using UnityEngine;
using System.Collections;

public class AccelController : MonoBehaviour {

    public int _shakes = 5;
    [Range(0f,1f)]
    public float _sensivility;
    public float _delay;

    private int _counter;
    private bool _midShake;
    private Game _game;
    private float _timer;
    // Use this for initialization
    void Start () {
        _game = GetComponent<Game>();
        _counter = 0;
        _timer = _delay;
        _midShake = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (_counter < _shakes)
        {
            if (_midShake && (Input.acceleration.x > _sensivility || Input.acceleration.y > _sensivility || Input.acceleration.z > _sensivility))
            {
                _counter++;
                _midShake = false;
                _timer = _delay;
            }
            else if (!_midShake && (Input.acceleration.x < -_sensivility || Input.acceleration.y < -_sensivility || Input.acceleration.z < -_sensivility))
            {
                _midShake = true;
                _timer = _delay;
            }
            else if(_timer < 0)
            {
                _counter = 0;
                _midShake = false;
                _timer = Mathf.Infinity;
            }
            _timer -= Time.deltaTime;

        }
        else
        {
            _midShake = false;
            _counter = 0;
            _game.Tremble();
        }
	}
}
