using UnityEngine;
using System.Collections;

public class AS_W1Boss : Singleton<AS_W1Boss> {

	public AudioClip jump;
	public AudioClip punchMiddle;
	public AudioClip burp;
	public AudioClip laugh;
	public AudioClip evilLaugh;
	public AudioClip lowHit;
	public AudioClip lowHit2;
	public AudioClip dead;
	public AudioClip openArms;
	public AudioClip evilOpenArms;
	public AudioClip openCape;
	public AudioClip fly;
	public AudioClip k1Ground;
	public AudioClip[]laughs; 
	//public AudioClip walk;
	//public AudioClip run;
	public bool lastLaugh = false;
	public bool una = true;
	public bool played = false;
	public bool capeOpened = false;
	public AudioSource aSource;
	protected AudioSource _aud;
	
	public AudioSource audio
	{
		get
		{
			if (_aud == null)
				_aud = GetComponent<AudioSource>();
			return _aud;
		}
	}
	// Use this for initialization
	void Reset()
	{
		played = false;
	}
	void Jump () {
		audio.clip = jump;
		audio.Play();

	}
	// Update is called once per frame
	void PunchMiddle () {
		audio.loop=false;
		audio.spatialBlend = 0;
		audio.clip = punchMiddle;
		audio.Play();
	}
	void Burp(){
		audio.loop=false;
		audio.spatialBlend = 0;
		audio.clip = burp;
		audio.Play();
	}
	void Laugh(){
		audio.loop=false;
		audio.spatialBlend = 1;
		if(lastLaugh)
		{
			//audio.clip = evilLaugh;
			//audio.Play();
		}else
		{
			audio.clip = laugh;
			audio.Play();
		}

	}
	void Laughs()
	{
		if(!played)
		{
			played = true;
			audio.loop = false;
			audio.spatialBlend = 1;
			audio.clip = laughs[Random.Range(0,laughs.Length)];
			audio.Play();
		}
	}
	void LowHit(){
		audio.loop=false;
		audio.spatialBlend = 0;
		audio.clip = lowHit;
		audio.Play();
	}
	void LowHit2(){
		audio.loop=false;
		audio.spatialBlend = 0;
		audio.clip = lowHit2;
		audio.Play();
	}
	void Dead(){
		audio.loop=false;
		audio.spatialBlend = 1;
		audio.clip = dead;
		audio.Play();
	}
	void OpenArms(){
		audio.loop=false;
		audio.spatialBlend = 1;
		if(lastLaugh && una)
		{
			una = false;
			audio.clip = evilOpenArms;
			audio.Play();
			Debug.Log ("openarmsplayEVIL");
		}
		if(!lastLaugh)
		{
			audio.clip = openArms;
			audio.Play();
			Debug.Log ("openarmsplay");
		}


	}
	void OpenCape(){
		if(!capeOpened)
		{
			capeOpened = true;
			audio.loop=false;
			audio.spatialBlend = 1;
			audio.clip = openCape;
			audio.Play();
			aSource.clip = fly;
			aSource.pitch = 0.9f;
			aSource.volume = 0.3f;
			aSource.loop = true;
			aSource.Play ();
		}
	}
	void Step()
	{
		capeOpened = false;
		aSource.clip = k1Ground;
		aSource.loop = false;
		aSource.pitch = 1;
		aSource.volume = 1;
		aSource.Play ();
		aSource.PlayDelayed (2f);
	}
	public void StopSFX()
	{
		aSource.Stop ();
	}
	public void LastLaugh()
	{
		lastLaugh = true;
		played = true;
	}
	/*void Run(){
		audio.loop=true;
		audio.clip = run;
		audio.Play();
	}
	void Walk(){
		audio.loop=true;
		audio.clip = walk;
		audio.Play();
	}*/

}
