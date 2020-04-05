﻿//
// ManagerPause.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerPause : Singleton<ManagerPause>
{

    #region Paused
    public delegate void onPauseGameBroadcast();
    public event onPauseGameBroadcast OnPauseGame;

    /// <summary>
    /// Subscribes funct to OnPauseGame.
    /// </summary>
    /// <param name="funct">Funct.</param>
    public static void SubscribeOnPauseGame(onPauseGameBroadcast funct)
    {
        if (Instance != null)
            Instance.OnPauseGame += funct;
    }
    /// <summary>
    /// Unsubscribe safetely funct Function to OnPauseGameDelegate
    /// </summary>
    /// <param name="funct">Function.</param>
    public static void UnSubscribeOnPauseGame(onPauseGameBroadcast funct)
    {
        if (InstanceExists())
            Instance.OnPauseGame -= funct;
    }
    #endregion

    #region Resume:
    public delegate void onResumeGameBroadcast();
    public event onResumeGameBroadcast OnResumeGame;

    /// <summary>
    /// Subscribes funct to OnResumeGame
    /// </summary>
    /// <param name="funct">Funct.</param>
    public static void SubscribeOnResumeGame(onResumeGameBroadcast funct)
    {
        Instance.OnResumeGame += funct;
    }
    /// <summary>
    /// Unsubscribe safetely funct Function to OnResumeGameDelegate
    /// </summary>
    /// <param name="funct">Function.</param>
    public static void UnSubscribeOnResumeGame(onResumeGameBroadcast funct)
    {
        if (InstanceExists())
            Instance.OnResumeGame -= funct;
    }
    #endregion

    private bool paused = false;
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ManagerPause"/> is pause.
    /// </summary>
    /// <value><c>true</c> if pause; otherwise, <c>false</c>.</value>
    public static bool Pause
    {
        get
        {
            if (Instance != null)
            {
                return Instance.paused;
            }
            else
            {
                return false;
            }

        }
        set
        {
            if (Instance != null)
            {
                Instance.paused = value;
                if (Instance.paused)
                {
                    Time.timeScale = 0f;
                    Instance.OnPauseGame();
                }
                else
                {
                    Time.timeScale = 1f;
                    Instance.OnResumeGame();
                }
            }

        }
    }
}
