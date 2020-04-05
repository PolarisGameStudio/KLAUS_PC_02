using UnityEngine;
using System.Collections;

public class K1Plane_Trophy : TrophieLogic
{
    bool isActive = false;
    void OnLevelWasLoaded(int level)
    {
        isActive = false;
    }

    public void StartTrophy()
    {

        if (isActive || UnLock)
            return;
        isActive = true;
    }
    public void EndTrophy()
    {
        if (isActive == false || UnLock)
            return;
        UnLock = true;
        enabled = false;
    }
}
