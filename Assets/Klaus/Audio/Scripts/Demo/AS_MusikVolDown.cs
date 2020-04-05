using UnityEngine;
using System.Collections;

public class AS_MusikVolDown : MonoBehaviour {

	public bool adentro = false;
	private GameObject aud;
	// Use this for initialization
	void Start () {
		aud = GameObject.Find("AS_MusikManager");
		//aud = GameObject.Find("AS_MusikManager");
	}
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player")){
			
			adentro = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(aud != null && aud.GetComponent<AudioSource>().volume > 0 && adentro){
			aud.GetComponent<AudioSource>().volume -= 0.003f;
		}
	}
}
