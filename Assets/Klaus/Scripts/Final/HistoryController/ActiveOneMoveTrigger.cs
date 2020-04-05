using UnityEngine;
using System.Collections;

public class ActiveOneMoveTrigger : TriggerHistory
{

    public bool OnlyRigth = true;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        MoveState move = other.GetComponent<MoveState>();
        if (OnlyRigth)
        {
            move.onlyMoveRigth = true;
            move.onlyMoveLeft = false;
        } else
        {
            move.onlyMoveLeft = true;
            move.onlyMoveRigth = false;
        }
    }
}
