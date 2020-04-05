using UnityEngine;
using System.Collections;

public class AS_EnableReverb : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GetComponent<AudioReverbZone>().enabled = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GetComponent<AudioReverbZone>().enabled = false;
		}
	}
}
