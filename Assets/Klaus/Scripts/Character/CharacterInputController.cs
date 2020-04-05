//
// CharacterInputController.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Rewired;

public class CharacterInputController : InputController
{

    /// <summary>
    /// Use the inverse input for Axis
    /// </summary>
    public bool useInverseInput = false;
    [HideInInspector]
    public bool isBlock = false;
    public IMoveState[] move;
    public IButtonState[] buttState;

    [SerializeField]
    InputActionOld Control = InputActionOld.Movement_X;

    [SerializeField]
    InputActionOld[] getButtonDown;

    [SerializeField]
    InputActionOld[] getButton;//Klaus:left shoulder, square, cross - K1: left shoulder,circle, left stick vertical, cross, right shoulder

    [SerializeField]
    InputActionOld[] getButtonUp; // CrossSquare - K1 Cross

    [SerializeField]
    InputActionOld[] getButtonAxis;// Left Stick Vertical


    public float TimeToStartTakeInput = 2.0f;
    bool canTakeInput = false;

    void Awake()
    {
        Component[] comp2 = gameObject.GetComponents(typeof(IMoveState));
        move = new IMoveState[comp2.Length];
        for (int i = 0; i < comp2.Length; ++i)
        {
            move[i] = (IMoveState)comp2[i];
        }
        Component[] comp = gameObject.GetComponents(typeof(IButtonState));
        buttState = new IButtonState[comp.Length];
        for (int i = 0; i < comp.Length; ++i)
        {
            buttState[i] = (IButtonState)comp[i];
           
        }

        SetNoInput();

        Invoke("TakeInput", TimeToStartTakeInput);

    }

    void TakeInput()
    {
        canTakeInput = true;
    }

    protected override void ActionMovement(Vector2 movement)
    {
        if (!canTakeInput)
            return;

        for (int i = 0; i < move.Length; ++i)
            move[i].SetMovement(movement);
    }

    void LateUpdate()
    {
        if (!ManagerPause.Pause && canTakeInput)
        {
            foreach (var butt in getButton)
            {
                for (int i = 0; i < buttState.Length; ++i)
                {
                    buttState[i].SetButton(butt, ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(butt)));
                }
            }
            foreach (var butt in getButtonDown)
            {
                for (int i = 0; i < buttState.Length; ++i)
                {
                    buttState[i].SetButtonDown(butt, ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(butt)));
                }
            }
            foreach (var butt in getButtonAxis)
            {
                for (int i = 0; i < buttState.Length; ++i)
                    buttState[i].SetButton(butt, ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(butt)));
            }
            foreach (var butt in getButtonUp)
            {
                for (int i = 0; i < buttState.Length; ++i)
                    buttState[i].SetButtonUp(butt, ReInput.players.GetPlayer(0).GetButtonUp(InputEnum.GetInputString(butt)));
            }
        }
        else
        {
            SetNoInput();
        }
    }

    void OnDisable()
    {
        SetNoInput();
    }

    public void SetNoInput()
    {
        flickHandlerDirection(Vector2.zero);
        foreach (var butt in getButton)
        {
            for (int i = 0; i < buttState.Length; ++i)
                buttState[i].SetButton(butt, false);
        }
        foreach (var butt in getButtonDown)
        {
            for (int i = 0; i < buttState.Length; ++i)
                buttState[i].SetButtonDown(butt, false);
        }
        foreach (var butt in getButtonAxis)
        {
            for (int i = 0; i < buttState.Length; ++i)
                buttState[i].SetButton(butt, 0);
        }
        foreach (var butt in getButtonUp)
        {
            for (int i = 0; i < buttState.Length; ++i)
                buttState[i].SetButtonUp(butt, false);
        }
    }

    protected override void ControlPad()
    {
        if (!ManagerPause.Pause && canTakeInput)
        {
            //if (InputManager.GetAxisRaw(InputEnum.GetInputString(Control)) > 0)
            if (ReInput.players.GetPlayer(0).GetAxisRaw(InputEnum.GetInputString(Control)) > SaveManager.Instance.dataKlaus.controlSensitivity)
            {
              //  Debug.Log("This is the Joystick sensitivity " + ReInput.players.GetPlayer(0).GetAxisRaw(InputEnum.GetInputString(Control)));
                if (!useInverseInput)
                {
                    flickHandlerDirection(Vector2.right);
                }
                else
                {
                    flickHandlerDirection(Vector2.right * -1);
                }
            }
            else if (ReInput.players.GetPlayer(0).GetAxisRaw(InputEnum.GetInputString(Control)) < -SaveManager.Instance.dataKlaus.controlSensitivity)
            {
          //      Debug.Log("This is the Joystick sensitivity " + ReInput.players.GetPlayer(0).GetAxisRaw(InputEnum.GetInputString(Control)));
                if (!useInverseInput)
                {
                    flickHandlerDirection(Vector2.right * -1);
                }
                else
                {
                    flickHandlerDirection(Vector2.right);
                }
            }
            else
            {
                flickHandlerDirection(Vector2.zero);
            }
        }
    }
}
