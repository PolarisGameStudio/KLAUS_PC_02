//
// ManagerPause.cs
//
// Modified:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelManager : PersistentSingleton<LoadLevelManager>
{

    private AsyncOperation m_asop = null;

    private bool m_quitAfterCurrentLoad = false;

    public bool IsLoading
    {
        get
        {
            if (m_asop == null)
                return false;

            return !m_asop.allowSceneActivation ? m_asop.progress < 0.9f : !m_asop.isDone;
        }
    }

    #region HelperWithSceneLoading:
    public string SceneLoading = "Loading";
    bool LoadWithLoadScene = false;
    object level = null;
    bool manualActivation = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    /// <summary>
    /// Load the level level using a SceneLoading and manualActivation.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="manualActivation"></param>
    public void LoadLevelWithLoadingScene(object level, bool manualActivation, bool checkWithSaving = true)
    {
        if (IsLoading)
        {
            Debug.LogError("Call attempted to LoadLevel while a level is already in the process of loading; ignoring the load request...");
        }
        else
        {
            this.level = level;
            this.manualActivation = manualActivation;
            LoadWithLoadScene = true;
            LoadLevelImmediate(SceneLoading, checkWithSaving);//Cargar Scene of loading
        }
    }

    #endregion

    /// <summary>
    /// Load the current level.
    /// </summary>
    public void RestartCurrentLevel(bool checkWithSaving = true)
    {
        if (IsLoading)
        {
            Debug.LogError("Call attempted to LoadLevel while a level is already in the process of loading; ignoring the load request...");
        }
        else
        {
            StartCoroutine(RestartCurrentLevelWithSave(checkWithSaving));
        }
    }
    IEnumerator RestartCurrentLevelWithSave(bool checkWithSaving)
    {
        if (checkWithSaving)
        {
            while (SaveManager.Instance.isSaving)
                yield return null;
        }
        LoadWithLoadScene = false;
        _LoadLevelImmediateProxy(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Pass either the level name as a string or the level index
    /// </summary>
    /// <param name="level">Level.</param>
    /// <param name="manualActivation">If set to <c>true</c> manual activation.</param>
    public void LoadLevel(object level, bool manualActivation, bool checkWithSaving = true)
    {
        if (IsLoading)
        {
            Debug.LogError("Call attempted to LoadLevel while a level is already in the process of loading; ignoring the load request...");
        }
        else
        {
            StartCoroutine(LoadLevelAsyncWithSave(level, manualActivation, checkWithSaving));
        }
    }
    IEnumerator LoadLevelAsyncWithSave(object level, bool manualActivation, bool checkWithSaving)
    {
        if (checkWithSaving)
        {
            while (SaveManager.Instance.isSaving)
                yield return null;
        }

        m_asop = _LoadLevelAsyncProxy(level);
        if (null != m_asop)
        {
            LoadWithLoadScene = false;
            m_asop.allowSceneActivation = !manualActivation;
        }
    }

    /// <summary>
    /// Load the level immdiate ( without proxy)
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevelImmediate(object level, bool checkWithSaving = true)
    {
        if (IsLoading)
        {
            Debug.LogError("Call attempted to LoadLevel while a level is already in the process of loading; ignoring the load request...");
        }
        else
        {
            StartCoroutine(LoadLevelImmediateWithSave(level, checkWithSaving));
        }
    }
    IEnumerator LoadLevelImmediateWithSave(object level, bool checkWithSaving)
    {
        if (checkWithSaving)
        {
            while (SaveManager.Instance.isSaving)
                yield return null;
        }
        _LoadLevelImmediateProxy(level);
    }
    /// <summary>
    /// This will immediately activate the currently loaded level if it hasn't been activated.
    /// If the level hasn't finished loading, it will be activated immediately after finishing.
    /// </summary>
    public void ActivateLoadedLevel()
    {
        StartCoroutine("CheckForSetManualActivation");
    }
    IEnumerator CheckForSetManualActivation()
    {
        if (m_asop == null)
        {
            Debug.LogError("SceneMgr::ActivateLoadedLevel was called, but there is no inactive scene to activate!");
            yield break;
        }
        while (IsLoading)
        {
            yield return null;
        }
        m_asop.allowSceneActivation = true;

    }
    static AsyncOperation _LoadLevelAsyncProxy(object level)
    {
        if (level is int)
        {
            return SceneManager.LoadSceneAsync((int)level);
        }
        else if (level is string)
        {
            return SceneManager.LoadSceneAsync((string)level);
        }
        else
        {
            Debug.LogError("SceneMgr.LoadLevel was called with the wrong parameter type " + level.GetType() + "; must be int or string.");
        }
        return null;
    }

    static void _LoadLevelImmediateProxy(object level)
    {
        if (level is int)
        {
            SceneManager.LoadScene((int)level);
        }
        else if (level is string)
        {
            SceneManager.LoadScene((string)level);
        }
        else
        {
            Debug.LogError("SceneMgr.LoadLevelImmediate was called with the wrong parameter type " + level.GetType() + "; must be int or string.");
        }
    }


    #region Unity Callbacks:

    bool checkLateUpdate = false;
    void LateUpdate()
    {
        if (checkLateUpdate)
        {
            checkLateUpdate = false;
            if (LoadWithLoadScene)
            {
                StartCoroutine(CheckLoadingScene());

            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(_onLevelWasLoaded(scene, mode));

    }

    IEnumerator _onLevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        while (IsLoading && !m_asop.allowSceneActivation)
        {
            // Wait until the just-loaded scene is allowed to start
            yield return null;
        }

        StopCoroutine("CheckForSetManualActivation");
        m_asop = null;

        if (m_quitAfterCurrentLoad)
        {
            Application.Quit();
        }
        checkLateUpdate = true;
    }

    IEnumerator CheckLoadingScene()
    {
        while (IsLoading)
        {
            yield return null;
        }

        LoadLevel(level, manualActivation, false);
    }
    void OnApplicationQuit()
    {
        if (IsLoading)
        {
            m_quitAfterCurrentLoad = true;
            Application.CancelQuit();
        }

    }
    #endregion

}
