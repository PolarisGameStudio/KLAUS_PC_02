//
// ChangueScene.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    static int LoadingScene = 1;
    public string SceneToLoad = "";

    /// <summary>
    /// Load the the scene
    /// </summary>
    /// <param name="scene"></param>
    public void Load(string scene)
    {
        SceneToLoad = scene;
        LoadDefault();

    }
    /// <summary>
    /// Load the scene in SceneToLoad
    /// </summary>
    public void LoadDefault()
    {
        CameraFade.Instance.m_OnFadeFinish -= LoadDefault;
        LoadLevelManager.Instance.LoadLevelImmediate(SceneToLoad, false);
        //        Application.LoadLevel(SceneToLoad);
    }
    /// <summary>
    /// Load the scene with loadingScene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadWithLoadingScene(string scene)
    {
        SceneToLoad = scene;
        LoadDefaultWithLoadingScene();
    }
    /// <summary>
    /// Load the scene in SceneToload witj LoadingScene
    /// </summary>
    public void LoadDefaultWithLoadingScene()
    {
        CameraFade.Instance.m_OnFadeFinish -= LoadDefaultWithLoadingScene;
        LoadLevelManager.Instance.LoadLevelWithLoadingScene(LoadingScene, false, false);
    }

    public void LoadDefaultSceneWithFadeWithLoadingScene(float timer)
    {
        CameraFade.StartAlphaFade(Color.black, false, timer, 0);
        CameraFade.Instance.m_OnFadeFinish += LoadDefaultWithLoadingScene;
    }
    public void LoadWithFadeWithLoadingScene(string scene, float timer)
    {
        SceneToLoad = scene;
        CameraFade.StartAlphaFade(Color.black, false, timer, 0);
        CameraFade.Instance.m_OnFadeFinish += LoadDefaultWithLoadingScene;
    }
    public void LoadDefaultSceneWithFade(float timer)
    {
        CameraFade.StartAlphaFade(Color.black, false, timer, 0);
        CameraFade.Instance.m_OnFadeFinish += LoadDefault;
    }
    public void LoadWithFade(string scene, float timer)
    {
        SceneToLoad = scene;
        CameraFade.StartAlphaFade(Color.black, false, timer, 0);
        CameraFade.Instance.m_OnFadeFinish += LoadDefault;
    }

    #region UseLoadLevelManager:
    public void PreLoadScene()
    {
        LoadLevelManager.Instance.LoadLevel(SceneToLoad, true, false);

    }
    public void LoadPreloadedSceneWithFade(float timer)
    {
        CameraFade.StartAlphaFade(Color.black, false, timer, 0);
        CameraFade.Instance.m_OnFadeFinish += LoadPreloadedScene;
    }
    public void LoadPreloadedScene()
    {
        CameraFade.Instance.m_OnFadeFinish -= LoadPreloadedScene;
        LoadLevelManager.Instance.ActivateLoadedLevel();

    }
    #endregion
}

