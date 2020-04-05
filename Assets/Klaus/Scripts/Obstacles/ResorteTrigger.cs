//
// ResorteTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResorteTrigger : MonoBehaviour
{

    public Vector2 Force;
    public float ForceYWhenOnlyX = 200.0f;
    public float TimeNotInput = 0.0f;
    public bool played = false;
    public Animator anim;
    

    public string animationName;
    protected Dictionary<int, bool> isUsed = new Dictionary<int, bool>();
    public Action<Vector2> ForceCallback;
    private AudioSource _audio;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    void ApplyForce(Collider2D other)
    {
        IForceObject forceObj = other.GetComponent<IForceObject>();
        if (forceObj != null)
        {
            int id = other.gameObject.GetInstanceID();
            if (!isUsed.ContainsKey(id))
            {
                isUsed [id] = false;
            }
            if (!isUsed [id])
            {
                float yValue = transform.up.y;
                if ((yValue > -0.09f) && (yValue < 0.09f))
                {
                    yValue = 0;
                }
                float xValue = transform.up.x;
                if ((xValue > -0.09f) && (xValue < 0.09f))
                {
                    xValue = 0;
                }
                Vector2 dir = new Vector2(xValue, yValue);
               
                if (dir.x != 0 && dir.y == 0)
                {
                    isUsed [id] = forceObj.ApplyForce(dir.normalized, Force.x, ForceYWhenOnlyX, true, TimeNotInput);
                } else
                {
                    isUsed [id] = forceObj.ApplyForce(dir.normalized, Force.x, Force.y, true, TimeNotInput);
                }
                if (isUsed [id])
                {
                    anim.SetBool(animationName, true);
                    audio.Stop();
                    audio.Play();
                }
                if (ForceCallback != null)
                {
                    ForceCallback(dir.normalized);
                }
                //  isUsed [id] = true;
            } 

            //Fix Loco 
            else
            {
                isUsed [id] = false;
                anim.SetBool(animationName, false);
            }
        }
    }

    void  OnTriggerEnter2D(Collider2D other)
    {
        ApplyForce(other);
    }

    void  OnTriggerStay2D(Collider2D other)
    {
        ApplyForce(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {

        int id = other.gameObject.GetInstanceID();

        if (isUsed.ContainsKey(id))
        {

            if (isUsed [id])
            {
                isUsed [id] = false;
                anim.SetBool(animationName, false);
            }
        }
    }
	
}
