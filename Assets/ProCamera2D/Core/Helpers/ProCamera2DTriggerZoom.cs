﻿using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DTriggerZoom : BaseTrigger
    {
        public bool SetSizeAsMultiplier = true;

        public float TargetZoom = 1.5f;

        public float ZoomSmoothness = 1f;

        [RangeAttribute(0, 1)]
        public float ExclusiveInfluencePercentage = .25f;

        public bool ResetSizeOnExit = false;
        public float ResetSizeSmoothness = 1f;

        float _startCamSize;
        float _initialCamSize;
        float _targetCamSize;
        float _targetCamSizeSmoothed;
        float _velocity;

        float _gameCameraSize
        {
            get
            {
                if (ProCamera2D.GameCamera.orthographic)
                {
                    #if PC2D_TK2D_SUPPORT
                    if (ProCamera2D.Tk2dCam != null)
                        return ProCamera2D.Tk2dCam.CameraSettings.orthographicSize / ProCamera2D.Tk2dCam.ZoomFactor;
                    #endif

                    return ProCamera2D.GameCamera.orthographicSize;
                }
                else
                    return Mathf.Abs(Vector3D(ProCamera2D.CameraPosition)) * Mathf.Tan(ProCamera2D.GameCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            }
        }

        override protected void Start()
        {
            base.Start();

            if (ProCamera2D == null)
                return;
            
            _startCamSize = _gameCameraSize;
        }

        protected override void EnteredTrigger()
        {
            base.EnteredTrigger();

            if (ProCamera2D.CurrentZoomTriggerID != _instanceID)
            {
                ProCamera2D.CurrentZoomTriggerID = _instanceID;
                
                _initialCamSize = _gameCameraSize;
                _targetCamSize = _initialCamSize;
                _targetCamSizeSmoothed = _initialCamSize;
            }

            StartCoroutine(InsideTriggerRoutine());
        }

        protected override void ExitedTrigger()
        {
            base.ExitedTrigger();

            if (ResetSizeOnExit)
                _targetCamSize = _startCamSize;

            StartCoroutine(OutsideTriggerRoutine());
        }

        IEnumerator InsideTriggerRoutine()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (_insideTrigger &&
                   _instanceID == ProCamera2D.CurrentZoomTriggerID)
            {
                _exclusiveInfluencePercentage = ExclusiveInfluencePercentage;

                var distancePercentage = GetDistanceToCenterPercentage(new Vector2(Vector3H(ProCamera2D.TargetsMidPoint), Vector3V(ProCamera2D.TargetsMidPoint)));

                float finalTargetSize;
                if (SetSizeAsMultiplier)
                    finalTargetSize = _startCamSize / TargetZoom;
                else if (ProCamera2D.GameCamera.orthographic)
                    finalTargetSize = TargetZoom;
                else
                    finalTargetSize = Mathf.Abs(Vector3D(ProCamera2D.CameraPosition)) * Mathf.Tan(TargetZoom * 0.5f * Mathf.Deg2Rad);

                var newTargetOrtographicSize = (_initialCamSize * distancePercentage) + (finalTargetSize * (1 - distancePercentage));

                if ((finalTargetSize > _gameCameraSize
                    && newTargetOrtographicSize > _targetCamSize) ||
                    (finalTargetSize < _gameCameraSize
                    && newTargetOrtographicSize < _targetCamSize))
                {
                    _targetCamSize = newTargetOrtographicSize;
                }

                if (ProCamera2D.IsCameraSizeBounded)
                {
                    _targetCamSize = _gameCameraSize;
                    _targetCamSizeSmoothed = _targetCamSize;
                }

                if (Mathf.Abs(_gameCameraSize - _targetCamSize) > .0001f)
                    UpdateScreenSize(ZoomSmoothness);

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }
        }

        IEnumerator OutsideTriggerRoutine()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (!_insideTrigger &&
                   _instanceID == ProCamera2D.CurrentZoomTriggerID &&
                   Mathf.Abs(_gameCameraSize - _targetCamSize) > .0001f)
            {
                UpdateScreenSize(ResetSizeOnExit ? ResetSizeSmoothness : ZoomSmoothness);
                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }
            _velocity = 0f;
        }

        protected void UpdateScreenSize(float smoothness)
        {
            _targetCamSizeSmoothed = Mathf.SmoothDamp(_targetCamSizeSmoothed, _targetCamSize, ref _velocity, smoothness);

            ProCamera2D.UpdateScreenSize(_targetCamSizeSmoothed);

            _targetCamSizeSmoothed = _gameCameraSize;
        }

        #if UNITY_EDITOR
        int _drawGizmosCounter;

        override protected void OnDrawGizmos()
        {
            // HACK to prevent Unity bug on startup: http://forum.unity3d.com/threads/screen-position-out-of-view-frustum.9918/
            _drawGizmosCounter++;
            if (_drawGizmosCounter < 5 && UnityEditor.EditorApplication.timeSinceStartup < 60f)
                return;

            _exclusiveInfluencePercentage = ExclusiveInfluencePercentage;

            base.OnDrawGizmos();
            
            if (_gizmosDrawingFailed)
                return;

            var gameCamera = ProCamera2D.GetComponent<Camera>();
            var cameraDimensions = Utils.GetScreenSizeInWorldCoords(gameCamera, Mathf.Abs(Vector3D(ProCamera2D.transform.localPosition)));
            float cameraDepthOffset = Vector3D(ProCamera2D.transform.localPosition) + Mathf.Abs(Vector3D(ProCamera2D.transform.localPosition)) * Vector3D(ProCamera2D.transform.forward);
            var cameraCenter = VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset);

            var startOrthoSize = Application.isPlaying ? _startCamSize : cameraDimensions.y / 2;

            float finalTargetSize;
            if (SetSizeAsMultiplier)
                finalTargetSize = startOrthoSize / TargetZoom;
            else if (ProCamera2D.GameCamera.orthographic)
                finalTargetSize = TargetZoom;
            else
                finalTargetSize = Mathf.Abs(Vector3D(ProCamera2D.transform.localPosition)) * Mathf.Tan(TargetZoom * 0.5f * Mathf.Deg2Rad);

            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, .15f);
            Gizmos.DrawWireCube(cameraCenter, VectorHV(finalTargetSize * 2 * ProCamera2D.GameCamera.aspect, finalTargetSize * 2));

            if (startOrthoSize > finalTargetSize)
                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_zoom_in.png", false);
            else
                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_zoom_out.png", false);
        }
        #endif
    }
}