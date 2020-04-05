using UnityEngine;
using System.Collections;

public class AS_K1Anim : MonoBehaviour {

    public GameObject ladderStepA;
    public GameObject ladderStepB;
    public GameObject wallJump;
	public AudioClip idleCapeSFX;
	public AudioClip idle2SFX;
	public AudioSource audio1;
    // Use this for initialization
    void Start()
    {
        ladderStepA.CreatePool(1);
        ladderStepB.CreatePool(1);
        wallJump.CreatePool(1);
    }
    void StepA()
    {
        ladderStepA.Spawn(transform.position, transform.rotation);
    }
    void StepB()
    {
        ladderStepB.Spawn(transform.position, transform.rotation);
    }
    void WallJump()
    {
        wallJump.Spawn(transform.position, transform.rotation);
    }
	public void IdleCapeSFX()
	{
		audio1.clip = idleCapeSFX;
		audio1.Play ();
	}
	public void Idle2SFX()
	{
		audio1.clip = idle2SFX;
		audio1.Play ();
	}
}
