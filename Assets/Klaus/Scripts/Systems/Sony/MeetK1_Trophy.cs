using UnityEngine;
using System.Collections;

public class MeetK1_Trophy : TrophieLogic
{
    public NPChar npcChar;
    // Use this for initialization
    void OnEnable()
    {
        npcChar.ActiveNpcCallback += MeetK1;
    }

    // Update is called once per frame
    void OnDisable()
    {
        if (npcChar != null)
            npcChar.ActiveNpcCallback -= MeetK1;

    }

    void MeetK1()
    {
        if (UnLock)
            return;
        if (SaveManager.Instance.comingFromTimeArcadeMode)
            return;
        UnLock = true;
        enabled = false;
    }
}
