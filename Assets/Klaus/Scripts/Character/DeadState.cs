//
// DeadState.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System;

public enum DeadType
{
    Normal,
    Ray,
    Acid,
    MegaRay
}

public class DeadState : FSMState
{

    public Animator anim;
    PlayerInfo myInfo;

    public float store_gravity = 2.2f;

    Rigidbody2D _rig2D = null;

    public Rigidbody2D rigidBody2D
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

    CharacterInputController _controllerInput = null;

    public CharacterInputController inputController
    {

        get
        {

            if (_controllerInput == null)
            {
                _controllerInput = GetComponent<CharacterInputController>();
            }

            return _controllerInput;
        }
    }

    [HideInInspector]
    public DeadType typeOfDead = DeadType.Normal;

    public delegate void onStartBroadcast();

    public event onStartBroadcast OnStart;

    public Action<Vector3> onRespawn;

    public GameObject SFX;

    public float deathTime = 0.8f;

    public Vector3 offSetRespawn = new Vector3(0, 0, 0);

    public float TimeToRemove = 0.6f;

    public float GravityScaleWhenDead = 0f;
    MoveState _move = null;
    public MoveState move
    {
        get
        {
            if (_move == null)
                _move = GetComponent<MoveState>();
            return _move;
        }
    }
    public const float FacctorSpeedReduce = 0.5f;
    [HideInInspector]
    public bool blockRespawn;

    public void SuscribeOnStart(onStartBroadcast funct)
    {
        OnStart += funct;
    }

    public void UnSuscribeOnStart(onStartBroadcast funct)
    {
        OnStart -= funct;
    }


    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Dead;

        AddTransition(Transition.DeadToMove, StateID.Move);


        StopCoroutine("ReloadLevel");

        myInfo = GetComponent<PlayerInfo>();
    }

    public override void DoBeforeEntering()
    {
       

        //   rigidBody2D.velocity = Vector2.zero;
        anim.SetBool("isDead", true);
        switch (typeOfDead)
        {
            case DeadType.Normal:
                break;
            case DeadType.Acid:
                anim.SetBool("DeadAcid", true);
                break;
            case DeadType.Ray:
                anim.SetBool("DeadRay", true);
                break;
            case DeadType.MegaRay:
                anim.SetBool("DeadRayForReal", true);
                break;
        }
        ManagerAnalytics.CharacterDied(Application.loadedLevelName,
    gameObject.name,
    transform.position.x,
    transform.position.y);

        anim.SetFloat("SpeedX", 0);

        anim.SetTrigger("firstIsDead");

        if (!blockRespawn)
            StartCoroutine("ReloadLevel", (deathTime));

        //ManagerPause.Pause = true;
        if (OnStart != null)
            OnStart();

        //KLVO DIGAMOS QUE AQUI LA MUSICA
        if (SFX != null)
            SFX.Spawn();

        ManagerStop.Instance.StopAll();
        if (SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.AddDeath();

        inputController.SetNoInput();

        if (!_controllerInput.enabled)
        {
            CameraFollow.Instance.ChangueTargetOnly(transform, deathTime + TimeToRemove);
        }
        rigidBody2D.gravityScale = GravityScaleWhenDead;
      
    }

    public override void DoBeforeLeaving()
    {
        StopCoroutine("ReloadLevel");
        anim.SetBool("isDead", false);
        rigidBody2D.velocity = Vector2.zero;
        typeOfDead = DeadType.Normal;
        anim.SetBool("DeadAcid", false);
        anim.SetBool("DeadRay", false);
        SFX = null;

        rigidBody2D.gravityScale = store_gravity;
        inputController.SetNoInput();

    }

    protected override void FixedUpdateChild()
    {
        // anim.SetBool("firstIsDead", false);
        if (!ManagerPause.Pause)
        {
            if (move != null)
                rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x * FacctorSpeedReduce, -move.MaxSpeed.x, move.MaxSpeed.x), Mathf.Clamp(rigidBody2D.velocity.y, -move.MaxSpeed.y, move.MaxSpeed.y));
        }
    }



    IEnumerator ReloadLevel(float time)
    {
        //Aqui deberia Invocar alguna Accion
        if (onRespawn != null)
            onRespawn(transform.position);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.isKinematic = true;
        //Invoco al manejador de Checkpoints
        float timeTOMove = ManagerCheckPoint.Instance.NeedWaitRespawn();
        Vector3 newPos = ManagerCheckPoint.Instance.PositionToRespawn(myInfo.playerType) + offSetRespawn;
        yield return new WaitForSeconds(timeTOMove);
        move.CurrentPlatformExit(move.getOnPlatform());
        transform.position = newPos;
        anim.SetBool("isDead", false);
        anim.SetBool("isGround", true);
        ManagerCheckPoint.Instance.ActivateCHeckpoint(myInfo.playerType);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToRemove));
        rigidBody2D.isKinematic = false;
        fsm.PerformTransition(Transition.DeadToMove);
        /*
        ManagerPause.Pause = true;

		ChangueLevelFade cha = new ChangueLevelFade();
		cha.ChangueTo(Application.loadedLevelName,1.0f);*/

    }

}
