using UnityEngine;
using System.Collections;

public class AS_isGround : MonoBehaviour {

	public AudioSource groundSFX;
	public IsGroundTool groundTool;
	public int count;
	void Awake ()
	{
		groundTool.GroundAction += PlayGround;
	}
	void PlayGround()
	{
		groundSFX.Play ();
	}
}
