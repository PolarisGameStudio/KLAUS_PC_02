//
// ChangueLevelFade.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class ChangueLevelFade
{

    private string sceneChangue;

    public void ChangueTo(string nameScene, float time)
    {
        sceneChangue = nameScene;
        CameraFade.StartAlphaFade(Color.black, false, time, 0);
        CameraFade.Instance.m_OnFadeFinish += Load;

    }
    void OnDestroy()
    {
        CameraFade.Instance.m_OnFadeFinish -= Load;
    }
    public void ChangueToHard(string nameScene)
    {
        sceneChangue = nameScene;
        Load();

    }
    protected void Load()
    {
        LoadLevelManager.Instance.LoadLevel(sceneChangue, false);
    }
}

