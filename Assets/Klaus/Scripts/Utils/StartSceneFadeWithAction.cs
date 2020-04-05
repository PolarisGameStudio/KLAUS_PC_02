//
// StartSceneFade.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class StartSceneFadeWithAction : MonoBehaviour
{
    public bool DestroyGameObject = true;
    public float time = 1.0f;

    public AutomaticLoadScene function;
    // Use this for initialization
    void Awake()
    {
        Debug.Log("There should be a fade");
        CameraFade.StartAlphaFade(Color.black, true, time, 0);
        CameraFade.Instance.m_OnFadeFinish += DestroyFade;
    }

    void OnDestroy()
    {
        if (!CameraFade.IsInstanceNull())
            CameraFade.Instance.m_OnFadeFinish -= DestroyFade;
    }

    void DestroyFade()
    {
        if (DestroyGameObject)
        {
            Destroy(gameObject);
        }
        else
        {

            this.enabled = false;
        }
        if (function != null)
            function.enabled = true;
    }

}

