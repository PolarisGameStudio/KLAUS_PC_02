using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AS_MixerCollectOUT : MonoBehaviour {

	public AudioMixer mainMixer;

	void Start () 
	{
		mainMixer.FindSnapshot("CollectOUT").TransitionTo(0.1f * Time.deltaTime);
	}
}
