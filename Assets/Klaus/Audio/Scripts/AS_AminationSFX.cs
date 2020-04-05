using UnityEngine;
using System.Collections;

public class AS_AminationSFX : MonoBehaviour {

	public float secs;
	public float muestra;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//animation["HumoTubo02_HD"].time = muestra;
		if(GetComponent<Animation>()["HumoTubo02_HD"].time == secs)
		{
			GetComponent<AudioSource>().Play ();
		}
	}
}
