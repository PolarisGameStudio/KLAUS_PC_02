using UnityEngine;
using System.Collections;

public class AI_IdleState : BaseState
{
    private Behaviour_AI_Idle ai_Idle_Smb;

    public AI_IsVisibleSprite isVisible;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Idle;

        AddTransition(Transition.AI_IdleToDead, StateID.AI_Dead);
        AddTransition(Transition.AI_IdleToPersue, StateID.AI_Persue);
    }

    void Start()
    {
        ai_Idle_Smb = animator.GetBehaviour<Behaviour_AI_Idle>();
        ai_Idle_Smb.ai_IdleState_Mb = this;
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        if (!Force.isForcing && IsGround.isCollide)
            Velocity = Vector2.zero;
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();

    }

    protected override void Reason()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (isVisible.isVisible)
            if (finderPlayer.isPlayerNear)
                fsm.PerformTransition(Transition.AI_IdleToPersue);
        }
    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (!Force.isForcing)
                ManagerCurrentPlatform(0);
            //base.FixedUpdateChild();
            animator.SetFloat("SpeedInX", 0);

        }
    }

    protected override void UpdateChild()
    {
    }

    protected override void LateUpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop && !Force.isForcing)
        {
            if (IsGround.isCollide && !currentPlatform.isInPlatform)
                Velocity = Vector3.zero;//TEner cuidado con esto
        }
    }

    public override void Kill()
    {
        fsm.PerformTransition(Transition.AI_IdleToDead);
    }
}
