using UnityEngine;
using System.Collections;

public class BaseState : FSMState
{
    protected Collider2D[] _coll;

    public Collider2D[] colliders
    {
        get
        {
            if (_coll == null || _coll.Length == 0)
            {
                _coll = GetComponents<Collider2D>();
            }
            return _coll;
        }
    }

    [SerializeField]
    private Animator _anim;
    public string speedAnimName = "SpeedInX";

    public Animator animator
    {

        get
        {

            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }

            return _anim;
        }
    }

    [SerializeField]
    private Rigidbody2D _rig2D = null;

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

    public Vector2 Velocity
    {
        get
        {
            return rigidBody2D.velocity;
        }

        set
        {
            rigidBody2D.velocity = value;
            SetVelocityAnimator();
        }
    }

    public FlipSprite m_Flip;

    public BoolCircle2DOverlap IsGround;

    public LookToPlayer finderPlayer;

    public AI_ForceApply Force;

    public AI_CurrentPlatform currentPlatform;
    public float percentPlatformMoveSameDir = 0.5f;
    public float percentPlatformMoveOposDir = 0.8f;

    public void ManagerCurrentPlatform(float speedH)
    {
        if (currentPlatform.isInPlatform)
        {
            float valueX = 0;
            float valueY = 0;

            if (currentPlatform.getOnPlatform().velocity.y != 0)
            {
                if (!currentPlatform.isMovingInPaltformY)
                {
                    currentPlatform.isMovingInPaltformY = true;
                }
                valueY = currentPlatform.getOnPlatform().velocity.y;
            } else
            {
                if (currentPlatform.isMovingInPaltformY)
                {

                    currentPlatform.isMovingInPaltformY = false;
                    valueY = 0;
                } else
                {
                    valueY = Velocity.y;
                }
            }
            if ((currentPlatform.getOnPlatform().velocity.x > 0 && speedH > 0)
                || (currentPlatform.getOnPlatform().velocity.x < 0 && speedH < 0))
            {
                valueX = speedH * percentPlatformMoveSameDir + currentPlatform.getOnPlatform().velocity.x;
            } else if ((currentPlatform.getOnPlatform().velocity.x > 0 && speedH < 0)
                       || (currentPlatform.getOnPlatform().velocity.x < 0 && speedH > 0))
            {
                valueX = speedH * percentPlatformMoveOposDir + currentPlatform.getOnPlatform().velocity.x;
            } else
            {
                valueX = speedH + currentPlatform.getOnPlatform().velocity.x;
            }

            Velocity = new Vector2(valueX, valueY);
        }
    }

    protected void SetVelocityAnimator()
    {
        m_Flip.FlipIfCanFlip(Velocity);
        animator.SetFloat(speedAnimName, Mathf.Abs(Velocity.x));

    }

    public override void DoBeforeEntering()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        ManagerStop.SubscribeOnStopGame(OnStopGame);
    }

    public override void DoBeforeLeaving()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
        ManagerStop.UnSubscribeOnStopGame(OnStopGame);

    }

    protected override void FixedUpdateChild()
    {
        SetVelocityAnimator();
    }

    public virtual void Kill()
    {
        
    }



    #region Pause/Stop/Freeze:

    protected Vector2 velPause = Vector3.zero;
    protected float animSpeedPause = 0;
    protected Vector2 velStop = Vector3.zero;
    protected float animSpeedStop = 0;
    protected bool isKinematicStop = false;

    public virtual void OnStopGame(bool value)
    {
        if (value)
        {
            isKinematicStop = rigidBody2D.isKinematic;
            rigidBody2D.isKinematic = true;
            velStop = rigidBody2D.velocity;
            rigidBody2D.velocity = Vector2.zero;
            animSpeedStop = animator.speed;
            animator.speed = 0;
        } else
        {
            rigidBody2D.isKinematic = isKinematicStop;
            animator.speed = animSpeedStop;
            rigidBody2D.velocity = velStop;
        }
    }


    public virtual void OnPauseGame()
    {
        rigidBody2D.isKinematic = true;

        velPause = rigidBody2D.velocity;
        rigidBody2D.velocity = Vector2.zero;
        animSpeedPause = animator.speed;
        animator.speed = 0;

    }

    public virtual void OnResumeGame()
    {
        rigidBody2D.isKinematic = false;
        animator.speed = animSpeedPause;
        rigidBody2D.velocity = velPause;

    }

    protected bool isFreeze = false;

    public virtual void Freeze()
    {
        isFreeze = true;
        Velocity = Vector2.zero;
    }

    public virtual void UnFreeze()
    {
        isFreeze = false;
    }

    #endregion
}
