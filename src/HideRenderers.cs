using UnityEngine;
using System.Collections;

public class HideRenderers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
		
		for (int i = 0; i < spr.Length; i++)
		{
			spr[i].enabled = false;
		}
	}
}
