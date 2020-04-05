using UnityEngine;
using System.Collections;

public class KillFinalBossTrophy : TrophieLogic
{

    public void CompleteTrophy()
    {
        if (UnLock)
            return;

        UnLock = true;
        enabled = false;
    }
}
