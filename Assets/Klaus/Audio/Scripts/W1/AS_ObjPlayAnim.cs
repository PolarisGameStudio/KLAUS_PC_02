using UnityEngine;
using System.Collections;

public class AS_ObjPlayAnim : MonoBehaviour {

	public GameObject FXRaySFX;
	public AudioClip[] SFX;
	public void PlayObj ()
	{	
		FXRaySFX.GetComponent<AudioSource>().clip = SFX[Random.Range(0,SFX.Length)];
		FXRaySFX.GetComponent<AudioSource>().Play();
	}
}
