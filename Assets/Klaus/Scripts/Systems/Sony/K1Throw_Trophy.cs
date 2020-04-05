using UnityEngine;
using System.Collections;

public class K1Throw_Trophy : TrophieLogic
{

    public void OnLevelWasLoaded(int level)
    {
        if (UnLock)
            return;
        ThrowState state = GameObject.FindObjectOfType<ThrowState>();
        if (state != null)
            state.OnEnd += OnThrow;
    }


    void OnThrow()
    {
        if (UnLock)
            return;
        UnLock = true;
        enabled = false;
    }
}
