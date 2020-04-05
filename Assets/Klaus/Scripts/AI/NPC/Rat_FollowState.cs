using UnityEngine;
using System.Collections;

public class Rat_FollowState : BaseState
{
    public float SpeedInX = 1.0f;

    public float TimeConfusing = 1.0f;
    protected bool isConfusing = false;
    protected Vector2? lastDirection;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Persue;

        AddTransition(Transition.AI_PersueToDead, StateID.AI_Dead);
        AddTransition(Transition.AI_PersueToIdle, StateID.AI_Idle);
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        if (!Force.isForcing && IsGround.isCollide)
            Velocity = new Vector2(finderPlayer.DirectionToPlayer.x * SpeedInX, Velocity.y);
        lastDirection = Velocity;
        isConfusing = false;
        lastDirection = null;

    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();

        StopCoroutine("ChengueStateBecauseConfusing");

    }

    protected override void Reason()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (!finderPlayer.isPlayerNear)
            {
                fsm.PerformTransition(Transition.AI_PersueToIdle);
            }
        }
    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (!isConfusing)
            {
                if (!isFreeze && !Force.isForcing)
                    Velocity = new Vector2(finderPlayer.DirectionToPlayer.x * SpeedInX, Velocity.y);

                if ((lastDirection.HasValue) &&
                    ((Velocity.x >= 0 && lastDirection.Value.x <= 0)
                    || (Velocity.x <= 0 && lastDirection.Value.x >= 0)))
                {
                    isConfusing = true;
                    animator.SetTrigger("Watch");

                    m_Flip.FlipIfCanFlip(lastDirection.Value);
                    StartCoroutine("ChengueStateBecauseConfusing");
                }
                else
                {
                    lastDirection = Velocity;
                }
            }
            if (!Force.isForcing)
                ManagerCurrentPlatform(Velocity.x);
            base.FixedUpdateChild();
        }
    }
    protected virtual IEnumerator ChengueStateBecauseConfusing()
    {
        if (!Force.isForcing)
            Velocity = new Vector2(0, Velocity.y);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(TimeConfusing * 0.95f));
        fsm.PerformTransition(Transition.AI_PersueToIdle);


    }

    public override void Kill()
    {
        fsm.PerformTransition(Transition.AI_PersueToDead);
    }
}
