using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using Luminosity.IO;
using Rewired;

public class InGameSubPanel : MonoBehaviour
{
    public string nameShow;
    public CanvasGroup canvasGroup;
    public GameObject firstSelected;
    public GameObject SecondSelected;

    public EventSystem evenSys;

    public GameObject[] AllInfoShows;
    public GameObject[] InfoShow;
    public InMenuPanel panelDad;
    public BackButtonMenu back;
    public BackgroundSelect bgSelectItem;
    int countSubmit = 0;


    public MouseOver[] mbtns;
    public Collider2D[] Colmbtns;


    public bool[] currentMbts;

    public Button[] buttonsInteractive = null;
    public Slider[] slidersInteractive = null;
    public Toggle[] togglesInteractive = null;
    public Dropdown[] dropdownHide;
    public bool menuOption = false;


    InputActionOld InputSubmit = InputActionOld.UI_Submit;





    public void Update()
    {
        Debug.Log("This is the Gamepad " + InputEnum.GamePad);
        if (menuOption)
        { 
            if (buttonsInteractive[0].interactable && countSubmit > 0)
            {
                countSubmit = 0;
            }
            if (SubmitPressed() && !buttonsInteractive[0].interactable)
            {
                countSubmit++;

            }
            if ((!dropdownHide[0].interactable || (SubmitPressed() && countSubmit >= 2 )) && !buttonsInteractive[0].interactable)
            {


                countSubmit = 0;


                for (int i = 0; i < buttonsInteractive.Length; i++)
                {
                    if (!buttonsInteractive[i].interactable)
                        buttonsInteractive[i].interactable = true;
                }

                for (int i = 0; i < slidersInteractive.Length; i++)
                {
                    if (!slidersInteractive[i].interactable)
                        slidersInteractive[i].interactable = true;
                }

                for (int i = 0; i < togglesInteractive.Length; i++)
                {
                    if (!togglesInteractive[i].interactable)
                        togglesInteractive[i].interactable = true;
                }

                for (int i = 0; i < dropdownHide.Length; i++)
                {
                    dropdownHide[i].Hide();
                    dropdownHide[i].interactable = false;
                }

                SelectFirstButton();
                buttonsInteractive[1].enabled = false;
                buttonsInteractive[1].enabled = true;
            }
        }



        if (InputEnum.GamePad == "keyboard" && canvasGroup.interactable && canvasGroup.alpha==1 && mbtns !=null)
        {
            for (int i = 0; i < mbtns.Length; i++)
            {

                if (mbtns[i].selected && !currentMbts[i])
                {
                    EventSystem.current.SetSelectedGameObject(mbtns[i].gameObject);
                    currentMbts[i] = true;
                }
                if (!mbtns[i].selected && currentMbts[i])
                {
                    currentMbts[i] = false;
                }


                }
       
        }

    }


        void Start()
    {
        back.Callback = BackMenu;

        currentMbts = new bool[mbtns.Length];

        for (int i = 0; i < currentMbts.Length; i++)
        {
            currentMbts[i] = false;
        }

        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = false;
        }

    }
    public void BackMenu()
    {
        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = false;
        }
        ResetSelectFirst();
        bgSelectItem.Out();
        panelDad.StartOut();
        SaveManager.Instance.Save();
    }
    void ResetSelectFirst()
    {
        evenSys.SetSelectedGameObject(null);
    }

    public void SelectFirstButton()
    {
        if (firstSelected.GetComponent<Selectable>().interactable)
            evenSys.SetSelectedGameObject(firstSelected);
        else
            evenSys.SetSelectedGameObject(SecondSelected);

    }

    public void Show(CanvasGroup father)
    {
        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = true;
        }

        StartCoroutine(ShowCou(father));
    }
    IEnumerator ShowCou(CanvasGroup father)
    {
        ResetSelectFirst();
        while (!father.interactable)
            yield return null;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        yield return null;
        foreach (GameObject item in AllInfoShows)
            item.SetActive(System.Array.Find<GameObject>(InfoShow, x => x == item) != null);

        back.enabled = true;
        yield return null;
        SelectFirstButton();
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        foreach (GameObject item in AllInfoShows)
            item.SetActive(System.Array.Find<GameObject>(InfoShow, x => x == item) != null);

        back.enabled = true;
        SelectFirstButton();

    }

    public void Hide()
    {
        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = false;
        }
        StartCoroutine(HideCou());

    }
    public bool SubmitPressed()
    {
        var value = ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputSubmit));
        return value;
    }

    IEnumerator HideCou()
    {
        ResetSelectFirst();
        yield return null;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        yield return null;
        foreach (GameObject item in AllInfoShows)
            item.SetActive(false);

        back.enabled = false;
    }

    public void ToggleInteractions(bool enable)
    {
        canvasGroup.alpha = enable ? 1f : 0f;
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;
        back.enabled = enable;
    }

    public void OpenCreditsScene()
    {
        LoadLevelManager.Instance.LoadLevelImmediate("Credits");
    }
}
