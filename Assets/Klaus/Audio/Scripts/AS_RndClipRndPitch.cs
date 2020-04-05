using UnityEngine;
using System.Collections;

public class AS_RndClipRndPitch : MonoBehaviour {

    public AudioClip[] SFX;
    public int delay;
    public float minPitch;
    public float maxPitch;
	public bool pitchRND = true;
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
    void Start()
    {
        InvokeRepeating("PlayRnd", 1, Random.Range(1, delay));
    }
    void PlayRnd()
    {
		if(pitchRND)
		{
        	Invoke("PitchRnd",delay);
		}
        audio.clip = SFX[Random.Range(0, SFX.Length)];
        audio.Play();
    }
    void PitchRnd()
    {
        audio.pitch = Random.Range(minPitch, maxPitch);
    }
}
