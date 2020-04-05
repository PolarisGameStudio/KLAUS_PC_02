using UnityEngine;
using System.Collections;

public class AS_OntriggerExitPlay : MonoBehaviour {

	[Header("Components")]
	public GameObject targetObj;
	public AudioSource audioSource;

	void Start()
	{
		audioSource = targetObj.GetComponent<AudioSource>();
	}
	void  OnTriggerExit2D(Collider2D other) 
	{        
		if(other.CompareTag("Player"))
		{
			audioSource.Play ();
		}
	}
}
