//
// LockTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class LockTrigger : MonoBehaviour
{

    public int numberOfKey = 1;
    public GameObject lockSFX;
    protected bool unLocked = false;

    public Animator doorAnim;

    void  OnTriggerEnter2D(Collider2D other)
    {
        if (!unLocked)
        {
            if (other.CompareTag("Player"))
            {

                if (KeyChain.Instance.useKey(other.GetComponent<PlayerInfo>().playerType, numberOfKey))
                {
                    unlockDoor();
                    lockSFX.Spawn(transform.position, transform.rotation);
                }
                
            } else if (other.CompareTag("Enemy"))
            {
                AI_KeyChain chainAI = other.GetComponent<AI_KeyChain>();
                if (chainAI.hasKey)
                {
                    chainAI.OpenDoor();
                    unlockDoor();
                    lockSFX.Spawn(transform.position, transform.rotation);
                }
            }
        }
    }

    public void unlockDoor()
    {  
        unLocked = true;
        doorAnim.SetTrigger("Destroy");
        Invoke("DeActiveObject", 0.25f);
    }

    void DeActiveObject()
    {
        gameObject.SetActive(false);

    }

}
