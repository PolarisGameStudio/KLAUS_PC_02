using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rewired;

public class Arcade_PopCheck : MonoBehaviour
{
    public Arcade_LevelPanel panel;
    public CanvasGroup canvas;
    public GameObject firstSelected;
    public EventSystem evenSys;

    InputActionOld InputBack = InputActionOld.UI_Cancel;
    protected bool isShow = false;

    public Text companyRecordText, localRecordText, collectiblesText;
    public Animator recordAnimator;

    [HideInInspector]
    public string[] sections;

    [HideInInspector]
    public int section = -1;

    public CanvasGroup sectionCanvas;
    public Text sectionText;

    Vector2 lastAxisValue;
    public bool canChangeSection = false;
    public void ResetSelectFirst()
    {
        evenSys.SetSelectedGameObject(null);
    }

    public void SelectFirstButton()
    {

        evenSys.SetSelectedGameObject(firstSelected);

    }

    public void Setup(float companyRecord, float localRecord, int localCollectibles, int totalCollectibles)
    {
        companyRecordText.text = HUD_TimeAttack.FormatTime(companyRecord);
        localRecordText.text = localRecord <= 0 ? "--:--:--.--" : HUD_TimeAttack.FormatTime(localRecord);
        recordAnimator.SetBool("Enabled", localRecord > 0 && localRecord < companyRecord);

        collectiblesText.text = localCollectibles.ToString("00") + "/" + totalCollectibles.ToString("00");
    }

    public void Show()
    {

        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        //ResetSelectFirst();
        SelectFirstButton();
        isShow = true;
        enabled = true;

        // DEBUG STUFF
        section = -1;
        ChangeSection(false);
    }

    public void Hide()
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        isShow = false;
        enabled = false;

        // DEBUG STUFF
        ChangeSection(false);
    }

    void ChangeSection(bool showCanvas = true)
    {
        if (sectionCanvas)
        {
            if (showCanvas && Mathf.Approximately(sectionCanvas.alpha, 0))
                section = 0;

            sectionCanvas.alpha = showCanvas ? 1f : 0f;
        }

        if (sectionText) sectionText.text = section == -1 ? string.Empty : "Section: " + sections[section];
    }

    void Update()
    {
        if (isShow && canvas.interactable)
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputBack)))
            {
                panel.CancelSelectLevel();
                isShow = false;
            }
            // DEBUG STUFF
            else if (sections.Length != 0 && canChangeSection)
            {
                if (ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Click_Select_Platform)) >= 0.9f && lastAxisValue.x < 0.9f)
                {
                    if (--section < 0) section = sections.Length - 1;
                    ChangeSection();
                }
                else if (ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Select_All)) >= 0.9f && lastAxisValue.y < 0.9f)
                {
                    if (++section == sections.Length) section = 0;
                    ChangeSection();
                }
            }
        }

        if (canChangeSection)
        {
            lastAxisValue.x = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Click_Select_Platform));
            lastAxisValue.y = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Select_All));
        }
    }

    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
