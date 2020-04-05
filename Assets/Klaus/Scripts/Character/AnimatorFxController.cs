using UnityEngine;
using System.Collections;

public class AnimatorFxController : MonoBehaviour
{

    public Animator animCharacter;
    public MoveState move;

    public Animator JumpFx;
    public Animator StartStepFx;
    public Animator StepRunFx;
    public Animator ChangeDirFx;


    protected float speedStore = 0;
    float dirStore = 1;
    bool canSpawnSteps = true;
    bool canSpawnChangeDir = true;

    public float TimeForEachStep = 0.5f;

    void Awake()
    {
        JumpFx.CreatePool(1);
        StartStepFx.CreatePool(2);
        StepRunFx.CreatePool(4);
        ChangeDirFx.CreatePool(2);
    }
    // Use this for initialization
    protected virtual void OnEnable()
    {
        move.SuscribeOnJump(Jump);
        speedStore = 0;
    }

    protected virtual void OnDisable()
    {
        move.UnSuscribeOnJump(Jump);
    }

    IEnumerator RespawnStep(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        canSpawnSteps = true;
    }

    IEnumerator RespawnChangeDir(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        canSpawnChangeDir = true;
    }

    void Update()
    {
        if (!ManagerPause.Pause)
        {
            if (animCharacter.GetBool("isGround"))
            {
                if (animCharacter.GetFloat("SpeedX") > 4)
                {
                    if (speedStore < animCharacter.GetFloat("SpeedX"))
                    {

                        
                        Animator anim = StartStepFx.Spawn(transform.position);
                        anim.transform.localScale = new Vector3(-1 * transform.parent.localScale.x,
                            anim.transform.localScale.y,
                            anim.transform.localScale.z);
                        anim.Rebind();
                        canSpawnSteps = false;
                        StopCoroutine("RespawnStep");
                        StartCoroutine("RespawnStep", TimeForEachStep);

                    } else if (dirStore != transform.parent.localScale.x)
                    {
                        //     if (canSpawnChangeDir)
                        {
                            EffectChangeFast();

                        }
                    } else
                    {
                        if (canSpawnSteps)
                        {
                            Animator anim = StepRunFx.Spawn(transform.position);
                            anim.transform.localScale = new Vector3(-1 * transform.parent.localScale.x,
                                anim.transform.localScale.y,
                                anim.transform.localScale.z);
                            anim.Rebind();
                            canSpawnSteps = false;
                            StopCoroutine("RespawnStep");
                            StartCoroutine("RespawnStep", TimeForEachStep);
                        }
                    }
                }
                dirStore = transform.parent.localScale.x;
                speedStore = animCharacter.GetFloat("SpeedX");
            }
        }
    }

    protected void Jump(float per)
    {
        if (animCharacter.GetBool("isGround"))
        {
            Animator anim = JumpFx.Spawn(transform.position);
            anim.transform.localScale = transform.parent.localScale;
            anim.Rebind();
        }
    }

    public void EffectJump()
    {
        Jump(0);
    }

    public void EffectChangeFast()
    {
        Animator anim = ChangeDirFx.Spawn(transform.position);
        anim.transform.localScale = new Vector3(-1 * transform.parent.localScale.x,
            anim.transform.localScale.y,
            anim.transform.localScale.z);
        anim.Rebind();
        canSpawnSteps = false;
        canSpawnChangeDir = false;
        StopCoroutine("RespawnStep");
        StartCoroutine("RespawnStep", TimeForEachStep);
        StopCoroutine("RespawnChangeDir");
        StartCoroutine("RespawnChangeDir", TimeForEachStep);
    }
}
