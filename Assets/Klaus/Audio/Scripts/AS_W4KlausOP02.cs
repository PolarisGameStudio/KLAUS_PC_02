using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AS_W4KlausOP02 : MonoBehaviour {

	public AudioMixer mainMixer;
	// Use this for initialization
	void Awake () 
	{
		mainMixer.FindSnapshot("Unpaused").TransitionTo(4*Time.deltaTime);
	}
}
