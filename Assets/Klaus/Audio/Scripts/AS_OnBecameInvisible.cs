using UnityEngine;
using System.Collections;

public class AS_OnBecameInvisible : MonoBehaviour
{

    public AudioSource[] AS_Target;
    public float speed;
    public float[] vol;
	void Awake()
	{
		vol = new float[AS_Target.Length];
	}
    void OnBecameInvisible()
    {
        if (AS_Target == null)
            return;
		 
        if (gameObject.activeInHierarchy)
		{
			StopCoroutine("Up");
			for(int i = 0; i< AS_Target.Length; i++)
			{
				vol[i] = AS_Target[i].volume;
			}
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
			for(int i = 0; i< AS_Target.Length; i++)
			{
				vol[i] = AS_Target[i].volume;
			}
            StartCoroutine("Up");
		}
    }
    IEnumerator Up()
    {
		for(int i = 0; i< AS_Target.Length; i++)
		{
	        for (float f = vol[i]; f < 1; f += speed * Time.deltaTime)
	        {
	            AS_Target[i].volume = f;
	            yield return null;
	        }
		}
    }
    IEnumerator Down()
    {
		for(int i = 0; i< AS_Target.Length; i++)
		{
	        for (float g = vol[i]; g >= 0f; g -= speed * Time.deltaTime)
	        {
	            AS_Target[i].volume = g;
	            yield return null;

	        }
		}
    }
}
