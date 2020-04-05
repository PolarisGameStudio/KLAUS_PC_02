using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

public class TweenTextShowCutScene : MonoBehaviour
{
    public static float timeToShow = 0.5f;
    public static Vector3 DistanceToShow = Vector3.up * 0.5f;
    public bool moveLocally;


    public bool initTween = false;

    Renderer rend;
    TextMeshPro textPro;
    Color baseColor;
    LTDescr tween;

    void Awake()
    {
        if (!initTween)
        {
            DOTween.Init();
            initTween = true;
        }
        textPro = GetComponentInChildren<TextMeshPro>();
        rend = GetComponentInChildren<Renderer>();
        baseColor = textPro ? textPro.color : rend.material.color;
        baseColor.a = 0;

        if (textPro)
            textPro.color = baseColor;
        else
            rend.material.color = baseColor;

        baseColor.a = 1;


    }

    public void InitText()
    {
        if (initTween)
        {
            initTween = false;
        }

        tween = LeanTween.value(gameObject,
            delegate (Color obj)
            {
                if (textPro != null)
                    textPro.color = obj;
                else
                    rend.material.color = obj;
            },
            textPro ? textPro.color : rend.material.color,
            baseColor,
            timeToShow);

        if (!moveLocally)
            transform.DOMove(transform.position - DistanceToShow, timeToShow).From();
        else
            transform.DOLocalMove(transform.localPosition - DistanceToShow, timeToShow).From();
    }

    public void HideText()
    {
        if (initTween)
        {
            initTween = false;
        }

        Color hideColor = baseColor;
        hideColor.a = 0;

        tween = LeanTween.value(gameObject,
            delegate (Color obj)
            {
                if (textPro != null)
                    textPro.color = obj;
                else
                    rend.material.color = obj;
            },
            textPro ? textPro.color : rend.material.color,
            hideColor,
            timeToShow);

        //transform.DOMove(transform.position + DistanceToShow, timeToShow).From();
    }

    bool firstRun = true;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
    }

    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    #region Pause:

    int twennPaused = 0;

    public void OnPauseGame()
    {
        if (tween != null)
            tween.pause();
        twennPaused = 1;
        transform.DOPause();
    }

    public void OnResumeGame()
    {
        if (twennPaused > 0)
        {
            if (tween != null)
                tween.resume();
            twennPaused = 0;
            transform.DOPlay();
        }
    }

    #endregion

}
