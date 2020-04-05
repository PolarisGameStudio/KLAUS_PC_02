using UnityEngine;
using System.Collections;

public class AS_KeyAnim : MonoBehaviour {

	public AudioClip keyBreak;
	public AudioClip keyRe;
	public AudioClip keyGround;
	public AudioSource aSource;
	public IsGroundTool test;

	void Start ()
	{
		test.GroundAction += PlayGround;
        Invoke("UnMute", 5f);
	}
    void UnMute()
    {
        aSource.mute = false;
    }
	void PlayBreak () 
	{
		aSource.volume = 0.8f;
		aSource.pitch = 1.6f;
		aSource.clip = keyBreak;
		aSource.Play ();
	}
	void PlayRe () 
	{
		aSource.volume = 1f;
		aSource.pitch = 1f;
		aSource.clip = keyRe;
		aSource.Play ();
	}
	void PlayGround()
	{
		aSource.volume = 0.5f;
		aSource.pitch = 1.9f;
		aSource.clip = keyGround;
		aSource.Play ();
	}
}
