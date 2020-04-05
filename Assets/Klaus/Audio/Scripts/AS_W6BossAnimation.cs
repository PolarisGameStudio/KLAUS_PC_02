using UnityEngine;
using System.Collections;

public class AS_W6BossAnimation : Singleton<AS_W6BossAnimation> {

	public AudioSource audio1;
	public AudioSource audio2;
	public AudioSource audio3;
	public AudioClip[] SFXS;
	public float maxVol = 0.3f;
	public bool bendDown = true;

	public void ChainDownSFX()
	{
		audio1.Play ();
		StartCoroutine("VolUp",audio1);
	}
	public void ChainSTOPSFX()
	{
		audio1.Stop ();
	}
	public void ChainUpSFX()
	{
		audio1.Play ();
		StartCoroutine("VolDown",audio1);
	}
	public void BendLeftDownSFX()
	{
		if(bendDown){
		audio2.clip = SFXS[0];
		audio2.Play ();
		StartCoroutine("PanLeft",audio2);
		}
	}
	public void BendLeftUpSFX()
	{
		if(!bendDown){
		audio2.clip = SFXS[0];
		audio2.Play ();
		StartCoroutine("PanCenterFL",audio2);
		}
	}
	public void BendRightDownSFX()
	{
		if(bendDown){
		audio1.clip = SFXS[0];
		audio1.Play ();
		StartCoroutine("PanRight",audio2);
		}
	}
	public void BendRightUpSFX()
	{
		if(!bendDown){
		audio1.clip = SFXS[0];
		audio1.Play ();
		StartCoroutine("PanCenterFR",audio2);
		}
	}
	public void EyeLaserSFX()
	{
		audio1.clip = SFXS[1];
		audio1.volume = 1;
		audio1.loop = false;
		audio1.Play ();
		// Debug.Log ("EyeLaserSFX");
	}
	public void EyeLaser2SFX()
	{
		audio1.clip = SFXS[10];
		audio1.Play ();
		// Debug.Log ("EyeLaser2SFX");
	}
	public void CannonsSFX()
	{
		audio2.clip = SFXS[3];
		audio2.Play ();
		// Debug.Log ("CannonsSFX");
	}
	public void MouthLaserSFX()
	{
		audio1.clip = SFXS[2];
		audio1.Play ();
		// Debug.Log ("MouthLaserSFX");
	}
	public void TrollSFX()
	{
		audio3.clip = SFXS[4];
		audio3.Play ();
		// Debug.Log ("TrollSFX");
	}
	public void LiftArmsSFX()
	{
		audio2.clip = SFXS[5];
		audio2.Play ();
		// Debug.Log ("LiftArmsSFX");
	}
	public void PunchSFX()
	{
		audio1.clip = SFXS[6];
		audio1.Play ();
		// Debug.Log ("PunchSFX");
	}
	public void LooseHandsSFX()
	{
		audio3.clip = SFXS[7];
		audio3.Play ();
		// Debug.Log ("LooseHandsSFX");
	}
	public void RecoverHandsSFX()
	{
		audio3.clip = SFXS[8];
		audio3.Play ();
		// Debug.Log ("RecoverHandsSFX");
	}
	public void TakeHitSFX()
	{
		audio2.clip = SFXS[9];
		audio2.Play ();
		// Debug.Log ("TakeHit");
	}
	public void isBendDownSFX()
	{
		bendDown = false;
	}
	public void isBendUpSFX()
	{
		bendDown = true;
	}
	public void ResetSFX()
	{
		audio1.Stop ();
		audio2.Stop ();
	}
	IEnumerator PanCenterFL(AudioSource audio)
	{
		for (float i = -1f; i < 0; i += 0.9f*Time.deltaTime)
		{
			audio.panStereo = i;
			yield return null;
		}
	}
	IEnumerator PanCenterFR(AudioSource audio)
	{
		for (float i = 1f; i > 0; i -= 0.9f*Time.deltaTime)
		{
			audio.panStereo = i;
			yield return null;
		}
	}
	IEnumerator PanLeft(AudioSource audio)
	{
		for (float i = 0f; i > -1; i -= 0.8f*Time.deltaTime)
		{
			audio.panStereo = i;
			yield return null;
		}
	}
	IEnumerator PanRight(AudioSource audio)
	{
		for (float i = 0f; i < 1; i += 0.8f*Time.deltaTime)
		{
			audio.panStereo = i;
			yield return null;
		}
		audio.panStereo = 0;
	}
	IEnumerator VolUp(AudioSource audio)//CHAIN
	{
		for (float i = 0f; i < maxVol; i += 0.25f*Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
		}
	}
	IEnumerator VolDown(AudioSource audio)//CHAIN
	{
		for (float i = maxVol; i > 0f; i -= 0.15f*Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
		}
	}
}
