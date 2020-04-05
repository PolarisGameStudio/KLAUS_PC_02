using UnityEngine;
using System.Collections;

public class CollectablePiece : MonoBehaviour
{
    protected string TypeLevel = "W1C01";
    public PausaMenu_PopUp pausa;

    bool isActive = false;

    public CollectableDoorPiece Door;
    public CollectablePiecePorttal Portal;
    public float timeToShowNextStep = 0.5f;

    int posToShow = -1;

    CollectableUI collectable_UI;
    Vector2 floatY;
    float originalY;

    public float floatStrength = 2;

    public GameObject StingerSFX;

    void Awake()
    {
        TypeLevel = Application.loadedLevelName;
        collectable_UI = GameObject.FindObjectOfType<CollectableUI>();
    }

    IEnumerator Start()
    {
        this.originalY = this.transform.position.y;
        yield return null;
        ManagerAnalytics.MissionStarted(Application.loadedLevelName, false);
        CounterTimerPlay.Instance.StartTime();
        CheckForNextStep();


    }

    void CheckForNextStep()
    {
        /*
        if (SaveManager.Instance.dataKlaus == null)
        {
            SaveManager.Instance.StartSaveManager();
        }*/

        if (CollectablesManager.isCollectableReady(TypeLevel))//Pregunto si este collectable ya se agarro
        {
            SaveManager.Instance.LevelCollectableLoaded = Application.loadedLevelName;

            if (CollectablesManager.isCollectableFull(TypeLevel))//Pregunto si ya se colleciono todo
            {
                SaveManager.Instance.LevelToLoadVideoCollectable = Door.NextScene;
                //Muestro Puerta
                Door.pausa = pausa;
                Door.gameObject.SetActive(true);
            }
            else
            {
                SaveManager.Instance.LevelToLoadVideoCollectable = SaveManager.Instance.LevelToLoadCollectable;
                //Muestro Portal
                Portal.pausa = pausa;
                Portal.gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        pausa.callbackChangeLevel += ResetCollectableLevelChange;
    }

    void OnDisable()
    {
        if (pausa != null)
        {
            pausa.callbackChangeLevel -= ResetCollectableLevelChange;
        }
    }

    void ResetCollectableLevelChange()
    {
        SaveManager.Instance.comingFromCollectables = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnEnterAction(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnExitAction(other);
        }
    }


    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && !isActive;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
        isActive = true;
        StartCoroutine("WaitingPortal");
        Instantiate(StingerSFX, transform.position, transform.rotation);
    }

    protected virtual void OnExitAction(Collider2D other)
    {
        isActive = false;
        StopCoroutine("WaitingPortal");
    }

    IEnumerator WaitingPortal()
    {
        CharacterManager.Instance.FreezeAll();
        CharacterManager.Instance.BecomeInmortal(true);
        //Salvo
        posToShow = CollectablesManager.setCollectable(TypeLevel);

        GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < transform.childCount; ++i)
            transform.GetChild(i).gameObject.SetActive(false);

        yield return StartCoroutine(new TimeCallBacks().WaitPause(timeToShowNextStep));
        collectable_UI.onFinish += FinishGivePiece;
        if (posToShow >= 0)
            collectable_UI.SetCollect(posToShow);
        else
            collectable_UI.SetCollect(0);

        float timer = CounterTimerPlay.Instance.EndTime();
        SaveManager.Instance.AddPlayTime(timer);
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
            false, timer, 0, false);


        //Precargo la escena de video
        //    LoadLevelManager.Instance.LoadLevel(Application.loadedLevelName + "_Video", ThreadPriority.Low, true);

    }

    void FinishGivePiece()
    {
        collectable_UI.onFinish -= FinishGivePiece;
        collectable_UI.gameObject.SetActive(false);


        SaveManager.Instance.LevelCollectableLoaded = Application.loadedLevelName;

        if (posToShow == -1)
        {
            SaveManager.Instance.LevelToLoadVideoCollectable = SaveManager.Instance.LevelToLoadCollectable;
        }
        else
        {
            if (CollectablesManager.isCollectableReady(TypeLevel))
            {
                if (CollectablesManager.isCollectableFull(TypeLevel))
                {
                    SaveManager.Instance.LevelToLoadVideoCollectable = Door.NextScene;

                }
                else
                {
                    SaveManager.Instance.LevelToLoadVideoCollectable = SaveManager.Instance.LevelToLoadCollectable;
                }
            }
        }

        CameraFade.StartAlphaFade(Color.black, false, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += LoadLevel;
    }

    void LoadLevel()
    {
        CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
        LoadLevelManager.Instance.LoadLevelImmediate(Application.loadedLevelName + "_Video");
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
    }

    void Update()
    {
        floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time) * floatStrength);
        transform.position = floatY;

    }
}
