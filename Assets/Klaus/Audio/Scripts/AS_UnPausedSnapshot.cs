using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class AS_UnPausedSnapshot : MonoBehaviour 
{
	
	public void UnpauseSnapshot () 
	{
		if(GameObject.Find("AS_MusikManager_Arrecho") != null)
		{
			GameObject.Find("AS_MusikManager_Arrecho").GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo (0.1f);
		}
	}
}
