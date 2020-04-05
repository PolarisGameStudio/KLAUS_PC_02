using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class LadderState : FSMState, IButtonState
{
    public MoveState move;
    public Animator anim;
    public string AnimLadder = "isLadder";
    public string AnimLadderTRigerJump = "JumpLadder";
    public string AnimLadderSpeed = "SpeedLadder";
    Tweener twen;

    Rigidbody2D _rig2D = null;
    TimerVarHelper _timevar;
    TimerVarHelper timevar
    {
        get
        {
            if (_timevar == null)
                _timevar = GetComponent<TimerVarHelper>();
            return _timevar;
        }
    }
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

    public float SpeedY = 5.0f;
    [SerializeField]
    InputActionOld VerticalAxis = InputActionOld.Movement_Y;
    protected Dictionary<InputActionOld, float> buttonMapAxis = new Dictionary<InputActionOld, float>();

    protected bool isGround = true;
    public Transform groundCheck;


    public LayerMask whatIsGround;
    protected float groundRadius = 0.09f;
    protected float groundRadiusTotal
    {
        get
        {
            return groundRadius * transform.localScale.x;
        }
    }

    protected Collider2D[] result = new Collider2D[5];

    public Transform HeadCheck;
    public LayerMask layerLadder;
    public string TagLadder = "Ladder";

    private float storeGravityScale = 0;
    bool isForcing = false;

    public float timetoChangeAfterImpulse = 0.35f;
    public float ForceImpulse = 100.0f;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Ladder;
        AddTransition(Transition.Move_Ladder, StateID.Move);
        AddTransition(Transition.MoveToDead, StateID.Dead);

    }

    public override void DoBeforeEntering()
    {
        _rigidbody2D.velocity = Vector2.zero;
        //     ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        //    ManagerPause.SubscribeOnResumeGame(OnResumeGame);

        //Set ladder true
        storeGravityScale = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0;
        isGround = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, whatIsGround) > 0;
        for (int i = 0; i < result.Length; ++i)
        {
            if (result[i] != null)
            {
                if (result[i] == move.LadderTop)
                {
                    isGround = false;
                    break;
                }
            }
        }
        float xCenter = 0;
        if (move.Ladder != null)
        {
            xCenter = move.Ladder.transform.position.x;
        }
        else
        {
            xCenter = move.LadderTop.ParentTrigger.transform.position.x;
        }
        transform.position = new Vector3(xCenter, transform.position.y, transform.position.z);
        //    _rigidbody2D.MovePosition(new Vector2(move.Ladder.position.x, transform.position.y));
        //_rigidbody2D.position = new Vector2(move.Ladder.position.x, transform.position.y);
        anim.SetBool(AnimLadder, true);
        anim.SetFloat(AnimLadderSpeed, 1);
        anim.SetTrigger("LadderTrigger");
        twen = null;
    }

    public override void DoBeforeLeaving()
    {
        //  ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        //ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        //Set ladder false
        _rigidbody2D.gravityScale = storeGravityScale;
        if (move.LadderTop != null)
        {
            if (Physics2D.GetIgnoreCollision(move.LadderTop.colliderOneTop, move.colliders[0]))
            {
                for (int i = 0; i < move.colliders.Length; ++i)
                {
                    Physics2D.IgnoreCollision(move.LadderTop.colliderOneTop, move.colliders[i], false);
                }

            }
        }
        isForcing = false;
        anim.SetBool(AnimLadder, false);

    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause)
        {
            if (!isForcing)
            {
                if (move.LadderTop != null)
                {
                    if (!Physics2D.GetIgnoreCollision(move.LadderTop.colliderOneTop, move.colliders[0]))
                    {
                        for (int i = 0; i < move.colliders.Length; ++i)
                        {
                            Physics2D.IgnoreCollision(move.LadderTop.colliderOneTop, move.colliders[i], true);
                        }

                    }
                }
                #region Check ground
                isGround = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, whatIsGround) > 0;
                //Fix Loco
                for (int i = 0; i < result.Length; ++i)
                {
                    if (result[i] != null)
                    {
                        if (result[i] == move.LadderTop)
                        {
                            isGround = false;
                            break;
                        }
                    }
                }
                //
                anim.SetBool("isGround", isGround);
                #endregion

                float speedX = 0;
                float speedY = buttonMapAxis[VerticalAxis] * SpeedY;
                if (move.getOnPlatformLadder() != null)
                {
                    if ((move.getOnPlatformLadder().velocity.y > 0 && speedY > 0)
                        || (move.getOnPlatformLadder().velocity.y < 0 && speedY < 0))
                    {
                        speedY = speedY * move.percentPlatformMoveSameDir + move.getOnPlatformLadder().velocity.y;
                    }
                    else if ((move.getOnPlatformLadder().velocity.y > 0 && speedY < 0)
                             || (move.getOnPlatformLadder().velocity.y < 0 && speedY > 0))
                    {
                        speedY = speedY * move.percentPlatformMoveOposDir + move.getOnPlatformLadder().velocity.y;
                    }
                    else
                    {
                        speedY = speedY + move.getOnPlatformLadder().velocity.y;
                    }


                    speedX = move.getOnPlatformLadder().velocity.x;
                }
                _rigidbody2D.velocity = new Vector2(speedX, speedY);
                anim.SetFloat(AnimLadderSpeed, Mathf.Abs(buttonMapAxis[VerticalAxis]));

            }
            else
            {
                if (move.getOnPlatformLadder() != null)
                {
                    _rigidbody2D.velocity = move.getOnPlatformLadder().velocity;

                }
            }
        }
    }

    protected void GroundManagement()
    {
        if (buttonMapAxis[VerticalAxis] < -0.5f && move.LadderTop == null)
        {
            fsm.PerformTransition(Transition.Move_Ladder);
        }

    }

    protected override void Reason()
    {
        if (!ManagerPause.Pause && !isForcing)
        {

            if (Physics2D.OverlapCircleNonAlloc(HeadCheck.position, groundRadiusTotal, result, layerLadder) <= 0)
            {            //Fix Loco
                for (int i = 0; i < result.Length; ++i)
                {
                    if (result[i] != null)
                    {
                        if (result[i] == move.LadderTop)
                        {
                            if (buttonMapAxis[VerticalAxis] > 0f)
                            {
                                isForcing = true;
                                _rigidbody2D.velocity = Vector2.zero;
                                anim.SetTrigger(AnimLadderTRigerJump);

                                twen = transform.DOMoveY(move.LadderPosTop.position.y, timetoChangeAfterImpulse).OnComplete(OnCompleteMove);

                                //      StartCoroutine("ChangeToMove", timetoChangeAfterImpulse);
                            }
                            break;
                        }
                    }
                }


            }
            if (!isForcing)
            {
                if (move.PressJump && buttonMapAxis[VerticalAxis] < 0.5f && buttonMapAxis[VerticalAxis] > -0.5f)
                {
                    move.CloseLadderState();
                    move.FirstJump(0, 0, true);
                    timevar.StartTimer(move.OpenLadderState, 0.2f);
                    fsm.PerformTransition(Transition.Move_Ladder);
                    return;

                }
                move.CheckForceOutLadder();
                if (!move.canLadder)
                {
                    fsm.PerformTransition(Transition.Move_Ladder);
                    return;
                }


            }
        }
    }

    void OnCompleteMove()
    {
        fsm.PerformTransition(Transition.Move_Ladder);
    }

    protected override void LateUpdateChild()
    {
        if (!ManagerPause.Pause && !isForcing)
        {
            if (isGround)
            {
                GroundManagement();
            }
            else
            {
                if (!Mathf.Approximately(_rigidbody2D.gravityScale, 0.0f))
                    _rigidbody2D.gravityScale = 0;
            }
        }
    }

    #region Pause:

    Vector2 velPause = Vector3.zero;
    bool resume = true;

    public void OnPauseGame()
    {
        resume = false;

        velPause = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        anim.speed = 0;
        _rigidbody2D.isKinematic = true;
        if (twen != null)
            twen.Pause();
    }

    public void OnResumeGame()
    {
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.velocity = velPause;
        anim.speed = 1;
        Invoke("CanMoveAgain", 0.25f);
        if (twen != null)
            twen.Play();
    }
    void CanMoveAgain()
    {
        resume = true;
    }
    #endregion

    public void SetButton(InputActionOld button, bool value)
    {

    }

    public void SetButtonUp(InputActionOld button, bool value)
    {
    }

    public void SetButtonDown(InputActionOld button, bool value)
    {
    }

    public void SetButton(InputActionOld button, float value)
    {
        buttonMapAxis[button] = value;
    }
}
