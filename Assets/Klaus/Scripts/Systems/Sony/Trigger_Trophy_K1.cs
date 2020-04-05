using UnityEngine;
using System.Collections;

public class Trigger_Trophy_K1 : MonoBehaviour
{

    public bool isStart = true;
    public K1Plane_Trophy trophy;
    bool isActivated = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated)
            return;
        if (CompareDefinition(other))
        {
            OnEnterAction(other);
        }
    }
    protected virtual bool CompareDefinition(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return other.GetComponent<MoveState>().info.playerType == PlayersID.Player2K1;
        }
        return false;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
        isActivated = true;
        if (isStart)
            trophy.StartTrophy();
        else
            trophy.EndTrophy();
    }
}
