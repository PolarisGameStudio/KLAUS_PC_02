using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Luminosity.IO;
using Rewired;

public class EnterDoorManager : MonoBehaviour, ICompleteLevel
{
    public bool preLoadScene = true;
    const float enterDoorValueStick = 0.5f;
    public string NextScene = "N101";
    public Animator doorAnim;
    public GameObject doorSFX;
    bool isChanguin = false;
    Dictionary<int, bool> isInPlayer = new Dictionary<int, bool>();
    Dictionary<int, int> PlayerPos = new Dictionary<int, int>();
    public bool isFinalLevel = false;
    public Animator FinalLevelAsset;
    public bool isStartLevel = false;
    public bool canStartAnalitic = true;
    public string GropulLevelNameText = "Basament Floor";
    public string ChapterNameText = "Chapter 1";
    public string LevelNameText = "Walking Life";
    public string QuoteText = "\"We just have to follow\nthe shadow and keep together\"";
    public string CompleteSectionName = "\"According to my records this is room 1, section 1 of the Basement Floor\"";
    public float TimeToShowLevelName = 2.5f;
    public Action callbackEnterDoor;
    public AudioMixer mainMixer;
    [Header("Especial Entry")]
    public bool EntrarMismoTiempo = true;
    [Header("Uno Entry")]
    public CharacterInputController SoloPaEntrar = null;
    public const string SceneToGoArcade = "PrincipalMenu";
    public bool isTutorial = false;

    public float TimeMoveCameraWhenPlayerEnter = 0.5f;

    public bool StartOff = false;

    public bool isEndTimeAttack = false;
    public bool ItsAnotherDoor = false;

