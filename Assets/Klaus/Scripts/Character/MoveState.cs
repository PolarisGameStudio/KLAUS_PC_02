//
// MoveState.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System;
using Luminosity.IO;

public class MoveState : FSMState, IMoveState, IButtonState, ICurrentPlatform, IForceObject
{
    public PlayerInfo info;
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

    Collider2D[] _colliders;

    public Collider2D[] colliders
    {
        get
        {
            if (_colliders == null)
                _colliders = GetComponents<Collider2D>();

            return _colliders;
        }
    }

    #region Move Var

    protected Dictionary<InputActionOld, bool> buttonMap = new Dictionary<InputActionOld, bool>();
    protected Dictionary<InputActionOld, float> buttonMapAxis = new Dictionary<InputActionOld, float>();
    protected Dictionary<InputActionOld, bool> buttonMapDown = new Dictionary<InputActionOld, bool>();
    protected Dictionary<InputActionOld, bool> buttonMapUp = new Dictionary<InputActionOld, bool>();

    [SerializeField]
    protected InputActionOld JumpButton = InputActionOld.Jump;
    [SerializeField]
    protected InputActionOld VerticalAxis = InputActionOld.Movement_Y;
    [SerializeField]
    protected InputActionOld RunButton = InputActionOld.Run;
    public InputActionOld RunButtonValue
    {
        get
        {
            return RunButton;
        }
    }

    [SerializeField]
    protected InputActionOld ActionButton = InputActionOld.Action;
    [SerializeField]
    protected InputActionOld ActionButton2 = InputActionOld.Throw;

    public void SetButton(InputActionOld button, bool value)
    {
        buttonMap[button] = value;
    }

    public void SetButton(InputActionOld button, float value)
    {
        buttonMapAxis[button] = value;
    }

    public void SetButtonUp(InputActionOld button, bool value)
    {
        buttonMapUp[button] = value;
    }

    public void SetButtonDown(InputActionOld button, bool value)
    {
        buttonMapDown[button] = value;
    }

    public bool activeRun = true;
    public bool onlyMoveLeft = false;
    public bool onlyMoveRigth = false;

    /// <summary>
    /// If player can run
    /// </summary>
    public bool canRun
    {
        get
        {
            if (!buttonMap.ContainsKey(RunButton))
                buttonMap[RunButton] = false;

            return (buttonMap[RunButton] && activeRun);
        }
    }

    public bool activeJump = true;

    /// <summary>
    /// if Player can Jump in Ground
    /// </summary>
    protected bool canJumpGround
    {
        get
        {
            return ((ground || jumpAuxOffGround) && PressJump);
        }
    }

    public bool PressJump
    {
        get
        {
            if (!buttonMap.ContainsKey(JumpButton))
                buttonMap[JumpButton] = false;
            return buttonMap[JumpButton] && activeJump;
        }
    }

    protected bool jumpAuxOffGround = false;
    protected float timeForTryFirstJump = 0.1f;
    public Vector2 MaxSpeed = new Vector2(20, 20);

    /// <summary>
    /// The max speed.
    /// </summary>
    public float maxSpeedX = 8.0f;
    /// <summary>
    /// The normal Speed
    /// </summary>
    public float speedX = 4.0f;
    public float speedXPlanning = 2.0f;

    /// <summary>
    /// The real maxSpeed, depends if canRun or not
    /// </summary>
    public float speedInX
    {
        get
        {
            if (canRun)
            {
                return maxSpeedX;

            }
            else
            {
                return speedX;
            }
        }
    }

    /// <summary>
    /// The direction of movement setting by input
    /// </summary>
    [HideInInspector]
    public Vector2 inputDirection = Vector2.zero;

    public bool isMovingByControl
    {
        get
        {
            return inputDirection != Vector2.zero;
        }
    }

    /// <summary>
    /// If the input set direction on last frame
    /// </summary>
    protected bool inputSet = false;

    public float percentPlatformMoveSameDir = 0.5f;
    public float percentPlatformMoveOposDir = 0.8f;

    #endregion

    #region Jump Var:

    /// <summary>
    /// Porcentaje de fuerza para el salto
    /// </summary>
    [Header("Jump")]
    public float jumpPercentOfForce = 0.35f;
    /// <summary>
    /// Time to reset all var of jumping
    /// </summary>
    public float TimeToResetJumpingVar = 0.1f;
    /// <summary>
    /// Time to apply half jump
    /// </summary>
    public float timeForHalfJump = 0.1f;
    /// <summary>
    /// Percent of Initial Force jump para apply to half jump
    /// </summary>
    public float percentOfJumpForHalfJump = 0.5f;

    /// <summary>
    /// If first Jump is ready
    /// </summary>
    protected bool isReadyFirstJump = false;
    /// <summary>
    /// If can apply half jump to the first jump
    /// </summary>
    protected bool canApplyHalfJump = false;
    /// <summary>
    /// If Player is Jumping desde el Ground
    /// </summary>
    protected bool isJumping = false;
    /// <summary>
    /// If can set all var of jump to false
    /// </summary>
    protected bool canSetJumpInFalse = false;

    /// <summary>
    /// Sirve para saber cuando se levanto el boton de salto para pvoer vovler saltar
    /// </summary>
    protected bool buttonJumpAux = false;

    public int maxHalf = 5;
    protected int currentHalf = 0;
    protected float BaseSpeedYHalfJump = 0.0f;

    public float GraviyScaleForDown = 4.5f;
    public float GraviyScaleForUp = 2.2f;

    protected void SetTrueHalfJump()
    {
        canApplyHalfJump = true;
    }

