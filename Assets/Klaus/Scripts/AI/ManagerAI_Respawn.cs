using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerAI_Respawn : Singleton<ManagerAI_Respawn>
{


    Dictionary<int, BaseState> ais = new Dictionary<int, BaseState>();
    Dictionary<int, Coroutine> ais_Time = new Dictionary<int, Coroutine>();

    public void AddAI(BaseState ai)
    {

        ais.Add(ai.gameObject.GetInstanceID(), ai);
        ais_Time.Add(ai.gameObject.GetInstanceID(), null);

    }

    public void Respawn(BaseState ai)
    {
        ais_Time[ai.gameObject.GetInstanceID()] = StartCoroutine(TimeRespawning(ai, ((AI_DeadState)ai).TimeResp));
    }

    IEnumerator TimeRespawning(BaseState ai, float time)
    {
        Transform spot = ((AI_DeadState)ai).transform;
        bool isCheckPoit = false;
        if (((AI_DeadState)ai).positionToRespawn != null)
        {
            isCheckPoit = true;
            if (((AI_DeadState)ai).positionToRespawn.gameObject.activeSelf)
            {
                spot = ((AI_DeadState)ai).positionToRespawn.transform;
            }
        }
        ai.transform.position = spot.position;
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        if (!isCheckPoit)
        {
            ((AI_DeadState)ai).Respawn();
            ais_Time[ai.gameObject.GetInstanceID()] = null;
        }
        else if (((AI_DeadState)ai).positionToRespawn != null && ((AI_DeadState)ai).positionToRespawn.gameObject.activeSelf)
        {
            ((AI_DeadState)ai).positionToRespawn.RotateArrow();
            ((AI_DeadState)ai).Respawn();
            ais_Time[ai.gameObject.GetInstanceID()] = null;
        }

    }

    public void FreezeAll()
    {
        foreach (var ai in ais)
        {
            if (ai.Value.gameObject.activeSelf)
                ai.Value.SendMessage("Freeze");
        }
    }

    public void UnFreezeAll()
    {
        foreach (var ai in ais)
        {
            if (ai.Value.gameObject.activeSelf)
                ai.Value.SendMessage("UnFreeze");
        }
    }
}
