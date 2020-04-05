using UnityEngine;
using System.Collections;

public class AS_PlaySpecificAudioSource : MonoBehaviour {

	public AudioSource audioSource;
	public float delay;
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player")){

			audioSource.PlayDelayed (delay);
		}
	}
}
