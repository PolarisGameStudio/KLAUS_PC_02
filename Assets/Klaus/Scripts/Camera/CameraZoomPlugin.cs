using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraZoomPlugin : BasePlugin
{

    public float ZoomSmoothness = 1f;
    public float minDistancetoReach = .01f;
    float _initialOrtographicSize;
    float _targetOrtographicSize;
    float _targetOrtographicSizeSmoothed;
    float _velocity;


    protected float TargetZoom;

    float _gameCameraSize
    {
        get
        {
            return ProCamera2D.GameCamera.orthographicSize;
        }
    }


    public void ChangeZoom(float newTargetZoom, float newZoomSmothness)
    {
        StopCoroutine("InsideTriggerRoutine");
        ZoomSmoothness = newZoomSmothness;
        TargetZoom = newTargetZoom;
        _initialOrtographicSize = _gameCameraSize;
        _targetOrtographicSize = _initialOrtographicSize;
        _targetOrtographicSizeSmoothed = _initialOrtographicSize;
        StartCoroutine("InsideTriggerRoutine");

    }

    public void ChangeZoom(float newTargetZoom)
    {
        StopCoroutine("InsideTriggerRoutine");
        TargetZoom = newTargetZoom;
        _initialOrtographicSize = _gameCameraSize;
        _targetOrtographicSize = _initialOrtographicSize;
        _targetOrtographicSizeSmoothed = _initialOrtographicSize;
        StartCoroutine("InsideTriggerRoutine");

    }

    public void SetZoom(float newTargetZoom)
    {
        StopCoroutine("InsideTriggerRoutine");
        TargetZoom = newTargetZoom;
        _initialOrtographicSize = TargetZoom;
        _targetOrtographicSize = TargetZoom;
        _targetOrtographicSizeSmoothed = TargetZoom;
        UpdateScreenSize();
        StartCoroutine("InsideTriggerRoutine");


    }

    public void FinishZoom()
    {
        SetZoom(TargetZoom);
    }

    public void StopZoom()
    {
        StopCoroutine("InsideTriggerRoutine");
    }


    IEnumerator InsideTriggerRoutine()
    {
        while (true)
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            float finalTargetSize = TargetZoom;


            var newTargetOrtographicSize = finalTargetSize;

            if ((finalTargetSize > _gameCameraSize
                && newTargetOrtographicSize > _targetOrtographicSize) ||
                (finalTargetSize < _gameCameraSize
                && newTargetOrtographicSize < _targetOrtographicSize))
            {
                _targetOrtographicSize = newTargetOrtographicSize;
            }

            if (ProCamera2D.IsCameraSizeBounded)
            {
                _targetOrtographicSize = _gameCameraSize;
                _targetOrtographicSizeSmoothed = _targetOrtographicSize;
            }

            if (Mathf.Abs(_gameCameraSize - _targetOrtographicSize) > minDistancetoReach)
                UpdateScreenSize();
            else
                break;

            yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
        }
    }

    protected void UpdateScreenSize()
    {
        _targetOrtographicSizeSmoothed = Mathf.SmoothDamp(_targetOrtographicSizeSmoothed, _targetOrtographicSize, ref _velocity, ZoomSmoothness);

        ProCamera2D.UpdateScreenSize(_targetOrtographicSizeSmoothed);

        _targetOrtographicSizeSmoothed = _gameCameraSize;
    }

}
