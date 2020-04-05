using UnityEngine;
using System.Collections;

public class AS_PlayOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GetComponent<AudioSource>().Play ();
	}
}
