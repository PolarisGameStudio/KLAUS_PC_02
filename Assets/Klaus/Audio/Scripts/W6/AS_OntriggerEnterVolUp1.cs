using UnityEngine;
using System.Collections;

public class AS_OntriggerEnterVolUp1: MonoBehaviour {

	private float value;
	[Header("Behavior")]
	public bool enterIsDown;
	public bool isPitch;
	public float speedUp = 0.015f;
	public float speedDown = 0.03f;
	public float max = 1;
	public float min =0;
	[Header("Components")] 
	public GameObject targetObj;
	public AudioSource audioSource;
	void Start()
	{
		audioSource = targetObj.GetComponent<AudioSource>();
	}
	void  OnTriggerEnter2D(Collider2D other) {        
		if(other.CompareTag("Player"))
		{
			if (enterIsDown)
			{
				StopCoroutine("VolUp");
				if(isPitch)
				{
					value = audioSource.pitch;
				}
				if(!isPitch)
				{
					value = audioSource.volume;
				}
				StartCoroutine("VolDown",min);
			}
			if(!enterIsDown)
			{
				StopCoroutine("VolDown");
				if(isPitch)
				{
					value = audioSource.pitch;
				}
				if(!isPitch)
				{
					value = audioSource.volume;
				}
				StartCoroutine("VolUp",max);
			}
		}
	}
	void  OnTriggerExit2D(Collider2D other) {        
		if(other.CompareTag("Player"))
		{
			if(enterIsDown)
			{
				StopCoroutine("VolDown");
				if(isPitch)
				{
					value = audioSource.pitch;
				}
				if(!isPitch)
				{
					value = audioSource.volume;
				}
				StartCoroutine("VolUp",max);
			}
			if(!enterIsDown)
			{
				StopCoroutine("VolUp");
				if(isPitch)
				{
					value = audioSource.pitch;
				}
				if(!isPitch)
				{
					value = audioSource.volume;
				}
				StartCoroutine("VolDown",min);
			}
		}
	}
	IEnumerator VolUp(float max)
	{
		for(float f = value; f <= max; f+=speedUp*Time.deltaTime)
		{
			if(isPitch)
			{
				audioSource.pitch = f;
			}
			if(!isPitch)
			{
				audioSource.volume = f;
			}
			yield return null;
		}
	}
	IEnumerator VolDown (float min)
	{
		for(float g = value; g >= min; g -=speedDown * Time.deltaTime)
		{
			if(isPitch)
			{
				audioSource.pitch = g;
			}
			if(!isPitch)
			{
				audioSource.volume = g;
			}
			yield return null;
			
		}
		
	}
}
