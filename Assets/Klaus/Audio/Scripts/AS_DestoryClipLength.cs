using UnityEngine;
using System.Collections;

public class AS_DestoryClipLength : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		Destroy (transform.gameObject, GetComponent<AudioSource>().clip.length);
	}
}
