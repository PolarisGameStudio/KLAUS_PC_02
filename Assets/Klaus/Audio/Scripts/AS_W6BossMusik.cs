using UnityEngine;
using System.Collections;

public class AS_W6BossMusik : Singleton<AS_W6BossMusik> 
{
	public float speed;
	public float vol;
	public AudioSource musikIntro;
	public AudioSource musikFight;
	public int cont = 0;

	public void OnLevelWasLoaded()
	{
		cont +=1;
		if(cont == 2)
		{
			Destroy (gameObject);
		}

	}
	public void MusikFight () 
	{
		musikFight.PlayDelayed (6f);
	}
	public void MusikIntro ()
	{
		StartCoroutine ("VolUp",musikIntro);
		musikIntro.Play();
	}
	public void MusikIntroDown()
	{
		vol = musikIntro.volume;
		StartCoroutine("VolDown",musikIntro);
	}
	public void MusikFightDown ()
	{
		vol = musikFight.volume;
		StartCoroutine("VolDown",musikFight);
	}
	IEnumerator VolUp(AudioSource audio)
	{
		for (float i = 0; i <= vol; i += speed * Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
			
		}
	}
	IEnumerator VolDown(AudioSource audio)
	{
		for (float i = vol; i >= 0f; i -= speed * Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
		}
		audio.Stop ();
	}
}
