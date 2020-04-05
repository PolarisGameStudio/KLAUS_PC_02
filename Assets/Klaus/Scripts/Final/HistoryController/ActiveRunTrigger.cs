using UnityEngine;
using System.Collections;

public class ActiveRunTrigger : TriggerHistory {

    protected override void OnEnterAction(Collider2D other) {
        base.OnEnterAction(other);
        MoveState move = other.GetComponent<MoveState>();
        move.activeRun = false;
    }
}
