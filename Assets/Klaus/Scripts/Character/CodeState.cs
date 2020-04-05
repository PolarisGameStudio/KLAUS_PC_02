//
// CodeState.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Luminosity.IO;


public class CodeState : FSMState, IButtonState
{
    public Animator anim;
    [SerializeField]
    InputActionOld accionButton = InputActionOld.Action;

    public void SwapActionWithJump(InputActionOld actionB)
    {
        accionButton = actionB;
    }

    public float TimeToFinish = 3.0f;

    public bool NearPc { get; private set; }
    public void SetHack(float timehack, onStartCodeBroadcast funct1, onCancelCodeBroadcast funct3, onFinishCodeBroadcast funct2)
    {

        NearPc = true;
        OnFinishCode += funct2;
        OnStartCode += funct1;
        OnCancelCode += funct3;
        TimeToFinish = timehack;
    }
    public void SetFunct(onStartCodeBroadcast funct1, onCancelCodeBroadcast funct3, onFinishCodeBroadcast funct2)
    {
        OnFinishCode += funct2;
        OnStartCode += funct1;
        OnCancelCode += funct3;
    }
    public void ClearHack(onStartCodeBroadcast funct1, onCancelCodeBroadcast funct3, onFinishCodeBroadcast funct2)
    {
        NearPc = false;
        OnFinishCode -= funct2;
        OnStartCode -= funct1;
        OnCancelCode -= funct3;
    }

    public void SetButton(InputActionOld button, bool value)
    {
        //Aqui e suna prueba donde ocupa mas cpu pero menos memoria
        if (accionButton == button)
        {
            codeInput = value;
        }
    }
    public void SetButton(InputActionOld button, float value)
    {

    }
    public void SetButtonUp(InputActionOld button, bool value)
    {
    }
    public void SetButtonDown(InputActionOld button, bool value)
    {
    }
    protected bool codeInput = false;
    public bool isCoding
    {
        get
        {
            return codeInput;
        }
    }

    bool compileCode = false;

    #region StartDelegate
    public delegate void onFinishCodeBroadcast();
    public event onFinishCodeBroadcast OnFinishCode;

    public delegate void onStartCodeBroadcast(float time);
    public event onStartCodeBroadcast OnStartCode;

    public delegate void onCancelCodeBroadcast();
    public event onCancelCodeBroadcast OnCancelCode;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Code;
        AddTransition(Transition.CodeToMove, StateID.Move);
        AddTransition(Transition.MoveToDead, StateID.Dead);


    }

    public override void DoBeforeEntering()
    {
        Debug.Log("Entre el golpe");
        compileCode = false;
        //rigidbody2D.isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        anim.SetBool("TurnAround", true);
        anim.SetBool("isCoding", true);
        anim.SetFloat("SpeedX", 0);

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);

        if (OnStartCode != null)
            OnStartCode(TimeToFinish);


        StartCoroutine("StartCode", TimeToFinish);
    }
    public override void DoBeforeLeaving()
    {

        //rigidbody2D.isKinematic = false;
        anim.SetBool("isCoding", false);
        anim.SetBool("TurnAround", false);

        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        //ClearHack();
    }

    protected override void Reason()
    {
        if (resume)
        {
            if (compileCode)
            {
                fsm.PerformTransition(Transition.CodeToMove);
            }
            else if (!isCoding)
            {
                if (OnCancelCode != null)
                    OnCancelCode();
                StopCoroutine("StartCode");
                fsm.PerformTransition(Transition.CodeToMove);
            }
        }
    }

    IEnumerator StartCode(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        compileCode = true;
        if (OnFinishCode != null)
            OnFinishCode();
    }

    public void CancelAllCoding()
    {
        if (OnCancelCode != null)
            OnCancelCode();
        StopCoroutine("StartCode");
        fsm.PerformTransition(Transition.CodeToMove);
    }


    #region Pause:
    bool resume = true;
    Vector2 velPause = Vector3.zero;
    public void OnPauseGame()
    {
        resume = false;
        velPause = GetComponent<Rigidbody2D>().velocity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        anim.speed = 0;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    public void OnResumeGame()
    {
        resume = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        GetComponent<Rigidbody2D>().velocity = velPause;
        anim.speed = 1;
    }
    #endregion
}