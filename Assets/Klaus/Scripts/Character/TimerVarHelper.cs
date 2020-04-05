using UnityEngine;
using System.Collections;
using System;

public class TimerVarHelper : MonoBehaviour
{
    public void StartTimer(Action finish, float time)
    {
        StartCoroutine(StartTime(finish, time));
    }
    public IEnumerator StartTime(Action finish, float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        if (finish != null)
            finish();
    }

}
