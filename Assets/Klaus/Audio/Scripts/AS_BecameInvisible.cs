using UnityEngine;
using System.Collections;

public class AS_BecameInvisible : MonoBehaviour {

	public float vol;
	void Start()
	{
		vol = GetComponent<AudioSource>().volume;
	}
	void OnBecameInvisible()
	{
		if(!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().volume = 0;
		}
	}
	void OnBecameVisible()
	{
		GetComponent<AudioSource>().volume = vol;
	}
}
