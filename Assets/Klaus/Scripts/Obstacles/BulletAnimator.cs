//
// BulletAnimator.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class BulletAnimator : MonoBehaviour
{
	protected Animator anim;
    public BulletHandler handler;
    bool firstTime = true;
	void Awake(){
	
		anim = GetComponent<Animator>();
	}
    void OnEnable()
    {
        if (!firstTime)
        {
            if (anim) anim.SetBool("Destroy", false);
        }
        else
        {
            firstTime = false;
        }
    }
	public void DestroyAnim(){
		if (anim) anim.SetBool("Destroy",true);
        StartCoroutine(DestroyGameObject());
	}
	IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(0.1f);
        handler.Recycle();
	}
}
