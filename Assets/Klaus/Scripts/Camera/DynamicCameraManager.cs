using UnityEngine;
using System.Collections;


public enum TargetCamera
{
    Klaus,
    K1,
    EspecialKlaus,
    EspecialK1
}

public class DynamicCameraManager : Singleton<DynamicCameraManager>
{
    public float DefaultTimeToReachZoom = 2.0f;
    [HideInInspector]
    public Transform Klaus;
    public float ZoomKlaus = 4;
    [HideInInspector]
    public Transform K1;
    public float ZoomK1 = 4;

    [HideInInspector]
    public Transform TargetEspecialKlaus;
    [HideInInspector]
    public float ZoomEspecialForKlaus = 3;
    [HideInInspector]
    public Transform TargetEspecialK1;
    [HideInInspector]
    public float ZoomEspecialForK1 = 3;

    protected TargetCamera currentTargetKlaus = TargetCamera.Klaus;
    protected TargetCamera currentTargetK1 = TargetCamera.K1;
    protected TargetCamera currentTargetGlobal = TargetCamera.Klaus;

    #region Manajeador de Seguir Personajes:

    protected override void AwakeChild()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; ++i)
        {
            PlayerInfo info = player [i].GetComponent<PlayerInfo>();

            switch (info.playerType)
            {
                case PlayersID.Player1Klaus:
                    Klaus = player [i].transform;
                    break;
                case PlayersID.Player2K1:
                    K1 = player [i].transform;
                    break;
            }
        }
    }

    public void ChangueToTarget(Transform target)
    {
        PlayerInfo info = target.GetComponent<PlayerInfo>();
        if (info)
        {
            switch (info.playerType)
            {
                case PlayersID.Player1Klaus:
                    ChangueForKlaus(target);
                    break;
                case PlayersID.Player2K1:
                    ChangueForK1(target);
                    break;
                default:
                    Debug.LogWarning("DANNNGEEER: hay otro personaje o alguien usando algo q no debe.");
                    ChangueForKlaus(target);
                    break;
            }
        }
    }

    protected void ChangueForKlaus(Transform espKlaus)
    {

        if (currentTargetKlaus != TargetCamera.EspecialKlaus)
        {
            if (Klaus == null)
            {
                Klaus = espKlaus;
            }

            TargetEspecialKlaus = Klaus;
            ZoomEspecialForKlaus = ZoomKlaus;

            currentTargetKlaus = TargetCamera.Klaus;

        } else
        {
            currentTargetKlaus = TargetCamera.EspecialKlaus;
        }

        SetCameraToKlaus();
        currentTargetGlobal = currentTargetKlaus;
    }

    protected void ChangueForK1(Transform espK1)
    {

        if (currentTargetK1 != TargetCamera.EspecialK1)
        {
            if (K1 == null)
            {
                K1 = espK1;
            }

            TargetEspecialK1 = K1;
            ZoomEspecialForK1 = ZoomK1;

            currentTargetK1 = TargetCamera.K1;

        } else
        {
            currentTargetK1 = TargetCamera.EspecialK1;
        }

        SetCameraToK1();
        currentTargetGlobal = currentTargetK1;
    }

    #endregion

    #region Manejador de Set Camera en Klaus:

    /// <summary>
    /// Zoom y movimiento hacia klaus
    /// </summary>
    protected void SetCameraToKlaus()
    {
        CameraFollow.Instance.SetTarget(TargetEspecialKlaus);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForKlaus, DefaultTimeToReachZoom);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia klaus
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    protected void SetCameraToKlaus(float time)
    {
        CameraFollow.Instance.SetTarget(TargetEspecialKlaus);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForKlaus, time);
    }
    protected void SetCameraToKlaus(float timeReach,float timeZoom)
    {
        CameraFollow.Instance.SetTarget(TargetEspecialKlaus, timeReach, timeReach);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForKlaus, timeZoom);
    }

    public void ChangueEspecialTargetForKlaus(Transform espKlaus, float espZoom)
    {
        TargetEspecialKlaus = espKlaus;
        ZoomEspecialForKlaus = espZoom;

        if (currentTargetKlaus == TargetCamera.Klaus)
        {
            currentTargetKlaus = TargetCamera.EspecialKlaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetCameraToKlaus();
                currentTargetGlobal = currentTargetKlaus;

            }


        }
    }

    public void ChangueEspecialTargetForKlaus(Transform espKlaus, float espZoom, float time)
    {
        TargetEspecialKlaus = espKlaus;
        ZoomEspecialForKlaus = espZoom;

        if (currentTargetKlaus == TargetCamera.Klaus)
        {
            currentTargetKlaus = TargetCamera.EspecialKlaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetCameraToKlaus(time);
                currentTargetGlobal = currentTargetKlaus;

            }
        }
    }
    public void ChangueEspecialTargetForKlaus(Transform espKlaus, float timeReach,float espZoom, float time)
    {
        TargetEspecialKlaus = espKlaus;
        ZoomEspecialForKlaus = espZoom;

        if (currentTargetKlaus == TargetCamera.Klaus)
        {
            currentTargetKlaus = TargetCamera.EspecialKlaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetCameraToKlaus(timeReach, time);
                currentTargetGlobal = currentTargetKlaus;

            }
        }
    }

    public void ChangueEspecialTargetForK1(Transform espK1, float timeReach, float espZoom, float time)
    {
        TargetEspecialK1 = espK1;
        ZoomEspecialForK1 = espZoom;

        if (currentTargetK1 == TargetCamera.K1)
        {
            currentTargetK1 = TargetCamera.EspecialK1;

            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                
                SetCameraToK1(timeReach, time);
                currentTargetGlobal = currentTargetK1;

            }
        }

    }
    public void RemoveEspecialTargetForKlaus()
    {

        TargetEspecialKlaus = Klaus;
        ZoomEspecialForKlaus = ZoomKlaus;

        if (currentTargetKlaus == TargetCamera.EspecialKlaus)
        {
            currentTargetKlaus = TargetCamera.Klaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetCameraToKlaus();
                currentTargetGlobal = currentTargetKlaus;

            }
        }
    }
    public void RemoveEspecialTargetForKlaus(float time)
    {

        TargetEspecialKlaus = Klaus;
        ZoomEspecialForKlaus = ZoomKlaus;

        if (currentTargetKlaus == TargetCamera.EspecialKlaus)
        {
            currentTargetKlaus = TargetCamera.Klaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetCameraToKlaus(time, DefaultTimeToReachZoom);
                currentTargetGlobal = currentTargetKlaus;

            }
        }
    }

    public void RemoveEspecialTargetForKlaus_DontSetCamera()
    {
        TargetEspecialKlaus = Klaus;
        ZoomEspecialForKlaus = ZoomKlaus;

        if (currentTargetKlaus == TargetCamera.EspecialKlaus)
        {
            currentTargetKlaus = TargetCamera.Klaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                currentTargetGlobal = currentTargetKlaus;
            }
        }
    }

    #region newZoom:

    protected void SetZoomToKlaus()
    {
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForKlaus, DefaultTimeToReachZoom);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia klaus
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    protected void SetZoomToKlaus(float time)
    {
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForKlaus, time);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia klaus
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    public void ChangueZoomToKlaus(float espZoom, float time)
    {
        ZoomEspecialForKlaus = espZoom;
        TargetEspecialKlaus = Klaus;
        if (currentTargetKlaus == TargetCamera.Klaus)
        {
            currentTargetKlaus = TargetCamera.EspecialKlaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetZoomToKlaus(time);
            }
        }
    }

    public void RemoveEspecialZoomForKlaus()
    {

        ZoomEspecialForKlaus = ZoomKlaus;
        TargetEspecialKlaus = Klaus;

        if (currentTargetKlaus == TargetCamera.EspecialKlaus)
        {
            currentTargetKlaus = TargetCamera.Klaus;

            if (currentTargetGlobal == TargetCamera.Klaus
                || currentTargetGlobal == TargetCamera.EspecialKlaus)
            {
                SetZoomToKlaus();
            }
        }
    }

    #endregion

    #endregion

    #region Manejador de Set Camera en Klaus:

    /// <summary>
    /// Zoom y movimiento hacia K1
    /// </summary>
    protected void SetCameraToK1()
    {
        CameraFollow.Instance.SetTarget(TargetEspecialK1);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForK1, DefaultTimeToReachZoom);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia K1
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    protected void SetCameraToK1(float time)
    {
        CameraFollow.Instance.SetTarget(TargetEspecialK1);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForK1, time);
    }
    protected void SetCameraToK1(float timeReach,float timeZoom)
    {
        CameraFollow.Instance.SetTarget(TargetEspecialK1, timeReach, timeReach);
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForK1, timeZoom);
    }
    /// <summary>
    /// Cuando Ya no se debe seguir a K1 si no a un trigger
    /// </summary>
    /// <param name="espK1">Esp k1.</param>
    /// <param name="espZoom">Esp zoom.</param>
    public void ChangueEspecialTargetForK1(Transform espK1, float espZoom)
    {
        TargetEspecialK1 = espK1;
        ZoomEspecialForK1 = espZoom;

        if (currentTargetK1 == TargetCamera.K1)
        {
            currentTargetK1 = TargetCamera.EspecialK1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetCameraToK1();
                currentTargetGlobal = currentTargetK1;

            }
        }


    }

    /// <summary>
    /// Cuando Ya no se debe seguir a K1 si no a un trigger, y el zoom tiene un tiempo default
    /// </summary>
    /// <param name="espK1">Esp k1.</param>
    /// <param name="espZoom">Esp zoom.</param>
    /// <param name="time">Time.</param>
    public void ChangueEspecialTargetForK1(Transform espK1, float espZoom, float time)
    {
        TargetEspecialK1 = espK1;
        ZoomEspecialForK1 = espZoom;

        if (currentTargetK1 == TargetCamera.K1)
        {
            currentTargetK1 = TargetCamera.EspecialK1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetCameraToK1(time);
                currentTargetGlobal = currentTargetK1;

            }
        }

    }

    public void RemoveEspecialTargetForK1()
    {

        TargetEspecialK1 = K1;
        ZoomEspecialForK1 = ZoomK1;

        if (currentTargetK1 == TargetCamera.EspecialK1)
        {
            currentTargetK1 = TargetCamera.K1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetCameraToK1();
                currentTargetGlobal = currentTargetK1;

            }

        }
    }
    public void RemoveEspecialTargetForK1(float time)
    {

        TargetEspecialK1 = K1;
        ZoomEspecialForK1 = ZoomK1;

        if (currentTargetK1 == TargetCamera.EspecialK1)
        {
            currentTargetK1 = TargetCamera.K1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetCameraToK1(time, DefaultTimeToReachZoom);
                currentTargetGlobal = currentTargetK1;

            }

        }
    }
    #region newZoom:

    protected void SetZoomToK1()
    {
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForK1, DefaultTimeToReachZoom);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia klaus
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    protected void SetZoomToK1(float time)
    {
        CameraZoom.Instance.SetTargetSize(ZoomEspecialForK1, time);
    }

    /// <summary>
    /// Zoom objetivo con tiempo y camra hacia klaus
    /// </summary>
    /// <param name="time">Time to ReachNewZoom</param>
    public void ChangueZoomToK1(float espZoom, float time)
    {
        ZoomEspecialForK1 = espZoom;

        if (currentTargetK1 == TargetCamera.K1)
        {
            currentTargetK1 = TargetCamera.EspecialK1;
            TargetEspecialK1 = K1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetZoomToK1(time);
            }
        }
    }

    public void RemoveEspecialZoomForK1()
    {

        TargetEspecialK1 = K1;
        ZoomEspecialForK1 = ZoomK1;

        if (currentTargetK1 == TargetCamera.EspecialK1)
        {
            currentTargetK1 = TargetCamera.K1;
            if (currentTargetGlobal == TargetCamera.K1
                || currentTargetGlobal == TargetCamera.EspecialK1)
            {
                SetZoomToK1();
            }
        }
    }

    #endregion

    #endregion

}
