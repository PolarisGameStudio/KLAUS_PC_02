using UnityEngine;
using System.Collections;

public class TriggerWeakPlatformHistory : TriggerHistory
{
    [Tooltip("Aqui Agregas los Collider")]
    public Behaviour[] monotoDestroy;
    [Tooltip("Aqui Agregas los SpriteRenderer")]
    public Renderer[] rendertoDestroy;
    [Tooltip("Aqui Agregas los Animator")]
    public Animator[] anim;

    public float TimeToDestroy = 0.5f;


    void ManagerDestroyMono(bool value)
    {
        for (int i = 0; i < monotoDestroy.Length; ++i)
            monotoDestroy [i].enabled = value;

    }

    void ManagerDetroyRender(bool value)
    {
        for (int i = 0; i < rendertoDestroy.Length; ++i)
            rendertoDestroy [i].enabled = value;
    }

    void ManagerAnimatorDestroy()
    {
        for (int i = 0; i < anim.Length; ++i)
            anim [i].SetTrigger("Destroy");
    }

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        StartCoroutine("DestroyPlatform", TimeToDestroy);

    }

    IEnumerator DestroyPlatform(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        ManagerDestroyMono(false);
        ManagerAnimatorDestroy();
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.4f));
        ManagerDetroyRender(false);
    }
}
