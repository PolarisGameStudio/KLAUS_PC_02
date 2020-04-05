using UnityEngine;
using System.Collections;

public class AS_AuxAudioListener : MonoBehaviour {

	public GameObject klausListener;
	public AudioListener mAudioListener;

	// Use this for initialization
	void Start () {

        klausListener = GameObject.Find("Klaus");

    }
	
	// Update is called once per frame
	void Update () 
	{
		if (klausListener.activeInHierarchy == false)
		{
			mAudioListener.enabled = true;
		}
		
	}
}
