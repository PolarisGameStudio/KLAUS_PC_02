using UnityEngine;
using System.Collections;

public class AS_WnEnding : MonoBehaviour {

	public AudioSource audio1;
	public AudioClip tone;
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);
		audio1.Play ();
		Invoke ("ChangeClip",2.5f);
	}
	void ChangeClip()
	{
		audio1.clip = tone;
		audio1.Play ();
		Destroy (gameObject,audio1.clip.length);
	}
}
