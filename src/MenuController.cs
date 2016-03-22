using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
	public GameObject _title;
	
	public GameObject _nameEntry;
	public UnityEngine.UI.InputField _nameEntryField;
	
	public GameObject _mainMenu;
	public UnityEngine.UI.Text _nicknameMenu;
	
	public GameObject _accessDialog;
	public GameObject _networkErrorDialog;
	
	public GameObject _playMenu;
	
	public GameObject _settingsMenu;
	public UnityEngine.UI.Text _languageLabel;
	public UnityEngine.UI.Toggle _musicToggle;
	public UnityEngine.UI.Toggle _sfxToggle;
	
	public GameObject _wipeLocalMenu;
	
	public GameObject _changeUsername;
	
	public GameObject _extrasMenu;
	public GameObject _creditsMenu;
	
	public AudioClip _music;
	public AudioSource _audioSource;
	
	public enum State
	{
		IdleIntro,
		FadingOutIntro,
		
		FadingInNameEntry,
		IdleNameEntry,
		FadingOutNameEntry,
		
		FadingInNameRegistration,
		IdleNameRegistration,
		FadingOutNameRegistration,
		
		FadingInGetUserRanking,
		IdleGetUserRanking,
		FadingOutGetUserRanking,
		
		FadingInMainMenu,
		IdleMainMenu,
		FadingOutMainMenu,
		
		FadingInPlayMenu,
		IdlePlayMenu,
		FadingOutPlayMenu,
		
		FadingInSettingsMenu,
		IdleSettingsMenu,
		FadingOutSettingsMenu,
		
		FadingInWipeLocalData,
		IdleWipeLocalData,
		FadingOutWipeLocalData,
		
		FadingInChangeUsername,
		IdleChangeUsername,
		FadingOutChangeUsername,
		
		FadingInExtrasMenu,
		IdleExtrasMenu,
		FadingOutExtrasMenu,
		
		FadingInCreditsMenu,
		IdleCreditsMenu,
		FadingOutCreditsMenu
	}
	
	public State _state = State.IdleIntro;
	private State _targetState;
	
	void Start()
	{
		if (SettingsController.instance.PlayMusic())
		{
			AudioSource.PlayClipAtPoint(_music, transform.position);
			_audioSource = GameObject.Find("One shot audio").GetComponent<AudioSource>();
		}
	}
	
	void Update()
	{
		if (_state == State.IdleIntro)
		{
			if (Input.GetMouseButtonDown(0))
			{
				_state = State.FadingOutIntro;
			}
		}
		else if (_state == State.FadingOutIntro)
		{
			Vector3 s = _title.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_title.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_title.SetActive(false);
				
				if (SettingsController.instance.GetNickname() == "")
				{
					_nameEntry.SetActive(true);
					_nameEntry.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
					_state = State.FadingInNameEntry;
				}
				else
				{
					_accessDialog.SetActive(true);
					_accessDialog.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
					_state = State.FadingInGetUserRanking;
				}
			}
		}
		else if (_state == State.FadingInNameEntry)
		{
			Vector3 s = _nameEntry.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_nameEntry.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_state = State.IdleNameEntry;
			}
		}
		else if (_state == State.FadingOutNameEntry)
		{
			Vector3 s = _nameEntry.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_nameEntry.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_nameEntry.SetActive(false);
				_accessDialog.SetActive(true);
				_accessDialog.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				_state = State.FadingInNameRegistration;
			}
		}
		else if (_state == State.FadingInNameRegistration)
		{
			Vector3 s = _accessDialog.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_accessDialog.transform.localScale = s;
			
			if (s.x > 1f)
			{
				SettingsController.instance.SetInternalUsername(SettingsController.instance.GetNickname());
				NetworkController.instance.RegisterNewUser(SettingsController.instance.GetNickname(), 
					SettingsController.instance.GetInternalUsername(), gameObject);
				_state = State.IdleNameRegistration;
			}
		}
		else if (_state == State.FadingOutNameRegistration)
		{
			Vector3 s = _accessDialog.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_accessDialog.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_accessDialog.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				_state = State.FadingInGetUserRanking;
			}
		}
		else if (_state == State.FadingInGetUserRanking)
		{
			Vector3 s = _accessDialog.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_accessDialog.transform.localScale = s;
			
			if (s.x > 1f)
			{
				Debug.Log("User ID: " + SettingsController.instance.GetInternalUsername());
				NetworkController.instance.GetRankById(SettingsController.instance.GetInternalUsername(), gameObject);
				_state = State.IdleGetUserRanking;
			}
		}
		else if (_state == State.FadingOutGetUserRanking)
		{
			Vector3 s = _accessDialog.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_accessDialog.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_accessDialog.SetActive(false);
				
				if (_targetState == State.FadingInMainMenu)
				{
					_mainMenu.SetActive(true);
					_mainMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else if (_targetState == State.FadingInNameEntry)
				{
					_nameEntry.SetActive(true);
					_nameEntry.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				
				_state = _targetState;
			}
		}
		else if (_state == State.FadingInMainMenu)
		{
			Vector3 s = _mainMenu.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_mainMenu.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_nicknameMenu.text = "#" + SettingsController.instance.GetRanking() + " - " + 
					SettingsController.instance.GetNickname();
				_state = State.IdleMainMenu;
			}
		}
		else if (_state == State.FadingOutMainMenu)
		{
			Vector3 s = _mainMenu.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_mainMenu.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_mainMenu.SetActive(false);
				
				if (_targetState == State.FadingInPlayMenu)
				{
					_playMenu.SetActive(true);
					_playMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else if (_targetState == State.FadingInSettingsMenu)
				{
					_settingsMenu.SetActive(true);
					_settingsMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
					
					if (_sfxToggle.isOn && !SettingsController.instance.PlaySFX())
					{
						_sfxToggle.isOn = false;
					}
					else if (!_sfxToggle.isOn && SettingsController.instance.PlaySFX())
					{
						_sfxToggle.isOn = true;
					}
					
					if (_musicToggle.isOn && !SettingsController.instance.PlayMusic())
					{
						_musicToggle.isOn = false;
					}
					else if (!_musicToggle.isOn && SettingsController.instance.PlayMusic())
					{
						_musicToggle.isOn = true;
					}
				}
				else if (_targetState == State.FadingInExtrasMenu)
				{
					_extrasMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
					_extrasMenu.SetActive(true);
				}
				
				_state = _targetState;
			}
		}
		else if (_state == State.FadingInPlayMenu)
		{
			Vector3 s = _playMenu.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_playMenu.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_state = State.IdlePlayMenu;
			}
		}
		else if (_state == State.FadingOutPlayMenu)
		{
			Vector3 s = _playMenu.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_playMenu.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_playMenu.SetActive(false);
				
				if (_targetState == State.FadingInMainMenu)
				{
					_mainMenu.SetActive(true);
					_mainMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				
				_state = _targetState;
			}
		}
		else if (_state == State.FadingInSettingsMenu)
		{
			Vector3 s = _settingsMenu.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_settingsMenu.transform.localScale = s;
			
			if (s.x > 1f)
			{	
				_state = State.IdleSettingsMenu;
			}
		}
		else if (_state == State.FadingOutSettingsMenu)
		{
			Vector3 s = _settingsMenu.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_settingsMenu.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_settingsMenu.SetActive(false);
				
				if (_targetState == State.FadingInMainMenu)
				{
					_mainMenu.SetActive(true);
					_mainMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else if (_targetState == State.FadingInWipeLocalData)
				{
					_wipeLocalMenu.SetActive(true);
					_wipeLocalMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else if (_targetState == State.FadingInChangeUsername)
				{
					_changeUsername.SetActive(true);
					_changeUsername.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				
				_state = _targetState;
			}
		}
		else if (_state == State.FadingInWipeLocalData)
		{
			Vector3 s = _wipeLocalMenu.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_wipeLocalMenu.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_state = State.IdleWipeLocalData;
			}
		}
		else if (_state == State.FadingOutWipeLocalData)
		{
			Vector3 s = _wipeLocalMenu.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_wipeLocalMenu.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				_wipeLocalMenu.SetActive(false);
				_settingsMenu.SetActive(true);
				_settingsMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				_state = State.FadingInSettingsMenu;
			}
		}
		else if (_state == State.FadingInChangeUsername)
		{
			Vector3 s = _changeUsername.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_changeUsername.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_state = State.IdleChangeUsername;
			}
		}
		else if (_state == State.FadingOutChangeUsername)
		{
			Vector3 s = _changeUsername.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_changeUsername.transform.localScale = s;
			
			if (s.x < 0.1f)
			{
				if (_targetState == State.FadingInSettingsMenu)
				{
					_changeUsername.SetActive(false);
					_settingsMenu.SetActive(true);
					_settingsMenu.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				else if (_targetState == State.FadingInNameEntry)
				{
					_changeUsername.SetActive(false);
					_nameEntry.SetActive(true);
					_nameEntry.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
				}
				
				_state = _targetState;
			}
		}
		else if (_state == State.FadingInExtrasMenu)
		{
			Vector3 s = _extrasMenu.transform.localScale;
			s.x += Time.deltaTime*4f;
			s.y += Time.deltaTime*4f;
			_extrasMenu.transform.localScale = s;
			
			if (s.x > 1f)
			{
				_state = State.IdleExtrasMenu;
			}
		}
		else if (_state == State.FadingOutExtrasMenu)
		{
			Vector3 s = _extrasMenu.transform.localScale;
			s.x -= Time.deltaTime*4f;
			s.y -= Time.deltaTime*4f;
			_extrasMenu.transform.localScale = s;
			
			if (s.x < 0f)
			{
				_extrasMenu.SetActive(false);
				_mainMenu.SetActive(true);
				_state = State.FadingInMainMenu;
			}
		}
		else if (_state == State.FadingInCreditsMenu)
		{
		
		}
		else if (_state == State.FadingOutCreditsMenu)
		{
			
		}
	}
	
	public void OnNameEntryAccept()
	{
		if (_nameEntryField.text != "")
		{
			SettingsController.instance.SetNickname(_nameEntryField.text);
			_state = State.FadingOutNameEntry;
		}
	}
	
	public void OnNetworkError(string error)
	{
		Debug.LogWarning("Network error! " + error);
		_networkErrorDialog.SetActive(true);
		
		if (_state == State.IdleNameRegistration)
		{
			_state = State.FadingOutNameRegistration;
		}
		else if (_state == State.IdleGetUserRanking)
		{
			_state = State.FadingOutGetUserRanking;
		}
	}
	
	public void OnNetworkMessage(string data)
	{
		if (_state == State.IdleNameRegistration)
		{
			Debug.Log("Network command received (" + data.Length + " bytes): " + data);
			JSONObject json = new JSONObject(data);
			
			if (json)
			{
				if (json["command"].str != "register")
				{
					Debug.LogWarning("Network error! (" + json["status"].str + ")" + json["data"].str);
					_networkErrorDialog.SetActive(true);
				}
			}
			
			_state = State.FadingOutNameRegistration;
		}
		else if (_state == State.IdleGetUserRanking)
		{
			Debug.Log("Network command received (" + data.Length + " bytes): " + data);
			JSONObject json = new JSONObject(data);
			
			if (json)
			{
				if (json["command"].str == "rankById")
				{
					if (json["status"].str == "-3")
					{
						Debug.LogWarning("User tried to log in with an account that doesn't exist!");
						_targetState = State.FadingInNameEntry;
					}
					else
					{
						SettingsController.instance.SetRanking(int.Parse(json["data"]["rank"].str));
						_targetState = State.FadingInMainMenu;
					}
				}
				else
				{
					Debug.LogWarning("Network error! (" + json["status"].str + ")" + json["data"].str);
					_networkErrorDialog.SetActive(true);
					_targetState = State.FadingInMainMenu;
				}
			}
			
			_state = State.FadingOutGetUserRanking;
		}
	}
	
	public void OnNetworkErrorAccept()
	{
		_networkErrorDialog.SetActive(false);
	}
	
	public void OnPlay()
	{
		_state = State.FadingOutMainMenu;
		_targetState = State.FadingInPlayMenu;
	}
	
	public void OnSettings()
	{
		_state = State.FadingOutMainMenu;
		_targetState = State.FadingInSettingsMenu;
	}
	
	public void OnExtras()
	{
		_targetState = State.FadingInExtrasMenu;
		_state = State.FadingOutMainMenu;
	}
	
	public void OnExtrasBack()
	{
		_state = State.FadingOutExtrasMenu;
	}
	
	public void OnRanking()
	{
	}
	
	public void OnEasy()
	{
		StartDifficulty("Easy");
	}
	
	public void OnMedium()
	{
		StartDifficulty("Medium");
	}
	
	public void OnHard()
	{
		StartDifficulty("Hard");
	}
	
	public void OnTimeAttack()
	{
		StartDifficulty("TimeAttack");
	}
	
	private void StartDifficulty(string dif)
	{
		SettingsController.instance.SetDifficulty(dif);
		
		if (SettingsController.instance.IsFirstPlay())
		{
			SceneManager.LoadScene("cutscene");
			SettingsController.instance.ToggleFirstPlay();
		}
		else
		{
			SceneManager.LoadScene("scene");
		}
	}
	
	public void OnPlayBack()
	{
		_targetState = State.FadingInMainMenu;
		_state = State.FadingOutPlayMenu;
	}
	
	public void OnSettingsBack()
	{
		_targetState = State.FadingInMainMenu;
		_state = State.FadingOutSettingsMenu;
	}
	
	public void OnToggleMusic()
	{
		SettingsController.instance.ToggleMusic(_musicToggle.isOn);
		
		if (SettingsController.instance.PlayMusic())
		{
			if (_audioSource == null)
			{
				AudioSource.PlayClipAtPoint(_music, transform.position);
				_audioSource = GameObject.Find("One shot audio").GetComponent<AudioSource>();
			}
			else
			{
				_audioSource.Play();
			}
		}
		else
		{
			if (_audioSource != null)
			{
				_audioSource.Pause();
			}
		}
	}
	
	public void OnToggleSFX()
	{
		SettingsController.instance.ToggleSFX(_sfxToggle.isOn);
	}
	
	public void OnNextLanguage()
	{
	}
	
	public void OnPreviousLanguage()
	{
	}
	
	public void OnWipeData()
	{
		_targetState = State.FadingInWipeLocalData;
		_state = State.FadingOutSettingsMenu;
	}
	
	public void OnWipeDataCancel()
	{
		_state = State.FadingOutWipeLocalData;
	}
	
	public void OnWipeDataAccept()
	{
		SettingsController.instance.Wipe();
		SceneManager.LoadScene("intro");
	}
	
	public void OnChangeUsername()
	{
		_targetState = _state = State.FadingInChangeUsername;
		_state = State.FadingOutSettingsMenu;
	}
	
	public void OnChangeUsernameAccept()
	{
		_targetState = State.FadingInNameEntry;
		_state = State.FadingOutChangeUsername;
	}
	
	public void OnChangeUsernameCancel()
	{
		_targetState = State.FadingInSettingsMenu;
		_state = State.FadingOutChangeUsername;
	}
}
