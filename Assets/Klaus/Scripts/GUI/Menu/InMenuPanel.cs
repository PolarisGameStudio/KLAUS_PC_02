using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SmartLocalization.Editor;
using Steamworks;

[RequireComponent(typeof(Animator))]

public class InMenuPanel : MonoBehaviour
{
    private Animator anim;

    public Animator animator
    {
        get
        {
            if (anim == null)
                anim = GetComponent<Animator>();

            return anim;
        }
    }

    public InGameSubPanel newGame;
    public InGameSubPanel option;
    public InGameSubPanel extras;
    public InGameSubPanel exitGame;

    public LocalizedText nameShow;
    public Text nameShowText;

    protected InGameSubPanel current;
    public SpriteRenderer black;



    public string newGameScene = "VideoIntro";
    public void NewGameYes()
    {
        SaveManager.Instance.SetHistory();
        SaveManager.Instance.dataKlaus.SetCurrentLevel("W1L01-1");

        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += ActivateManualty;

    }

    public void exitGameYes()
    {
        SaveManager.Instance.Save();
        SteamAPI.Shutdown();
        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.2f);
        StartCoroutine(WaitSeconds());
        CameraFade.Instance.m_OnFadeFinish += QuitGame;
  
        
    }

    IEnumerator WaitSeconds()
    {
        Debug.LogError("Waiting for shutting down");
        yield return new WaitForSeconds(5);
        Debug.LogError("Done waiting");
    }

    void OnDestroy()
    {
        if (current == exitGame)
        {
            SteamAPI.Shutdown();
        }

        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= ActivateManualty;
    }

    protected void QuitGame()
    {

        CameraFade.Instance.image.color = new Color(0, 0, 0, 255);//extra code to leave it black
        black.enabled = true;
        MonoBehaviour[] scripts = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        Application.Quit();
    }

    protected void ActivateManualty()
    {
        black.enabled = true;
        LoadLevelManager.Instance.LoadLevelImmediate(newGameScene);
    }

    public void ShowNewGame()
    {
        nameShow.UpdateKey(newGame.nameShow);
        newGame.Show(GetComponent<CanvasGroup>());
        current = newGame;
      
        nameShow.localizedKey = "UI.MainMenu.NewGame";
    }

    public void ShowExitGame()
    {
        nameShow.UpdateKey(exitGame.nameShow);
        exitGame.Show(GetComponent<CanvasGroup>());
        current = exitGame;
       
        nameShow.localizedKey = "UI.Pause.Exit";
    }

    public void ShowOption()
    {
        nameShow.UpdateKey(option.nameShow);
        option.Show(GetComponent<CanvasGroup>());

        current = option;
    }

    public void ShowExtras()
    {
        nameShow.UpdateKey(extras.nameShow);
        extras.Show(GetComponent<CanvasGroup>());

        current = extras;
    }

    public void Hide()
    {
        if (current != null)
        {
            current.Hide();
            current = null;
        }
    }

    public void StartOut()
    {
        animator.SetTrigger("Out");
    }

    public void ChangueBackMenu()
    {
        ManagerMenuUI.Instance.ChangueToMenuMenu();

    }
}
