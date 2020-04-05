using UnityEngine;
using System.Collections;

public class MoveStateKlaus : MoveState
{
    /// <summary>
    /// Percent of Initial Force jump para apply to half jump
    /// </summary>
    [Header("Klaus Parameters")]
    public float percentOfDoubleJump = 0.5f;
    public float percentOfDoubleJumpForHalfJump = 0.1f;
    public int maxHalfDoubleJump = 1;
    bool JumpInAirComplete = true;

    /// <summary>
    /// If klaus can be throw
    /// </summary>
    public bool isAvailableToThrow
    {
        get
        {
            return enabled && isGround;
        }
    }


    public CodeState codeState;

    /// <summary>
    /// If player can make a accion
    /// Need near accion and press Circle
    /// </summary>
    public bool canCode
    {
        get
        {
            return isGround && codeState.NearPc && buttonMap[ActionButton];
        }
    }

    public bool BlockThrow = false;


    protected override void TransitionAddManagement()
    {
        AddTransition(Transition.MoveToCode, StateID.Code);
        AddTransition(Transition.Move_WallJump, StateID.WallJump);

    }

    protected override void JumpManagementElse()
    {
        if (canJumpAir && canMoveInput)
        {

            if (!isJumpingInAir)
            {

                if (buttonMapDown[JumpButton])
                {
                    /*
                    ManagerAnalytics.CharacterJump(Application.loadedLevelName,
                                                             Application.loadedLevelName,
                                                             SaveManager.Instance.comingFromTimeArcadeMode,
                                                             gameObject.name,
                                                             transform.position.x,
                                                             transform.position.y,
                                                             "Double");*/

                    buttonJumpAux = true;
                    _rigidbody2D.gravityScale = 0;

                    //initialVelOffGround = 0;

                    Jump(percentOfDoubleJump);
                    isJumpingInAir = true;
                    canSetJumpInFalse = true;
                    JumpInAirComplete = false;

                    isJumping = true;
                    isReadyFirstJump = true;
                    canApplyHalfJump = false;
                    currentHalf = 0;
                    CancelInvoke("SetTrueHalfJump");
                    Invoke("SetTrueHalfJump", timeForHalfJump);
                }

            }
            else if (!JumpInAirComplete)
            {

                if (canApplyHalfJump)
                {
                    if (currentHalf < maxHalfDoubleJump)
                    {
                        ++currentHalf;
                        canApplyHalfJump = false;
                        Invoke("SetTrueHalfJump", timeForHalfJump);
                        if (currentHalf == maxHalfDoubleJump)
                        {                    
                            /*
                            ManagerAnalytics.CharacterJump(Application.loadedLevelName,
                                                             Application.loadedLevelName,
                                                             SaveManager.Instance.comingFromTimeArcadeMode,
                                                             gameObject.name,
                                                             transform.position.x,
                                                             transform.position.y,
                                                             "DoubleExtended");*/
                        }
                        JumpHalf(percentOfDoubleJump, percentOfDoubleJumpForHalfJump);
                    }
                    else
                    {
                        _rigidbody2D.gravityScale = GraviyScaleForUp;
                        CancelInvoke("SetTrueHalfJump");
                    }
                }
                else if (!buttonMap[JumpButton])
                {
                    _rigidbody2D.gravityScale = GraviyScaleForUp;
                    canApplyHalfJump = false;
                    JumpInAirComplete = true;
                    CancelInvoke("SetTrueHalfJump");
                }


            } /*else
            {
                _rigidbody2D.gravityScale = GraviyScaleForUp;
            }*/
        }
    }


    protected override void GroundManagement()
    {
        base.GroundManagement();
        isJumpingInAir = false;
        JumpInAirComplete = true;
    }

    protected override void ActionManagement()
    {
        if (canCode)
        {
            fsm.PerformTransition(Transition.MoveToCode);

        }
        base.ActionManagement();
    }

    public void Stop()
    {
        inputDirection = Vector2.zero;
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

    }
    /*
    public override bool checkGround()
    {
        if (OneWaySingleton.Instance.isKlausOneWay)
        {
            ground = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadius, result, whatIsGround & ~(1 << LayerMask.NameToLayer("OneWayPlatform"))) > 0;
            return ground;
        }

        return base.checkGround();
    }*/
}
