using UnityEngine;
using System.Collections;

public class AS_Steam1Amin : MonoBehaviour {

	// Use this for initialization
	public float vol;
	public bool visible;
	private float speed = 1f;
	void Start()
	{	
		if (!visible)
		{
			GetComponent<AudioSource>().volume =0;
		}else{

			GetComponent<AudioSource>().volume = vol;
		}
	}
	void OnBecameInvisible()
	{
		visible = false;
		//audio.volume = 0;
	}
	void OnBecameVisible()
	{
		visible = true;
		//audio.volume = vol;
	}
	void PlayAudio () 
	{
		GetComponent<AudioSource>().Play ();
	}
	void Update ()
	{
		if(visible)
		{
            if (GetComponent<AudioSource>().volume < vol)
			    GetComponent<AudioSource>().volume += speed*Time.deltaTime;

		}else{
            if (GetComponent<AudioSource>().volume > 0)
				GetComponent<AudioSource>().volume -= speed*Time.deltaTime;//esto esta mal
		}
	}
}
