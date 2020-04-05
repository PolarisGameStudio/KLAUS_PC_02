using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplaceTextSprite : MonoBehaviour {

    TextMeshProUGUI message;


    // Use this for initialization
    void Start () {
		message= gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;



    }


	
	// Update is called once per frame
	void Update () {
        if(message!=null)
        { 
            message.SetText(convertButtons(message.text));
        }
    }


    public string convertButtons(string message)
    {
        if(InputEnum.GamePad.ToString() == "xbox 360" || InputEnum.GamePad.ToString() == "wireless controller")
        {
           
            if (message.Contains("<sprite name=X") && !message.Contains("sprite name=X-A") && !message.Contains("sprite name=X-X") && !message.Contains("sprite name=X-Y") && !message.Contains("sprite name=X-B") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=X>", "<sprite name=X-A>"));
            }

            if (message.Contains("<sprite name=Square>") && InputEnum.GamePad.ToString() == "xbox 360" && !message.Contains("sprite name=X-A") && !message.Contains("sprite name=X-X") && !message.Contains("sprite name=X-Y") && !message.Contains("sprite name=X-B") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=Square>", "<sprite name=X-X>"));
            }

            if (message.Contains("<sprite name=Triangle>") && InputEnum.GamePad.ToString() == "xbox 360" && !message.Contains("sprite name=X-A") && !message.Contains("sprite name=X-X") && !message.Contains("sprite name=X-Y") && !message.Contains("sprite name=X-B") && !message.Contains("Yellow"))
            {
               // Debug.Log("Entre aca");
                message = (message.Replace("<sprite name=Triangle>", "<sprite name=X-Y>"));
            }

            if (message.Contains("<sprite name=Circle>") && !message.Contains("sprite name=X-A") && !message.Contains("sprite name=X-X") && !message.Contains("sprite name=X-Y") && !message.Contains("sprite name=X-B") && !message.Contains("Yellow"))
            {
                //Debug.Log("Entre aca");
                message = (message.Replace("<sprite name=Circle>", "<sprite name=X-B>"));
            }

            if (message.Contains("<sprite name=Down>"))
            {
                //Debug.Log("Entre aca");
                message = (message.Replace("<sprite name=Down>", "<sprite name=Down_X>"));
            }

            if (message.Contains("<sprite name=Move>") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=Move>", "<sprite name=Move_X>"));

            }

            if (message.Contains("<sprite name=MoveYellow>"))
            {
                message = (message.Replace("<sprite name=MoveYellow>", "<sprite name=MoveYellow_X>"));

            }


            if (message.Contains("<sprite name=EnterYellow>"))
            {
                message = (message.Replace("<sprite name=EnterYellow>", "<sprite name=EnterYellow_X>"));
            }



            if (message.Contains("<sprite name=BackYellow>"))
            {
                message = (message.Replace("<sprite name=BackYellow>", "<sprite name=BackYellow_X>"));
            }
            
        }

        if (InputEnum.GamePad.ToString() == "keyboard")
        {
            
            if (message.Contains("<sprite name=X-A") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=X-A>", "<sprite name=X>"));
            }

            if (message.Contains("<sprite name=X-X>") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=X-X>", "<sprite name=Square>"));
            }

            if (message.Contains("<sprite name=X-Y>") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=X-Y>", "<sprite name=Triangle>"));
            }

            if (message.Contains("<sprite name=X-B>") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=X-B>", "<sprite name=Circle>"));
            }
            if (message.Contains("<sprite name=Move_X>") && !message.Contains("Yellow"))
            {
                message = (message.Replace("<sprite name=Move_X>", "<sprite name=Move>"));
            }

            if (message.Contains("<sprite name=MoveYellow_X>"))
            {
                message = (message.Replace("<sprite name=MoveYellow_X>", "<sprite name=MoveYellow>"));

            }

            if (message.Contains("<sprite name=EnterYellow_X>"))
            {
                message = (message.Replace("<sprite name=EnterYellow_X>", "<sprite name=EnterYellow>"));
            }
         

            if (message.Contains("<sprite name=BackYellow_X>"))
            {
                message = (message.Replace("<sprite name=BackYellow_X>", "<sprite name=BackYellow>"));
            }

            if (message.Contains("<sprite name=Down_X>"))
            {
                //Debug.Log("Entre aca");
                message = (message.Replace("<sprite name=Down_X>", "<sprite name=Down>"));
            }

        }

        return message;

    }
}
