using UnityEngine;
using System.Collections;

public class AS_ListenerVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioListener.volume = 1;
        GameObject aud = GameObject.Find("AS_MusikManager");
        if(aud != null){
		aud.GetComponent<AudioSource>().volume = 0.6f;
        }
	}
	
	// Update is called once per frame
	void Update () {
		//AudioListener.volume = 1;
	}
}
