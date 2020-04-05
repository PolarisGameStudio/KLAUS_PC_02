using UnityEngine;
using System.Collections;

public class CompanyRecorTrophy : TrophieLogic
{

    public void CompleteTrophy()
    {
        if (UnLock)
            return;
        UnLock = true;
        enabled = false;
    }

}
