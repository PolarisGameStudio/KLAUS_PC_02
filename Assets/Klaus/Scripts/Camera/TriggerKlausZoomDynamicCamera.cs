using UnityEngine;
using System.Collections;

public class TriggerKlausZoomDynamicCamera : TriggerDinamicCamera
{
    [HideInInspector]
    public PlayersID TypePlayer = PlayersID.Player1Klaus;

    protected override bool CompareDefinition(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MoveState move = other.GetComponent<MoveState>();
            if (move.getLegsCollider() == other)
            {
                if (move.info.playerType == TypePlayer)
                    return true;
            }
        }
        return false;
    }

    protected override void OnEnterAction(Collider2D other)
    {
        DynamicCameraManager.Instance.ChangueZoomToKlaus(Zoom, ZoomSmoothness);
    }

    protected override void OnExitAction(Collider2D other)
    {
        DynamicCameraManager.Instance.RemoveEspecialZoomForKlaus();
    }
}
