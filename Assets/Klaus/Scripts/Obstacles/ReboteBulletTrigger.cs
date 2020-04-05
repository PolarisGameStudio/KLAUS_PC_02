using UnityEngine;
using System.Collections;

public class ReboteBulletTrigger : BulletTrigger, IForceObject
{
    bool isForcing = false;
    protected BulletInfo info;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Glitch"))
        {
            if (box.enabled)
            {
                if (other.CompareTag("Player"))
                {
                    if (isElectricKill)
                        other.GetComponent<DeadState>().typeOfDead = DeadType.Ray;
                    other.GetComponent<MoveState>().Kill(bulletDeathSFX);
                    box.enabled = false;
                    HandlerDestroy();
                } else if (other.CompareTag("Resorte"))
                {

                } else if (other.CompareTag("Pistol"))
                {

                } else
                {
                    KillObject killer = other.GetComponent<KillObject>();
                    //                Debug.Log(other.name +" Killobject: "+ killer);
                    if (killer != null)
                        killer.Kill();

                    box.enabled = false;
                    HandlerDestroy();
                }
            }
        }
    }

    public bool ApplyForce(Vector2 dir, float forceX, float forceY, bool isObject, float time = 0f)
    {
        if (!isForcing)
        {
            isForcing = true;
            info.direction = dir;
            info.timeLive = 100;
            handler.SetDirection(info);
            StartCoroutine(ResetIsForcing((time == 0 ? 0.1f : time)));
            return true;
        }
        return false;
    }

    IEnumerator ResetIsForcing(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isForcing = false;
    }

}
