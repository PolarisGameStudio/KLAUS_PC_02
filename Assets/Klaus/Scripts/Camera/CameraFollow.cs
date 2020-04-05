//
// CameraMovement.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

[RequireComponent(typeof(ProCamera2D))]
public class CameraFollow : Singleton<CameraFollow>
{

    public float TimeToReachNewTarget = 0.5f;
    protected float store_HorizontalFollowSmoothness;
    protected float store_VerticalFollowSmoothness;

    public Transform currentTarget { get; protected set; }

    #region Cambiar de targetTemporalmente

    Transform storeTarget;
    bool isTemporal = false;

    #endregion

    ProCamera2D _proCamera = null;

    public ProCamera2D proCamera2D
    {

        get
        {
            if (_proCamera == null)
                _proCamera = GetComponent<ProCamera2D>();

            return _proCamera;
        }
    }

    bool firstRun = true;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;

        store_HorizontalFollowSmoothness = proCamera2D.HorizontalFollowSmoothness;
        store_VerticalFollowSmoothness = proCamera2D.VerticalFollowSmoothness;
    }

    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
        if (!proCamera2D.enabled)
        {
            proCamera2D.enabled = true;
        }
    }
    public void MoveCameraToOwnPos()
    {
        proCamera2D.MoveCameraInstantlyToPositionKlaus();

    }
    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        proCamera2D.enabled = false;

    }

    public void OnPauseGame()
    {
        proCamera2D.enabled = false;
    }

    public void OnResumeGame()
    {
        proCamera2D.enabled = true;

    }
    public void ChangueTargetOnly(Transform temporalTarget, float timeLook, float speedX, float speedY)
    {
        if (isTemporal)
            return;
        CharacterManager.Instance.FreezeAll();
        storeTarget = currentTarget;
        SetTarget(temporalTarget, speedX, speedY);
        StartCoroutine("ChangeTargetForTemporal", timeLook + TimeToReachNewTarget);
        isTemporal = true;
    }
    public void ChangueTargetOnly(Transform temporalTarget, float timeLook)
    {
        if (isTemporal)
            return;
        CharacterManager.Instance.FreezeAll();
        storeTarget = currentTarget;
        SetTarget(temporalTarget);
        StartCoroutine("ChangeTargetForTemporal", timeLook);
        isTemporal = true;
    }

    IEnumerator ChangeTargetForTemporal(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isTemporal = false;
        SetTarget(storeTarget);
        storeTarget = null;
        CharacterManager.Instance.UnFreezeAll();
    }

    public void SetTarget(Transform newTarget)
    {
        SetTargetBase(newTarget, TimeToReachNewTarget);
        proCamera2D.HorizontalFollowSmoothness = store_HorizontalFollowSmoothness;
        proCamera2D.VerticalFollowSmoothness = store_VerticalFollowSmoothness;
    }
    public void SetTarget(Transform newTarget, float time)
    {
        SetTargetBase(newTarget, time);
        proCamera2D.HorizontalFollowSmoothness = store_HorizontalFollowSmoothness;
        proCamera2D.VerticalFollowSmoothness = store_VerticalFollowSmoothness;
    }
    void SetTargetBase(Transform newTarget, float timeReachNewTarget)
    {
        if (isTemporal)
            return;

        proCamera2D.RemoveCameraTarget(currentTarget);
        proCamera2D.RemoveCameraTarget(newTarget);
        currentTarget = newTarget;
        proCamera2D.AddCameraTarget(newTarget, 1, 1, timeReachNewTarget);
    }

    public void SetTarget(Transform newTarget, float timeH, float timeY)
    {
        if (isTemporal)
            return;
        SetTargetBase(newTarget, TimeToReachNewTarget);
        proCamera2D.HorizontalFollowSmoothness = timeH;
        proCamera2D.VerticalFollowSmoothness = timeY;
    }
    public void SetTarget(Transform newTarget, float time, float timeH, float timeY)
    {
        if (isTemporal)
            return;
        SetTargetBase(newTarget, time);
        proCamera2D.HorizontalFollowSmoothness = timeH;
        proCamera2D.VerticalFollowSmoothness = timeY;
    }
    public float FollowSmoothH
    {
        get
        {
            return proCamera2D.HorizontalFollowSmoothness;
        }
    }

    public float FollowSmoothV
    {
        get
        {
            return proCamera2D.VerticalFollowSmoothness;
        }
    }
}
