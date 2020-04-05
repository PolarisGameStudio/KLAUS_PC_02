using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ManagerCheckPoint : Singleton<ManagerCheckPoint>
{
    public float RestartLevelTime = 2.0f;
    protected Dictionary<PlayersID, CheckPoint> currentsCheckPoint = new Dictionary<PlayersID, CheckPoint>();
    public Action<CheckPoint> callbackKill;
    public float NeedWaitRespawn()
    {
        if (callbackKill != null)
            return 0.5f;
        else
            return 0.0f;
    }
    //  protected CheckPoint currentCheckpoint = null;
    public Vector3 PositionToRespawn(PlayersID player)
    {
        if (callbackKill != null)
            callbackKill(currentsCheckPoint[player]);
        return currentsCheckPoint[player].transform.position;

    }
    public void ActivateCHeckpoint(PlayersID player)
    {
        currentsCheckPoint[player].RotateArrow();
        currentsCheckPoint[player].CheckPointUsed();
    }
    public Vector3? getPosition(PlayersID player)
    {
        if (!currentsCheckPoint.ContainsKey(player))
        {
            return null;
        }
        return currentsCheckPoint[player].transform.position;
    }


    public void AddPosition(PlayersID player, CheckPoint pos)
    {
        if (currentsCheckPoint.ContainsKey(player))
        {
            if (currentsCheckPoint[player] != pos)
            {
                currentsCheckPoint[player].ResetCheckPoint(player);

            }
            currentsCheckPoint[player] = pos;
        }
        else
        {
            currentsCheckPoint.Add(player, pos);
        }
    }


    bool isRestarting = false;

    void RestartLevel()
    {
        if (isRestarting)
        {
            LoadLevelManager.Instance.RestartCurrentLevel();
        }
    }
}
