namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEditor;
    using Luminosity.IO;

    [RequireComponent(typeof(Text))]
    public class Set_Button_Text : MonoBehaviour
    {
        public string inputKey = "INSERT_KEY_HERE";
        public bool NegativeButton = false;
        Text textObject;
        /*/private InputBinding m_inputBinding;
        private InputAction m_inputAction;
        /*/

        public enum RebindType
        {
            Keyboard, GamepadButton, GamepadAxis
        }

        public string ObjectText
        {
            get
            {

                if (textObject == null)
                    textObject = this.GetComponent<Text>();

                return textObject.text;
            }
            set
            {
                if (textObject == null)
                    textObject = this.GetComponent<Text>();

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
                /*/
                var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

                SerializedObject obj = new SerializedObject(inputManager);

                SerializedProperty axisArray = obj.FindProperty("m_Axes");

                for (int i = 0; i < axisArray.arraySize; ++i)
                {
                    var axis = axisArray.GetArrayElementAtIndex(i);
                    var axisName = axis.FindPropertyRelative("m_Name").stringValue;
                    if (axisName == inputKey)
                    {
                        Debug.Log("This is the input " + axis.FindPropertyRelative("positiveButton").stringValue);

                        string tempTest = "";
                        if(NegativeButton)
                        {
                            tempTest = axis.FindPropertyRelative("negativeButton").stringValue.ToUpper();
                        }

                        else
                        {
                            tempTest = axis.FindPropertyRelative("positiveButton").stringValue.ToUpper();
                        }

                        if (tempTest.Contains("CTRL") || tempTest.Contains("SHIFT"))
                        {
                            tempTest = tempTest.Replace("LEFT", "L");
                            tempTest = tempTest.Replace("RIGHT", "R");
                            tempTest = tempTest.Replace("CTRL", "Ctrl");
                            tempTest = tempTest.Replace("SHIFT", "Shift");
                        }

                        tempTest = tempTest.Replace("ESCAPE", "Esc");


                        ObjectText = tempTest;


                        break;
                    }
                }

                UpdateInputKey();
                /*/
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

        void UpdateInputKey()
        {
            
        }

    }
}