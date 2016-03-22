using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
	public GameObject _frame1;
	public GameObject _frame2;
	public GameObject _frame3;
	public GameObject _frame4;
	public GameObject _frame5;
	public GameObject _frame6;
	public GameObject _frame7;
	public GameObject _frame8;
	public GameObject _frame9;
	public GameObject _frame10;
	public GameObject _frame11;
	public GameObject _frame12;
	public GameObject _overlay;
	
	public AudioClip _musica;
	public int _state = 0;
	float _timer = 0f;

	void Start()
	{
		if (SettingsController.instance.PlayMusic()) AudioSource.PlayClipAtPoint(_musica, transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
            SceneManager.LoadScene("scene");
		}
		
		if (_state == 0)
		{
			_frame1.SetActive(true);
			_state++;
		}
		else if (_state == 1)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_timer = 0f;
				_frame1.SetActive(false);
				_frame2.SetActive(true);
				_state++;
			}
		}
		else if (_state == 2)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_timer = 0f;
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 3)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_timer = 0f;
				_frame2.SetActive(false);
				_frame3.SetActive(true);
				_state++;
			}
		}
		else if (_state == 4)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_timer = 0f;
				_overlay.SetActive(false);
				_state++;
			}
		}
		else if (_state == 5)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_overlay.SetActive(true);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 6)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame3.SetActive(false);
				_frame4.SetActive(true);
				_state++;
			}
		}
		else if (_state == 7)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_state++;
			}
		}
		else if (_state == 8)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_timer = 0f;
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 9)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame4.SetActive(false);
				_frame5.SetActive(true);
				_state++;
			}
		}
		else if (_state == 10)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 11)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_overlay.SetActive(true);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 12)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame5.SetActive(false);
				_frame6.SetActive(true);
				_state++;
			}
		}
		else if (_state == 13)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_state++;
			}
		}
		else if (_state == 14)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_timer = 0f;
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 15)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame6.SetActive(false);
				_frame7.SetActive(true);
				_state++;
			}
		}
		else if (_state == 16)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_state++;
			}
		}
		else if (_state == 17)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_timer = 0f;
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 18)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame7.SetActive(false);
				_frame8.SetActive(true);
				_state++;
			}
		}
		else if (_state == 19)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_state++;
			}
		}
		else if (_state == 20)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 1f)
			{
				_frame9.SetActive(true);
				_frame8.SetActive(false);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 21)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 0.5f)
			{
				_frame10.SetActive(true);
				_frame9.SetActive(false);
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 22)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame10.SetActive(false);
				_frame11.SetActive(true);
				_state++;
			}
		}
		else if (_state == 23)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 24)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_overlay.SetActive(true);
				_state ++;
			}
		}
		else if (_state == 25)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_frame11.SetActive(false);
				_frame12.SetActive(true);
				_state++;
			}
		}
		else if (_state == 26)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a -= Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a < 0f)
			{
				_overlay.SetActive(false);
				_timer = 0f;
				_state++;
			}
		}
		else if (_state == 27)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 4f)
			{
				_overlay.SetActive(true);
				_state++;
			}
		}
		else if (_state == 28)
		{
			Color c = _overlay.GetComponent<UnityEngine.UI.Image>().color;
			c.a += Time.deltaTime * 2f;
			_overlay.GetComponent<UnityEngine.UI.Image>().color = c;
			
			if (c.a > 1f)
			{
				_state++;
				_timer = 0f;
			}
		}
		else if (_state == 29)
		{
			_timer += Time.deltaTime;
			
			if (_timer > 3f)
			{
                SceneManager.LoadScene("scene");
			}
		}
	}
}
