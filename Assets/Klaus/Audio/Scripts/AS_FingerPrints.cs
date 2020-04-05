using UnityEngine;
using System.Collections;

public class AS_FingerPrints : MonoBehaviour {

	public float pitchUp;
	public float pitchDown;
	void RollUP () 
	{
		GetComponent<AudioSource>().pitch = pitchUp;
		GetComponent<AudioSource>().Play();
		Debug.Log("sube");
	
	}
	

	void RollDown () 
	{
		GetComponent<AudioSource>().pitch = pitchUp;
		GetComponent<AudioSource>().Play();
		Debug.Log("baja");
	}
}
