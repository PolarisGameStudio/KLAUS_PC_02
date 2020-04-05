using UnityEngine;
using System.Collections;

public class AI_ExplodeState : AI_DeadState
{
    public override void OnStopGame(bool value)
    {
        if (value)
        {
            isKinematicStop = rigidBody2D.isKinematic;

            rigidBody2D.isKinematic = true;
            velStop = rigidBody2D.velocity;
            rigidBody2D.velocity = Vector2.zero;

        } else
        {
            rigidBody2D.isKinematic = isKinematicStop;
            rigidBody2D.velocity = velStop;
        }
    }
        
        
}
