using UnityEngine;
using System.Collections;

public class TriggerPlayerZoomDynamicCamera : TriggerDinamicCamera
{
    protected override void OnEnterAction(Collider2D other)
    {
        if (other.GetComponent<PlayerInfo>().playerType ==  PlayersID.Player1Klaus)
        {
            DynamicCameraManager.Instance.ChangueZoomToKlaus(Zoom, ZoomSmoothness);

        } else if (other.GetComponent<PlayerInfo>().playerType ==  PlayersID.Player2K1)
        {
            DynamicCameraManager.Instance.ChangueZoomToK1(Zoom, ZoomSmoothness);
         
        }
    }

    protected override void OnExitAction(Collider2D other)
    {
        if (other.GetComponent<PlayerInfo>().playerType ==  PlayersID.Player1Klaus)
        {
            DynamicCameraManager.Instance.RemoveEspecialZoomForKlaus();

        } else if (other.GetComponent<PlayerInfo>().playerType ==  PlayersID.Player2K1)
        {
            DynamicCameraManager.Instance.RemoveEspecialZoomForK1();

        }
    }
}
