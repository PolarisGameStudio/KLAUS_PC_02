using UnityEngine;
using System.Collections;

public class AI_KilleableExplode : AI_Killeable
{

    public AI_ExplodeTrigger killer;
    protected MoveStateK1 _moveStateTarget;


    public MoveStateK1 moveStateTarget
    {
        get
        {
            if (_moveStateTarget == null)
            {
                _moveStateTarget = GameObject.FindObjectOfType<MoveStateK1>();

            }
            return _moveStateTarget;
        }
    }

    public override void Crush(TypeCrush type = TypeCrush.Middle)
    {
        if (CameraShake.Instance != null)
            CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);

        killer.Explode(moveStateTarget);
        ManagerStop.Instance.StopAll(0.1f);

    }
}
