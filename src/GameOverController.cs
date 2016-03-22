using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour
{
	public UnityEngine.UI.Text _result;
	public UnityEngine.UI.Text _score;
	
	void Start()
	{
		int w = PlayerPrefs.GetInt("Win");
		
		if (w == 1)
		{
			_result.text = "You won!";
		}
		else
		{
			_result.text = "You lost...";
		}
		
		string m = PlayerPrefs.GetString("Difficulty");
		
		if (m.Contains("TimeAttack"))
		{
			int sc = PlayerPrefs.GetInt("TimeAttackScore");
			_score.text = sc.ToString() + " points";
		}
	}
	
	public void OnPlay()
	{
        SceneManager.LoadScene("scene");
	}
	
	public void OnMenu()
	{
        SceneManager.LoadScene("menu");
	}
}
