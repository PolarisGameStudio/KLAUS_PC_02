using UnityEngine;
using System.Collections;

public class AS_IgnoreALV : MonoBehaviour {

	public AudioSource audio1;
	// Use this for initialization
	void Awake()
	{
		audio1.ignoreListenerVolume = true;
		audio1.Play ();
	}
}
