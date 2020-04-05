using UnityEngine;
using System.Collections;
using Luminosity.IO;

public class CrouchState : FSMState, IButtonState
{
    public Animator anim;
    public Collider2D collideHead;
    public Transform headGameObject;


    public Vector2 positionCrouch;
    protected Vector2 positionNormal;
    [SerializeField]
    InputActionOld accionButton = InputActionOld.Movement_Y;

    public void SetButton(InputActionOld button, bool value)
    {
    }
    public void SetButton(InputActionOld button, float value)
    {
        //Aqui e suna prueba donde ocupa mas cpu pero menos memoria
        if (accionButton == button)
        {
            crouchInput = value;
        }
    }
    public void SetButtonUp(InputActionOld button, bool value)
    {
    }
    public void SetButtonDown(InputActionOld button, bool value)
    {
    }
    protected float crouchInput = 0;
    public bool isCrouching
    {
        get
        {
            return crouchInput <= -1;
        }
    }

    MoveState moveStat = null;

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
    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Crouch;
        AddTransition(Transition.CrouchToMove, StateID.Move);
        AddTransition(Transition.CrouchToDead, StateID.Dead);
    }

    public override void DoBeforeEntering()
    {
        //rigidbody2D.isKinematic = true;
        anim.SetBool("isCrouch", true);
        collideHead.enabled = false;
        positionNormal = headGameObject.localPosition;
        headGameObject.localPosition = positionCrouch;

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);

        if (moveStat == null)
            moveStat = (MoveState)fsm.GetState(StateID.Move);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

        if (OnStart != null)
            OnStart();
    }
    public override void DoBeforeLeaving()
    {
        //rigidbody2D.isKinematic = false;
        anim.SetBool("isCrouch", false);
        collideHead.enabled = true;
        headGameObject.localPosition = positionNormal;

        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

    }

    protected override void Reason()
    {
        if (resume)
        {
            if (!isCrouching)
            {
                fsm.PerformTransition(Transition.CrouchToMove);
            }
        }
    }

    protected override void FixedUpdateChild()
    {
        if (resume)
        {
            if (moveStat.getOnPlatform() != null)
            {

                GetComponent<Rigidbody2D>().velocity = new Vector2(moveStat.getOnPlatform().velocity.x, moveStat.getOnPlatform().velocity.y);
            }
        }
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
