using UnityEngine;
using System.Collections;

public class AS_KlausAnim : MonoBehaviour {

	public GameObject ladderStepA;
	public GameObject ladderStepB;
    public GameObject wallJump;
	// Use this for initialization
	void Start () 
	{
		ladderStepA.CreatePool (1);
		ladderStepB.CreatePool (1);
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
}
