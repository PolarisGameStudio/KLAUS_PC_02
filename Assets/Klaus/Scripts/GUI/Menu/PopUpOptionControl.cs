using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Luminosity.IO;
using Rewired;

using System.Collections;

public class PopUpOptionControl : PopupOption
{
    public GameObject ControlVita;
    public GameObject ControlPS4;
    public GameObject ControlXBOX;
    public GameObject ControlKeyboard;
    InputActionOld InputBack = InputActionOld.UI_Cancel;
    public GameObject optionObject;
    public bool isMapping = false;
    public GameObject doneButton;

    public override void Show()
    {
        /*/
        #if UNITY_PSP2
        ControlVita.SetActive(true);
        #else
        ControlPS4.SetActive(true);
        #endif
        base.Show();
        /*/

        if(optionObject!=null)
        {
          //  optionObject.SetActive(false);
        }
        
      /*/ if (InputEnum.GamePad.ToString() == "xbox 360")
            ControlXBOX.SetActive(true);
        else
            ControlKeyboard.SetActive(true);
          /*/  
          if(doneButton!=null)
        {
            EventSystem.current.SetSelectedGameObject(doneButton);
        }

        base.Show();
    }

    public void Update()
    {
        if (BackPressed() && ManagerPause.Pause && !isMapping)
        {
            Debug.Log("I'm hiding");
            Hide();

        }
    }

    public void noMapping()
    {
        isMapping = false;
    }

    public void Mapping()
    {
        isMapping = true;
    }

    public bool BackPressed()
    {
        var value = ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputBack));
        return value;
    }


    public override void Hide()
    {
        /*/
        #if UNITY_PSP2
        ControlVita.SetActive(false);
        #else
        ControlPS4.SetActive(false);
        #endif
        base.Hide();
        /*/


        if (InputEnum.GamePad.ToString() == "xbox 360")
            ControlXBOX.SetActive(false);

        else
            ControlKeyboard.SetActive(false);

        base.Hide();
    }
}
