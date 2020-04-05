using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PausaMenu_PopUp : MonoBehaviour
{
    public PausaMenu_List mastercontrol;
    public CanvasGroup popupCanvas;
    public CanvasGroup Menu;
    public GameObject firstSelected;
    public EventSystem evenSys;

    public void ResetSelectFirst()
    {
        evenSys.SetSelectedGameObject(null);
    }

    public void SelectFirstButton()
    {
        evenSys.SetSelectedGameObject(firstSelected);
    }

    protected Action callback;

    void OnEnable()
    {
        ManagerPause.SubscribeOnResumeGame(onResumeGame);
    }

    public void SetRestartLevel()
    {
        callback = RestartLevel;
        StartCoroutine(Show());
    }

    public void SetExitGame()
    {
        callback = ExitMain;
        StartCoroutine(Show());
    }

    public void SetExitMain()
    {
        callback = ExitGame;
        StartCoroutine(Show());
    }

    public void SetRestartSection()
    {
        callback = RestartSection;
        StartCoroutine(Show());
    }

    public Action callbackChangeLevel;

    void RestartLevel()
    {
        var levelName = SceneManager.GetActiveScene().name;

        ManagerAnalytics.MissionFailed(levelName,
            false, "Reset", CounterTimerPlay.Instance.TimePlayingLevel);

        TimeAttackSystem.Instance.ResetTimer();
        Time.timeScale = 1;


        string FirstLetter = levelName.Substring(0, 1);
        if (FirstLetter == "W")
        {
            callbackChangeLevel?.Invoke();
            string typeLevel = levelName.Substring(2, 1);
            if (typeLevel == "L")
            {

                LoadLevelManager.Instance.LoadLevelWithLoadingScene(levelName.Substring(0, 6) + "1", false);
            }
            else if (typeLevel == "B")
            {
                LoadLevelManager.Instance.RestartCurrentLevel();
            }
            else if (typeLevel == "C")
            {
                if (SaveManager.Instance.LevelToLoadCollectable != "")
                {
                    LoadLevelManager.Instance.LoadLevelWithLoadingScene(SaveManager.Instance.LevelToLoadCollectable.Substring(0, 6) + "1", false);
                }
                else
                {
                    LoadLevelManager.Instance.RestartCurrentLevel();
                }
                //LoadLevelManager.Instance.RestartCurrentLevel();
            }
        }
        else if (FirstLetter == "C")
        {

            LoadLevelManager.Instance.RestartCurrentLevel();

        }
    }

    void RestartSection()
    {
        var levelName = SceneManager.GetActiveScene().name;

        ManagerAnalytics.MissionFailed(levelName,
             false, "Reset", CounterTimerPlay.Instance.TimePlayingLevel);

        /*  if (callbackChangeLevel != null)
              callbackChangeLevel();*/

        Time.timeScale = 1;
        LoadLevelManager.Instance.RestartCurrentLevel();
    }

    void ExitMain()
    {
        var levelName = SceneManager.GetActiveScene().name;

        ManagerAnalytics.MissionFailed(levelName,
            false, "Leave", CounterTimerPlay.Instance.TimePlayingLevel);

        TimeAttackSystem.Instance.ResetTimer();
        Time.timeScale = 1;

        callbackChangeLevel?.Invoke();

        LoadLevelManager.Instance.LoadLevelWithLoadingScene("PrincipalMenu", false);

    }

    void ExitGame()
    {
        var levelName = SceneManager.GetActiveScene().name;

        ManagerAnalytics.MissionFailed(levelName,
            false, "Leave", CounterTimerPlay.Instance.TimePlayingLevel);

        TimeAttackSystem.Instance.ResetTimer();
        Time.timeScale = 1;

        callbackChangeLevel?.Invoke();

        LoadLevelManager.Instance.LoadLevelWithLoadingScene("PrincipalMenu", false);

    }

    public void Yes()
    {
        var musikArrehc0 = GameObject.Find("AS_MusikManager_Arrecho");
        if (musikArrehc0 != null)
        {
            musikArrehc0.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(0.1f);
        }
        callback();
    }

    public void No()
    {

        mastercontrol.ResetSelectFirst();
        StartCoroutine(Hide());
        
    }

    IEnumerator Show()
    {
        ResetSelectFirst();
        yield return null;
        Menu.blocksRaycasts = false;
        Menu.interactable = false;
        yield return null;
        popupCanvas.alpha = 1;
        popupCanvas.blocksRaycasts = true;
        popupCanvas.interactable = true;
        yield return null;
        SelectFirstButton();
    }

    IEnumerator Hide()
    {
        ResetSelectFirst();
        
        yield return null;
        callback = null;
        popupCanvas.alpha = 0;
        popupCanvas.blocksRaycasts = false;
        popupCanvas.interactable = false;
        yield return null;

        Menu.blocksRaycasts = true;
        Menu.interactable = true;
        yield return null;
        mastercontrol.SelectFirstButton();

    }


    public void onResumeGame()
    {
        if (Mathf.Approximately(popupCanvas.alpha, 1.0f))
        {
            No();
        }
    }
}
