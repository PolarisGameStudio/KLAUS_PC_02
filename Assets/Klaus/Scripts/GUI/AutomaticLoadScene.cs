//
// AutomaticLoadScene.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class AutomaticLoadScene : LoadScene
{
    public float TimeBeforeLoad = 0;
    public float TimeToLoad = 3.0f;
    public bool useFade = true;
    public bool useLoadingScene = false;
	// Use this for initialization
	void Start ()
	{

        Invoke("ManageLoad", TimeBeforeLoad);
	}

    void ManageLoad() {
        if (useLoadingScene) {
            if (useFade) {
                LoadDefaultSceneWithFadeWithLoadingScene(TimeToLoad);
            } else {
                Invoke("LoadDefaultWithLoadingScene", TimeToLoad);
            }
        } else {
            if (useFade) {
                LoadDefaultSceneWithFade(TimeToLoad);
            } else {
                Invoke("LoadDefault", TimeToLoad);
            }
        }
    }


}

