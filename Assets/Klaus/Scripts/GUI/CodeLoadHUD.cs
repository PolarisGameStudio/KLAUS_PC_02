using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CodeLoadHUD : MonoBehaviour
{

    public SpriteRenderer BG;
    public SpriteRenderer Front;


    CodeState codePlayer;
    Vector3 startScale;
    Tweener tween = null;
    // Use this for initialization
    void Awake()
    {
        BG.enabled = false;
        Front.enabled = false;
        startScale = Front.transform.parent.localScale;
        codePlayer = gameObject.GetComponentInParent<CodeState>();
    }

    bool firstRun = true;
    void Start()
    {

        codePlayer.SetFunct(OnStartCode, OnCancelCode, OnFinishCode);
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
    }
    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
            ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
        }
    }
    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }
    void Reset()
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        Front.transform.parent.localScale = startScale;
        BG.enabled = false;
        Front.enabled = false;

    }

    void OnFinishCode()
    {
        Reset();
    }
    void OnStartCode(float time)
    {
        BG.enabled = true;
        Front.enabled = true;
        tween = Front.transform.parent.DOScaleX(0, time);
    }
    void OnCancelCode()
    {
        Reset();
    }


    #region Pause:
    public void OnPauseGame()
    {
        tween.Pause();
    }
    public void OnResumeGame()
    {
        tween.Play();
    }
    #endregion
}
