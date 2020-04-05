using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TriggerMovePrefab : TriggerHistory
{
    public Transform Prefab;
    public Transform Spot;
    public float TimeToReach = 1.0f;
    Tween tween;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        tween = Prefab.DOMove(Spot.position, TimeToReach).OnComplete(OnComplete);

    }

    void OnComplete()
    {
        tween = null;
    }

    public override void OnPauseGame()
    {
        if (tween != null)
        {
            tween.Pause();
        }
    }

    public override void OnResumeGame()
    {
        if (tween != null)
        {
            tween.Play();
        }
    }
}
