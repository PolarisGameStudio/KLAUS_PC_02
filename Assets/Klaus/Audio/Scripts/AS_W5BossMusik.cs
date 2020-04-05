using UnityEngine;
using System.Collections;

public class AS_W5BossMusik : Singleton<AS_W5BossMusik> 
{

	public AudioSource audio1;
	public AudioSource audio2;
	public float vol;

	void Start()
	{
		audio1.time = Random.Range (30.000f,200.000f);
		//audio1.volume = SaveManager.Instance.dataKlaus.fxVolume;
		audio1.Play ();
	}
	public void K1Musik()
	{
		vol = audio1.volume;
		StartCoroutine ("VolDown",audio1);
		audio2.volume = SaveManager.Instance.dataKlaus.bgVolume;
		//audio2.PlayDelayed (4f*Time.deltaTime);
		audio2.Play();

	}
	public void StopK1Musik()
	{
		vol=audio2.volume;
		StartCoroutine ("VolDown",audio2);
	}

	IEnumerator VolDown(AudioSource audio)
	{
		for (float i = vol; i >= 0f; i -= 0.5f * Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
		}
		audio.Stop ();
	}
}
