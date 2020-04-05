using UnityEngine;
using System.Collections;

public class AI_DeadState : BaseState
{
    public float TimeMuriendo = 3.0f;

    public float TimeResp = 2.0f;

    public AI_CheckPoint positionToRespawn;

    public bool CanRevive = true;

    public AI_KeyChain key;

    public GameObject robotDead;

    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Dead;

        AddTransition(Transition.AI_DeadToIdle, StateID.AI_Idle);
        AddTransition(Transition.AI_DeadToPersue, StateID.AI_Persue);
        AddTransition(Transition.AI_FollowToDead, StateID.AI_Follow);

        robotDead.CreatePool(1);
        ManagerAI_Respawn.Instance.AddAI(this);
    }

    float store_gravity = 0;
    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        animator.SetTrigger("Kill");
        StartCoroutine("FinishDie");
        robotDead.Spawn(transform.position, transform.rotation);
        rigidBody2D.isKinematic = true;
        store_gravity = rigidBody2D.gravityScale;
        rigidBody2D.gravityScale = 0;

        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].enabled = false;
        }
        Velocity = Vector2.zero;


        key.DropKey();
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
        rigidBody2D.isKinematic = false;
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].enabled = true;
        }
        rigidBody2D.gravityScale = store_gravity;
        StopCoroutine("FinishDie");
    }

    IEnumerator FinishDie()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(TimeMuriendo));
        currentPlatform.ResetPlatform();
        if (CanRevive)
        {
            ManagerAI_Respawn.Instance.Respawn(this);
        }

        gameObject.SetActive(false);

    }

    protected override void Reason()
    {
    }

    protected override void FixedUpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
            base.FixedUpdateChild();

    }

    protected override void UpdateChild()
    {
    }

    protected override void LateUpdateChild()
    {

    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Respawn");
        animator.Rebind();
        fsm.PerformTransition(Transition.AI_FollowToDead);
    }


}