    protected void SetTrueCanSetJumpInFalse()
    {
        canSetJumpInFalse = true;
    }

    bool isMovingInPaltformY = false;
    bool isRightWhenJump = false;
    bool isNONWhenJump = false;

    const float lessFactorChangueDirAir = 0.75f;

    /// <summary>
    /// If K1 is planning with his capita
    /// </summary>
    protected bool isPlanning = false;


    /// <summary>
    /// if Player can Jump in Air
    /// </summary>
    protected bool canJumpAir
    {
        get
        {
            return (!ground && !isForcing);
        }
    }

    /// <summary>
    /// If i jump in the air
    /// </summary>
    protected bool isJumpingInAir = false;

    #endregion

    #region Sprite/Animator:

    public Animator anim;
    public FlipSprite m_Flip;

    #endregion

    #region SideVar:

    protected Collider2D[] result = new Collider2D[5];

    #endregion

    #region ground Var:

    public bool isGround
    {

        get
        {
            return ground;
        }
    }

    /// <summary>
    /// if is grounded
    /// </summary>
    protected bool ground = false;
    /// <summary>
    /// The ground radius for check the ground
    /// </summary>
    public float groundRadius = 0.09f;

    public float groundRadiusTotal
    {
        get
        {
            return groundRadius * Math.Abs(transform.localScale.x);
        }
    }

    /// <summary>
    /// The ground transform, we need the position to overlap
    /// </summary>
    public Transform groundCheck;
    /// <summary>
    /// What layers are ground.
    /// </summary>
    public LayerMask whatIsGround;
    [Header("Head Check")]
    public Transform HeadCheck;
    public float headRadius = 0.09f;

    public float headRadiusTotal
    {
        get
        {
            return headRadius * Math.Abs(transform.localScale.x);
        }
    }

    protected LayerMask whatsKill;
    protected LayerMask whatIsGroundSinWall;

    /// <summary>
    /// Remove What is ground for dont die
    /// </summary>
    /// <param name="layer"></param>
    public void RemoveLayerToWhatisGround(string layer)
    {

        whatIsGround &= ~(1 << LayerMask.NameToLayer(layer));
        whatIsGroundSinWall &= ~(1 << LayerMask.NameToLayer(layer));
    }

    public void AddLayerToWhatisGround(string layer)
    {

        whatIsGround |= (1 << LayerMask.NameToLayer(layer));
        whatIsGroundSinWall |= (1 << LayerMask.NameToLayer(layer));
    }

