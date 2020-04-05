using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerStop : Singleton<ManagerStop>
{
    protected bool isStop = false;

    public static bool Stop
    {
        get
        {
            if (Instance != null)
            {
                return Instance.isStop;
            }
            else
            {
                return false;
            }

        }
    }

    public float StoppingTime = 1.0f;

    public delegate void onStopGameBroadcast(bool isStop);

    public event onStopGameBroadcast OnStopGame;

    public static void SubscribeOnStopGame(onStopGameBroadcast funct)
    {
        if (Instance != null)
            Instance.OnStopGame += funct;
    }

    public static void UnSubscribeOnStopGame(onStopGameBroadcast funct)
    {
        if (InstanceExists())
            Instance.OnStopGame -= funct;
    }

    public void StopAll()
    {
        StartCoroutine("CallStop", StoppingTime);
    }

    public void StopAll(float time)
    {
        StartCoroutine("CallStop", time);
    }

    IEnumerator CallStop(float time)
    {
        if (!isStop)
        {
            isStop = true;
            if (OnStopGame != null)
                OnStopGame(true);
            yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
            isStop = false;
            if (OnStopGame != null)
                OnStopGame(false);
        }
    }
}
