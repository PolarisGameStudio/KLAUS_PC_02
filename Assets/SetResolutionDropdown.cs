using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class SetResolutionDropdown : MonoBehaviour {

    Dropdown dropdown;
    public Button dropdownButton;
    int initialvalue = 0;
    List<string> m_DropOptions = new List<string> {
        "640 x 480 (4:3)",
        "720 x 480 (3:2)",
        "720 x 576 (5:4)",
        "800 x 600 (4:3)",
        "1024 x 768 (4:3)",
        "1152 x 864 (4:3)",
        "1176 x 764 (16:9)",
        "1280 x 720 (16:9)",
        "1280 x 768 (5:3)",
        "1280 x 800 (16:10)",
        "1280 x 960 (4:3)",
        "1280 x 1024 (5:4)",
        "1360 x 768 (16:9)",
        "1366 x 768 (16:9)",
        "1440 x 900 (8:5)",
        "1600 x 900 (4:3)",
        "1600 x 1080 (16:10)",
        "1600 x 1200 (4:3)",
        "1680 x 1050 (14:19)",
        "1920 x 1080 (16:9)",
        "1920 x 1200 (16:10)",
        "2560 x 1080 (21:9)",
        "2560 x 1440 (16:9)",
        "3440 x 1440 (21:9)",
        "3840 x 2160 (16:9)",
    };

    public CanvasGroup option;
    public GameObject firstOption;
    public EventSystem evenSys;
    public NoMouseInputModule noMouse;

    int[][] resolutions = new int[][]
{
    new int[] {640,480},
    new int[] {720,480},
    new int[] {720,576},
    new int[] {800,600},
    new int[] {1024,768},
    new int[] {1152,864},
    new int[] {1176,764},
    new int[] {1280,720},
    new int[] {1280,768},
    new int[] {1280,800},
    new int[] {1280,960},
    new int[] {1280,1024},
    new int[] {1360,768},
    new int[] {1366,768},
    new int[] {1440,900},
    new int[] {1600,900},
    new int[] {1600,1080},
    new int[] {1600,1200},
    new int[] {1680,1050},
    new int[] {1920,1080},
    new int[] {1920,1200},
    new int[] {2560,1440},
    new int[] {3440,1440},
    new int[] {3840,2160},
};

   
// Use this for initialization
void Start () {
        

        int currentWith = Screen.width;
        int currentHeigh = Screen.height;

        int nativeWith = Screen.currentResolution.width;
        int nativeHeigh = Screen.currentResolution.height;

        //Debug.Log("Current res is" + currentWith + "x" + currentHeigh);

        int currentValue = 0;

        for (int i = 0; i < m_DropOptions.Count; i++)
        {
            if(SaveManager.Instance.dataKlaus!=null)
            { 
                if (m_DropOptions[i].Contains(SaveManager.Instance.dataKlaus.nativeWidth.ToString()) && m_DropOptions[i].Contains(SaveManager.Instance.dataKlaus.nativeHeight.ToString()))
                {
                    m_DropOptions[i] = m_DropOptions[i] + " native"; //sets the word "native" on the list

                }
            }

            if (m_DropOptions[i].Contains(Screen.width.ToString()) && m_DropOptions[i].Contains(Screen.height.ToString()))
            {
                currentValue = i;
            }
        }


        dropdown = gameObject.GetComponent(typeof(Dropdown)) as Dropdown;
        dropdown.AddOptions(m_DropOptions);
        dropdown.value = currentValue;
        dropdown.interactable = false;
    }

    public void SetInteractable()
    {
        dropdown.interactable = true;
    }

    public void SetDefault()
    {
        int currentValue = 0;

        for (int i = 0; i < m_DropOptions.Count; i++)
        {
            if (m_DropOptions[i].Contains("native"))
            {
                currentValue = i;
            }
        }
        dropdown.value = currentValue;
    }

    public void SetResolution()
    {
        dropdown.SetValueWithoutNotify(dropdown.value);
        Debug.Log("This is the value selected " + resolutions[dropdown.value][0].ToString() + "x" + resolutions[dropdown.value][1].ToString());
        Debug.Log("This is the game resolution " + Screen.width.ToString() + " X " + Screen.height.ToString());
        Debug.Log("This is your screen resolution " + Screen.currentResolution.width.ToString() + " X " + Screen.currentResolution.height.ToString());

        SaveManager.Instance.dataKlaus.reswidth = resolutions[dropdown.value][0];
        SaveManager.Instance.dataKlaus.resheight = resolutions[dropdown.value][1];

        Screen.SetResolution(resolutions[dropdown.value][0], resolutions[dropdown.value][1], SaveManager.Instance.dataKlaus.fullscreen, 60);
      //  Debug.LogError("FUll screen save data " + SaveManager.Instance.dataKlaus.fullscreen);
        dropdown.interactable = false;
       
      //  evenSys.SetSelectedGameObject(firstOption);

    }

    public void SaveInitialValue()
    {
        initialvalue = dropdown.value;


    }

    public void CompareValue()
    {

        if(dropdown.value==initialvalue)
        {
            dropdownButton.enabled = false;
            dropdownButton.enabled = true;

        }

    }

    // Update is called once per frame
    void Update () {
        
        if(!option.interactable && !dropdown.isActiveAndEnabled)
        {
            //toggleInteraction.isOn = true;
            Debug.Log("Hiciste hide del dropdown");
        }

        if (dropdown.IsInvoking("Hide"))
        {
            Debug.Log("Se escondio el dropdown");
        }

        if(noMouse!=null)
        { 
        if(!option.interactable && noMouse.enabled)
        {
                noMouse.enabled = false;
        }
        }


        //        dropdown.OnPointerExit();

    }



}
