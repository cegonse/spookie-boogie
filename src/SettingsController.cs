using UnityEngine;
using System.Collections;
using System.IO;

public class GameSettings
{
	public int Ranking, OwnBest;
	public string Nickname, InternalUsername, Difficulty;
	public bool First, PlayMusic, PlaySfx;
	public SystemLanguage Language;
	
	// Empty constructor
	public GameSettings()
	{
		First = PlayMusic = PlaySfx = true;
		Nickname = InternalUsername = Difficulty = "";
		Ranking = OwnBest = 0;
		
		if (Application.systemLanguage == SystemLanguage.English ||
			Application.systemLanguage == SystemLanguage.Spanish ||
			Application.systemLanguage == SystemLanguage.Catalan ||
			Application.systemLanguage == SystemLanguage.Japanese)
		{
			Language = Application.systemLanguage;
		}
		else
		{
			Language = SystemLanguage.English;
		}
	}
}

public class SettingsController : MonoBehaviour
{
	public static SettingsController instance;

	private string _savePath;
	private GameSettings _settings;
	
	public bool PlayMusic()
	{
		return _settings.PlayMusic;
	}
	
	public bool PlaySFX()
	{
		return _settings.PlaySfx;
	}
	
	public void ToggleMusic(bool t)
	{
		_settings.PlayMusic = t;
		Serialize();
	}
	
	public void ToggleSFX(bool t)
	{
		_settings.PlaySfx = t;
		Serialize();
	}
	
	public string GetNickname()
	{
		return _settings.Nickname;
	}
	
	public string GetInternalUsername()
	{
		return _settings.InternalUsername;
	}
	
	public void SetNickname(string n)
	{
		_settings.Nickname = n;
		Serialize();
	}
	
	public void SetInternalUsername(string n)
	{
		_settings.InternalUsername = n + "_" + SystemInfo.deviceUniqueIdentifier;
		Serialize();
	}
	
	public int GetRanking()
	{
		return _settings.Ranking;
	}
	
	public void SetRanking(int r)
	{
		_settings.Ranking = r;
		Serialize();
	}
	
	public string GetDifficulty()
	{
		return _settings.Difficulty;
	}
	
	public void SetDifficulty(string d)
	{
		_settings.Difficulty = d;
		Serialize();
	}
	
	public bool IsFirstPlay()
	{
		return _settings.First;
	}
	
	public void ToggleFirstPlay()
	{
		_settings.First = !_settings.First;
		Serialize();
	}
	
	public int GetOwnBestScore()
	{
		return _settings.OwnBest;
	}
	
	public void SetOwnBestScore(int s)
	{
		_settings.OwnBest = s;
		Serialize();
	}
	
	public SystemLanguage GetLanguage()
	{
		return _settings.Language;
	}
	
	public void SetLanguage(SystemLanguage l)
	{
		_settings.Language = l;
		Serialize();
	}
	
	public void Serialize()
	{
		string json = JsonUtility.ToJson(_settings);
		File.WriteAllText(_savePath, json);
	}
	
	public void Refresh()
	{
		string json = File.ReadAllText(_savePath);
		JsonUtility.FromJsonOverwrite(json, _settings);
	}

	public void Wipe()
	{
		_settings = new GameSettings();
		Serialize();
	}
	
	void Awake()
	{
		if (instance == null) instance = this;
		
		_savePath = Application.persistentDataPath + "/save0.dat";
		_settings = new GameSettings();
		
		if (File.Exists(_savePath))
		{
			Refresh();
		}
		else
		{
			// Save the first file
			Serialize();
		}
	}
}
