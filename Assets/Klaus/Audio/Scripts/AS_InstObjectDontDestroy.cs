using UnityEngine;
using System.Collections;

public class AS_InstObjectDontDestroy : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		Destroy(gameObject,GetComponent<AudioSource>().clip.length);
	}
}
