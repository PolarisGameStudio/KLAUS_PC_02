using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AS_KlausVideo : MonoBehaviour {

	public AudioMixer mainMixer;
	// Use this for initialization
	void Awake () 
	{
		mainMixer.FindSnapshot("LevelCompleted").TransitionTo(2*Time.deltaTime);
	}
}
