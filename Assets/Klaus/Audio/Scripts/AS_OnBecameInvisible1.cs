using UnityEngine;
using System.Collections;

public class AS_OnBecameInvisible1 : MonoBehaviour
{
	
	public AudioSource AS_Target;
	public float speed;
	public float vol;

	void OnBecameInvisible()
	{
		if (AS_Target == null)
			return;
		
		if (gameObject.activeInHierarchy)
		{
			StopCoroutine("Up");
			vol = AS_Target.volume;
			StartCoroutine("Down");
		}
	}
	void OnBecameVisible()
	{
		if (AS_Target == null)
			return;
		
		if (gameObject.activeInHierarchy)
		{
			StopCoroutine("Down");
			vol = AS_Target.volume;
			StartCoroutine("Up");
		}
	}
	IEnumerator Up()
	{
		for (float f = vol; f < 1; f += speed * Time.deltaTime)
		{
			AS_Target.volume = f;
			yield return null;
		}
	}
	IEnumerator Down()
	{
		for (float g = vol; g >= 0f; g -= speed * Time.deltaTime)
		{
			AS_Target.volume = g;
			yield return null;
				
		}
	}
}
