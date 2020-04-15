using System.Collections;
using UnityEngine;
using Luminosity.IO;
using Rewired;

public class CheckControlHardware : MonoBehaviour
{
    public CanvasGroup arcadeCanvas = null;
    /*/
    void OnEnable()
    {
        StartCoroutine(CheckHardware());
    }
    /*/
    public static string DefaultGamePad { get; protected set; } = "keyboard";

    static string[] m_gamePad = { "xbox 360", "playstation(r)3", "wireless controller" };

    static string[] m_platform = { "win", "osx" };

    static bool firstTime = true;

    /// <summary>
    /// Gamepad which need normalizaiton between [0,1] -> [-1,1]
    /// </summary>
    static string[] m_normalization = { "Win Xbox" };

    public void Awake()
    {
        Cursor.visible = true;
        Setear360();
    }

    /*/
    IEnumerator CheckHardware()
    {
        while (true)
        {
            var controlsName = Input.GetJoystickNames();
            if (controlsName.Length > 0)
            {
                var control0 = "";
                //Bug of unity which return empty gamepad
                foreach (var controlN in controlsName)
                {
                    if (!string.IsNullOrEmpty(controlN))
                    {
                        control0 = controlN.ToLower();
                    }
                }

                if (!string.IsNullOrEmpty(control0) && firstTime)
                {
                    Debug.Log("Soy 360 siempre");
                    firstTime = false;
                    //                    Debug.Log(control0);
                    //Platform support
                    string platform = Application.platform.ToString().ToLower();
                    for (var i = 0; i < m_platform.Length; ++i)
                    {
                        if (platform.Contains(m_platform[i]))
                        {
                            platform = m_platform[i];
                            break;
                        }
                    }
                    //Generic GamePad we will use the XBOX
                    int indexToApply = 0;
                    //gamepad support
                    for (var i = 0; i < m_gamePad.Length; ++i)
                    {
                        if (control0.Contains(m_gamePad[i]))
                        {
                            indexToApply = i;
                            break;
                        }
                    }
                    InputEnum.GamePad = m_gamePad[indexToApply];
                    InputEnum.CONFIG = GetConfig(platform, m_gamePad[indexToApply]);

                    Cursor.visible = false;

                    for (var i = 0; i < m_normalization.Length; ++i)
                    {
                        if (m_normalization[i] == InputEnum.CONFIG)
                        {
                            InputEnum.USE_NORMALIZATION = true;
                            break;
                        }
                    }
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }

            if (controlsName.Length == -1)//To Avoid Warning
                break;
            yield return new WaitForSeconds(2.0f);

        }
    }
    /*/
    void Reset()
    {
        InputEnum.CONFIG = "";
        InputEnum.GamePad = DefaultGamePad;
        Cursor.visible = true;
    }

