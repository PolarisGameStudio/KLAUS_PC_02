//
// RotateObject.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public float angleMaxSpeed = 200.0f;
    public bool showingInUI;
    bool isVisible = false;
    public bool suscribePause = true;
    bool pause = false;

    void FixedUpdate()
    {

        if (suscribePause)
            pause = ManagerPause.Pause;

        if (!pause && (isVisible || showingInUI))
            transform.Rotate(Vector3.forward, angleMaxSpeed * Time.fixedDeltaTime);
    }

    protected virtual void OnBecameInvisible()
    {
        isVisible = false;
    }
    protected virtual void OnBecameVisible()
    {
        isVisible = true;
    }

}
