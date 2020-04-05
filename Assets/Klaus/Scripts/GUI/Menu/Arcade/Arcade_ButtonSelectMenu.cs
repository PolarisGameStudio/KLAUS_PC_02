using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;
using SmartLocalization;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Arcade_ButtonSelectMenu : MonoBehaviour
{
    public ArcadeMenuPanel menuPanel;
    public Arcade_ItemPanel itemPanel;
    public ArcadeLevelsInfo info;
    public Arcade_LevelPanel panelLevel;
    public bool glitchEnabled;

    public Color BackgroundColor;

    public Text text;
    public Color NormalText;
    public Color HighlightedText;
    public Color PressText;
    public Color DisableText;
    public bool back = false;

    public RectTransform rectTransform
    {
        get
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();
            return rect;
        }
    }

    string timeAttackValue, collectablesValue;
    RectTransform rect;

    static bool animating;

    public void SetNormalColor()
    {
        text.color = NormalText;
    }

    public void SetHighlightedColor()
    {
        text.color = HighlightedText;
    }

    public void SetPressColor()
    {
        text.color = PressText;
    }

    public void SetDisableColor()
    {
        if (!animating)
            text.color = DisableText;
    }

    public virtual void SelectThis()
    {


            //This hack is need because the event system call the wrong selectable 
            //before it call the right one
            if (gameObject != EventSystem.current.currentSelectedGameObject)
            {
                gameObject.SendMessage("OnDeselect", new BaseEventData(EventSystem.current));
                return;
            }
            itemPanel.levelSelector.Setup(rectTransform, BackgroundColor, glitchEnabled, timeAttackValue, collectablesValue);

        if (back)
        {
            itemPanel.levelSelector.HideSprites();
        }

        else
        {
            itemPanel.levelSelector.RestoreSprites();
        }
        
    }

    public void OnButtonSelected()
    {
        if (!animating)
            StartCoroutine("SelectElement");
    }

    IEnumerator SelectElement()
    {
        if(!back)
        { 
            animating = true;
            itemPanel.ToggleInteractions(false);
            yield return itemPanel.levelSelector.StartCoroutine("Blink");

            // Show Loading
            panelLevel.loadingPanel.alpha = 1f;
            yield return null;
            panelLevel.SetLevel(text.text, info);
            yield return null;
            panelLevel.loadingPanel.alpha = 0f;

            // Change panel
            menuPanel.ShowLevelPanel();
            animating = false;
        }
    }

    public void SelectWorldInmediatly()
    {
        if (!back)
        {
            itemPanel.ToggleInteractions(false);
            panelLevel.SetLevel(text.text, info);
            menuPanel.ShowLevelPanel();
        }
    }

    #region Level Info

    void OnEnable()
    {
        SaveManager.onGameLoaded += OnGameLoaded;
        OnGameLoaded();
    }

    void OnDisable()
    {
        SaveManager.onGameLoaded -= OnGameLoaded;
    }

    void OnGameLoaded()
    {
        if (!back)
        {
            // Set collectables info
            int currentPieces = 0;
            int totalPieces = 0;
            int currentSpeedruns = 0;
            int totalSpeedruns = 0;

            for (int i = 0; i != info.levels.Length; ++i)
            {
                string sceneName = info.levels[i].sceneName.Split('-')[0];

                currentPieces += CollectablesManager.GetCollectedPieces(sceneName);
                totalPieces += CollectablesManager.GetTotalPieces(sceneName);

                float currentScore = SaveManager.Instance.dataKlaus.GetTime(sceneName);
                currentSpeedruns += info.levels[i].hasTimeAttack && currentScore > 0 && currentScore < info.levels[i].companyRecordSeconds ? 1 : 0;
                totalSpeedruns += info.levels[i].hasTimeAttack ? 1 : 0;
            }

            timeAttackValue = currentSpeedruns + "/" + totalSpeedruns;
            collectablesValue = currentPieces + "/" + totalPieces;
        }
    }


    #endregion
}
