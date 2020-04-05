using UnityEngine;
using System.Collections;

public class CrushInRaw_Trophy : TrophieLogic
{
    public int MinDestroyObjectInRaw = 4;

    public void OnGetDestroy(int destroy)
    {
        if (UnLock)
            return;

        if (destroy >= MinDestroyObjectInRaw)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
