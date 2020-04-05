using UnityEngine;
using System.Collections;

public class Rat_DeadState : BaseState
{
    public float TimeMuriendo = 3.0f;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Dead;

        AddTransition(Transition.AI_DeadToIdle, StateID.AI_Idle);
        AddTransition(Transition.AI_DeadToPersue, StateID.AI_Persue);
    }


    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        StartCoroutine("FinishDie");
        rigidBody2D.isKinematic = true;
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].enabled = false;
        }
        Velocity = Vector2.zero;
        animator.SetFloat(speedAnimName, 0);


    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
        StopCoroutine("FinishDie");
    }

    IEnumerator FinishDie()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(TimeMuriendo));
        currentPlatform.ResetPlatform();

        gameObject.SetActive(false);
    }
}