    /// <summary>
    /// Ignore the layer to collison the player
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="value"></param>
    public void IngnoreLayer(string layer, bool value)
    {
        if (Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer(layer), gameObject.layer) != value)
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layer), gameObject.layer, value);

    }

    public float initialVelOffGround { get; protected set; }

    protected float initialVelOffGroundStore = 0;

    protected bool GroundAfter = true;
    [Range(0, 1)]
    protected int
        canGetInputInX = 1;

    #endregion

    [HideInInspector]
    public bool isEnter = false;

    [HideInInspector]

    public bool isEnterElevator = false;

    public bool isDead { get; private set; }


    #region ImpulsoVar:

    /// <summary>
    /// Si se impulso
    /// No s epuede mover en true
    /// </summary>
    protected bool isForcing = false;
    [Header("Impulso Var")]
    /// <summary>
    /// Le quita tanto porcentaje de velocidad en X en el aire.
    /// </summary>
    [Range(0, 1)]
    public float RoceAirX = 0.99f;
    /// <summary>
    /// Cuanto porcentaje se tomara del input para la velocidad en X en el aire.
    /// </summary>
    [Range(0, 1)]
    public float PercentLessToSpeed = 0.8f;
    /// <summary>
    /// Tiempo que transcurre para poder volverme a mover en el aire
    /// Despues de un impulso
    /// </summary>
    public float TimeAfterImpulse = 2.5f;
    /// <summary>
    /// Valores para el impulso.
    /// </summary>
    public Vector2 ForceImpulse = new Vector2(500.0f, 500.0f);

    protected float initialVelocityImpulse = 0;

    #endregion

    #region PlatformVar:
    Rigidbody2D getAnyPlatform()
    {
        if (getOnPlatform() != null)
            return getOnPlatform();
        if (_platformLadder != null && CanLadderDown)
            return _platformLadder;
        return null;
    }
    Rigidbody2D plat_rigidbody2D;

    public Rigidbody2D getOnPlatform()
    {
        return plat_rigidbody2D;

    }

    public bool isInPlatform
    {
        get
        {
            return getOnPlatform() != null;
        }
    }

    public Collider2D LegsCollider;

    public Collider2D getLegsCollider()
    {
        return LegsCollider;
    }

    public void CurrentPlatformEnter(Rigidbody2D platform)
    {
        Assert.IsNotNull(platform);
        plat_rigidbody2D = platform;
        Assert.IsNotNull(plat_rigidbody2D);

    }

    public void CurrentPlatformExit(Rigidbody2D platform)
    {
        if (plat_rigidbody2D == null)
        {
            return;
        }
        Assert.IsNotNull(platform);

        if (plat_rigidbody2D.GetInstanceID() == platform.GetInstanceID())
        {

            ResetPlatform();
        }

    }

    #endregion

    #region SwapControls:

    public bool SwapActionJump = false;
    InputActionOld storeActionButton;
    InputActionOld storeJumpButton;

    public InputActionOld SwapActionWithJump(bool value)
    {
        if (value)
        {
            ActionButton = JumpButton;
            JumpButton = storeActionButton;

        }
        else
        {
            ActionButton = storeActionButton;
            JumpButton = storeJumpButton;
        }

        return ActionButton;
    }

    #endregion

    #region Laddder:

    [Header("Ladder:")]
    bool LadderStateOpen = true;
    public bool canLadder = false;
    public LadderTrigger Ladder;
    [HideInInspector]
    public LadderTopTrigger LadderTop;
    public bool CanLadderDown = false;
    [HideInInspector]
    public Transform LadderPosTop;
    [Range(-1, 1)]
    public float minValueToUpLadder = 0.2f;
    [Range(-1, 1)]
    public float minValueToDownLadder = -0.5f;
    [SerializeField]
    Rigidbody2D _platformLadder;
    public void setOnPlatformLadder(Rigidbody2D rigid)
    {
        _platformLadder = rigid;
    }
    public void exitOnPlatformLadder(Rigidbody2D rigid)
    {
        if (!canLadder && !CanLadderDown)
            if (_platformLadder == rigid)
                _platformLadder = null;

    }
    public Rigidbody2D getOnPlatformLadder()
    {
        return _platformLadder;
    }
    public void CloseLadderState()
    {
        LadderStateOpen = false;
    }

    public void OpenLadderState()
    {
        LadderStateOpen = true;
    }

    #endregion

    #region Broadcast:

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

    public delegate void onJumpBroadcast(float per);

    public event onJumpBroadcast OnJump;

    public void SuscribeOnJump(onJumpBroadcast funct)
    {
        OnJump += funct;
    }

    public void UnSuscribeOnJump(onJumpBroadcast funct)
    {
        OnJump -= funct;
    }

    #endregion

    #region Animator

    [Header("Idle Animator")]
    public int numIdles = 1;
    public float[] TimeToChangeIdle;
    protected int currentIdle = 0;
    bool isChangeIdle = false;
    public float TimeSelected = 0.4f;

    public bool useAllIdle
    {
        get
        {
            return _useAllIdle;
        }
        set
        {
            _useAllIdle = value;
            if (_useAllIdle == false)
            {
                isChangeIdle = true;
                currentIdle = 0;
                anim.SetInteger("IdleRandom", currentIdle);
                StopCoroutine("ChangeIdle");
                StartCoroutine("ChangeIdle", TimeToChangeIdle[0]);
            }
        }
    }

    protected bool _useAllIdle = true;

    public void SetIdleAnimatorSelect()
    {
        if (isChangeIdle)
        {
            anim.SetInteger("IdleRandom", numIdles);
            currentIdle = -1;
            StopCoroutine("ChangeIdle");
            StartCoroutine("ChangeIdle", TimeSelected);
        }

    }

    protected virtual void SetAnimatorVarSpeed(float speed)
    {
        anim.SetFloat("SpeedX", Mathf.Abs(speed));

        if (anim.GetFloat("SpeedX") != 0)
        {
            currentIdle = 0;
            isChangeIdle = false;
            StopCoroutine("ChangeIdle");

        }
        else
        {
            if (!isChangeIdle)
            {
                isChangeIdle = true;
                currentIdle = 0;
                anim.SetInteger("IdleRandom", currentIdle);
                StopCoroutine("ChangeIdle");
                StartCoroutine("ChangeIdle", TimeToChangeIdle[0]);
            }
        }

    }

    // TODO: Esto se ejecuta en TODOS los estados por siempre jamas
    IEnumerator ChangeIdle(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        if (currentIdle == 0 && useAllIdle)
        {
            int assit = UnityEngine.Random.Range(1, numIdles);
            assit = assit % numIdles;
            currentIdle = assit;
        }
        else
            currentIdle = 0;

        anim.SetInteger("IdleRandom", currentIdle);
        StartCoroutine("ChangeIdle", TimeToChangeIdle[currentIdle]);

    }

    #endregion

    [Header("Audio")]
    public AudioClip audioClipChange;

    [Header("Wall State")]
    public WallState wallState;

    #region MoveEqual

    [HideInInspector]
    public MoveState AnotherCharacter;
    float speedXFollow = 0;
    float MaxDistanceToAttach = 1.5f;
    float factorSpeed = 1.2f;

    #endregion

    #region UnityCallbacks:

    // Use this for initializatiogn
    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Move;
        AddTransition(Transition.MoveToDead, StateID.Dead);
        AddTransition(Transition.MoveToEnter, StateID.EnterDoor);
        AddTransition(Transition.Move_Ladder, StateID.Ladder);

        isDead = false;

        TransitionAddManagement();

        storeActionButton = ActionButton;
        storeJumpButton = JumpButton;
        SwapActionWithJump(SwapActionJump);
    }

    /// <summary>
    /// Addd transtition to fsm
    /// </summary>
    protected virtual void TransitionAddManagement()
    {

    }

    bool groundByWall = false;

    ContactPoint2D[] m_surfaceContacts = new ContactPoint2D[10];
    [SerializeField]
    float m_factorToInreaseTreadmillSpeed = 1;
    float GetSpeedBySurface()
    {
        int numbTest = _rigidbody2D.GetContacts(m_surfaceContacts);
        for (int i = 0; i < numbTest; ++i)
        {
            var surface = m_surfaceContacts[i].collider.GetComponent<SurfaceEffector2D>();
            if (surface != null)
            {
                return surface.speed * Time.fixedDeltaTime * m_factorToInreaseTreadmillSpeed;
            }
        }
        return 0;
    }

    public virtual bool checkGround()
    {
        int numColliders = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, whatIsGroundSinWall);
        ground = numColliders > 0;

        int numOfPlatform = 0;
        for (int i = 0; i < numColliders; ++i)
        {
            if (result[i] != null)
            {
                if (result[i].CompareTag("Platform"))
                {
                    ++numOfPlatform;
                }
            }
        }
        if (!ground)
        {
            numColliders = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, (1 << LayerMask.NameToLayer("Wall")));
            ground = numColliders > 0;
            for (int i = 0; i < numColliders; ++i)
            {
                if (result[i] != null)
                {
                    if (result[i].CompareTag("Platform"))
                    {
                        ++numOfPlatform;
                    }
                }
            }

            if (ground)
            {
                groundByWall = true;
            }
            else
            {
                groundByWall = false;
            }
        }
        else
        {
            groundByWall = false;
        }
        //Fix: when paltform go down never sound klaus.
        if (!ground)
        {
            if (getAnyPlatform() != null && _rigidbody2D.velocity.y < 0)
            {
                ground = true;
            }
        }

        if (numOfPlatform == 0)
        {
            //Aqui elimino la paltaforma
            //TODO: CHECK: WARNING: this can cause a lot of bugs
            //  ResetPlatform();
        }
        return ground;
    }

    //public GameObject crushDeadSFX;
    // Update is called once per frame
    protected override void FixedUpdateChild()
    {
        if (resume)
        {
            #region Check ground
            checkGround();
            CheckVelocityOffGround();
            anim.SetBool("isGround", isGround);
            #endregion


            #region HorizontalMove:
            float speedH = 0;
            if (!isForcing)
            {// Si no ha pasado el tiempo de aplicacion de fuerza
                if (!isGround)
                {//Si no estoy en el piso
                    if (!isPlanning)
                    {
                        speedH = inputDirection.x * speedX * PercentLessToSpeed;

                        initialVelOffGround += speedH;

                        if (initialVelOffGround > initialVelOffGroundStore)
                        {
                            initialVelOffGround = initialVelOffGroundStore;
                            if (isNONWhenJump)
                            {
                                //initialVelOffGroundStore *= lessFactorChangueDirAir;
                            }
                            else if (!isRightWhenJump)
                            {
                                if (initialVelOffGroundStore > 1)
                                    initialVelOffGroundStore *= lessFactorChangueDirAir;
                                isRightWhenJump = true;
                            }

                        }
                        else if (initialVelOffGround < -1 * initialVelOffGroundStore)
                        {
                            initialVelOffGround = -1 * initialVelOffGroundStore;
                            if (isNONWhenJump)
                            {
                                //initialVelOffGroundStore *= lessFactorChangueDirAir;
                            }
                            else if (isRightWhenJump)
                            {
                                if (initialVelOffGroundStore > 1)
                                    initialVelOffGroundStore *= lessFactorChangueDirAir;
                                isRightWhenJump = false;
                            }
                        }
                        if (Mathf.Approximately(_rigidbody2D.velocity.x, 0) && !Mathf.Approximately(initialVelocityImpulse, 0))
                        {
                            Debug.Log("AQUI no deberia pasar");
                            initialVelocityImpulse = 0;
                            initialVelOffGround = 0;
                        }

                        initialVelocityImpulse *= RoceAirX;
                        _rigidbody2D.velocity = new Vector2(initialVelocityImpulse /** Time.fixedDeltaTime*/ + initialVelOffGround, _rigidbody2D.velocity.y);

                    }
                    else
                    {
                        speedH = inputDirection.x * speedXPlanning;

                        initialVelocityImpulse = 0;
                        initialVelOffGround = 0;
                        _rigidbody2D.velocity = new Vector2(speedH, _rigidbody2D.velocity.y);

                    }

                }
                else
                {
                    speedH = inputDirection.x * speedInX;

                    float valueX = 0;
                    float valueY = 0;
                    Rigidbody2D platform = getAnyPlatform();
                    if (platform != null)
                    {
                        if (!Mathf.Approximately(platform.velocity.y, 0))
                        {
                            if (!isMovingInPaltformY)
                            {
                                isMovingInPaltformY = true;
                            }

                            if (!isJumping)
                                valueY = platform.velocity.y;
                            else
                                valueY = _rigidbody2D.velocity.y;
                        }
                        else
                        {
                            //                            Debug.Log("nUNCA AQUI");
                            if (isMovingInPaltformY)
                            {

                                isMovingInPaltformY = false;
                                valueY = 0;
                            }
                            else
                            {
                                valueY = _rigidbody2D.velocity.y;
                            }
                        }

                        if ((platform.velocity.x > 0 && speedH > 0)
                            || (platform.velocity.x < 0 && speedH < 0))
                        {
                            valueX = speedH * percentPlatformMoveSameDir + platform.velocity.x;
                        }
                        else if ((platform.velocity.x > 0 && speedH < 0)
                                 || (platform.velocity.x < 0 && speedH > 0))
                        {
                            valueX = speedH * percentPlatformMoveOposDir + platform.velocity.x;
                        }
                        else
                        {
                            valueX = speedH + platform.velocity.x;
                        }

                        _rigidbody2D.velocity = new Vector2(valueX + GetSpeedBySurface(), valueY);

                    }
                    else
                    {
                        _rigidbody2D.velocity = new Vector2(speedH + GetSpeedBySurface(), _rigidbody2D.velocity.y);
                    }




                }
            }
            if (AnotherCharacter != null)
            {
                Vector3 distanceVector = AnotherCharacter.transform.position - transform.position;
                if (distanceVector.magnitude <= MaxDistanceToAttach && !Mathf.Approximately(_rigidbody2D.velocity.x, 0))
                {
                    speedXFollow = distanceVector.normalized.x * distanceVector.magnitude * factorSpeed;
                }
                else
                {
                    speedXFollow = 0;
                }
            }
            else
            {
                speedXFollow = 0;
            }
            _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x + speedXFollow, -MaxSpeed.x, MaxSpeed.x), Mathf.Clamp(_rigidbody2D.velocity.y, -MaxSpeed.y, MaxSpeed.y));

            SetAnimatorVarSpeed(speedH);
            m_Flip.FlipIfCanFlip(inputDirection);


            #endregion


            #region Checkeo de aplastamiento
            if (ground)
            {
                CheckHeadKill();
            }
            #endregion
        }
    }

    public void CheckHeadKill()
    {
        Collider2D groundCheckCol = null;
        //Aqui no puedo morir aplastado por un wall
        Debug.DrawLine(HeadCheck.position, HeadCheck.position + headRadiusTotal * transform.up, Color.green);
        Debug.DrawLine(HeadCheck.position, HeadCheck.position + headRadiusTotal * transform.right, Color.green);
        Debug.DrawLine(HeadCheck.position, HeadCheck.position + headRadiusTotal * transform.right * -1, Color.green);

        if (groundByWall)
        {
            //whatsKill |= (1 << LayerMask.NameToLayer("Wall"));
        }
        else
        {
            if (result.Length > 0)
            {
                groundCheckCol = result[0];
            }
        }
        if (Physics2D.OverlapCircleNonAlloc(HeadCheck.position, headRadiusTotal, result, whatsKill) > 0)
        {
            bool canKill = true;
            for (int i = 0; i < result.Length; ++i)
            {
                if (result[i] != null)
                {
                    if (result[i] == LadderTop)
                    {
                        canKill = false;
                        break;
                    }
                    else
                    {
                        if (groundByWall)
                        {
                            if (Math.Abs(result[i].transform.position.y - transform.position.y) > 0.5f)
                                if (result[i].attachedRigidbody != null)
                                {
                                    if (result[i].attachedRigidbody.velocity.y > -0.1f && 0.1f > result[i].attachedRigidbody.velocity.y)
                                    {
                                        canKill = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    canKill = false;
                                    break;
                                }
                        }
                        else
                        {
                            if (groundCheckCol != null)
                            {
                                if (Math.Abs(groundCheckCol.transform.position.y - transform.position.y) > 0.5f)
                                    if (groundCheckCol.attachedRigidbody != null)
                                    {
                                        if (groundCheckCol.attachedRigidbody.velocity.y > -0.1f && 0.1f > groundCheckCol.attachedRigidbody.velocity.y)
                                        {
                                            canKill = false;
                                            break;
                                        }
                                    }
                            }
                        }
                    }
                }
                if (canKill)
                    Kill();
            }
            if (groundByWall)
            {
                //  whatsKill &= ~(1 << LayerMask.NameToLayer("Wall"));
            }
        }
    }

    protected override void UpdateChild()
    {
        if (resume)
        {
            JumpManagement();

        }
    }

    IEnumerator ResetCanInput(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        CanMove(true);

    }

    public void FirstJump(float time = 0, int dir = 0, bool checkGrounAfter = false)
    {
        if (checkGrounAfter)
            GroundAfter = true;
        if (time > 0)
        {
            CanMove(false);
            StartCoroutine("ResetCanInput", time);
            inputDirection = new Vector2(dir, 0);
        }
        /*
        ManagerAnalytics.CharacterJump(Application.loadedLevelName,
            Application.loadedLevelName,
            SaveManager.Instance.comingFromTimeArcadeMode,
            gameObject.name,
            transform.position.x,
            transform.position.y,
            "Normal");*/

        _rigidbody2D.gravityScale = 0;
        isReadyFirstJump = true;
        Jump(jumpPercentOfForce);

        currentHalf = 0;

        isJumping = true;
        canApplyHalfJump = false;
        Invoke("SetTrueHalfJump", timeForHalfJump);
        Invoke("SetTrueCanSetJumpInFalse", TimeToResetJumpingVar);

        buttonJumpAux = true;
    }

    /// <summary>
    /// Set how to jump
    /// </summary>
    protected virtual void JumpManagement()
    {
        if (!buttonMapAxis.ContainsKey(VerticalAxis))
            buttonMapAxis[VerticalAxis] = 0;

        if ((buttonMapAxis[VerticalAxis] > -0.5f && !isJumping) || isJumping)
        {

            if ((canJumpGround) && !isJumping && !isReadyFirstJump && !buttonJumpAux)
            {
                FirstJump();
            }
            else if (isReadyFirstJump && isJumping && !isJumpingInAir)
            {
                if (canApplyHalfJump)
                {
                    if (currentHalf < maxHalf)
                    {
                        currentHalf++;
                        canApplyHalfJump = false;
                        Invoke("SetTrueHalfJump", timeForHalfJump);
                        if (currentHalf == maxHalf)
                        {
                            /*
                            ManagerAnalytics.CharacterJump(Application.loadedLevelName,
                                Application.loadedLevelName,
                                SaveManager.Instance.comingFromTimeArcadeMode,
                                gameObject.name,
                                transform.position.x,
                                transform.position.y,
                                "NormalExtended");*/
                        }
                        JumpHalf(jumpPercentOfForce, percentOfJumpForHalfJump);
                    }
                    else
                    {
                        isReadyFirstJump = false;
                        _rigidbody2D.gravityScale = GraviyScaleForUp;
                        CancelInvoke("SetTrueHalfJump");

                    }
                }
                else if (!buttonMap[JumpButton])
                {
                    _rigidbody2D.gravityScale = GraviyScaleForUp;
                    isReadyFirstJump = false;
                    CancelInvoke("SetTrueHalfJump");
                }

            }
            else
            {

                JumpManagementElse();
            }
        }
    }

    protected override void LateUpdateChild()
    {
        if (resume)
        {
            if (isGround)
            {
                if (isForcing)
                {
                    StopCoroutine("setForceFalse");
                    isForcing = false;
                }

                GroundManagement();
                if (!isJumping)
                {
                    _rigidbody2D.gravityScale = GraviyScaleForUp;
                }
            }
            else
            {
                AirManagement();
                if (_rigidbody2D.velocity.y < 0 && !isForcing)
                {
                    _rigidbody2D.gravityScale = GraviyScaleForDown;
                }
            }
            CheckForceOutLadder();
        }
    }
    public void CheckForceOutLadder()
    {
        string TagLadder = "Ladder";
        int numberSystemsCollide = 0;
        int numberLadder = 0;
        int numberLadderTop = 0;

        if ((numberSystemsCollide = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, 1 << LayerMask.NameToLayer("Systems"))) > 0)
        {
            for (int i = 0; i < numberSystemsCollide; ++i)
            {
                if (result[i] != null && result[i].CompareTag(TagLadder))
                {
                    if ("BaseForLadder" == result[i].name)
                    {
                        ++numberLadder;

                    }
                    else if ("OneWayLadder" == result[i].name)
                    {
                        ++numberLadderTop;
                    }
                }
            }
        }
        if ((numberSystemsCollide = Physics2D.OverlapCircleNonAlloc(HeadCheck.position, groundRadiusTotal, result, 1 << LayerMask.NameToLayer("Systems"))) > 0)
        {
            for (int i = 0; i < numberSystemsCollide; ++i)
            {
                if (result[i] != null && result[i].CompareTag(TagLadder))
                {
                    if ("BaseForLadder" == result[i].name)
                    {
                        ++numberLadder;

                    }
                    else if ("OneWayLadder" == result[i].name)
                    {
                        ++numberLadderTop;
                    }
                }
            }
        }
        if (numberLadder == 0)
        {
            canLadder = false;
            Ladder = null;
            if (!CanLadderDown)
                _platformLadder = null;
        }
        if (numberLadderTop == 0)
        {
            LadderTop = null;
            LadderPosTop = null;
            CanLadderDown = false;
            if (!canLadder)
            {
                Ladder = null;
                _platformLadder = null;
            }
        }
    }

    protected virtual void AirManagement()
    {
    }

    void SetNonJumpAll()
    {
        canSetJumpInFalse = false;
        isReadyFirstJump = false;
        canApplyHalfJump = false;
        CancelInvoke("SetTrueHalfJump");
        isJumping = false;
    }

    public void ResetForce()
    {
        initialVelocityImpulse = 0;
        initialVelOffGround = 0;
        _rigidbody2D.Sleep();
    }

    public void ResetForceX()
    {
        initialVelocityImpulse = 0;
        initialVelOffGround = 0;
        //_rigidbody2D.Sleep();
    }

    /// <summary>
    /// Called when i stay over grounds
    /// </summary>
    protected virtual void GroundManagement()
    {
        if (!isForcing)
            initialVelocityImpulse = 0;

        if (isJumping && canSetJumpInFalse)
        {
            SetNonJumpAll();

            _rigidbody2D.gravityScale = GraviyScaleForUp;

        }
        if (!isJumping && !buttonMap[JumpButton])
        {
            //Lo hago para que tengas q levantar el dedo apra poder saltar
            buttonJumpAux = false;
        }


        if (!isJumping && buttonMapAxis[VerticalAxis] <= -0.5f && isGround)
        {
            if (buttonMapDown[JumpButton])
            {
                if (Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundRadiusTotal, result, 1 << LayerMask.NameToLayer("OneWayPlatform")) > 0)
                {
                    ground = false;
                    Collider2D oneway = result[0].GetComponent<Collider2D>();
                    OneWaySingleton.Instance.SetNeedDown(oneway, gameObject.name, true);
                    for (int i = 0; i < colliders.Length; ++i)
                    {
                        Physics2D.IgnoreCollision(oneway, colliders[i], true);
                    }


                }
            }

        }


    }

    protected virtual void JumpManagementElse()
    {
    }

    #endregion

    #region StateCallBacks:

    public override void DoBeforeEntering()
    {
        isDead = false;
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        ManagerStop.SubscribeOnStopGame(OnStopGame);
        if (OnStart != null)
            OnStart();

        whatsKill = whatIsGround;
        whatsKill &= ~(1 << LayerMask.NameToLayer("OneWayPlatform"));
        whatsKill &= ~(1 << LayerMask.NameToLayer("Wall"));
        whatIsGroundSinWall = whatIsGround;
        whatIsGroundSinWall &= ~(1 << LayerMask.NameToLayer("Wall"));

        jumpAuxOffGround = false;
    }

    public override void DoBeforeLeaving()
    {
        initialVelocityImpulse = 0;
        initialVelOffGround = 0;
        //    initialVelOffGroundStore = 0;
        isReadyFirstJump = false;
        if (!resumeStop)
        {
            resumeStop = true;
            anim.speed = 1;

        }
        if (isJumping)
        {
            _rigidbody2D.gravityScale = GraviyScaleForUp;
        }

        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
        ManagerStop.UnSubscribeOnStopGame(OnStopGame);
        currentIdle = 0;
        anim.SetInteger("IdleRandom", 0);

        if (OnEnd != null)
            OnEnd();
    }


    protected override void Reason()
    {
        if (resume)
        {
            /*if (isDead) {
                isDead = false;
                fsm.PerformTransition(Transition.MoveToDead);
            } else*/
            if (isEnter)
            {
                isEnter = false;
                fsm.PerformTransition(Transition.MoveToEnter);

            }
            else if (isEnterElevator)
            {
                isEnterElevator = false;
                GetComponent<EnterDoorState>().SetElevator();
                fsm.PerformTransition(Transition.MoveToEnter);

            }
            if (LadderStateOpen)
            {
                if (canLadder)
                {
                    if (!CanLadderDown)//SI no esta en el tope
                    {
                        if (buttonMapAxis[VerticalAxis] > minValueToUpLadder)//Si esta en ´tocando hacia arriba
                        {

                            GroundAfter = true;
                            GroundManagement();
                            fsm.PerformTransition(Transition.Move_Ladder);
                            return;
                        }
                        else if (!isGround)//Si no esta en el piso
                        {
                            if (buttonMapAxis[VerticalAxis] < -1 * minValueToUpLadder)//puede subir toando hacia abajo
                            {
                                GroundAfter = true;
                                GroundManagement();
                                fsm.PerformTransition(Transition.Move_Ladder);
                                return;
                            }
                        }
                    }
                }
                if (CanLadderDown)
                {
                    if (buttonMapAxis[VerticalAxis] < minValueToDownLadder)//Si esta arriba puede bajar
                    {
                        GroundAfter = true;
                        GroundManagement();
                        fsm.PerformTransition(Transition.Move_Ladder);
                        return;
                    }
                }
            }
            ActionManagement();
        }
    }

    /// <summary>
    /// Call for Action Managemente perfom transition
    /// </summary>
    protected virtual void ActionManagement()
    {
        if (wallState.canWallJump() && wallState.isWallJump(inputDirection) && !isGround)
        {
            fsm.PerformTransition(Transition.Move_WallJump);

        }
    }

    #endregion


    #region InputManager

    protected bool canMoveInput = true;

    /// <summary>
    /// Sets the movement direction
    /// </summary>
    /// <param name="move">Move.</param>
    public void SetMovement(Vector2 move)
    {
        if (canMoveInput)
        {
            if (onlyMoveLeft)
            {
                if (move.x > 0)
                {
                    move.x *= -1;
                }
            }
            else if (onlyMoveRigth)
            {
                if (move.x < 0)
                {
                    move.x *= -1;
                }
            }
            inputDirection = move;
        }
    }

    public void CanMove(bool value)
    {
        canMoveInput = value;
        if (!canMoveInput)
        {
            inputDirection = Vector2.zero;
        }

    }

    #endregion

    #region DeadFunction

    protected DeadState deadState;

    public DeadState _deadState
    {
        get
        {
            if (object.ReferenceEquals(deadState, null))
                deadState = GetComponent<DeadState>();
            return deadState;
        }
    }
    [HideInInspector]
    public bool Inmortal = false;
    /// <summary>
    /// Check player is Dead
    /// </summary>
    public void Kill(GameObject sfx = null)
    {
        if (Inmortal)
            return;
        if (useGodMode)
        {
           /*/ if (InputManager.GetAxis("L2") >= 0.9f)
            {
                return;
            }
            /*/
        }
        if (!isDead)
        {

            isDead = true;
            //Test
            if (fsm.CurrentStateID != StateID.Dead)
            {
                _deadState.SFX = sfx;
                ResetForce();
                fsm.PerformTransition(Transition.MoveToDead);
            }
        }
    }

    #endregion

    #region Force:Jump

    protected void ResetPlatform()
    {
        isMovingInPaltformY = false;
        plat_rigidbody2D = null;
    }

    /// <summary>
    /// Set all var for Jump
    /// </summary>
    protected void Jump(float percent)
    {
        if (OnJump != null)
            OnJump(percent);
        //initialVelocityImpulse = 0;
        _rigidbody2D.velocity = new Vector2(initialVelOffGround + initialVelocityImpulse * Time.fixedDeltaTime, 0);
        _rigidbody2D.AddForce(new Vector2(0, ForceImpulse.y * percent * transform.up.y), ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        ground = false;
        anim.SetBool("isGround", isGround);
        BaseSpeedYHalfJump = _rigidbody2D.velocity.y;

        ResetPlatform();

        if (SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.AddJump();
    }

    protected void JumpHalf(float basePercent, float percent)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, BaseSpeedYHalfJump);
        _rigidbody2D.AddForce(new Vector2(0, ForceImpulse.y * basePercent * percent * transform.up.y), ForceMode2D.Impulse);

        ResetPlatform();
    }

    protected virtual void ReserVarInApplyForce()
    {
        GroundAfter = true;
    }

    /// <summary>
    /// Apply forcedir to the rigidbody attached
    /// </summary>
    /// <param name="forceDir"></param>
    public bool ApplyForce(Vector2 forceDir, float forceValueX, float forceValueY, bool isObject, float time = 0)
    {
        if (!isForcing)
        {
            bool isDirY = true;
            if (!Mathf.Approximately(forceDir.x, 0) && (forceDir.y > -0.09f) && (forceDir.y < 0.09f))
            {
                isDirY = false;
                if (isObject)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                }
            }
            //			Debug.Log ("Fuerza");

            //Falta ver si es necesario resetear la velocidad en x si el impulso te lo hara en x
            if (isObject)
            {
                isJumpingInAir = false;
                ReserVarInApplyForce();
            }
            _rigidbody2D.gravityScale = GraviyScaleForUp;
            StopCoroutine("setForceFalse");

            bool storesIsGround = isGround;
            if (isGround)
            {
                ground = false;

                CheckVelocityOffGround();
                if (Mathf.Approximately(initialVelOffGroundStore, 0))
                {
                    SetGroundStoreMaxSpeed();
                }
            }

            initialVelocityImpulse = forceDir.x * forceValueX;

            if (!Mathf.Approximately(initialVelocityImpulse, 0))
            {

                _rigidbody2D.AddForce(new Vector2(initialVelocityImpulse, 0), ForceMode2D.Impulse);
                initialVelocityImpulse = _rigidbody2D.velocity.x;
                /*if (Math.Round(forceDir.y, 2) == 0.0f)
                {
                    if (isObject)
                    _rigidbody2D.AddForce(new Vector2(0, forceValueY));

                }*/
                if (isObject)//Test Luis
                {
                    if (_rigidbody2D.velocity.x > 0)
                    {
                        isNONWhenJump = false;
                        isRightWhenJump = true;
                        if (storesIsGround)
                            initialVelOffGround = initialVelOffGroundStore;
                    }
                    else if (_rigidbody2D.velocity.x < 0)
                    {
                        isNONWhenJump = false;
                        isRightWhenJump = false;
                        if (storesIsGround)
                            initialVelOffGround = initialVelOffGroundStore * -1;

                    }
                    else
                    {
                        isNONWhenJump = true;
                        isRightWhenJump = false;
                    }

                }
            }
            else
            {
                // SetGroundStoreMaxSpeed();
                _rigidbody2D.velocity = new Vector2(initialVelOffGround, 0);
            }

            if (isDirY)
            {
                _rigidbody2D.AddForce(new Vector2(0, forceDir.y * forceValueY), ForceMode2D.Impulse);
            }
            else
            {
                if (isObject)
                {
                    _rigidbody2D.AddForce(new Vector2(0, forceValueY), ForceMode2D.Impulse);
                }
            }
            transform.position += Vector3.up * 0.2f;

            isForcing = true;
            if (Mathf.Approximately(time, 0))
            {
                StartCoroutine("setForceFalse", TimeAfterImpulse);
            }
            else
            {
                StartCoroutine("setForceFalse", time);
            }

            return true;
        }

        return true;
    }

    void SetGroundStoreMaxSpeed()
    {
        initialVelOffGroundStore = Mathf.Abs(_rigidbody2D.velocity.x);
        if (initialVelOffGroundStore <= speedInX/*0.01f*/)
        {
            initialVelOffGroundStore = speedInX;
        }
    }

    IEnumerator CheckJumpOffGround(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        jumpAuxOffGround = false;
    }

    public void CheckVelocityOffGround()
    {
        //Aqui Checho para las variables si antes estaba tocanod piso o no.
        if (!ground && GroundAfter)
        {
            //Inicio el contador para ver si puedo saltar
            StopCoroutine("CheckJumpOffGround");
            StartCoroutine("CheckJumpOffGround", timeForTryFirstJump);
            jumpAuxOffGround = true;


            SetGroundStoreMaxSpeed();

            initialVelOffGround = _rigidbody2D.velocity.x;

            //			Debug.Log ("Velocidad Initial: " + initialVelOffGround);
            GroundAfter = false;
            if (initialVelOffGround > 0)
            {
                isNONWhenJump = false;
                isRightWhenJump = true;
                if (isJumping)
                    initialVelOffGround = initialVelOffGroundStore;
            }
            else if (initialVelOffGround < 0)
            {
                isNONWhenJump = false;
                isRightWhenJump = false;
                if (isJumping)
                    initialVelOffGround = initialVelOffGroundStore * -1;
            }
            else
            {
                isNONWhenJump = true;
            }

        }
        else if (ground && !GroundAfter)
        {
            initialVelOffGround = 0;
            initialVelOffGroundStore = 0;
            GroundAfter = true;
        }
    }

    IEnumerator setForceFalse(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isForcing = false;

    }

    /// <summary>
    /// Usamos esto para evitar que en el aire se mueva mas lento
    /// </summary>
    public void SetIsNONWhenJump()
    {
        isNONWhenJump = true;
    }

    #endregion



    public void ChangueToEnter()
    {
        fsm.PerformTransition(Transition.MoveToEnter);
    }

    #region Pause:

    bool resumePause = true;
    bool resumeStop = true;

    bool resume
    {
        get
        {
            return resumePause && resumeStop;
        }
    }

    Vector2 velPause = Vector3.zero;
    bool StoreIsKinematic = false;

    public void OnPauseGame()
    {
        resumePause = false;
        velPause = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        anim.speed = 0;
        StoreIsKinematic = _rigidbody2D.isKinematic;
        _rigidbody2D.isKinematic = true;
    }

    public void OnStopGame(bool isStop)
    {
        resumeStop = !isStop;
        if (!resumeStop)
        {
            _rigidbody2D.velocity = Vector2.zero;
            anim.speed = 0;
        }
        else
        {
            if (resumePause)
            {
                anim.speed = 1;
            }
        }

        /*
        velPause = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        anim.speed = 0;
        StoreIsKinematic = _rigidbody2D.isKinematic;
        _rigidbody2D.isKinematic = true;*/
    }

    public void OnResumeGame()
    {
        //  resume = true;
        Invoke("CanMoveAgain", 0.25f);
        _rigidbody2D.isKinematic = StoreIsKinematic;
        _rigidbody2D.velocity = velPause;
        anim.speed = 1;
    }

    void CanMoveAgain()
    {
        resumePause = true;
    }

    #endregion

    #region GodMode:

    [Header("GodMode")]
    public bool useGodMode = false;

    #endregion

}
