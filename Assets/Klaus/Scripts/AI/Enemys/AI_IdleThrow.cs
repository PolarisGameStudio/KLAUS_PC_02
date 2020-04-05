using UnityEngine;
using System.Collections;

public class AI_IdleThrow : BaseState
{
    public AI_IsVisibleSprite isVisible;

    public AI_TazaHandler taza;

    public float TimeToLaunch = 0.25f;

    public float TimeResetLaunch = 1.0f;

    public float ForceY = 600;

    public float DistanceX = 3;
    public float ForceInDistanceX = 120;


    bool Launching = false;
    public GameObject throwSFX;
    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Idle;

        AddTransition(Transition.AI_IdleToDead, StateID.AI_Dead);

        taza.CreatePool(2);
        if (throwSFX != null)
            throwSFX.CreatePool(2);
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        if (!Force.isForcing && IsGround.isCollide)
            Velocity = Vector2.zero;

        Launching = false;
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
        StopCoroutine("Launch");
    }

    protected override void Reason()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {

        }
    }

    protected override void UpdateChild()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (isVisible.isVisible)
            {
                Vector2 dir = finderPlayer.DirectionToPlayer;
                if (dir != Vector2.zero)
                {
                    m_Flip.FlipIfCanFlip(dir);

                    if (Launching == false)
                    {
                        Launching = true;
                        StopCoroutine("Launch");
                        StartCoroutine("Launch", dir);
                    }
                }
            }
        }
    }

    IEnumerator Launch(Vector2 dir)
    {
        animator.SetTrigger("Throw");
        if (throwSFX != null)
            throwSFX.Spawn(transform.position, transform.rotation);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToLaunch));
        float newForceX = finderPlayer.DistanceToPlayer * ForceInDistanceX / DistanceX;
        AI_TazaHandler tacita = taza.Spawn(transform.position + Vector3.up * 0.5f, transform.rotation);
        tacita.Force(new Vector2(dir.x * newForceX, 0.5f * ForceY));
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeResetLaunch));
        Launching = false;

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

    public override void Kill()
    {
        fsm.PerformTransition(Transition.AI_IdleToDead);
    }
}
