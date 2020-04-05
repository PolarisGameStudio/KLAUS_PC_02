using UnityEngine;
using System.Collections;

public class TriggerShakeWithText : TriggerHistory
{
    public float TimeStop = 0.3f;
    public float ShakeTime = 0.5f;
    public int ShakePreset = 1;

    public TweenTextShow tween;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        CharacterManager.Instance.FreezeAllWithTimer(TimeStop);
        if (CameraShake.Instance)
            CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);

        StartCoroutine("ShowText", TimeStop);
    }

    IEnumerator ShowText(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        tween.InitText();
    }
}


