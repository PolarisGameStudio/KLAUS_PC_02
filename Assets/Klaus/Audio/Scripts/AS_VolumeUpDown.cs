using UnityEngine;
using System.Collections;

public class AS_VolumeUpDown : MonoBehaviour {
	
	public bool speed;
	public void VolumeUp () {
		
		StartCoroutine("Up");
	}

	public void VolumeDown () {
		
		StartCoroutine("Down");
	}
	IEnumerator Up()
	{
		for(float f = 0; f < 1; f+=1*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = f;
			yield return 0;
		}
	}
	IEnumerator Down ()
	{
		for(float g = 1; g >= 0f; g -=1*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = g;
			yield return 0;
			
		}
		
	}
}
