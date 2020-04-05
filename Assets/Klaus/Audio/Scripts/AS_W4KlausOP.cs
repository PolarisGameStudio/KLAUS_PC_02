using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AS_W4KlausOP : MonoBehaviour {

	public AudioMixer mainMixer;
	// Use this for initialization
	void Awake () 
	{
		mainMixer.FindSnapshot("DownMusik").TransitionTo(2*Time.deltaTime);
	}
}
