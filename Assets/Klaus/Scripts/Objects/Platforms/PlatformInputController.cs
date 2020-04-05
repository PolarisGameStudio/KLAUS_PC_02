//
// PlatformInputController.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Luminosity.IO;
using Rewired;




public class PlatformInputController : InputController
{

    public IMovePlatform move;

    public bool isVertical = false;

    public InputSelectionPlatform inputSelection;

    protected InputActionOld Control;
    protected InputActionOld ClickControl = InputActionOld.Click_Move_Platform;

    float SENSITIVE
    {
        get { if (InputEnum.USE_CONTROL) return 1.0f; return 1.0f; }
    }

    protected virtual void Awake()
    {
        move = (IMovePlatform)gameObject.GetComponent(typeof(IMovePlatform));
        if (isVertical)
        {
            Control = InputActionOld.Move_Platform_Y;
        }
        else
        {
            Control = InputActionOld.Move_Platform_X;
        }
    }
    protected virtual void OnDisable()
    {
        ResetDirection();

    }
    protected override void ActionMovement(Vector2 movement)
    {
        move.SetMovement(movement);
    }
    protected override void FixedUpdate()
    {

    }
    protected virtual void Update()
    {
        ControlPad();
    }
    float m_lastDir = 0;
    bool m_useLastDir = false;

    void ResetDirection()
    {
        m_useLastDir = false;
        flickHandlerDirection(Vector2.zero);

    }
    protected override void ControlPad()
    {
        if (!ManagerPause.Pause && ManagerPlatform.CanMovePlatform)//Second check because new gamepad behaviour
        {
            Debug.Log("I can move a platform");
            bool isClick = InputEnum.GamePad=="keyboard" && ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(ClickControl));

            if(!isClick)
            { 
            Debug.Log("It's not a click, this is the value "+ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(ClickControl)));

            }

            if (isClick || InputEnum.GamePad != "keyboard")
            {


                float direction = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(Control));
                Debug.Log("This is the direction " + direction);

                if (InputEnum.GamePad == "keyboard")
                    if (m_useLastDir && Mathf.Approximately(direction, 0.0f))
                        direction = m_lastDir;

                if (direction > 0)
                {
                    if (isVertical)
                    {
                        flickHandlerDirection(Vector2.up * SENSITIVE);
                    }
                    else
                    {
                        flickHandlerDirection(Vector2.right * SENSITIVE);
                    }
                    m_useLastDir = true;
                    m_lastDir = direction;
                }
                else if (direction < 0)
                {

                    if (isVertical)
                    {
                        flickHandlerDirection(Vector2.up * -1 * SENSITIVE);
                    }
                    else
                    {
                        flickHandlerDirection(Vector2.right * -1 * SENSITIVE);
                    }
                    m_useLastDir = true;
                    m_lastDir = direction;
                }
                else
                {
                    ResetDirection();
                }
            }
            else
            {
                ResetDirection();
            }
        }
        else
        {
            ResetDirection();

        }
    }

    public void ToggleControl(bool on)
    {
        if (inputSelection) inputSelection.gameObject.SetActive(on);
    }
}


