using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour
{
	public static NetworkController instance;
	public string _address = "http://www.jumbledevs.net/spookieapi/";
	
	private GameObject _caller;
	private WWW _rest;

	public string DecodeHttpFriendly(string data)
	{
		string result = data;
		return result;
	}
	
	public string EncodeHttpFriendly(string data)
	{
		string result = data;
		return result;
	}
	
	public enum NetworkCommands : int
	{
		RegisterNewUser = 0,
		GetRankById,
		GetRankByName,
		GetRanking,
		SetScore
	};
	
	private readonly string[] NetworkCommandNames =
	{
		"register/",
		"rankById/",
		"rankByName/",
		"ranking/",
		"score/"
	};

	void Start()
	{
		if (instance == null)
		{
			instance = this;
			GameObject.DontDestroyOnLoad(this);
		}
	}
	
	void Update()
	{
		if (_rest != null)
		{
			if (_rest.isDone)
			{
				if (!string.IsNullOrEmpty(_rest.error))
				{
					_caller.SendMessage("OnNetworkError", _rest.error);
				}
				else
				{
					_caller.SendMessage("OnNetworkMessage", _rest.text);
				}
			
				_rest = null;
				_caller = null;
			}
		}
	}
	
	public bool RegisterNewUser(string username, string internalUsername, GameObject caller)
	{
		bool failed = true;
	
		if (_rest == null)
		{
			_rest = new WWW(_address + NetworkCommandNames[(int)NetworkCommands.RegisterNewUser] + username + "/" + internalUsername);
			_caller = caller;
			failed = false;
		}
		
		return failed;
	}
	
	public bool GetRankById(string id, GameObject caller)
	{
		bool failed = true;
	
		if (_rest == null)
		{
			_rest = new WWW(_address + NetworkCommandNames[(int)NetworkCommands.GetRankById] + id);
			_caller = caller;
			failed = false;
		}
		
		return failed;
	}
	
	public bool GetRankByName(string username, GameObject caller)
	{
		bool failed = true;
	
		if (_rest == null)
		{
			_rest = new WWW(_address + NetworkCommandNames[(int)NetworkCommands.GetRankByName] + username);
			_caller = caller;
			failed = false;
		}
		
		return failed;
	}
	
	public bool SetScore(int score, GameObject caller)
	{
		bool failed = true;
	
		if (_rest == null)
		{
			_rest = new WWW(_address + NetworkCommandNames[(int)NetworkCommands.SetScore] + score.ToString());
			_caller = caller;
			failed = false;
		}
		
		return failed;
	}
	
	public bool GetRanking(GameObject caller)
	{
		bool failed = true;
	
		if (_rest == null)
		{
			_rest = new WWW(_address + NetworkCommandNames[(int)NetworkCommands.GetRanking]);
			_caller = caller;
			failed = false;
		}
		
		return failed;
	}
}
