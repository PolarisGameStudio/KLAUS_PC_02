using UnityEngine;
using System.Collections;

public class AS_Instantiate : MonoBehaviour {
	public GameObject sound;
	// Use this for initialization
	public void Instansea () {
		Instantiate(sound, transform.position, transform.rotation);
	}

}
