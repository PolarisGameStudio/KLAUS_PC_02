using UnityEngine;
using System.Collections;

public class AI_TazaTrigger : BulletBaseTrigger
{

    public AI_TazaAnimator anim;
    public AI_TazaHandler handler;
	public GameObject mugBreakSFX;
    protected override void OnEnable()
    {
        base.OnEnable();
        mugBreakSFX.CreatePool(2);
    }

    public override void HandlerDestroy()
    {
        anim.DestroyAnim();
        mugBreakSFX.Spawn(transform.position, transform.rotation);
        handler.Stop();

    }
}
