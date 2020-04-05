using UnityEngine;
using System.Collections;

public class UnlockArcade_Trophy : TrophieLogic
{

    public void UnlockArcade()
    {
        if (UnLock)
            return;

        UnLock = true;
        enabled = false;
    }
}
