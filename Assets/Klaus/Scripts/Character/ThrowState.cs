//
// ThrowState.cs
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

public class ThrowState : FSMState, IButtonState
{

    public Animator anim;

    public static float TimeToMoveAgain = 0.5f;
    [HideInInspector]
    public MoveStateKlaus
        bodyToForce;
    public MoveStateK1 statusVar;
    InputActionOld lounchButtonX = InputActionOld.Move_Throw_X;
    InputActionOld lounchButtonY = InputActionOld.Move_Throw_Y;
    InputActionOld accionButton = InputActionOld.Throw;

    public FlipSprite m_Flip;

    public Transform Arrow;
    public Vector2 ForceThrow = new Vector2(250, 800);

    bool isThrowing = false;
    bool throwBody = false;

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
    MoveState _movestate;
    public MoveState moveState
    {

        get
        {

            if (_movestate == null)
            {
                _movestate = GetComponent<MoveState>();
            }

            return _movestate;
        }

    }


    public void SetButton(InputActionOld button, bool value)
    {
        if (accionButton == button)
        {
            isThrowing = value;
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

    protected Vector2 dir = Vector2.zero;
    protected Vector2 DirForThrow = Vector2.zero;


    public delegate void onStartBroadcast();

    public event onStartBroadcast OnStart;

    public void SuscribeOnStart(onStartBroadcast funct)
    {
        OnStart += funct;
    }

    public void UnSuscribeOnStart(onStartBroadcast funct)
    {
        OnStart -= funct;
    }

    public delegate void onEndBroadcast();

    public event onEndBroadcast OnEnd;

    public void SuscribeOnEnd(onEndBroadcast funct)
    {
        OnEnd += funct;
    }

    public void UnSuscribeOnEnd(onEndBroadcast funct)
    {
        OnEnd -= funct;
    }

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Throw;
        AddTransition(Transition.ThrowToMove, StateID.Move);
        AddTransition(Transition.MoveToDead, StateID.Dead);

        bodyToForce = FindObjectOfType<MoveStateKlaus>();
        Arrow = (Transform)Instantiate(Arrow, transform.position, transform.rotation);
        Arrow.parent = transform;
        Arrow.gameObject.SetActive(false);
    }
    Vector2 GetDirection()
    {
        var direction = Vector2.zero;
        if (InputEnum.USE_CONTROL)
        {
            direction = new Vector2(ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(lounchButtonX)), ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(lounchButtonY)));
            Debug.Log("This is the direction X:" + direction.x + " Y:" + direction.y);
        }
        else
        {
            var k1pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            direction = Input.mousePosition - k1pos;
            direction.Normalize();
        }
        return direction;
    }
    public override void DoBeforeEntering()
    {
        bodyToForce.CanMove(false);
        anim.SetBool("Throw", false);
        anim.SetBool("isThrowing", true);
        anim.SetTrigger("ThrowTrigger");
        _rigidbody2D.velocity = Vector2.zero;
        isThrowing = true;
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);

        throwBody = false;
        if (OnStart != null)
            OnStart();

        dir = GetDirection();
        if (dir == Vector2.zero)
        {
            dir = Vector3.right;
        }
        DirForThrow = dir;
        Arrow.gameObject.SetActive(true);
        Arrow.rotation = Quaternion.Euler(Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(DirForThrow.y, DirForThrow.x));

    }

    public override void DoBeforeLeaving()
    {
        bodyToForce.CanMove(true);

        anim.SetBool("isThrowing", false);
        anim.SetBool("Throw", false);

        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        Arrow.gameObject.SetActive(false);
    }

    protected override void Reason()
    {
        if (resume)
        {
            _rigidbody2D.velocity = Vector2.zero;

            if (!throwBody)
            {
                //muestro la posicion de la flehca
                dir = GetDirection();
                if (dir == Vector2.zero)
                {
                    //Arrow.gameObject.SetActive(false);

                }
                else
                {
                    DirForThrow = dir;
                    Arrow.rotation = Quaternion.Euler(Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(DirForThrow.y, DirForThrow.x));
                }


                if (!isThrowing)
                {
                    anim.SetBool("Throw", true);
                    OnEnd?.Invoke();
                    throwBody = true;
                    bodyToForce.ApplyForce(DirForThrow.normalized, ForceThrow.x, ForceThrow.y, false);
                    StartCoroutine("MoveAgain", TimeToMoveAgain);
                }
                else
                {
                    if (!statusVar.CanThrowKlaus)
                    {
                        fsm.PerformTransition(Transition.ThrowToMove);
                    }
                }
            }
        }
    }
    protected override void FixedUpdateChild()
    {

        if (resume)
        {

            float speedPaltformX = 0;
            float speedPaltformY = _rigidbody2D.velocity.y;

            if (moveState.getOnPlatform() != null)
            {
                speedPaltformX = moveState.getOnPlatform().velocity.x;
                speedPaltformY = moveState.getOnPlatform().velocity.y;
            }
            _rigidbody2D.velocity = new Vector2(Mathf.Clamp(speedPaltformX, -moveState.MaxSpeed.x, moveState.MaxSpeed.x), Mathf.Clamp(speedPaltformY, -moveState.MaxSpeed.y, moveState.MaxSpeed.y));

            moveState.checkGround();
            if (moveState.isGround)
                moveState.CheckHeadKill();
        }
    }
    IEnumerator MoveAgain(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        fsm.PerformTransition(Transition.ThrowToMove);

    }

    #region Pause:

    bool resume = true;
    Vector2 velPause = Vector3.zero;

    public void OnPauseGame()
    {
        resume = false;
        velPause = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        anim.speed = 0;
        _rigidbody2D.isKinematic = true;
    }

    public void OnResumeGame()
    {
        resume = true;
        _rigidbody2D.isKinematic = false;

        _rigidbody2D.velocity = velPause;
        anim.speed = 1;
    }

    #endregion
}
