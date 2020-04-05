//
// PickTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class PickTrigger : MonoBehaviour
{
	public GameObject spikeDeathSFX;
    public bool isElectricKill;

    // Use this for initialization
    void  OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Collision");
        if (other.CompareTag("Player"))
        {
            if (isElectricKill)
                other.GetComponent<DeadState>().typeOfDead = DeadType.Ray;
            
			other.GetComponent<MoveState>().Kill(spikeDeathSFX);
        } else
        {
            KillObject obj = other.GetComponent<KillObject>();
            if (obj != null)
            {
                obj.Kill();
            }
        }
    }
}
