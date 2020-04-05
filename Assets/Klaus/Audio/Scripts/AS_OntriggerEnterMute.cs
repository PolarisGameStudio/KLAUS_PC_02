using UnityEngine;
using System.Collections;

public class AS_OntriggerEnterMute : MonoBehaviour {

	void  OnTriggerEnter2D(Collider2D other) {        
			if(other.CompareTag("Player")){
				
			GetComponent<AudioSource>().mute = false;
			}
		}
	void  OnTriggerExit2D(Collider2D other) {        
		if(other.CompareTag("Player")){
			
			GetComponent<AudioSource>().mute = true;
		}
	}

}