    //Por ultimo cambio de nivel
    public void OnLevelWasLoad(Scene level, LoadSceneMode mode)
    {
        if (ItsAnotherDoor)
            return;

        SaveManager.Instance.SaveCurrentLevel(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelWasLoad;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelWasLoad;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        for (int i = 0; i < CharacterManager.Instance.charsInput.Length; ++i)
        {
            isInPlayer.Add(CharacterManager.Instance.charsInput[i].gameObject.GetInstanceID(), false);
            PlayerPos.Add(i, CharacterManager.Instance.charsInput[i].gameObject.GetInstanceID());
        }
        if (!ItsAnotherDoor)
        {

            TitleLevelHUD.Instance.SetCompleteName(CompleteSectionName);
            if (isFinalLevel || isEndTimeAttack)
            {
                FinalLevelAsset.gameObject.SetActive(true);
                FinalLevelAsset.Rebind();
            }
            else
            {
                FinalLevelAsset.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(TimeToShowLevelName);
            if (isStartLevel)
            {
                TitleLevelHUD.Instance.Show(GropulLevelNameText, ChapterNameText, LevelNameText);
            }

            if (canStartAnalitic)
            {
                ManagerAnalytics.MissionStarted(SceneManager.GetActiveScene().name, isTutorial);
            }

            CallTrophy();
        }
        SetLevelCounterTimer(0);
        if (StartOff)
            gameObject.SetActive(false);
        if (isFinalLevel || isEndTimeAttack)
        {
            //Al levecompleted need load scene
            preLoadScene = false;
        }
    }
    bool isTimeStarted = false;
    public void SetLevelCounterTimer(float value)
    {

        if (isTimeStarted)
        {

            value += TimePlayingLevel;
        }
        isTimeStarted = true;
        CounterTimerPlay.Instance.StartTime(value);
    }

    public float TimePlayingLevel
    {
        get
        {
            return CounterTimerPlay.Instance.TimePlayingLevel;
        }
    }

    public float StopTimerAndReturn()
    {
        return CounterTimerPlay.Instance.EndTime();

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (!isChanguin && ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Movement_Y)) > enterDoorValueStick)
            {
                if (SoloPaEntrar != null)
                {
                    if (SoloPaEntrar.enabled && isInPlayer[SoloPaEntrar.gameObject.GetInstanceID()])
                    {
                        SoloPaEntrar.GetComponent<MoveState>().isEnter = true;
                        CanCambiar(true);
                    }
                }
                else
                {
                    if (EntrarMismoTiempo)
                    {
                        DoorEnterAllKlaus();
                    }
                    else
                    {
                        DoorEnterOneKlaus();
                    }
                }
            }
        }
    }

    public void ForceChange()
    {
        CanCambiar(true);
    }

    void DoorEnterOneKlaus()
    {
        bool cambiar = true;
        for (int i = 0; i < CharacterManager.Instance.canMove.Length; ++i)
        {
            if (CharacterManager.Instance.canMove[i])
            {
                if (isInPlayer[PlayerPos[i]])
                {
                    EnterOneKlaus(i);
                }
                else
                {
                    cambiar = false;
                }

            }

        }
        CanCambiar(cambiar);
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

            //callback para las puertas.
            if (callbackEnterDoor != null)
                callbackEnterDoor();

            ChangueScene();
            isChanguin = true;
            Invoke("EnterKlauses", 0.1f);
            mainMixer.FindSnapshot("LevelCompleted").TransitionTo(2 * Time.deltaTime);
            if (doorSFX)
            {
                doorSFX.Spawn(transform.position, transform.rotation);
            }
            // EnterKlauses();

        }
    }

    void EnterOneKlaus(int i)
    {
        //   if (CharacterManager.Instance.characterCanMove > 1)
        //     CharacterManager.Instance.SetPlay(false, CharacterManager.Instance.charsInput[i]);
        StartCoroutine("SetOffPlayEnter", i);
        CharacterManager.Instance.charsInput[i].GetComponent<MoveState>().isEnter = true;
    }

    IEnumerator SetOffPlayEnter(int i)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeMoveCameraWhenPlayerEnter));
        if (CharacterManager.Instance.characterCanMove > 1)
            CharacterManager.Instance.SetPlay(false, CharacterManager.Instance.charsInput[i]);
    }

    void EnterKlauses()
    {
        float timeArcade = 0;
        if (TimeAttackSystem.Instance != null)
        {
            TimeAttackSystem.Instance.PauseTimer();
            if (isFinalLevel || isEndTimeAttack)
                timeArcade = TimeAttackSystem.Instance.timer;
        }
        doorAnim.SetBool("Open", true);

        for (int i = 0; i < CharacterManager.Instance.canMove.Length && EntrarMismoTiempo && SoloPaEntrar == null; ++i)
        {
            if (CharacterManager.Instance.canMove[i])
            {
                CharacterManager.Instance.charsInput[i].GetComponent<MoveState>().isEnter = true;
            }
        }
        float timer = CounterTimerPlay.Instance.EndTime();
        SaveManager.Instance.AddPlayTime(timer);

        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
            isTutorial, timer, timeArcade, isFinalLevel || isEndTimeAttack);

        CompleteScene();

        if (isFinalLevel || isEndTimeAttack)
        {
            CompleteLevel();

            HUD_LevelCompletedMenu.Instance.SetQuote(QuoteText);
            Camera.main.GetComponent<InputTouchPS4>().Block(true);

            if (SaveManager.Instance.isComingFromArcade)
            {
                ManagerHudUI.Instance.ShowLevelCompleted(SceneToGoArcade, preLoadScene);
            }
            else
            {
                ManagerHudUI.Instance.ShowLevelCompleted(NextScene, preLoadScene);
            }
        }
        else
        {
            EffectCamera();
        }

    }

    protected virtual void EffectCamera()
    {
        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.8f);
        CameraFade.Instance.m_OnFadeFinish += ActivateManualty;

    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= ActivateManualty;
    }

    void ChangueScene()
    {
        if (preLoadScene)
        {
            if (!isFinalLevel && !isEndTimeAttack)
            {
                LoadLevelManager.Instance.LoadLevel(NextScene, true);
            }
            else
            {
                if (SaveManager.Instance.isComingFromArcade)
                {
                    LoadLevelManager.Instance.LoadLevel(SceneToGoArcade, true);
                }
                else
                {
                    LoadLevelManager.Instance.LoadLevel(NextScene, true);
                }
            }
        }
    }

    protected void ActivateManualty()
    {
        Time.timeScale = 1;
        if (preLoadScene)
        {
            LoadLevelManager.Instance.ActivateLoadedLevel();
        }
        else
        {
            if (!isFinalLevel && !isEndTimeAttack)
            {
                LoadLevelManager.Instance.LoadLevelWithLoadingScene(NextScene, false);
            }
            else
            {
                if (SaveManager.Instance.isComingFromArcade)
                {
                    LoadLevelManager.Instance.LoadLevelWithLoadingScene(SceneToGoArcade, false);
                }
                else
                {
                    LoadLevelManager.Instance.LoadLevelWithLoadingScene(NextScene, false);
                }
            }

        }
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
