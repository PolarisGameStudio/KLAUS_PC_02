//
// KeyTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class KeyTrigger : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rigid;
    public Collider2D keycollider;
    public MonoBehaviour KeyScript;
    public GameObject keySFX;
    bool isUsed = false;
    bool usedByAi = false;

    void Awake()
    {
        anim.GetBehaviour<KeyGotBehaviour>().keyT = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUsed && other.CompareTag("Player") && !usedByAi)
        {

            KeyChain.Instance.AddKey(other.GetComponent<PlayerInfo>().playerType);
            keySFX.Spawn(transform.position, transform.rotation);
            rigid.bodyType= RigidbodyType2D.Static;
            OpenDoor();
        } else if (!isUsed && other.CompareTag("Enemy") && !usedByAi)
        {
            if (other.GetComponent<AI_KeyChain>().SetCurrentKey(this))
            {
                keySFX.Spawn(transform.position, transform.rotation);
                usedByAi = true;
            }

        }
    }

    public void ResetAI()
    {
        usedByAi = false;
    }

    public void OpenDoor()
    {
        ResetAI();
        isUsed = true;
        rigid.isKinematic = true;
        keycollider.enabled = false;
        KeyScript.enabled = false;
        anim.SetTrigger("Got");
        // transform.parent.gameObject.SetActive(false);
    }

    public void Got()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
