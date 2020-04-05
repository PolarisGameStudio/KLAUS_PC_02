using UnityEngine;
using System.Collections;

public class AS_AnimRndClip : MonoBehaviour {

	public AudioClip[] SFX;
	public float minPitch;
	public float maxPitch;
	AudioSource _audio;
	public AudioSource audio
	{
		get
		{
			if (_audio == null)
				_audio = GetComponent<AudioSource>();
			return _audio;
		}
	}
	void PlayRnd()
	{
		audio.clip = SFX[Random.Range(0, SFX.Length)];
		audio.Play();
	}
	void PitchRnd()
	{
		audio.pitch = Random.Range(minPitch, maxPitch);
	}
}
