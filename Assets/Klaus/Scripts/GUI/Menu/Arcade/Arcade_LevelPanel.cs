using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SmartLocalization.Editor;

public class Arcade_LevelPanel : UIEntryList<Arcade_ButtonSelect>
{
    public ArcadeMenuPanel menu;
    public CanvasGroup canvas;
    public BackButtonMenu back;
    public LocalizedText Title;
    public Arcade_PopCheck popup;
    public AudioSource audio;
    //public LeaderboardsManager leaderboards;
    public MouseOver[] mbtns;
    public Collider2D[] Colmbtns;
    public bool[] currentMbts;

    public CanvasGroup loadingPanel;

    public Arcade_LevelScrollAnim rectScroll
    {
        get
        {
            if (_rectScroll == null)
                _rectScroll = GameObject.FindObjectOfType<Arcade_LevelScrollAnim>();
            return _rectScroll;
        }
    }

    Arcade_LevelScrollAnim _rectScroll;
    protected int currentLevel = 0;
    protected int currentWorld;

    #region Stuff

    void Awake()
    {
        InitPools(7);
    }

    void Start()
    {
        back.Callback = menu.ShowItemPanel;
    }

    public void SelectFirstButton()
    {
        if (currentItems != null && currentItems.Count != 0)
            SelectButton(0);
        else
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(currentItems[index].gameObject);
    }

    #endregion

    #region Setup

    public void SetLevel(string worldTitle, ArcadeLevelsInfo info)
    {
        currentWorld = info.worldID;
        Title.UpdateKey(worldTitle);

        Font font = null;
        int index = (int)FontsManager.FontType.Arcade;
        if (FontsManager.Instance.RegularFonts != null && FontsManager.Instance.RegularFonts.Length > index)
            font = FontsManager.Instance.RegularFonts[index];

        for (int i = 0; i != info.levels.Length; ++i)
        {
            string sceneName = info.levels[i].sceneName.Split('-')[0];

            Arcade_ButtonSelect instance = prefab.Spawn<Arcade_ButtonSelect>();
            instance.Setup(font,
                currentWorld, i, info.levels[i].sceneName, info.levels[i].sectionNames, info.levels[i].picture,
                info.levels[i].hasTimeAttack, SaveManager.Instance.dataKlaus.GetTime(sceneName), info.levels[i].companyRecordSeconds,
                CollectablesManager.GetCollectedPieces(sceneName), CollectablesManager.GetTotalPieces(sceneName)
            );
            Add(instance, false);
        }

        AdjustContainer();
    }

    public override void ClearEntry(Arcade_ButtonSelect entry)
    {
        entry.Clear();
        entry.Recycle<Arcade_ButtonSelect>();
    }

    #endregion

    #region Level selection

    public bool useTimeAttack = false;

    public void SelectLevel(int level)
    {
        audio.Play();
        back.enabled = false;
        menu.ChangeWithGlitch();

        // If this level doesn't have time attack, go to story mode directly
        if (!currentItems[currentLevel].timeAttackAvailable)
        {
            SumitSelectLevel(false);
        }
        // Else, show popup for the user to decide the mode
        else
        {
            currentLevel = level;
            popup.sections = currentItems[currentLevel].sectionNames;

            string sceneName = currentItems[currentLevel].sceneName.Split('-')[0];

            popup.Setup(
                currentItems[currentLevel].companyRecord,
                SaveManager.Instance.dataKlaus.GetTime(sceneName),
                CollectablesManager.GetCollectedPieces(sceneName),
                CollectablesManager.GetTotalPieces(sceneName));
            popup.Show();
        }
    }

    IEnumerator LoadLevel()
    {
        yield return StartCoroutine(menu.ChangeWithGlitch());

        // Cargo nivel
        Arcade_ButtonSelect button = FindItem(x => x.levelID == currentLevel);

        if (button != null)
        {
            if (popup.section <= 0 || popup.sections.Length == 0)
            {

            }
            else
            {
                if (popup.canChangeSection)
                    SaveManager.Instance.comingFromHistoryArcadeMode = false;
            }
            string sceneName = popup.section <= 0 || popup.sections.Length == 0 ? button.sceneName : popup.sections[popup.section];
            if (!string.IsNullOrEmpty(sceneName))
                LoadLevelManager.Instance.LoadLevelWithLoadingScene(sceneName, false);
        }
    }

    public void CancelSelectLevel()
    {
        canvas.interactable = true;
        back.enabled = true;

        // Hide Popup
        popup.Hide();
        EventSystem.current.SetSelectedGameObject(currentItems[currentLevel].gameObject);
    }

    public void SumitSelectLevel(bool isTime)
    {
        SaveManager.Instance.comingFromTimeArcadeMode = isTime;
        SaveManager.Instance.comingFromHistoryArcadeMode = !isTime;
        SaveManager.Instance.comingFromMemoryMode = false;
        SaveManager.Instance.LevelToLoadCollectable = string.Empty;

        SaveManager.Instance.lastArcadeLevel = currentItems[currentLevel].sceneName;

        StartCoroutine("LoadLevel");
    }

    public void SetWatchedLevel(int level)
    {
        currentLevel = level;
    }

    #endregion
    /*/
    #region Leaderboards

    void Update()
    {

        if (canvas.interactable && back.enabled && !leaderboards.enabled && Input.GetButtonDown(InputEnum.GetInputString(InputActionOld.Info)))
            ShowLeaderboards();

    }




    public void ShowLeaderboards()
    {
        back.enabled = false;
        canvas.interactable = false;
        EventSystem.current.SetSelectedGameObject(null);

        audio.Play();
        menu.ChangeWithGlitch();

        leaderboards.worldIndex = currentWorld;
        leaderboards.levelIndex = currentLevel;
        leaderboards.Show();
        leaderboards.back.Callback = HideLeaderboards;
    }

    public void HideLeaderboards()
    {
        leaderboards.Hide();

        back.enabled = true;
        canvas.interactable = true;
        EventSystem.current.SetSelectedGameObject(currentItems[currentLevel].gameObject);
    }
  
    #endregion
    /*/
    #region UI management

    public void Show()
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        back.enabled = true;

        for (int i = 0; i < currentMbts.Length; i++)
        {
            currentMbts[i] = false;
        }


        rectScroll.forceReset = true;
        SelectFirstButton();

    }

    public void Hide()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        back.enabled = false;

        RemoveAll();
    }

    #endregion
}
