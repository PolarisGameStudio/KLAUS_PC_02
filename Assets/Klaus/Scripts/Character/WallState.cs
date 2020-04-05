using UnityEngine;
using System.Collections;
using Luminosity.IO;

public class WallState : FSMState, IButtonState, IMoveState
{
    public Animator anim;
    public FlipSprite Flip;
    [Header("Input")]
    [SerializeField]
    InputActionOld VerticalAxis = InputActionOld.Movement_Y;
    [SerializeField]
    InputActionOld JumpButton = InputActionOld.Jump;
    protected Vector2 inputDirection = Vector2.zero;
    protected float directionAxis = 0;
    protected bool jumpInput = false;
    protected bool jumpInputDown = false;
    public float InputForWallAgain = 0.25f;
    [Header("Fisica")]
    public string TagWall = "WallJump";
    public LayerMask whatIsWallJump;
    protected Collider2D[] result = new Collider2D[5];
    public float wallRadius = 0.09f;
    public float wallRadiusTotal
    {
        get
        {
            return wallRadius * transform.localScale.x;
        }
    }

    public Transform leftSide;
    public Transform rigthSide;
    Rigidbody2D _rigid;
    bool isJump = false;
    public float OffSetSpriteX = 0;
    public float MultyplyForceOtherDireccion = 2.0f;
    public Vector2 FactorForNormalSpeed = new Vector2(1.0f, 1.0f);
    public Vector2 FactorForRunSpeed = new Vector2(1.1f, 1.1f);

