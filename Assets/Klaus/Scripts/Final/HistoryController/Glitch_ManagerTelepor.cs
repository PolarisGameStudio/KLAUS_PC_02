using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Glitch_ManagerTelepor : Singleton<Glitch_ManagerTelepor>
{

    Dictionary<int, bool> isObjectTeleporting = new Dictionary<int, bool>();

    public static bool CanTeleportStatic(int ID)
    {
        if (Instance != null)
        {
            if (!Instance.isObjectTeleporting.ContainsKey(ID))
            {
                Instance.isObjectTeleporting.Add(ID, false);
            }
            return !Instance.isObjectTeleporting[ID];
        }
        return false;
    }

    public bool CanTeleport(int ID)
    {
        if (!isObjectTeleporting.ContainsKey(ID))
        {
            isObjectTeleporting.Add(ID, false);
        }
        return !isObjectTeleporting[ID];
    }

    public bool Teleport(int ID, float time)
    {
        if (!isObjectTeleporting[ID])
        {
            isObjectTeleporting[ID] = true;
            StartCoroutine(ResetTeleport(ID, time));
            return true;
        }
        return false;
    }

    IEnumerator ResetTeleport(int id, float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        isObjectTeleporting[id] = false;
    }
}
