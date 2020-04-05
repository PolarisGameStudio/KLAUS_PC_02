using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AS_VideoMixRouting : MonoBehaviour {

	// Use this for initialization
	public AudioSource audio1;
	public AudioMixerGroup mixerGroup;

	void Start () 
	{
		if(GetComponent<AudioSource>()!= null)
		{
			audio1 = GetComponent<AudioSource>();
			audio1.outputAudioMixerGroup = mixerGroup;
		}
	}
}
