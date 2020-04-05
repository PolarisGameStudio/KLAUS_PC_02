using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using SmartLocalization;


public class SetGamepadInput : MonoBehaviour {

    Dropdown dropdown;
    public Button dropdownButton;
    int initialvalue = 0;
    public string currentButton = "";
    public int currentValue = 0;
    List<string> m_DropOptionsKeys = new List<string> {
        "UI.Controls.PS4.R1",
        "UI.Controls.PS4.Triangle",
        "UI.Controls.PS4.Target",
        "UI.Controls.PS4.DPad",
        "UI.Controls.PS4.Circle",
        "UI.Controls.PS4.Square",
        "UI.Controls.PS4.X",
        "UI.Controls.PS4.L1",
    };

    List<string> m_DropOptionsValues = new List<string> {
        "Select Both Characters",
        "Switch Characters",
        "Target Platforms",
        "MoveCamera",
        "Throw Klaus",
        "Hack/Punch",
        "Jump",
        "Run",
    };

    public CanvasGroup option;
    public GameObject firstOption;
    public EventSystem evenSys;


   
// Use this for initialization
void Start () {

        //Subscribe to the change language event
        LanguageManager languageManager = LanguageManager.Instance;
        languageManager.OnChangeLanguage += OnChangeLanguage;

        //Run the method one first time
        OnChangeLanguage(languageManager);

        dropdown = gameObject.GetComponent(typeof(Dropdown)) as Dropdown;
        dropdown.AddOptions(m_DropOptionsValues);
       // dropdown.value = currentValue;
        dropdown.interactable = false;

        for (int i = 0; i < 8; i++)
        {

             if(SaveManager.Instance.dataKlaus.gamepadController[i][0]== currentButton)
            {
                if(SaveManager.Instance.dataKlaus.gamepadController[i][1]== "SelectBothCharacters")
                {
                    currentValue = 0;
                }
                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "SwitchCharacters")
                {
                    currentValue = 1;
                }
                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "Target")
                {
                    currentValue = 2;
                }
                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "MoveCamera")
                {
                    currentValue = 3;
                }
                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "Throw")
                {
                    currentValue = 4;
                }

                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "Hack/Punch")
                {
                    currentValue = 5;
                }

                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "Jump")
                {
                    currentValue = 6;
                }

                if (SaveManager.Instance.dataKlaus.gamepadController[i][1] == "Run")
                {
                    currentValue = 7;
                }
            }
         }

        dropdown.value = currentValue;


    }

    void OnChangeLanguage(LanguageManager languageManager)
    {

        for (int i = 0; i < m_DropOptionsKeys.Count; i++)
        {
            m_DropOptionsValues[i] = LanguageManager.Instance.GetTextValue(m_DropOptionsKeys[i]) ?? m_DropOptionsKeys[i];
        }
    }

    public void SetInteractable()
    {
        dropdown.interactable = true;
    }

    public void SetDefault()
    {
    }

    public void SetButtons()
    {

        currentValue = dropdown.value;

            for (int i = 0; i < m_DropOptionsKeys.Count; i++)
            {
                if (currentValue==0)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "SelectBothCharacters";
                }
                if (currentValue == 1)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "SwitchCharacters";
                }
                if (currentValue == 2)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "Target";
                }

                if (currentValue == 3)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "MoveCamera";
                }

                if (currentValue == 4)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "Throw";
                }

                if (currentValue == 5)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "Hack/Punch";
                }

                if (currentValue == 6)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "Jump";
                }

                if (currentValue == 7)
                {
                    SaveManager.Instance.dataKlaus.gamepadController[i][1] = "Run";
                }

            }
        
    }

    public void SaveInitialValue()
    {

    }

    public void CompareValue()
    {


    }

    // Update is called once per frame
    void Update () {
       

    }



}
