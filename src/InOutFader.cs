using UnityEngine;
using System.Collections;

public class InOutFader : MonoBehaviour
{
	public CanvasRenderer[] _renderers;
	
	public enum State : int
	{
		FadingIn = 0,
		FadingOut
	};
	
	private State _state = State.FadingOut;
	
	void Update ()
	{
		if (_state == State.FadingIn)
		{
			for (int i = 0; i < _renderers.Length; i++)
			{
				Color c = _renderers[i].GetColor();
				c.a += Time.deltaTime*0.5f;
				_renderers[i].SetColor(c);
				
				if (c.a > 1f)
				{
					_state = State.FadingOut;
				}
			}
		}
		else if (_state == State.FadingOut)
		{
			for (int i = 0; i < _renderers.Length; i++)
			{
				Color c = _renderers[i].GetColor();
				c.a -= Time.deltaTime*0.5f;
				_renderers[i].SetColor(c);
				
				if (c.a < 0.4f)
				{
					_state = State.FadingIn;
				}
			}
		}
	}
}
