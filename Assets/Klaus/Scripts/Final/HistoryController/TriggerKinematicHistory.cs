using UnityEngine;
using System.Collections;

public class TriggerKinematicHistory : TriggerHistory
{
    public Rigidbody2D[] rigids;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        for (int i = 0; i < rigids.Length; ++i)
        {
            rigids [i].isKinematic = false;
        }

    }
}
