namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEditor;
    using TMPro;
    //using Luminosity.IO;
    using Rewired;


    public class Set_Button_Text_TUT : MonoBehaviour
    {
        public string inputKey = "INSERT_KEY_HERE";
        public bool NegativeButton = false;
        TextMeshPro textObject;
        private InputAction m_inputAction;



        public string ObjectText
        {
            get
            {

                if (textObject == null)
                    textObject = this.GetComponent<TextMeshPro>();

                return textObject.text;
            }
            set
            {
                if (textObject == null)
                    textObject = this.GetComponent<TextMeshPro>();

                textObject.text = value;
            }
        }
        void Start()
        {



            /*/
            m_inputAction = InputManager.GetAction("Klaus", inputKey);
            string tempTest = "";


            if (m_inputAction != null)
            {
                m_inputBinding = m_inputAction.Bindings[0];

                if (!NegativeButton)
                {
                    tempTest = m_inputBinding.Positive == KeyCode.None ? "" : m_inputBinding.Positive.ToString();
                }
                else
                {
                    tempTest = m_inputBinding.Negative == KeyCode.None ? "" : m_inputBinding.Negative.ToString();
                }

            }


            ObjectText = setTextParameters(tempTest);
            /*/

            setKey();
        }

        public void setKey()
        {
            string tempTest = "test";

            //tempTest = ReInput.players.GetPlayer(0).controllers.maps.GetFirstButtonMapWithAction(inputKey, true).elementIdentifierName;

            Player player = ReInput.players.GetPlayer(0);


            if (NegativeButton)
            {
                ActionElementMap aem = player.controllers.maps.GetFirstButtonMapWithAction(ControllerType.Keyboard, inputKey, true);

                tempTest = aem.keyCode.ToString();
            }

            else
            {

                foreach (ActionElementMap aem in player.controllers.maps.ButtonMapsWithAction(ControllerType.Keyboard, inputKey, true))
                {
                    Debug.Log("This foreach for " + inputKey);
                    Debug.Log("And this is the code this time " + aem.keyCode.ToString());
                    tempTest = aem.keyCode.ToString();
                }
            }




            ObjectText = setTextParameters(tempTest);
        }

        public string setTextParameters(string text)
        {
            text = text.ToUpper();
            if (text.Contains("CTRL") || text.Contains("SHIFT") || text.Contains("CONTROL"))
            {
                text = text.Replace("LEFT", "L");
                text = text.Replace("RIGHT", "R");
                text = text.Replace("CTRL", " Ctrl");
                text = text.Replace("CONTROL", " Ctrl");
                text = text.Replace("SHIFT", " Shift");
            }

            text = text.Replace("ESCAPE", "Esc");
            text = text.Replace("ARROW", "");

            return text;
        }

        void OnDestroy()
        {

        }

        public void UpdateInputKey()
        {

                setKey();


        }

    }
}