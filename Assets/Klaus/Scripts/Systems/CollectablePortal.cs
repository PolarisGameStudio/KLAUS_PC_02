using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CollectablePortal : MonoBehaviour
{
    public string NextLevel = "";
    public Transform posForKlaus;

    bool isActive = false;
    public Animator anim;
    public GameObject collectibleSFX;
    public int ShakePReset = 1;

    public GameObject checkpoint;
    public Action EnterPortalCallback;

    EnterDoorManager[] doorsEnter;

    void Awake()
    {
        doorsEnter = GameObject.FindObjectsOfType<EnterDoorManager>();

        if (SaveManager.Instance.comingFromCollectables)
        {
            if (SaveManager.Instance.LevelCollectableLoaded == NextLevel)
            {
                for (int i = 0; i < doorsEnter.Length; ++i)
                {
                    if (doorsEnter[i] != null)
                    {
                        if (!doorsEnter[i].ItsAnotherDoor)
                        {
                            EnterDoorManager door = doorsEnter[i];
                            door.isStartLevel = false;
                            door.canStartAnalitic = false;
                            door.SetLevelCounterTimer(SaveManager.Instance.currentTimePlayLevel);
                            break;
                        }
                    }
                }
            }
        }

    }

    protected void Start()
    {
        if (SaveManager.Instance.comingFromTimeArcadeMode
            /*  || CollectablesManager.isCollectablePortalReady(Application.loadedLevelName)*/)
        {
            SaveManager.Instance.comingFromCollectables = false;
            SaveManager.Instance.LevelCollectableLoaded = "";
            gameObject.SetActive(false);
            return;
        }
        if (SaveManager.Instance.comingFromCollectables)
        {
            if (SaveManager.Instance.LevelCollectableLoaded == NextLevel)
            {
                SaveManager.Instance.comingFromCollectables = false;
                SaveManager.Instance.LevelCollectableLoaded = "";
                for (int i = 0; i < CharacterManager.Instance.charsInput.Length; ++i)
                {
                    if (CharacterManager.Instance.charsInput[i].name.Contains("Klaus"))
                    {
                        CharacterManager.Instance.charsInput[i].transform.position = posForKlaus.position;
                        Camera.main.transform.position = new Vector3(posForKlaus.position.x, posForKlaus.position.y, Camera.main.transform.position.z);

                    }
                    else if (CharacterManager.Instance.charsInput[i].name.Contains("K1"))
                    {
                        CharacterManager.Instance.charsInput[i].transform.position = SaveManager.Instance.posK1;
                        CheckPoint[] checkpoints = GameObject.FindObjectsOfType<CheckPoint>();
                        for (int j = 0; j < checkpoints.Length; ++j)
                        {
                            if (checkpoints[j].transform.position == SaveManager.Instance.posK1Checkpoint)
                            {
                                checkpoints[j].RotateArrow();
                                ManagerCheckPoint.Instance.AddPosition(PlayersID.Player2K1, checkpoints[j]);
                                break;
                            }
                        }

                    }
                }
                checkpoint.Spawn(transform.position);
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnEnterAction(other);
            Instantiate(collectibleSFX, transform.position, transform.rotation);
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
        bool isklaus = false;
        if (other.CompareTag("Player"))
        {
            isklaus = other.GetComponent<PlayerInfo>().playerType == PlayersID.Player1Klaus;
        }
        return isklaus && !isActive;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
        isActive = true;
        //Entro al nivel
        CharacterManager.Instance.FreezeAll();
        SaveManager.Instance.comingFromCollectables = true;
        SaveManager.Instance.posKlaus = posForKlaus.position;


        MoveStateK1 k1 = GameObject.FindObjectOfType<MoveStateK1>();
        if (k1 != null)
        {
            Vector3? K1pos = ManagerCheckPoint.Instance.getPosition(PlayersID.Player2K1);
            if (K1pos.HasValue)
                SaveManager.Instance.posK1Checkpoint = K1pos.Value;
            SaveManager.Instance.posK1 = k1.transform.position;
        }
        for (int i = 0; i < doorsEnter.Length; ++i)
        {
            if (doorsEnter[i] != null)
            {
                if (!doorsEnter[i].ItsAnotherDoor)
                {
                    SaveManager.Instance.currentTimePlayLevel = doorsEnter[i].TimePlayingLevel;
                }
            }
        }


        SaveManager.Instance.LevelToLoadCollectable = SceneManager.GetActiveScene().name;
        //Aviso a HUD
        LoadLevelManager.Instance.LoadLevel(NextLevel, true);
        gameObject.AddComponent<AudioListener>();

        for (int i = 0; i < CharacterManager.Instance.charsInput.Length; ++i)
        {
            CharacterManager.Instance.charsInput[i].gameObject.SetActive(false);
        }
        if (CameraShake.Instance != null)
            CameraShake.Instance.StartShake(ShakePReset);
        anim.SetTrigger("Play");

        if (EnterPortalCallback != null)
            EnterPortalCallback();
    }

    protected virtual void OnExitAction(Collider2D other)
    {
        //isActive = false;
    }


    public void ChangeLevel()
    {
        CameraShake.Instance.StopShake();
        Time.timeScale = 1;
        CameraFade.StartAlphaFade(Color.black, false, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += ActivateLevel;
    }

    void ActivateLevel()
    {
        CameraFade.Instance.m_OnFadeFinish -= ActivateLevel;
        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= ActivateLevel;
    }
}
