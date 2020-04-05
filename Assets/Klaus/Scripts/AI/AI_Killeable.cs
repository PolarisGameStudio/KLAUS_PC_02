using UnityEngine;
using System.Collections;

public class AI_Killeable : KillObject, ICrushObject
{

    public FSMSystem fsm;
    public float ShakeTime = 0.5f;
    public int ShakePreset = 0;
    public int TypeEnemy = -1;
    public override void Kill()
    {
        if (fsm.CurrentStateID != StateID.AI_Dead)
        {

            ((BaseState)fsm.CurrentState).Kill();
        }
    }

    public virtual void Crush(TypeCrush type = TypeCrush.NONE)
    {
        if (CameraShake.Instance)
            CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);
        if (type != TypeCrush.NONE)
            switch (TypeEnemy)
            {
                case 0:
                    if (SaveManager.Instance.dataKlaus != null)
                    {
                        SaveManager.Instance.dataKlaus.AddDestroy_Clone();
                    }
                    break;
                case 1:
                    if (SaveManager.Instance.dataKlaus != null)
                    {
                        SaveManager.Instance.dataKlaus.AddDestroy_MugClone();
                    }
                    break;
            }
        Kill();
        ManagerStop.Instance.StopAll(0.1f);
    }
}
