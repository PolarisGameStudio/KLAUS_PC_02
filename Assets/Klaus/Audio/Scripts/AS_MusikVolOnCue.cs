using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class AS_MusikVolOnCue : MonoBehaviour {

	public float speed;
	private float vol;
	public float secs;
	public float ShakeTime = 0.5f;
	private AudioSource AS_Target;
	private AudioListener AL_Klaus;
	private AudioListener AL_K1;
	private AudioSource _audio;
	public AudioSource audio {
		get {
			if (_audio == null)
				_audio = GetComponent<AudioSource>();
			return _audio;
		}
	}
	void  OnTriggerEnter2D(Collider2D other) { 
		return;
		if(other.CompareTag("Player"))
		{
			if(GameObject.Find ("AS_MusikManager_Arrecho") != null)//para la musica
			{
				AS_Target = GameObject.Find ("AS_MusikManager_Arrecho").GetComponent<AudioSource>();
				vol = AS_Target.volume;
				StartCoroutine ("Down");
				StartCoroutine ("DownAL");
				Invoke ("AudioShake", ShakeTime);
			}
		}
	}
	//MUSIK
	IEnumerator Down()
	{
		for (float g = vol; g >= 0f; g -= speed * Time.deltaTime)//musik
		{
			AS_Target.volume = g;
			yield return null;
			
		}
		Invoke ("UP",secs);
	}
	void UP ()
	{
		//vol = AS_Target.volume;
		StartCoroutine ("Up");
	}
	IEnumerator Up()
	{
		for (float f = 0; f < vol; f += speed * Time.deltaTime)
		{
			AS_Target.volume = f;
			yield return null;
		}
	}
	//AMBIENTE
	IEnumerator DownAL()
	{
		for (float g = 1; g >= 0f; g -= speed * Time.deltaTime)//musik
		{
			AudioListener.volume = g;
			yield return null;	
		}
		Invoke ("UPAL",secs);
	}
	void UPAL ()
	{
		StartCoroutine ("UpAL");
	}
	IEnumerator UpAL()
	{
		for (float f = 0; f < 1; f += speed * Time.deltaTime)
		{
			AudioListener.volume = f;
			yield return null;
		}
	}
	void AudioShake()
	{
		if (audio != null && audio.enabled)
		{
			
			#if UNITY_PS4 && !(UNITY_EDITOR)
			
			audio.PlayOnDualShock4(PS4Input.PadGetUsersDetails(0).userId);
			#else
			audio.Play();
			#endif
		}
	}
}
