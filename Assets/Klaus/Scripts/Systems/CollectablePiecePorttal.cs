using UnityEngine;
using System.Collections;

public class CollectablePiecePorttal : MonoBehaviour
{
    protected string DefaultScene = "PrincipalMenu";
    bool isActive = false;
    public PausaMenu_PopUp pausa;
    public Animator anim;
    public GameObject collectibleSFX;
    public int ShakePReset = 1;
    public bool PreLoadScene = false;
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
        float timer = CounterTimerPlay.Instance.EndTime();
        SaveManager.Instance.AddPlayTime(timer);
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
            false, timer, 0, false);

        isActive = true;
        //Entro al nivel
        CharacterManager.Instance.FreezeAll();
        gameObject.AddComponent<AudioListener>();
        for (int i = 0; i < CharacterManager.Instance.charsInput.Length; ++i)
        {
            CharacterManager.Instance.charsInput[i].gameObject.SetActive(false);
        }
        if (PreLoadScene)
        {
            LoadLevelManager.Instance.LoadLevel(SaveManager.Instance.comingFromCollectables ? SaveManager.Instance.LevelToLoadCollectable : DefaultScene, true);
        }
        else
        {
            CollectableLoadLevel loaded = GetComponentInChildren<CollectableLoadLevel>();
            if (loaded != null)
            {
                loaded.PreloadScene = false;
                if (SaveManager.Instance.comingFromCollectables)
                {
                    loaded.sceneToLoad = SaveManager.Instance.LevelToLoadCollectable;
                }
                else
                {
                    loaded.sceneToLoad = DefaultScene;
                }
            }
        }

        if (CameraShake.Instance != null)
            CameraShake.Instance.StartShake(ShakePReset);
        //Aviso a HUD
        anim.SetTrigger("Play");
    }

    protected virtual void OnExitAction(Collider2D other)
    {
        //  isActive = false;
        // StopCoroutine("WaitingPortal");
    }

}
