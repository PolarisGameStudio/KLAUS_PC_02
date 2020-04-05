using UnityEngine;
using System.Collections;

public class TriggerShake : TriggerHistory
{
    public float TimeStop = 0.3f;
    public float ShakeTime = 0.5f;
    public int ShakePreset = 1;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        CharacterManager.Instance.FreezeAllWithTimer(TimeStop);
        if (CameraShake.Instance && ShakeTime > 0.0f)
            CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);

    }
}
