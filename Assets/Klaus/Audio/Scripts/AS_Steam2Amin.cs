using UnityEngine;
using System.Collections;

public class AS_Steam2Amin : MonoBehaviour {

	// Use this for initialization
	public float vol;
	void Start()
	{
		vol = GetComponent<AudioSource>().volume;
	}
	void OnBecameInvisible()
	{
		Invoke ("volDown",GetComponent<AudioSource>().clip.length);
	}
	void OnBecameVisible()
	{
		GetComponent<AudioSource>().volume = vol;
	}
	void PlayAudio2 () {
		GetComponent<AudioSource>().Play();
		Debug.Log ("played2");
	}
	void volDown()
	{
		GetComponent<AudioSource>().volume = 0;
	}

}
