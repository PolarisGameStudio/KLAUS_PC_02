using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using SmartLocalization.Editor;
using System.Text.RegularExpressions;
using Rewired;

public class HUD_LevelCompletedMenu : MonoSingleton<HUD_LevelCompletedMenu>
{
    public GameObject firstSelected;
    public EventSystem evenSys;
    public Animator anim;
    protected string NextScene = "";
    protected bool isPreloadScene = true;
    bool isLoading = false;

    public GameObject klausGear, k1Gear;
    public GameObject timePanel;
    public LocalizedText quoteText;
    public AudioMixer mixer;
    public AudioSource buttonSFX;
    public GameObject continueButton;

    Regex defaultWorldRegex = new Regex(@"W\d+L\d+-\d+");
    Regex numbersRegex = new Regex(@"\d+");

    public void ResetSelectFirst()
    {
        evenSys.SetSelectedGameObject(null);
    }

    public void SelectFirstButton()
    {
        evenSys.SetSelectedGameObject(firstSelected);
    }

    public void SetQuote(string quote)
    {
        string levelName = Application.loadedLevelName;

        if (defaultWorldRegex.IsMatch(levelName))
        {
            int world = 1;
            int chapter = 1;
            int section = 1;

            // Get world
            Match match = numbersRegex.Match(levelName);
            if (match.Success) world = int.Parse(match.Value);

            // Get chapter
            match = match.NextMatch();
            if (match.Success) chapter = int.Parse(match.Value);

            quoteText.UpdateKey("EndQuote" + world.ToString("00") + "." + chapter.ToString("00"));
        }
        else
        {
            quoteText.UpdateKey(quote);
        }
    }

    public void Update()
    {
        if(continueButton!=null)
        { 
            if(ReInput.players.GetPlayer(0).GetButton("Submit") && continueButton.activeSelf && GetComponent<CanvasGroup>().alpha!=0)
            {
                SelectButton();
            }
        }
    }

    public void Show(string level, bool preloadScene)
    {
        ResetSelectFirst();
        SelectFirstButton();
        NextScene = level;
        isPreloadScene = preloadScene;
        klausGear.SetActive(false);
        k1Gear.SetActive(false);

        // Turn on proper players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i != players.Length; ++i)
        {
            if (players[i].name.Contains("Klaus"))
                klausGear.SetActive(true);
            else if (players[i].name.Contains("K1"))
                k1Gear.SetActive(true);
        }

        // Turn on black line content
        HUD_TimeAttack hudTimeAttack = GameObject.FindObjectOfType<HUD_TimeAttack>();
        if (hudTimeAttack == null || !hudTimeAttack.ShowLevelCompleted())
        {
            timePanel.SetActive(false);
            quoteText.gameObject.SetActive(true);
        }
        else
        {
            timePanel.SetActive(true);
            quoteText.gameObject.SetActive(false);
        }

        // Check if K1 is alone on this level
        MoveStateKlaus[] klauses = GameObject.FindObjectsOfType<MoveStateKlaus>();
        if (klauses == null || klauses.Length == 0)
        {
            UIFontSetter fontSetter = quoteText.GetComponent<UIFontSetter>();
            if (fontSetter != null)
                fontSetter.ChangeFontType(FontsManager.FontType.K1);
        }

        quoteText.ObjectText = quoteText.ObjectText.ToUpper();
    }

    public void SelectButton()
    {
        if (!isLoading)
        {
            Debug.Log("I'm going to the next screen");
            CameraFade.StartAlphaFade(Color.black, false, 0.1f, 0.7f);
            CameraFade.Instance.m_OnFadeFinish += ActivateManualty;
            isLoading = true;
            mixer.FindSnapshot("Unpaused").TransitionTo(3f);
            buttonSFX.Play();
        }
    }
    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= ActivateManualty;
    }

    protected void ActivateManualty()
    {
        // LoadLevelManager.Instance.LoadLevelWithLoadingScene(NextScene, "Loading", false);
        Time.timeScale = 1;
        if (isPreloadScene)
            LoadLevelManager.Instance.ActivateLoadedLevel();
        else
            LoadLevelManager.Instance.LoadLevelWithLoadingScene(NextScene, false);
    }

    public void EnableEffects()
    {
        ManagerHudUI.Instance.effectsMultiplier = 1f;
    }

    public void EnableGlitch()
    {
        ManagerHudUI.Instance.glitchEnabled = true;
    }

    public void DisableGlitch()
    {
        ManagerHudUI.Instance.glitchEnabled = false;
    }
}
