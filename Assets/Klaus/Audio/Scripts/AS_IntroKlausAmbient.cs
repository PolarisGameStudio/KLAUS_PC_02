using UnityEngine;
using System.Collections;

public class AS_IntroKlausAmbient : MonoBehaviour {

	public float speedUP;
    public float speedDown;
    private float vol;
	public float maxVol;
    private int count;
	public int counts;
    // Use this for initialization
    void Awake () 
	{
		DontDestroyOnLoad(transform.gameObject);
        StartCoroutine("Up");
        count = 0;
    }
	public void OnLevelWasLoaded()
	{
		if(this.gameObject.transform.name == "AS_IntroAmbient")
		{
			maxVol = 0.2f;
		}
		if(this.gameObject.transform.name == "AS_PrincipalMenuMusik")
		{
			maxVol = GameObject.Find ("AS_MusikManager_Arrecho").GetComponent<AudioSource>().volume;
		}
        count += 1;
		if (count == counts)
        {
            StopCoroutine("Up");
            vol = GetComponent<AudioSource>().volume;
            StartCoroutine("Down");
        }
	}
	public void DownLive ()
	{
		StopCoroutine ("Up");
		vol = GetComponent<AudioSource>().volume;
		StartCoroutine ("Down1");
	}
	IEnumerator Up()
	{
		for(float f = 0; f < maxVol; f+=speedUP*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = f;
			yield return null;
		}
	}
	IEnumerator Down ()
	{
		for(float g = vol; g >= 0f; g -=speedDown*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = g;
			yield return null;
			
		}
		Destroy(transform.gameObject);
	}
	IEnumerator Down1 ()
	{
		for(float g = vol; g >= 0f; g -=speedDown*Time.deltaTime)
		{
			GetComponent<AudioSource>().volume = g;
			yield return null;
			
		}
	}
}
