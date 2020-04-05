using UnityEngine;
using System.Collections;

public class AS_PrefinalMusik : MonoBehaviour {

	public bool adentro = false;
	public GameObject musikFinal;
	// Use this for initialization
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player")){
			
			adentro = true;
			Instantiate(musikFinal, transform.position, transform.rotation);
		}
	}
	void  OnTriggerExit2D(Collider2D other) {        
		if(other.CompareTag("Player")){
			
			adentro = false;
		}
	}
}
