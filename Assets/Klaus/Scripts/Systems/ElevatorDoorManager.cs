using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Luminosity.IO;
using Rewired;

public class ElevatorDoorManager : MonoBehaviour, ICompleteLevel
{

    public string NextScene = "N101";
    public Animator doorAnim;

    Dictionary<int, bool> isInPlayer = new Dictionary<int, bool>();
    Dictionary<int, int> PlayerPos = new Dictionary<int, int>();

    /* public string GropulLevelNameText = "Basament Floor";
     public string ChapterNameText = "Chapter 1";
     public string LevelNameText = "Walking Life";*/
    public float TimeToShowLevelName = 2.5f;
    public float TimeToWaitKlausEnter = 0.5f;
    public float TimeToWaitCloseDoor = 0.85f;

    bool isChanguin = false;

    public GameObject floatingMusik;

    public Action onEnterElevator;
    public const string SceneToGoArcade = "PrincipalMenu";

    //Por ultimo cambio de nivel
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SaveManager.Instance.SaveCurrentLevel(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    IEnumerator Start()
    {
        for (int i = 0; i < CharacterManager.Instance.charsInput.Length; ++i)
        {
            isInPlayer.Add(CharacterManager.Instance.charsInput[i].gameObject.GetInstanceID(), false);
            PlayerPos.Add(i, CharacterManager.Instance.charsInput[i].gameObject.GetInstanceID());
        }

        yield return new WaitForSeconds(TimeToShowLevelName);
        //     TitleLevelHUD.Instance.Show(GropulLevelNameText, ChapterNameText, LevelNameText);

        CounterTimerPlay.Instance.StartTime();
        ManagerAnalytics.MissionStarted(SceneManager.GetActiveScene().name, false);

        CallTrophy();
    }


    void LateUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (!isChanguin && ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Movement_Y)) > 0.5f)
            {
                DoorEnterAllKlaus();
            }
        }
    }

    void DoorEnterAllKlaus()
    {
        bool cambiar = true;
        for (int i = 0; i < CharacterManager.Instance.canMove.Length; ++i)
        {
            if (CharacterManager.Instance.canMove[i])
            {

                cambiar = cambiar && isInPlayer[PlayerPos[i]];
                if (!cambiar)
                    break;

            }
        }

        CanCambiar(cambiar);
    }

    void CanCambiar(bool value)
    {
        if (value)
        {
            ChangueScene();
            isChanguin = true;
            StartCoroutine("EnterKlauses");

        }
    }

    void ChangueScene()
    {
        LoadLevelManager.Instance.LoadLevel(SaveManager.Instance.isComingFromArcade ? SceneToGoArcade : NextScene, true);

    }

    protected void ActivateManualty()
    {
        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInPlayer[other.gameObject.GetInstanceID()] = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInPlayer[other.gameObject.GetInstanceID()] = false;
        }
    }



    IEnumerator EnterKlauses()
    {
        float timeArcade = 0;
        if (TimeAttackSystem.Instance != null)
        {
            TimeAttackSystem.Instance.PauseTimer();
            timeArcade = TimeAttackSystem.Instance.timer;
        }

        if (onEnterElevator != null)
            onEnterElevator();
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.1f));
        Instantiate(floatingMusik, transform.position, transform.rotation);//Musica

        Camera.main.GetComponent<InputTouchPS4>().Block(true);

        for (int i = 0; i < CharacterManager.Instance.canMove.Length; ++i)
        {
            if (CharacterManager.Instance.canMove[i])
            {
                CharacterManager.Instance.charsInput[i].GetComponent<MoveState>().isEnterElevator = true;
            }
        }

        float timer = CounterTimerPlay.Instance.EndTime();

        SaveManager.Instance.AddPlayTime(timer);

        ManagerAnalytics.MissionCompleted(SceneManager.GetActiveScene().name,
            false, timer, timeArcade, true);
        CompleteScene();
        CompleteLevel();

        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToWaitKlausEnter));
        doorAnim.SetTrigger("Close");//Se abre la reja
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToWaitCloseDoor));
        if (SaveManager.Instance.isComingFromArcade)
        {
            ManagerHudUI.Instance.ShowLevelCompleted(SceneToGoArcade);
        }
        else
        {
            ActivateManualty();
        }
    }

    Action completeSceneCallback;
    Action completeLevelCallback;

    public void CallTrophy()
    {
        CompleteScene_Trophy[] trophy = GameObject.FindObjectsOfType<CompleteScene_Trophy>();
        for (int i = 0; i < trophy.Length; ++i)
        {
            if (trophy[i] != null)
            {
                trophy[i].OnRegister(this);
            }
        }
    }

    public void CompleteScene()
    {
        if (completeSceneCallback != null)
            completeSceneCallback();
    }

    public void CompleteLevel()
    {
        if (completeLevelCallback != null)
            completeLevelCallback();
    }

    public void RegisterCompleteLevel(Action callback)
    {
        completeLevelCallback += callback;
    }

    public void UnRegisterCompleteLevel(Action callback)
    {
        if (completeLevelCallback != null)
        {
            completeLevelCallback -= callback;
        }
    }

    public void RegisterCompleteScene(Action callback)
    {
        completeSceneCallback += callback;
    }

    public void UnRegisterCompleteScene(Action callback)
    {
        if (completeSceneCallback != null)
        {
            completeSceneCallback -= callback;
        }
    }
}
