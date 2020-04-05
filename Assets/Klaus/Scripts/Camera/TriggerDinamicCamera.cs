using UnityEngine;
using System.Collections;

public class TriggerDinamicCamera : MonoBehaviour
{

    public float Zoom = 3;
    public float ZoomSmoothness = 1.0f;
    public float TimetoReach = 1.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnEnterAction(other);
        } 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            OnExitAction(other);
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MoveState move = other.GetComponent<MoveState>();
            if (move.getLegsCollider() == other)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void OnEnterAction(Collider2D other)
    {
       
        if (other.GetComponent<PlayerInfo>().playerType == PlayersID.Player1Klaus)
        {
            DynamicCameraManager.Instance.ChangueEspecialTargetForKlaus(transform,TimetoReach, Zoom, ZoomSmoothness);
        } else if (other.GetComponent<PlayerInfo>().playerType == PlayersID.Player2K1)
        {
            DynamicCameraManager.Instance.RemoveEspecialTargetForK1();
            DynamicCameraManager.Instance.ChangueEspecialTargetForK1(transform,TimetoReach, Zoom, ZoomSmoothness);
        }

    }

    protected virtual void OnExitAction(Collider2D other)
    {
        if (other.GetComponent<PlayerInfo>().playerType == PlayersID.Player1Klaus)
        {

            DynamicCameraManager.Instance.RemoveEspecialTargetForKlaus(TimetoReach);
        } else if (other.GetComponent<PlayerInfo>().playerType == PlayersID.Player2K1)
        {
            DynamicCameraManager.Instance.RemoveEspecialTargetForK1();
        }

    }
}
