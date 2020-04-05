using UnityEngine;
using System.Collections;

public class MeetK1Glitch_Trophy : TrophieLogic
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnEnterAction(other);
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") && !UnLock;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
        if (UnLock)
            return;
        UnLock = true;
        enabled = false;
    }

}
