using UnityEngine;
using System.Collections;

public class TriggerSound : TriggerHistory
{
    public float TimeStop = 0.3f;

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        CharacterManager.Instance.FreezeAllWithTimer(TimeStop);
        //REPRODUCIR SONIDO AQUI

    }
}
