using UnityEngine;
using System.Collections;

public class AS_IgnoreListenerVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().ignoreListenerVolume = true;
	}
}
