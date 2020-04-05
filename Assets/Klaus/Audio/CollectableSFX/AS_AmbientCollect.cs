using UnityEngine;
using System.Collections;

public class AS_AmbientCollect : MonoBehaviour {

	public AudioClip[] SFX;
	public int delay;
	public AudioSource audio1;
	// Use this for initialization
	void Start()
	{
		InvokeRepeating("PlayRnd", 1, Random.Range(5, delay));
	}
	void PlayRnd()
	{
		Invoke("PitchRnd",0.1f);
		audio1.clip = SFX[Random.Range(0, SFX.Length)];
		audio1.Play();
	}
	void PitchRnd()
	{	
		if(audio1.panStereo == 1)
		{
			audio1.panStereo = -1;
		}else
		if(audio1.panStereo == -1)
		{
			audio1.panStereo = 1;
		}
	}
}
