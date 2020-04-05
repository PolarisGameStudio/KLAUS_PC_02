using UnityEngine;
using System.Collections;

public class AI_PersueState : BaseState
{
    public float SpeedInX = 1.0f;

    public float TimeConfusing = 1.0f;
    protected bool isConfusing = false;
    protected Vector2? lastDirection;
    public GameObject robotOn;
    public GameObject robotActive;
    public bool on = false;
    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.AI_Persue;

        AddTransition(Transition.AI_PersueToDead, StateID.AI_Dead);
        AddTransition(Transition.AI_PersueToIdle, StateID.AI_Idle);
        robotOn.CreatePool(2);
    }
    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    private AudioSource _audio;
    void Start()
    {

    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
        if (!Force.isForcing && IsGround.isCollide)
            Velocity = new Vector2(finderPlayer.DirectionToPlayer.x * SpeedInX, Velocity.y);
        lastDirection = Velocity;
        isConfusing = false;
        lastDirection = null;
        StartCoroutine("parasonar", 0.1f);
        //GetComponent<AudioSource>().Play();
        //robotOn.Spawn (transform.position,transform.rotation);

    }
    IEnumerator parasonar(float time)
    {
        yield return new WaitForSeconds(time);
        if (audio != null)
            audio.Play();
        if (!on)
        {
            robotOn.Spawn(transform.position, transform.rotation);
            on = true;
        }
        else
            robotActive.Spawn(transform.position, transform.rotation);
    }
    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();

        StopCoroutine("ChengueStateBecauseConfusing");
        StopCoroutine("parasonar");
        audio.Stop();

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
                    StopCoroutine("parasonar");
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

    protected override void UpdateChild()
    {

    }

    protected override void LateUpdateChild()
    {
    }

    public override void Kill()
    {
        fsm.PerformTransition(Transition.AI_PersueToDead);
    }
}
