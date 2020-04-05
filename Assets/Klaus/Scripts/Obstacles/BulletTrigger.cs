//
// BulletTrigger.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class BulletTrigger : BulletBaseTrigger
{

    public BulletAnimator anim;
    public BulletHandler handler;


    bool initialPositionSetted;
    Vector3 initialPosition;

    public override void HandlerDestroy()
    {
        anim.DestroyAnim();
        handler.StopBullet();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (handler.transform == transform)
            return;

        if (!initialPositionSetted) {
            initialPosition = transform.localPosition;
            initialPositionSetted = true;
        }

        transform.localPosition = initialPosition;
    }
}

