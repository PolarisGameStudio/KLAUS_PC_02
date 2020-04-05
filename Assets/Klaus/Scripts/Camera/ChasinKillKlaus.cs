using UnityEngine;
using System.Collections;

public class ChasinKillKlaus : MonoBehaviour
{

    bool isVisible = true;

    public bool canKillCamera
    {

        get
        {
            return canKill;
        }
        set
        {
            canKill = value;
            if (canKill)
            {
                if (!isVisible)
                {
                    if (ChaseManager.InstanceExists())
                    {
                        ChaseManager.Instance.StartKill(Number);
                    }
                }
            }
        }
    }

    protected bool canKill = false;
    [HideInInspector]
    public int Number = -1;

    public void OnBecameInvisible()
    {
        isVisible = false;
        if (canKillCamera)
        {
            if (ChaseManager.InstanceExists())
            {
                ChaseManager.Instance.StartKill(Number);
            }
        }
    }

    public void OnBecameVisible()
    {
        isVisible = true;
        if (canKillCamera)
        {
            if (ChaseManager.InstanceExists())
                ChaseManager.Instance.StopKill(Number);

        }
    }

    public void PunchMiddle()
    {

    }

    public void Dead()
    {

    }
}
