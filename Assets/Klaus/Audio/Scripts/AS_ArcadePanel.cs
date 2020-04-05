using UnityEngine;
using System.Collections;

public class AS_ArcadePanel : MonoBehaviour {

	public AudioSource gAmbient;
	public AudioSource gAmbient2;
	public AudioSource MenuMusik;
	public float maxVol;
	public float vol;
	public float speedDown;
	public float speedUP;
	// Use this for initialization
	void Play () 
	{
		gAmbient.Play();
		gAmbient2.Play();
		maxVol = MenuMusik.GetComponent<AudioSource>().volume;
		vol = MenuMusik.GetComponent<AudioSource>().volume;
		StopCoroutine("Up");
		StartCoroutine("Down");

	}
	
	// Update is called once per frame
	void Stop () 
	{
		gAmbient.Stop();
		gAmbient2.Stop();
		StopCoroutine("Down");
		StartCoroutine("Up");
	}
	IEnumerator Down ()
	{
		for(float g = vol; g >= 0f; g -=speedDown*Time.deltaTime)
		{
			MenuMusik.GetComponent<AudioSource>().volume = g;
			yield return null;
			
		}
		//Destroy(transform.gameObject);
	}
	IEnumerator Up()
	{
		for(float f = 0; f < maxVol; f+=speedUP*Time.deltaTime)
		{
			MenuMusik.GetComponent<AudioSource>().volume = f;
			yield return null;
		}
	}
}
