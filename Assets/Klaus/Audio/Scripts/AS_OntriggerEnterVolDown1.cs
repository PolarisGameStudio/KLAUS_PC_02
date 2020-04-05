using UnityEngine;
using System.Collections;

public class AS_OntriggerEnterVolDown1 : MonoBehaviour 
{
	public bool enter = false;
	public float vol;
	public float speedUp = 0.015f;
	public float speedDown = 0.03f;
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player"))
        {
            StopCoroutine("VolDown");
            vol = GetComponent<AudioSource>().volume;
            StartCoroutine("VolUp");
		}
	}
	void  OnTriggerExit2D(Collider2D other) {        
		if(other.CompareTag("Player")){

            StopCoroutine("VolUp");
            vol = GetComponent<AudioSource>().volume;
            StartCoroutine("VolDown");
		}
	}
	IEnumerator VolUp()
	{
		for(float f = vol; f <= 1; f+=speedUp*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = f;
            yield return null;
        }
	}
	IEnumerator VolDown ()
	{
		for(float g = vol; g >= 0f; g -=speedDown * Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = g;
            yield return null;

        }
		
	}
}
