using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;

public class W2FinalTriggerMove : MonoBehaviour, ICompleteLevel
{

    public MoveState Klaus;

    public float TimeTochangeLevel = 10.0f;
    public string NextScene = "W2Ending";
    public const string SceneToGoArcade = "PrincipalMenu";
    public bool ShowArcade = true;
    public GameObject floatingMusik;
    public AudioMixer mainMixer;
    bool isEnter = false;

    //Por ultimo cambio de nivel
    public void OnLevelWasLoaded(int level)
    {
        SaveManager.Instance.SaveCurrentLevel(Application.loadedLevelName);
    }


    void Start()
    {

        CounterTimerPlay.Instance.StartTime();
        ManagerAnalytics.MissionStarted(Application.loadedLevelName, false);

        CallTrophy();
    }


    void ChangueScene()
    {

        if (SaveManager.Instance.isComingFromArcade && ShowArcade)
        {
            LoadLevelManager.Instance.LoadLevel(SceneToGoArcade,true);
        }
        else
        {
            LoadLevelManager.Instance.LoadLevel(NextScene, true);
        }
    }

    protected void ActivateManualty()
    {
        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other) && !isEnter)
        {
            isEnter = true;
            StopCoroutine("OnEnterAction");
            StartCoroutine("OnEnterAction");
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MoveState move = other.GetComponent<MoveState>();
            if (move.getLegsCollider() == other)
            {
                return true;
            }
        }
        return false;
    }

    protected IEnumerator OnEnterAction()
    {
        float timeArcade = 0;
        if (TimeAttackSystem.Instance != null)
        {
            TimeAttackSystem.Instance.PauseTimer();
            timeArcade = TimeAttackSystem.Instance.timer;
        }

        ChangueScene();
        StopCoroutine("LevelCounterTime");
        Klaus.CanMove(true);
        Klaus.activeJump = false;
        Instantiate(floatingMusik, transform.position, transform.rotation);//Musica
        Camera.main.GetComponent<InputTouchPS4>().Block(true);
        float timer = CounterTimerPlay.Instance.EndTime();

        SaveManager.Instance.AddPlayTime(timer);

        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
            false, timer, timeArcade, true);
        CompleteScene();
        CompleteLevel();
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeTochangeLevel));

        if (SaveManager.Instance.comingFromTimeArcadeMode && ShowArcade)
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
        completeLevelCallback -= callback;
    }
    public void RegisterCompleteScene(Action callback)
    {
        completeSceneCallback += callback;
    }

    public void UnRegisterCompleteScene(Action callback)
    {
        completeSceneCallback -= callback;
    }
}
