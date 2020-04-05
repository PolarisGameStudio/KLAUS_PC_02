using UnityEngine;
using System.Collections;

public class AI_FollowState : BaseState
{

    public Vector3 ToPosition = Vector3.zero;
    public float SpeedInX = 1;
    protected Vector2 Direction;

    public float minDistanceToReach = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Follow;

        AddTransition(Transition.AI_FollowToDead, StateID.AI_Dead);
        AddTransition(Transition.AI_FollowToPersue, StateID.AI_Persue);
        AddTransition(Transition.AI_FollowToIdle, StateID.AI_Idle);

        if (ToPosition == Vector3.zero)
            ToPosition = transform.position;
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
            if (finderPlayer.isPlayerNear)
            {
                fsm.PerformTransition(Transition.AI_FollowToPersue);
            }
            else if (Direction.magnitude <= minDistanceToReach)
            {
                fsm.PerformTransition(Transition.AI_FollowToIdle);
            }
            else if (Mathf.Abs(ToPosition.y - transform.position.y) > 5)
            {
                fsm.PerformTransition(Transition.AI_FollowToIdle);

            }
        }
    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            Direction = ToPosition - transform.position;
            if (!isFreeze && !Force.isForcing)
                Velocity = new Vector2(Direction.normalized.x * SpeedInX, Velocity.y);

            if (!Force.isForcing)
                ManagerCurrentPlatform(Velocity.x);

            base.FixedUpdateChild();
        }
    }

    protected override void UpdateChild()
    {
    }

    protected override void LateUpdateChild()
    {
    }

    public override void Kill()
    {
        fsm.PerformTransition(Transition.AI_FollowToDead);
    }
}
