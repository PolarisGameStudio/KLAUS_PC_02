using UnityEngine;
using System.Collections;

public class CounterTimerPlay : MonoSingleton<CounterTimerPlay>
{
    public float TimePlayingLevel
    {
        get
        {
            return currentTime;
        }
    }
    float currentTime = 0;

    public void StartTime(float startIn = 0)
    {
        currentTime = 0;
        currentTime += startIn;
        StopCoroutine("LevelCounterTime");
        StartCoroutine("LevelCounterTime");

    }

    public float EndTime()
    {
        StopCoroutine("LevelCounterTime");
        return currentTime;

    }
    IEnumerator LevelCounterTime()
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
