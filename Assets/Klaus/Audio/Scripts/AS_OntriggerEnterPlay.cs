using UnityEngine;
using System.Collections;

public class AS_OntriggerEnterPlay : MonoBehaviour {

	public bool played = false;
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player")){

			if(!played)
			{
				played = true;
				GetComponent<AudioSource>().Stop ();
				GetComponent<AudioSource>().Play ();
			}
		}
	}
}
