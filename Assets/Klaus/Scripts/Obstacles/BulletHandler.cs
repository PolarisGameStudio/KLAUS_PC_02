//
// BulletHandler.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct BulletInfo
{
    // variables
    public Vector2 direction;
    public float timeLive;


    public BulletInfo(Vector2 p1, float p2)
    {
        direction = p1;
        timeLive = p2;
    }
}

public class BulletHandler : MonoBehaviour
{
    public ParticleSystem sparks;
    public float maxSpeed = 5.0f;
    protected Vector2 direction = Vector2.zero;
    bool firstRun = true;
    private Rigidbody2D _rigid;

    public Rigidbody2D rigidBody2D
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponentInChildren<Rigidbody2D>();
            }
            return _rigid;
        }
    }

    private BulletBaseTrigger _trigger;

    public BulletBaseTrigger trigger
    {
        get
        {
            if (_trigger == null)
            {
                _trigger = GetComponentInChildren<BulletBaseTrigger>();
            }
            return _trigger;
        }
    }

    public bool destroyWithAnimation;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
        Debug.Log("this is StarBullet");
    }

    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    void OnDisable()
    {
        Debug.Log("this is Disable");
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    IEnumerator DestroyBullet(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));

        if (destroyWithAnimation && trigger != null)
            trigger.HandlerDestroy();
        else
            this.Recycle();
    }

    public void StopBullet()
    {
        if (sparks != null)
        {
            sparks.Play();
        }
        Debug.Log("this is StopBullet");
        StopCoroutine("DestroyBullet");
        direction = Vector2.zero;
        rigidBody2D.velocity = direction * maxSpeed;
    }

    public virtual void SetDirection(BulletInfo dir)
    {
        Debug.Log("this is SetDirection");

        direction = dir.direction;
        StartCoroutine("DestroyBullet", dir.timeLive);
        rigidBody2D.velocity = direction * maxSpeed;
    }

    #region Pause:

    Vector2 velPause;
    bool kinematic;

    public void OnPauseGame()
    {
        velPause = rigidBody2D.velocity;
        kinematic = rigidBody2D.isKinematic;

        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.isKinematic = true;
    }

    public void OnResumeGame()
    {
        rigidBody2D.isKinematic = kinematic;
        rigidBody2D.velocity = velPause;
    }

    #endregion
}
