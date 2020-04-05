using UnityEngine;

public class KlausCrushReceived : MonoBehaviour, ICrushObject
{
    public void Crush(TypeCrush type = TypeCrush.Middle)
    {
        GetComponentInChildren<MoveState>().Kill();
    }
}
