using UnityEngine;
using System.Collections;

public class TriggerK1ZoomDynamicCamera : TriggerDinamicCamera
{

    [HideInInspector]
    public PlayersID TypePlayer = PlayersID.Player2K1;

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
        DynamicCameraManager.Instance.ChangueZoomToK1(Zoom, ZoomSmoothness);
    }

    protected override void OnExitAction(Collider2D other)
    {
        DynamicCameraManager.Instance.RemoveEspecialZoomForK1();
    }
}
