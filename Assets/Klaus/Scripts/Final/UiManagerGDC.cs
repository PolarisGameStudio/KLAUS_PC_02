using UnityEngine;
using System.Collections;

public class UiManagerGDC : Singleton<UiManagerGDC> {

   // public UIWidget PauseMenu;
    public float TimeAlpha = 0.1f;
 //   public UIWidget TextPause;
    public Transform TransformShowTextPause;
    public Transform TransformOffTextPause;
    public float timeMovePause = 0.1f;
   // public UIWidget ListPausa;
    public Transform TransformShowListPausa;
    public Transform TransformOffListPausa;
    public float timeMoveListPausa = 0.1f;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(ShowPauseMenu);
        ManagerPause.SubscribeOnResumeGame(OffPauseMenu);
    }

    public void ShowPauseMenu()
    {
        /*PauseMenu.gameObject.SetActive(true);
       TweenTransform.Begin(TextPause.gameObject, timeMovePause, TransformOffTextPause, TransformShowTextPause);
        TweenTransform.Begin(ListPausa.gameObject, timeMoveListPausa, TransformOffListPausa, TransformShowListPausa);
        TweenAlpha.Begin(PauseMenu.gameObject, TimeAlpha, 1);*/

    }
    public void OffPauseMenu()
    {
        /*
        TweenTransform.Begin(ListPausa.gameObject, timeMoveListPausa, TransformShowListPausa, TransformOffListPausa);
        TweenTransform.Begin(TextPause.gameObject, timeMovePause, TransformShowTextPause, TransformOffTextPause);
        TweenAlpha.Begin(PauseMenu.gameObject, TimeAlpha, 0).onFinished.Add(new EventDelegate(this, "OffGameObject"));
        */
    }
    void OffGameObject()
    {
       /*
        PauseMenu.gameObject.SetActive(false);
        PauseMenu.GetComponent<TweenAlpha>().RemoveOnFinished(new EventDelegate(this, "OffGameObject"));
        * */
    }
    public void ResumeGame()
    {
        ManagerPause.Pause = false;
    }
    public void RestartLevel()
    {
        LoadLevelManager.Instance.RestartCurrentLevel();
    }
    public void RestartDemo()
    {
        LoadLevelManager.Instance.LoadLevelImmediate(0);
    }
}