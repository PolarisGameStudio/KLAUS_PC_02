using UnityEngine;
using System.Collections;

public class TriggerHistory : MonoBehaviour
{

    bool isActive = false;

    protected bool firstRun = true;

    public bool ActiveIfIsArcadeMode = false;
    protected virtual void Start()
    {
        if (SaveManager.Instance.comingFromTimeArcadeMode) {
            if (ActiveIfIsArcadeMode) {
                OnEnterAction(null);
                OnExitAction(null);

            } 
            gameObject.SetActive(false);
            return;
        }

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
    }

    protected virtual void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    protected virtual void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    public virtual void OnPauseGame()
    {

    }

    public virtual void OnResumeGame()
    {

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

    }

    protected virtual void OnExitAction(Collider2D other)
    {

    }
}
