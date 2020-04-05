using System.Collections.Generic;
using System;
using UnityEngine;
using Rewired;

[Serializable]
public enum InputActionOld
{
    Movement_X
    , Movement_Y
    , Action
    , Throw
    , Jump
    , Run
    , Planning
    , Click_Move_Platform
    , Click_Select_Platform
    , Move_Platform_X
    , Move_Platform_Y
    , Move_Camera_X
    , Move_Camera_Y
    , Click_Move_Camera
    , Change_Character
    , Select_All
    , Info
    , NextPage_Left
    , NextPage_Right
    , NextPage_Left_2
    , NextPage_Right_2
    , Move_Throw_X
    , Move_Throw_Y
    , Move_Select_Platform_X
    , UI_Restore
    , UI_Horizontal
    , UI_Vertical
    , UI_Submit
    , UI_Cancel
    , UI_Pause
    , Move_Select_Platform_Y

}
public static class InputEnum
{

    public static string[][] gamepadController = new string[][]
{
    new string[] {"LT","Select Both"},
    new string[] {"L","Change Character"},
    new string[] {"RT","Click Select Platform"},
    new string[] {"R","Click Move Camera"},
    new string[] {"Y","Throw"},
    new string[] {"B","Action"},
    new string[] {"R","Jump"},
    new string[] {"X","Run"},
};

    static Dictionary<InputActionOld, string> m_inputMap = new Dictionary<InputActionOld, string>()
    {
        {InputActionOld.Movement_X,"Move X"}
        ,{InputActionOld.Movement_Y,"Move Y"}
        ,{InputActionOld.Action, "Action"}
        ,{InputActionOld.Throw,"Throw"}
        ,{InputActionOld.Move_Throw_X,"Move X"}
        ,{InputActionOld.Move_Throw_Y,"Move Y"}
        ,{InputActionOld.Jump,"Jump"}
        ,{InputActionOld.Run,"Run"}
        ,{InputActionOld.Planning,"Jump"}
        ,{InputActionOld.Move_Platform_X,"Movement Platform X"}
        ,{InputActionOld.Move_Platform_Y,"Movement Platform Y"}
        ,{InputActionOld.Click_Move_Platform,"Click Move Platform"}
        ,{InputActionOld.Move_Camera_X,"Movement Camera X"}
        ,{InputActionOld.Move_Camera_Y,"Movement Camera Y"}
        ,{InputActionOld.Click_Move_Camera,"Click Move Camera"}
        ,{InputActionOld.Click_Select_Platform,"Click Select Platform"}
        ,{InputActionOld.Move_Select_Platform_X,"Movement Platform X"}
        ,{InputActionOld.Move_Select_Platform_Y,"Movement Platform Y"}
        ,{InputActionOld.Change_Character,"Change Character"}
        ,{InputActionOld.Select_All,"Select Both"}
        ,{InputActionOld.Info,"Select Both"}
        ,{InputActionOld.NextPage_Left,"Run"}
        ,{InputActionOld.NextPage_Right,"Select Both"}
        ,{InputActionOld.NextPage_Left_2,"Click Select Platform"}
        ,{InputActionOld.NextPage_Right_2,"Click Move Camera"}
        ,{InputActionOld.UI_Restore,"Change Character"}
        ,{InputActionOld.UI_Horizontal,"UIHorizontal"}
        ,{InputActionOld.UI_Vertical,"UIVertical"}
        ,{InputActionOld.UI_Submit,"Submit"}
        ,{InputActionOld.UI_Cancel,"Cancel"}
        ,{InputActionOld.UI_Pause,"Pause"}


    };

    static Dictionary<InputActionOld, string> m_inputMapBACKUP = new Dictionary<InputActionOld, string>();

    public static string GetInputString(InputActionOld actionI)
    {
        string result = "NULL";
        m_inputMap.TryGetValue(actionI, out result);
        return result;
    }

    static void CreateBackup()
    {
        if (m_inputMapBACKUP.Count != 0)
            return;

        foreach (var pair in m_inputMap)
        {
            m_inputMapBACKUP.Add(pair.Key, pair.Value);
        }
    }

    static void Restore()
    {

        m_inputMap.Clear();

        foreach (var pair in m_inputMapBACKUP)
        {
            m_inputMap.Add(pair.Key, pair.Value);
        }
    }
    public static string GamePad { get; set; } = CheckControlHardware.DefaultGamePad;

    static string m_config = "";
    public static string CONFIG
    {
        get
        {
            return m_config;
        }
        set
        {
            if (m_config != value)
            {
                USE_NORMALIZATION = false;//to safe check

                m_config = value;

                m_useControl = !string.IsNullOrEmpty(m_config);

                if (m_useControl)
                {
                    CreateBackup();
                    m_inputMap.Clear();

                    foreach (var pair in m_inputMapBACKUP)
                    {

                    //    Debug.Log("This is key " + pair.Key + " This is the value " + pair.Value+ "This is the config "+ m_config);

                        if(pair.Value=="Pause" || pair.Value == "Movement Platform Y")
                        {
                            if (SaveManager.nvidia)
                            {
                                m_inputMap.Add(pair.Key, pair.Value + " win_nvidia");
                            }
                            else
                            {
                                m_inputMap.Add(pair.Key, pair.Value + m_config);
                            }
                        }
                        else
                        {
                            m_inputMap.Add(pair.Key, pair.Value + m_config);
                        }
                           
                        
                    }
                }
                else
                {
                    Restore();
                }
            }

    
        }

       
    }

    static bool m_useControl = false;
    public static bool USE_CONTROL { get


        {
            if (GamePad == "keyboard") { return false; }
            else { return true; }
        }
    }

    static bool m_applyNormalization = false;
    public static bool USE_NORMALIZATION { get { return m_applyNormalization; } set { m_applyNormalization = value; } }

    public static float NormalizeTriggerL2(float input)
    {
        //[0,1] to [-1,1]
        if (USE_NORMALIZATION)
        {
            //Windows supports a 0 to 1 range for both triggers. Mac OS X supports -1 to 1,
            //however the trigger initially starts at 0 until it is first used.
            return UnityEngine.Mathf.Lerp(-1, 1, input);
        }

        return input;
    }

}
