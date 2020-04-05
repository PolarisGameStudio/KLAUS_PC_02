using UnityEngine;
using System.Collections;

public class AS_TestMusik : MonoBehaviour {

	public int world;
	public int track;
	public AudioClip[] w1;
	public AudioClip[] w2;
	public AudioClip[] w3;
	public AudioClip[] w4;
	public AudioClip[] w5;
	public AudioClip[] w6;
	                
	// Use this for initialization
	void Start () {

		switch (world)
		{
		case 1:
			GetComponent<AudioSource>().clip = w1[track];
			break;
		case 2:
			GetComponent<AudioSource>().clip = w2[track];
			break;
		case 3:
			GetComponent<AudioSource>().clip = w3[track];
			break;
		case 4:
			GetComponent<AudioSource>().clip = w4[track];
			break;
		case 5:
			GetComponent<AudioSource>().clip = w5[track];
			break;
		case 6:
			GetComponent<AudioSource>().clip = w6[track];
			break;
		}
		GetComponent<AudioSource>().Play();
	}
}
