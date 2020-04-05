using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Steamworks;
using Luminosity.IO;
using Rewired;

public class PausaMenu_List : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup MenuList;
    public GameObject MenuListObject;
    public GameObject firstSelected;
    public EventSystem evenSys;

    public CanvasGroup canvasControl;
    public CanvasGroup MenuOption;
    public GameObject MenuOptionObject;
    public GameObject firstSelectedOption;
    public BackButtonMenu back;
    public Animator leaderboards;
    public Button leaderboardButton;
    public GameObject MenuListB01;
    public GameObject MenuListB02;
    public GameObject MenuListB03;
   // public GameObject MenuListB04;
    public GameObject MenuListB05;
    public GameObject MenuListB06;

    public GameObject MenuOption01;
    public GameObject MenuOption02;
    public GameObject MenuOption03;
    public GameObject MenuOption04;
    public GameObject MenuOption05;
    public GameObject MenuOption06;
    public GameObject MenuOption07;
    public GameObject MenuOption08;
    public GameObject MenuOption09;

    public GameObject MenuYes;
    public GameObject MenuNo;

    int countSubmit = 0;



    MouseOver Listbutton01;
    MouseOver Listbutton02;
    MouseOver Listbutton03;
   // MouseOver Listbutton04;
    MouseOver Listbutton05;
    MouseOver Listbutton06;



    MouseOver OptionButton01;
    MouseOver OptionButton02;
    MouseOver OptionButton03;
    MouseOver OptionButton04;
    MouseOver OptionButton05;
    MouseOver OptionButton06;
    MouseOver OptionButton07;
    MouseOver OptionButton08;
    MouseOver OptionButton09;

    MouseOver YesButton;
    MouseOver NoButton;

    Collider2D ListCol01;
    Collider2D ListCol02;
    Collider2D ListCol03;
    // MouseOver Listbutton04;
    Collider2D ListCol05;
    Collider2D ListCol06;
    Collider2D OptionCol01;
    Collider2D OptionCol02;
    Collider2D OptionCol03;
    Collider2D OptionCol04;
    Collider2D OptionCol05;
    Collider2D OptionCol06;
    Collider2D OptionCol07;
    Collider2D OptionCol08;
    Collider2D OptionCol09;
    Collider2D YesCol;
    Collider2D NoCol;

    public Button[] buttonsMenu;
    public Button[] buttonsInteractive;
    public Slider[] slidersInteractive;
    public Toggle[] togglesInteractive;
    public Dropdown[] dropdownHide;
    public SetResolutionDropdown setResolution;
    public bool onMapping = false;
    public NoMouseInputModule noMouse;

    InputActionOld InputSubmit = InputActionOld.UI_Submit;




    //Add all the buttons here and then call SetSelectedGameObject

    float alpha;
    float minEffectsMultiplier;

    public void isMapping()
    {
        onMapping = true;
    }

    public void unMapping()
    {
        onMapping = false;
    }

    void Awake()
    {

        back.Callback = Resume;
        leaderboards.GetComponent<BackButtonMenu>().Callback = HideLeaderboards;

        Listbutton01 = (MouseOver)MenuListB01.GetComponent(typeof(MouseOver));
        Listbutton02 = (MouseOver)MenuListB02.GetComponent(typeof(MouseOver));
        Listbutton03 = (MouseOver)MenuListB03.GetComponent(typeof(MouseOver));
     //   Listbutton04 = (MouseOver)MenuListB04.GetComponent(typeof(MouseOver));
        Listbutton05 = (MouseOver)MenuListB05.GetComponent(typeof(MouseOver));
        Listbutton06 = (MouseOver)MenuListB06.GetComponent(typeof(MouseOver));

        OptionButton01 = (MouseOver)MenuOption01.GetComponent(typeof(MouseOver));
        OptionButton02 = (MouseOver)MenuOption02.GetComponent(typeof(MouseOver));
        OptionButton03 = (MouseOver)MenuOption03.GetComponent(typeof(MouseOver));
        OptionButton04 = (MouseOver)MenuOption04.GetComponent(typeof(MouseOver));
        OptionButton05 = (MouseOver)MenuOption05.GetComponent(typeof(MouseOver));
        OptionButton06 = (MouseOver)MenuOption06.GetComponent(typeof(MouseOver));
        OptionButton07 = (MouseOver)MenuOption07.GetComponent(typeof(MouseOver));
        OptionButton08 = (MouseOver)MenuOption08.GetComponent(typeof(MouseOver));
        OptionButton09 = (MouseOver)MenuOption09.GetComponent(typeof(MouseOver));

        YesButton = (MouseOver)MenuYes.GetComponent(typeof(MouseOver));
        NoButton = (MouseOver)MenuNo.GetComponent(typeof(MouseOver));

        //GetColliders
        ListCol01 = (Collider2D)MenuListB01.GetComponent(typeof(Collider2D));
        ListCol02 = (Collider2D)MenuListB02.GetComponent(typeof(Collider2D));
        ListCol03 = (Collider2D)MenuListB03.GetComponent(typeof(Collider2D));

        ListCol05 = (Collider2D)MenuListB05.GetComponent(typeof(Collider2D));
        ListCol06 = (Collider2D)MenuListB06.GetComponent(typeof(Collider2D));

        OptionCol01 = (Collider2D)MenuOption01.GetComponent(typeof(Collider2D));
        OptionCol02 = (Collider2D)MenuOption02.GetComponent(typeof(Collider2D));
        OptionCol03 = (Collider2D)MenuOption03.GetComponent(typeof(Collider2D));
        OptionCol04 = (Collider2D)MenuOption04.GetComponent(typeof(Collider2D));
        OptionCol05 = (Collider2D)MenuOption05.GetComponent(typeof(Collider2D));
        OptionCol06 = (Collider2D)MenuOption06.GetComponent(typeof(Collider2D));
        OptionCol07 = (Collider2D)MenuOption07.GetComponent(typeof(Collider2D));
        OptionCol08 = (Collider2D)MenuOption08.GetComponent(typeof(Collider2D));
        OptionCol09 = (Collider2D)MenuOption09.GetComponent(typeof(Collider2D));

        YesCol = (Collider2D)MenuYes.GetComponent(typeof(Collider2D));
        NoCol = (Collider2D)MenuNo.GetComponent(typeof(Collider2D));

        OptionCol01.enabled = false;
        OptionCol02.enabled = false;
        OptionCol03.enabled = false;
        OptionCol04.enabled = false;
        OptionCol05.enabled = false;
        OptionCol06.enabled = false;
        OptionCol07.enabled = false;
        OptionCol08.enabled = false;
        OptionCol09.enabled = false;
        YesCol.enabled =false;
        NoCol.enabled = false;

        ListCol01.enabled = false;
        ListCol02.enabled = false;
        ListCol03.enabled = false;
        ListCol05.enabled = false;
        ListCol06.enabled = false;




        if (leaderboardButton != null)
            leaderboardButton.interactable = SaveManager.Instance.comingFromTimeArcadeMode;
    }

    public void ButtonsFalse()
    {
        OptionButton01.selected = false;
        OptionButton02.selected = false;
        OptionButton03.selected = false;
        OptionButton04.selected = false;
        OptionButton05.selected = false;
        OptionButton06.selected = false;
        OptionButton07.selected = false;
        OptionButton08.selected = false;
        OptionButton09.selected = false;


        Listbutton01.selected = false;
        Listbutton02.selected = false;
        Listbutton03.selected = false;
    //    Listbutton04.selected = false;
        Listbutton05.selected = false;
        Listbutton06.selected = false;

        evenSys.SetSelectedGameObject(null);
        
    }


    public void ResetSelectFirst()
    {
        ButtonsFalse();
        evenSys.SetSelectedGameObject(null);
    }

    public void SelectDropdownResolution()
    {
        ShowOption();
    }

    public void SelectDropdownLanguage()
    {
        ShowOption();
    }

    public void SelectFirstButton()
    {
        evenSys.SetSelectedGameObject(null);
        ButtonsFalse();
        evenSys.SetSelectedGameObject(firstSelected);
    }

    public void Resume()
    {
        ManagerPause.Pause = false;
    }


    public void ShowOption()
    {
        if (!ListCol01.enabled && setResolution != null)
        {
            Debug.Log("I have to setup the resolution here");
           setResolution.SetResolution();
        }


        MenuOptionObject.SetActive(true);
        MenuListObject.SetActive(false);
        evenSys.SetSelectedGameObject(null);
        
        ListCol01.enabled = false;
        ListCol02.enabled = false;
        ListCol03.enabled = false;
        ListCol05.enabled = false;
        ListCol06.enabled = false;
        
        OptionCol01.enabled = true;
        OptionCol02.enabled = true;
        OptionCol03.enabled = true;
        OptionCol04.enabled = true;
        OptionCol05.enabled = true;
        OptionCol06.enabled = true;
        OptionCol07.enabled = true;
        OptionCol08.enabled = true;
        OptionCol09.enabled = true;

        if(noMouse!=null)
        {
            noMouse.Activate();
            noMouse.enabled = true;
        }

        StopCoroutine("ShowOptionCou");
        StartCoroutine("ShowOptionCou");
        
    }


    IEnumerator ShowOptionCou()
    {

        back.Callback = HideOption;

        MenuList.alpha = 0;
        MenuList.blocksRaycasts = false;
        ResetSelectFirst();
        yield return null;
        MenuList.interactable = false;


        MenuOption.alpha = 1;
        MenuOption.interactable = true;
        MenuOption.blocksRaycasts = true;
        yield return null;
        evenSys.SetSelectedGameObject(null);

        ButtonsFalse();

        evenSys.SetSelectedGameObject(firstSelectedOption);

    }

    public void HideOption()
    {
        StopCoroutine("HideOptionCou");
        StartCoroutine("HideOptionCou");
        ListCol01.enabled = true;
        ListCol02.enabled = true;
        ListCol03.enabled = true;
        ListCol05.enabled = true;
        ListCol06.enabled = true;

        OptionCol01.enabled = false;
        OptionCol02.enabled = false;
        OptionCol03.enabled = false;
        OptionCol04.enabled = false;
        OptionCol05.enabled = false;
        OptionCol06.enabled = false;
        OptionCol07.enabled = false;
        OptionCol08.enabled = false;
        OptionCol09.enabled = false;
        MenuOptionObject.SetActive(false);
        MenuListObject.SetActive(true);

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

    }
    IEnumerator HideOptionCou()
    {
        back.Callback = Resume;

        MenuOption.alpha = 0;
        MenuOption.blocksRaycasts = false;
        ResetSelectFirst();
        yield return null;
        MenuOption.interactable = false;

        //SAve values of option:
        SaveManager.Instance.Save();

        MenuList.alpha = 1;
        MenuList.blocksRaycasts = true;
        MenuList.interactable = true;
        yield return null;
        SelectFirstButton();
    }



    public void ShowLeaderboards()
    {
        StopCoroutine("ShowLeaderboardsCou");
        StartCoroutine("ShowLeaderboardsCou");
    }

    IEnumerator ShowLeaderboardsCou()
    {
        ResetSelectFirst();
        leaderboards.SetTrigger("In");

        back.enabled = false;
        MenuList.blocksRaycasts = false;

        SetAlpha(0);
        alpha = 1;

        yield return null;

        MenuList.interactable = false;
    }

    public void HideLeaderboards()
    {
        StopCoroutine("HideLeaderboardsCou");
        StartCoroutine("HideLeaderboardsCou");
    }

    IEnumerator HideLeaderboardsCou()
    {
        leaderboards.SetTrigger("Out");

        SetAlpha(1);
        back.enabled = true;
        MenuList.interactable = true;
        MenuList.blocksRaycasts = true;
        yield return null;
        SelectFirstButton();
    }
    public void SetAlpha(float value)
    {
        canvasGroup.alpha = value;
        alpha = value;
    }

    void OnEnable()
    {
        minEffectsMultiplier = TitleLevelHUD.Instance.enabled ? ManagerHudUI.Instance.effectsMultiplier : 0f;

        ListCol01.enabled = true;
        ListCol02.enabled = true;
        ListCol03.enabled = true;
        ListCol05.enabled = true;
        ListCol06.enabled = true;
    }

    void OnDisable()
    {
        if (!ManagerHudUI.IsInstanceNull() && !TitleLevelHUD.IsInstanceNull())
            ManagerHudUI.Instance.effectsMultiplier = TitleLevelHUD.Instance.enabled ? minEffectsMultiplier : 0f;
    }

    public bool SubmitPressed()
    {
        var value = ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputSubmit));
        return value;
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");


            //  ManagerPause.Pause = true;

        }
        else
        {
            Debug.Log("Steam Overlay has been closed");


            //    ManagerPause.Pause = false;

        }
    }

    void Update()
    {
       if(!onMapping)
        { 

        if (buttonsInteractive[0].interactable && countSubmit>0)
        {
            countSubmit = 0;
        }
        if (SubmitPressed() && !buttonsInteractive[0].interactable)
        {
            countSubmit++;

        }
        if(SubmitPressed() && !buttonsInteractive[0].interactable && countSubmit>=2 && canvasControl.alpha == 0)
        {


            countSubmit = 0;
            ShowOption();

            for (int i = 0; i < buttonsInteractive.Length; i++)
            {
                if (!buttonsInteractive[i].interactable)
                {
                    buttonsInteractive[i].enabled = false;
                    buttonsInteractive[i].enabled = true;
                    buttonsInteractive[i].interactable = true;
                }
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


        }
       
        if (MenuList.interactable && InputEnum.GamePad == "keyboard")
        { 
            if (Listbutton01.selected)
            {
                evenSys.SetSelectedGameObject(MenuListB01);
            }
            if (Listbutton02.selected)
            {
                evenSys.SetSelectedGameObject(MenuListB02);
            }

            if (Listbutton03.selected)
            {
                evenSys.SetSelectedGameObject(MenuListB03);
            }

            if (Listbutton05.selected)
            {
                evenSys.SetSelectedGameObject(MenuListB05);
            }

            if (Listbutton06.selected)
            {
                evenSys.SetSelectedGameObject(MenuListB06);
            }
        }
       
        if (MenuOption.interactable && InputEnum.GamePad == "keyboard")
        {
            if (OptionButton01.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption01);
            }
            if (OptionButton02.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption02);
            }
         
            if (OptionButton03.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption03);
            }
            if (OptionButton04.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption04);
            }

            if (OptionButton05.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption05);
            }

            if (OptionButton06.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption06);
            }
            if (OptionButton07.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption07);
            }
            if (OptionButton08.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption08);
            }
            if (OptionButton09.selected)
            {
                evenSys.SetSelectedGameObject(MenuOption09);
            }


        }

        if (!MenuList.interactable && !MenuOption.interactable && InputEnum.GamePad == "keyboard" && MenuList.alpha == 1)
        {
            YesCol.enabled = true;
            NoCol.enabled = true;
            if (YesButton.selected)
            {
                evenSys.SetSelectedGameObject(MenuYes);
            }
            if (NoButton.selected)
            {
                evenSys.SetSelectedGameObject(MenuNo);
            }
        }

            ManagerHudUI.Instance.effectsMultiplier = alpha < minEffectsMultiplier ? minEffectsMultiplier : alpha;

        if(!NoCol.enabled && !buttonsMenu[buttonsMenu.Length-1].interactable)
        { 
            for (int i = 0; i < buttonsMenu.Length; i++)
            {
                if (!buttonsMenu[i].interactable)
                {
                    buttonsMenu[i].interactable = true;
                    Debug.Log("Interactable " + buttonsMenu[i].name);
                }
            }

        }

        if(canvasControl.alpha==0 && MenuOptionObject.activeSelf && !buttonsInteractive[0].interactable && !dropdownHide[0].interactable)
        {
            ShowOption();
            for (int i = 0; i < buttonsInteractive.Length; i++)
            {
                    buttonsInteractive[i].interactable = true;
            }



            for (int i = 0; i < slidersInteractive.Length; i++)
            {
                    slidersInteractive[i].interactable = true;
            }

            for (int i = 0; i < togglesInteractive.Length; i++)
            {
                    togglesInteractive[i].interactable = true;
            }


            for (int i = 0; i < dropdownHide.Length; i++)
            {
                dropdownHide[i].Hide();
                dropdownHide[i].interactable = false;
            }

            SelectFirstButton();
            
        }
        }
    }
}
