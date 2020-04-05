using UnityEngine;
using System.Collections;

public class TrophieLogic : MonoBehaviour
{

    [Range(1, 40)]
    public int ID = 1;
    bool isUnlock = false;
    protected bool UnLock
    {
        get
        {
            return isUnlock;
        }
        set
        {
            if (value && !isUnlock)
            {
                isUnlock = true;
                TrophiesManager.Instance.AwardTrophy(ID);

            }
        }
    }
}
