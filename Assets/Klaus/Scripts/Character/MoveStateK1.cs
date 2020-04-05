using UnityEngine;
using System.Collections;
using Luminosity.IO;

public class MoveStateK1 : MoveState
{

    [SerializeField]
    InputActionOld CrouchButton = InputActionOld.Movement_Y;
    [SerializeField]
    InputActionOld PlanningButton = InputActionOld.Planning;

    public float minDistance = 1.0f;
    public ThrowState throwStat;

    // public float planningGravityScale = 0;
    public float maxVelocityPlanningInY = 2.5f;
    public GameObject openCapeSFX;
    public bool played = false;
    /// <summary>
    /// Percent of Initial Force jump para apply to half jump
    /// </summary>
    public float percentOfDoubleJump = 0.5f;
    public bool negro = false;


    /// <summary>
    /// If player can make a accion
    /// Need near accion and press Circle
    /// </summary>

    public bool CanThrowKlaus
    {
        get
        {
            if (!buttonMap.ContainsKey(ActionButton2))
                buttonMap[ActionButton2] = false;

            return !object.ReferenceEquals(throwStat.bodyToForce, null)
            && !throwStat.bodyToForce.BlockThrow
            && isGround
            && buttonMap[ActionButton2]
            && Vector3.Distance(throwStat.bodyToForce.transform.position, transform.position) <= minDistance
            && throwStat.bodyToForce.isAvailableToThrow;
        }
    }

    public CrushState crushStat;

    /// <summary>
    /// If player can make a accion
    /// Need near accion and press Square
    /// </summary>
    public bool CanCrush
    {
        get
        {
            if (!buttonMapDown.ContainsKey(ActionButton))
                buttonMapDown[ActionButton] = false;
            return /*isGround && crushStat.NearBlock &&*/ buttonMapDown[ActionButton] && crushStat.CanCrush();
        }

    }

    private AudioSource _audio;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        openCapeSFX.CreatePool(1);
    }

    protected override void TransitionAddManagement()
    {
        AddTransition(Transition.MoveToCrouch, StateID.Crouch);
        AddTransition(Transition.MoveToThrow, StateID.Throw);
        AddTransition(Transition.MoveToCrush, StateID.Crush);
        AddTransition(Transition.Move_WallJump, StateID.WallJump);


    }

    public override void DoBeforeLeaving()
    {
        CancelPlanning();
        base.DoBeforeLeaving();
    }
    /* protected override void JumpManagementElse()
     {
        
         if (canJumpAir)
         {
             if (isPlanning)
             {
                 CancelPlanning();
             }
             CancelInvoke("setApply");
             initialVelOffGround = 0;
             Jump(percentOfDoubleJump);
             isJumpingInAir = true;
         }
     }*/

    protected override void ActionManagement()
    {
        if(!negro)
        { 
            if (CanThrowKlaus)
            {

                fsm.PerformTransition(Transition.MoveToThrow);
            }

            else if (CanCrush)
            {
                fsm.PerformTransition(Transition.MoveToCrush);
            }
        }

          //   Debug.Log("current animation "+anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name=="K1@Idle_Selected_HD" || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "K1@Idle_1_HD" || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name== "K1@Idle_2_HD")
        {
            if(!anim.GetBool("isGround"))
            {
               // float var= anim.speed;
             //   anim.Stop();

            }
        }
        
        base.ActionManagement();
    }

    protected override void AirManagement()
    {
        if (!buttonMap.ContainsKey(PlanningButton))
            buttonMap[PlanningButton] = false;


        if (!isPlanning)
        {
            if (buttonMap[PlanningButton])//buttonMapDown [PlanningButton])
            {
                if (_rigidbody2D.velocity.y < 0 && !isReadyFirstJump)
                {
                    anim.SetBool("isPlanning", true);
                    //saca la capa
                    openCapeSFX.Spawn(transform.position, transform.rotation);
                    //  _rigidbody2D.gravityScale = planningGravityScale;
                    isPlanning = true;
                    if (Mathf.Abs(_rigidbody2D.velocity.y) > maxVelocityPlanningInY)
                    {
                        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, maxVelocityPlanningInY);
                    }
                }
            }
        }
        else
        {
            if (buttonMap[PlanningButton])
            {
                //Planeando
                if (!played)
                {
                    audio.Play();
                    played = true;
                }
                if (Mathf.Abs(_rigidbody2D.velocity.y) > maxVelocityPlanningInY)
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, maxVelocityPlanningInY);
                }
            }
            else
            {
                if (isPlanning)
                    CancelPlanning();
            }
        }


    }

    void CancelPlanning()
    {
        audio.Stop();
        played = false;
        anim.SetBool("isPlanning", false);
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = GraviyScaleForDown;
        }
        else
        {
            _rigidbody2D.gravityScale = GraviyScaleForUp;
        }
        isPlanning = false;
    }

    protected override void GroundManagement()
    {
        base.GroundManagement();
        isJumpingInAir = false;

        if (isPlanning)
        {
            CancelPlanning();
        }
        if (crushStat.timerPuchUp)
            crushStat.canPuchUp = true;
    }

    protected override void ReserVarInApplyForce()
    {
        //    if (crushStat.timerPuchUp)
        crushStat.canPuchUp = true;
        if (isPlanning)
            CancelPlanning();
    }
    /*
    public override bool checkGround()
    {
        if (OneWaySingleton.Instance.isK1OneWay)
        {
            ground = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadius, result, whatIsGround & ~(1 << LayerMask.NameToLayer("OneWayPlatform"))) > 0;
            return ground;
        }

        return base.checkGround();
    }*/
}
