using UnityEngine;
using System.Collections;

public class AS_AudioPlayAnim : MonoBehaviour {

	void PlayAudio () 
	{
		GetComponent<AudioSource>().Play ();
	}
}
