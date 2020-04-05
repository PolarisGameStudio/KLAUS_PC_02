//
// EnterState.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class EnterDoorState : FSMState
{

    public Animator anim;
    Rigidbody2D _rig2D = null;

    public Rigidbody2D _rigidbody2D
    {

        get
        {

            if (_rig2D == null)
            {
                _rig2D = GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }

    }

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.EnterDoor;

    }
    public void SetElevator() {
        anim.SetBool("TurnAroundElevator", true);
    }

    public override void DoBeforeEntering()
    {
        //rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        anim.SetBool("TurnAround", true);
        anim.SetBool("isGround", true);
        anim.SetFloat("SpeedX",0);

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);

    }

    public override void DoBeforeLeaving()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        //rigidbody2D.isKinematic = false;
        anim.SetBool("TurnAround", false);
        anim.SetBool("TurnAroundElevator", false);

    }

    #region Pause:

    Vector2 velPause = Vector3.zero;

    public void OnPauseGame()
    {
        velPause = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        anim.speed = 0;
        _rigidbody2D.isKinematic = true;
    }

    public void OnResumeGame()
    {
        _rigidbody2D.isKinematic = false;

        _rigidbody2D.velocity = velPause;
        anim.speed = 1;
    }

    #endregion
}