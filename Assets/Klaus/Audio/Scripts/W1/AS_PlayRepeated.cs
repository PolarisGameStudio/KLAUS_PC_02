using UnityEngine;
using System.Collections;

public class AS_PlayRepeated : MonoBehaviour {
	public bool rndPitch;
	public float maxPitch;
	public float minPitch;
	public float delay;
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
	void OnParticleCollision (GameObject other)
	{
		audio.pitch = Random.Range(minPitch,maxPitch);
		audio.Play();
	}
	// Use this for initialization
	void Start () 
	{
		//InvokeRepeating ("PlayRepeated",0.0f,delay); 
	}

	void PlayRepeated () 
	{
		audio.pitch = Random.Range(minPitch,maxPitch);
		audio.Play();
	}
}