    public Rigidbody2D rigidBody2D
    {
        get
        {


            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody2D>();
            }

            return _rigid;

        }
    }

    [Header("Wall Logic")]
    public MoveState move;

    protected bool canCheckWallJump = false;
    protected bool canInputCheckWallJump = true;

    public void SetInWall(bool value)
    {
        canCheckWallJump = value;
    }

    bool isLeftSide = false;
    bool isRigthSide = false;
    public float DownVelocity = 0.5f;
    bool isChange = false;
    public TimerVarHelper timerVar;

    public Vector2 force = new Vector2(20, 30);
    public float TimeNoInput = 0.5f;
    public bool useJump = false;
    [Header("FX")]
    public GameObject FX;

    [Header("Audio")]
    public AudioSource jumpSFX;
    #region IButtonState implementation

    public void SetButton(InputActionOld button, bool value)
    {
        if (JumpButton == button)
        {
            jumpInput = value;
        }
    }

    public void SetButtonUp(InputActionOld button, bool value)
    {
    }

    public void SetButtonDown(InputActionOld button, bool value)
    {
        if (JumpButton == button)
        {
            jumpInputDown = value;
        }
    }

    public void SetButton(InputActionOld button, float value)
    {
        if (VerticalAxis == button)
        {
            directionAxis = value;
        }
    }

    public void SetMovement(Vector2 move)
    {
        inputDirection = move;
    }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.WallJump;
        AddTransition(Transition.Move_WallJump, StateID.Move);
        AddTransition(Transition.MoveToDead, StateID.Dead);

    }

    bool needResetInput = false;

    public override void DoBeforeEntering()
    {
        canWallJump();
        rigidBody2D.velocity = Vector2.zero;
        isChange = false;
        anim.SetBool("isWallJump", true);
        anim.SetTrigger("WallTrigger");
        isJump = false;
        Flip.FlipIfCanFlip(inputDirection * -1);
        Flip.transform.localPosition = new Vector3(OffSetSpriteX * inputDirection.x * -1, Flip.transform.localPosition.y, Flip.transform.localPosition.z);

        needResetInput = jumpInputDown;
    }

    public override void DoBeforeLeaving()
    {
        anim.SetBool("isWallJump", false);

        isLeftSide = false;
        isRigthSide = false;
        Flip.transform.localPosition = new Vector3(0, Flip.transform.localPosition.y, Flip.transform.localPosition.z);
    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause)
        {
            if (!isJump)
            {
                if ((isLeftSide && inputDirection.x < 0)
                    || (isRigthSide && inputDirection.x > 0))
                {

                    Flip.FlipIfCanFlip(inputDirection * -1);
                    Flip.transform.localPosition = new Vector3(OffSetSpriteX * inputDirection.x * -1, Flip.transform.localPosition.y, Flip.transform.localPosition.z);
                    rigidBody2D.velocity = Vector3.up * -DownVelocity;
                    if (isChange)
                    {
                        StopCoroutine("ChangeStateMove");
                        isChange = false;
                    }
                }
                // rigidBody2D.velocity = Vector2.zero;
            }
            if (move.checkGround())
            {
                fsm.PerformTransition(Transition.Move_WallJump);
            }
        }
    }

    protected override void UpdateChild()
    {
        if (!ManagerPause.Pause)
        {
            if (!needResetInput)
            {
                if (jumpInputDown && !isJump)
                {

                    Jump();
                    /*
                if ((isLeftSide && inputDirection.x >= 0)
                  || (isRigthSide && inputDirection.x <= 0))
                {
                }*/
                }
            }
            else
            {

                needResetInput = jumpInputDown;
            }
        }
    }

    protected override void LateUpdateChild()
    {
        if (!ManagerPause.Pause)
        {
            if (isJump)
                return;
            /* if ((isLeftSide && inputDirection.x >= 0)
                 || (isRigthSide && inputDirection.x <= 0))
             {
                 if (!isChange)
                 {
                     isChange = true;
                     StartCoroutine("ChangeStateMove");
                 }
             }
             else */
            if (!canCheckWallJump)
            {
                fsm.PerformTransition(Transition.Move_WallJump);
            }
        }
    }

    IEnumerator ChangeStateMove()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.35f));
        fsm.PerformTransition(Transition.Move_WallJump);
        isChange = false;
    }


    void SetInputAgainTrue()
    {
        canInputCheckWallJump = true;
    }

    void Jump()
    {
        isJump = true;
        anim.SetTrigger("JumpWall");
        Flip.FlipIfCanFlip(inputDirection * -1);
        //   Flip.transform.localPosition = new Vector3(OffSetSpriteX * inputDirection.x * -1, Flip.transform.localPosition.y, Flip.transform.localPosition.z);
        Flip.transform.localPosition = new Vector3(OffSetSpriteX * 1 * -1, Flip.transform.localPosition.y, Flip.transform.localPosition.z);

        if (isChange)
        {
            StopCoroutine("ChangeStateMove");
            isChange = false;

        }
        canInputCheckWallJump = false;
        StartCoroutine(timerVar.StartTime(SetInputAgainTrue, InputForWallAgain));

        int dir = 0;
        if (isLeftSide)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        if (useJump)
            move.FirstJump(TimeNoInput, dir);
        else
        {
            float multiplyX = 1.0f;
            if ((isLeftSide && inputDirection.x > 0)
                || (isRigthSide && inputDirection.x < 0))
            {
                //  multiplyX = MultyplyForceOtherDireccion;
            }
            Vector2 normalFactorSpeed = FactorForNormalSpeed;
            if (move.canRun)
            {
                normalFactorSpeed = FactorForRunSpeed;
            }

            move.ApplyForce(new Vector2(dir, 1).normalized, force.x * multiplyX * normalFactorSpeed.x, force.y * normalFactorSpeed.y, true, TimeNoInput);

            move.SetIsNONWhenJump();
        }

        FX.Spawn(move.groundCheck.position);
        //Klvo
        jumpSFX.Play();
        fsm.PerformTransition(Transition.Move_WallJump);
    }

    public bool isWallJump(Vector2 input)
    {
        if ((isLeftSide && input.x < 0)
            || (isRigthSide && input.x > 0))
        {
            return true;
        }
        return false;
    }

    public bool canWallJump()
    {
        isLeftSide = false;
        isRigthSide = false;
        if (!canCheckWallJump || !canInputCheckWallJump)
            return false;
        if (Physics2D.OverlapCircleNonAlloc(leftSide.position, wallRadiusTotal, result, whatIsWallJump) > 0)
        {
            for (int i = 0; i < result.Length; ++i)
            {
                if (result[i] != null)
                {
                    if (result[i].CompareTag(TagWall))
                    {
                        isLeftSide = true;
                        return isLeftSide;
                    }
                }
            }

        }
        if (Physics2D.OverlapCircleNonAlloc(rigthSide.position, wallRadiusTotal, result, whatIsWallJump) > 0)
        {
            for (int i = 0; i < result.Length; ++i)
            {
                if (result[i] != null)
                {
                    if (result[i].CompareTag(TagWall))
                    {
                        isRigthSide = true;
                        return isRigthSide;

                    }
                }
            }
        }
        return false;
    }

    public Collider2D getLegs()
    {
        return move.getLegsCollider();
    }
}
