using UnityEngine;
using System.Collections;

public class AS_PlayDelayed : MonoBehaviour {

	public float delay;
	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().PlayDelayed(delay);
	}
}
