using UnityEngine;
using System.Collections;
using Luminosity.IO;
using Rewired;

public class CollectableDoorPiece : MonoBehaviour
{

    public string NextScene = "PrincipalMenu";
    bool isActive = false;
    public PausaMenu_PopUp pausa;
    public float timeEnter = 0.5f;
    public Animator anim;

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

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && !isActive;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
        isActive = true;
        StartCoroutine("WaitingPortal");
    }

    protected virtual void OnExitAction(Collider2D other)
    {
        isActive = false;
        StopCoroutine("WaitingPortal");
    }

    IEnumerator WaitingPortal()
    {
        while (true)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                if (ReInput.players.GetPlayer(0).GetAxis("Move Y") > SaveManager.Instance.dataKlaus.controlSensitivity)
                {
                    //Entro al nivel
                    CharacterManager.Instance.FreezeAll();
                    LoadLevelManager.Instance.LoadLevel(NextScene, true);
                    MoveStateKlaus[] moves = GameObject.FindObjectsOfType<MoveStateKlaus>();
                    for (int i = 0; i < moves.Length; ++i)
                    {
                        moves[i].isEnter = true;
                    }
                    anim.SetTrigger("Open");
                    StartCoroutine(EnterPortal(timeEnter));
                    float timer = CounterTimerPlay.Instance.EndTime();
                    SaveManager.Instance.AddPlayTime(timer);
                    ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
                        false, timer, 0, false);
                    break;
                }
            }

            yield return null;
        }
    }

    IEnumerator EnterPortal(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        CameraFade.StartAlphaFade(Color.black, false, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += LoadLevel;
    }

    void LoadLevel()
    {
        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
    }
}