    bool AnyGamePadButtonPressed()
    {
        bool pressed = false;
                if (ReInput.players.GetPlayer(0).controllers.Joysticks[0].GetAnyButton())
                {

            Cursor.visible = false;
            // Debug.Log("I pressed a gamepadButton");
            pressed = true;

            var controlsName = ReInput.controllers.GetJoystickNames();

            if (controlsName.Length > 0)
            {
                var control0 = "";
                //Bug of unity which return empty gamepad
                foreach (var controlN in controlsName)
                {
                    if (!string.IsNullOrEmpty(controlN))
                    {
                        control0 = controlN.ToLower();
                        Debug.Log("THis is the controller name " + control0);
                        if (control0.Contains("nvidia"))
                        {
                            SaveManager.nvidia = true;
                        }
                        else
                        {
                            SaveManager.nvidia = false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(control0))
                {
                    //Debug.Log(control0);
                    //Platform support
                    string platform = Application.platform.ToString().ToLower();
                    for (var i = 0; i < m_platform.Length; ++i)
                    {
                        if (platform.Contains(m_platform[i]))
                        {
                            platform = m_platform[i];
                            break;
                        }
                    }
                    
                    //Generic GamePad we will use the XBOX
                    int indexToApply = 0;
                    //gamepad support
                    for (var i = 0; i < m_gamePad.Length; ++i)
                    {
                        if (control0.Contains(m_gamePad[i]))
                        {
                            indexToApply = i;
                            break;
                        }
                    }
                   
                    InputEnum.GamePad = m_gamePad[indexToApply];
                   
                    Debug.Log("Platform " + platform + " m_gamepad " + m_gamePad[indexToApply] + " index " + indexToApply);
                    /*/
                   InputEnum.CONFIG = GetConfig(platform, "xbox 360");  //you get the current platform + gamepad
                   for (var i = 0; i < m_normalization.Length; ++i)
                   {
                       if (m_normalization[i] == InputEnum.CONFIG)
                       {
                           InputEnum.USE_NORMALIZATION = true;
                           break;
                       }
                   }
                   /*/
                  
                }
                else
                {
                    //Reset();
                }
            }
            else
            {
               // Reset();
            }

        }
        return pressed;
    }

    bool AnyKeyboardButtonPressed()
    {
        bool pressed = false;
        if (Input.anyKeyDown && !ReInput.players.GetPlayer(0).controllers.Joysticks[0].GetAnyButton())
        {
            Debug.Log("I pressed a keyboard");
            InputEnum.GamePad = "keyboard";
            Cursor.visible = true;
            pressed = true;
        }
        return pressed;
    }


    string GetConfig(string platform, string gamePad)
    {
        return " " + platform + "_" + gamePad;
    }


        void LogMouseValues()
        {
            Mouse mouse = ReInput.controllers.Mouse;
            Debug.Log("Left Mouse Button = " + mouse.GetButton(0));
            Debug.Log("Right Mouse Button (Hold) = " + mouse.GetButton(1));
            Debug.Log("Right Mouse Button (Down) = " + mouse.GetButtonDown(1));
            Debug.Log("Right Mouse Button (Up) = " + mouse.GetButtonUp(1));
        }

        void LogPlayerJoystickValues()
        {
            // Log the button and axis values for each joystick assigned to this Player
            for (int i = 0; i < ReInput.players.GetPlayer(0).controllers.joystickCount; i++)
            {
                Joystick joystick = ReInput.players.GetPlayer(0).controllers.Joysticks[i];
                Debug.Log("Joystick " + i + ":");
                LogJoystickElementValues(joystick); // log all the element values in this joystick
            }
        }

        void LogJoystickElementValues(Joystick joystick)
        {
            // Log Joystick button values
            for (int i = 0; i < joystick.buttonCount; i++)
            {
                Debug.Log("Button " + i + " = " + joystick.Buttons[i].value); // get the current value of the button
            }

            // Log Joystick axis values
            for (int i = 0; i < joystick.axisCount; i++)
            {
                Debug.Log("Axis " + i + " = " + joystick.Axes[i].value); // get the current value of the axis
            }
        }
    

    private void Update()
    {
        //LogMouseValues();
        //LogPlayerJoystickValues();

        AnyGamePadButtonPressed();
        AnyKeyboardButtonPressed();

        /*/
            if (!Cursor.visible && InputEnum.GamePad == "keyboard")
            {
                if(arcadeCanvas!=null)
                {
                    if(!arcadeCanvas.interactable)
                    {
                        Cursor.visible = true;
                    }

                    if (arcadeCanvas.interactable)
                    {
                        Cursor.visible = false;
                    }
                }

                else
                {
                    Cursor.visible = true;
                }

            }            

            if (Cursor.visible && InputEnum.GamePad != "keyboard")
            {
                Cursor.visible = false;

            }

            if (Input.anyKey && AnyGamePadButtonPressed())
            {

               // Debug.Log("Esto es lo que es Jump ");
              //  Debug.Log(InputEnum.GetInputString(InputAction.Jump).ToString());
                Cursor.visible = false;
                Setear360();
            }


            if (Input.anyKey && !AnyGamePadButtonPressed())
            {
                //   Cursor.visible = true;
                if (arcadeCanvas != null)
                {
                    Reset();
                    if (!arcadeCanvas.interactable)
                    {

                        Cursor.visible = false;
                    }
                }

                else
                {
                    Reset();
                }

              //  Reset();
            }

            if (arcadeCanvas != null)
            {
                if (arcadeCanvas.interactable && Cursor.visible)
                {
                    Cursor.visible = false;
                }
            }
            /*/
        }

    private void Setear360()
    {
        var controlsName = ReInput.controllers.GetJoystickNames();

        if (controlsName.Length > 0)
        {
            var control0 = "";
            //Bug of unity which return empty gamepad
            foreach (var controlN in controlsName)
            {
                if (!string.IsNullOrEmpty(controlN))
                {
                    control0 = controlN.ToLower();
                    Debug.Log("THis is the controller name " + control0);
                    if (control0.Contains("nvidia"))
                    {
                        SaveManager.nvidia = true;
                    }
                    else
                    {
                        SaveManager.nvidia = false;
                    }
                }
            }

            if (!string.IsNullOrEmpty(control0))
            {
                Cursor.visible = false;
            }
        }

        else
        {
            Cursor.visible = true;

        }

            /*/
            //var controlsName = InputManager.GetJoystickNames();
            var controlsName = ReInput.controllers.GetJoystickNames();

            if (controlsName.Length > 0)
            {
                var control0 = "";
                //Bug of unity which return empty gamepad
                foreach (var controlN in controlsName)
                {
                    if (!string.IsNullOrEmpty(controlN))
                    {
                        control0 = controlN.ToLower();
                       Debug.Log("THis is the controller name " + control0);
                        if (control0.Contains("nvidia"))
                        {
                            SaveManager.nvidia = true;
                        }
                        else
                        {
                            SaveManager.nvidia = false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(control0))
                {
                    //Debug.Log(control0);
                    //Platform support
                    string platform = Application.platform.ToString().ToLower();
                    for (var i = 0; i < m_platform.Length; ++i)
                    {
                        if (platform.Contains(m_platform[i]))
                        {
                            platform = m_platform[i];
                            break;
                        }
                    }
                    //Generic GamePad we will use the XBOX
                    int indexToApply = 0;
                    //gamepad support
                    for (var i = 0; i < m_gamePad.Length; ++i)
                    {
                        if (control0.Contains(m_gamePad[i]))
                        {
                            indexToApply = i;
                            break;
                        }
                    }
                    InputEnum.GamePad = m_gamePad[indexToApply];
                    InputEnum.CONFIG = GetConfig(platform, m_gamePad[indexToApply]);  //you get the current platform + gamepad

                    Cursor.visible = false;

                    for (var i = 0; i < m_normalization.Length; ++i)
                    {
                        if (m_normalization[i] == InputEnum.CONFIG)
                        {
                            InputEnum.USE_NORMALIZATION = true;
                            break;
                        }
                    }
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
            /*/
        }
    }
