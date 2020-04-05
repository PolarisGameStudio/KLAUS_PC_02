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

public class StartSceneFade : MonoBehaviour
{
    public float delayUntilFade = 1.0f;
    public float fadeDuration = 1.0f;

    void Awake()
    {
        CameraFade.StartAlphaFade(Color.black, true, fadeDuration, delayUntilFade);
    }
}

