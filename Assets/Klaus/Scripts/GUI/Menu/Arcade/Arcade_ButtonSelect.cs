using UnityEngine;
using UnityEngine.UI;
using SmartLocalization.Editor;

public class Arcade_ButtonSelect : MonoBehaviour
{
    public int levelID { get; protected set; }
    public string sceneName { get; protected set; }
    public string[] sectionNames { get; protected set; }
    public bool timeAttackAvailable { get; protected set; }
    public float companyRecord { get; protected set; }

    public GameObject timeAttackObject, memoriesObject;
    public LocalizedText levelNameText;
    public Text bestText;
    public Text piecesText;
    public Image screenshot;

    public Image NonSelected;
    public Vector2 posScrollRect;

    public Arcade_LevelScrollAnim rectScroll
    {
        get
        {
            if (_rectScroll == null) _rectScroll = GameObject.FindObjectOfType<Arcade_LevelScrollAnim>();
            return _rectScroll;
        }
    }

    public Arcade_LevelPanel levelPanel
    {
        get
        {
            if (_levelPanel == null) _levelPanel = GameObject.FindObjectOfType<Arcade_LevelPanel>();
            return _levelPanel;
        }
    }

    public CanvasGroup canvasGroup;
    public Text[] texts;

    Arcade_LevelScrollAnim _rectScroll;
    Arcade_LevelPanel _levelPanel;

    #region Animations

    public void SetNormal()
    {
        NonSelected.enabled = true;
        canvasGroup.alpha = 0.4f;
    }

    public void SetHighlighted()
    {
        NonSelected.enabled = false;
        canvasGroup.alpha = 1;
        rectScroll.SetNewNormalPos(levelID);
        levelPanel.SetWatchedLevel(levelID);
    }

    public void SetPress()
    {
        NonSelected.enabled = false;
        canvasGroup.alpha = 1;
    }

    public void SetDisable()
    {
        //NonSelected.enabled = true;
        //canvasGroup.alpha = 0.4f;
    }

    #endregion

    public void Setup(Font font, int worldID, int levelID, string codeLevel, string[] sections, Sprite picture, bool hasTimeAttack = false, float timeRecord = 0f, float companyTime = 0f, int currentPieces = 0, int totalPieces = 0)
    {
        this.levelID = levelID;
        sceneName = codeLevel;
        sectionNames = sections;
        screenshot.sprite = picture;

        // Set fonts;
        if (font != null)
        {
            for (int i = 0; i != texts.Length; ++i)
            {
                if (texts[i].font.name != font.name)
                    texts[i].font = font;
            }
        }

        // Time attack
        timeAttackObject.SetActive(hasTimeAttack);
        timeAttackAvailable = hasTimeAttack;
        bestText.text = timeRecord > 0 ? HUD_TimeAttack.FormatTime(timeRecord) : "--:--:--";
        companyRecord = companyTime;

        // Collectibles
        memoriesObject.SetActive(totalPieces > 0);
        piecesText.text = currentPieces + "/" + totalPieces;

        string key = "LName" + (worldID + 1).ToString("00") + "." + (levelID + 1).ToString("00");

        if (key == "LName01.07")
            key = "W1Boss.01";
        else if (key == "LName04.07")
            key = "W6LBOSS-28";
        else if (key == "LName06.06")
            key = "W6LBOSS-19";
        
        levelNameText.UpdateKey(key);
    }

    public void Clear()
    {
        levelID = -1;
        sceneName = string.Empty;
        sectionNames = null;
        timeAttackAvailable = false;
        screenshot.sprite = null;
        companyRecord = 0f;
    }

    public void OnClicked()
    {
        levelPanel.SelectLevel(levelID);
    }

    public void Update()
    {/*/
        if(!mainButton.interactable && !nonSelected.enabled)
        {
            nonSelected.enabled = true;
        }

        if (mainButton.interactable && nonSelected.enabled)
        {
            nonSelected.enabled = false;
        }
        /*/
    }
}
