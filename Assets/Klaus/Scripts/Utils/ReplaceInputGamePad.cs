using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReplaceInputGamePad : StandaloneInputModule
{
    public MouseOver[] mouseOver;
    public Dropdown[] dropdowns;
    public bool overMouseOver = false;

    public void Start()
    {
        /*/
        horizontalAxis = InputEnum.GetInputString(InputActionOld.UI_Horizontal);
        verticalAxis = InputEnum.GetInputString(InputActionOld.UI_Vertical);
        submitButton = InputEnum.GetInputString(InputActionOld.UI_Submit);
        cancelButton = InputEnum.GetInputString(InputActionOld.UI_Cancel);
        /*/

    }

    public override void Process()
    {
        if(horizontalAxis!= InputEnum.GetInputString(InputActionOld.UI_Horizontal))
        {
            horizontalAxis = InputEnum.GetInputString(InputActionOld.UI_Horizontal);
            verticalAxis = InputEnum.GetInputString(InputActionOld.UI_Vertical);
             submitButton = InputEnum.GetInputString(InputActionOld.UI_Submit);
            cancelButton = InputEnum.GetInputString(InputActionOld.UI_Cancel);

          
        }

        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        if (Cursor.visible)
        {
            overMouseOver = false;
            for (int i = 0; i < mouseOver.Length; i++)
             {
                 //if (mouseOver[i].selected)
                  //   overMouseOver = true;

                if (mouseOver[i].clicked)
                    overMouseOver = true;


            }



            for (int i=0; i<dropdowns.Length; i++)
            {
                if(dropdowns[i].interactable && !overMouseOver)
                {
                    overMouseOver = true;
                }
            }

    
            if (overMouseOver)
             {
                   ProcessMouseEvent();
            }

            if (!overMouseOver)
            {
                StopCoroutine("ProcessMouseEvent");
            }


        }
    }
}