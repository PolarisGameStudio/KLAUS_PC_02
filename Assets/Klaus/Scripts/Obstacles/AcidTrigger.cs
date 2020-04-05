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

public class AcidTrigger : MonoBehaviour
{
    public GameObject acidDeathSFX;
    // Use this for initialization
    void  OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<DeadState>().typeOfDead = DeadType.Acid;
            other.GetComponent<MoveState>().Kill(acidDeathSFX);
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
