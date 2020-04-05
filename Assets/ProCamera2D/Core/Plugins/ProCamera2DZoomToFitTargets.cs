using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DZoomToFitTargets : BasePlugin
    {
        public float ZoomOutBorder = .6f;
        public float ZoomInBorder = .4f;
        public float ZoomInSmoothness = 2f;
        public float ZoomOutSmoothness = 1f;

        public float MaxZoomInAmount = 2f;
        public float MaxZoomOutAmount = 4f;

        public bool DisableWhenOneTarget = true;

        float _velocity;

        float _initialCamSize;
        float _previousCamSize;
        float _targetCamSize;
        float _targetCamSizeSmoothed;

        float _minCameraSize;
        float _maxCameraSize;

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

        protected void OnEnable()
        {
            if (ProCamera2D == null)
                return;

            _initialCamSize = _gameCameraSize;
            _targetCamSize = _initialCamSize;
            _targetCamSizeSmoothed = _targetCamSize;
        }

        void LateUpdate()
        {
            if (ProCamera2D.UpdateType == UpdateType.LateUpdate)
                Step();
        }

        void FixedUpdate()
        {
            if (ProCamera2D.UpdateType == UpdateType.FixedUpdate)
                Step();
        }

        void Step()
        {
            if (DisableWhenOneTarget && ProCamera2D.CameraTargets.Count <= 1)
                _targetCamSize = _initialCamSize;
            else
                UpdateTargetOrthoSize();

            UpdateScreenSize();
        }

        void UpdateTargetOrthoSize()
        {
            float distanceMaxX = Mathf.NegativeInfinity;
            float distanceMaxY = Mathf.NegativeInfinity;
            for (int i = 0; i < ProCamera2D.CameraTargets.Count; i++)
            {
                var distance = 
                    VectorHV(Vector3H(ProCamera2D.CameraPosition), Vector3V(ProCamera2D.CameraPosition)) -
                    (VectorHV(Vector3H(ProCamera2D.CameraTargets[i].TargetPosition) + Vector3H(ProCamera2D.CameraTargets[i].TargetOffset), Vector3V(ProCamera2D.CameraTargets[i].TargetPosition) + Vector3V(ProCamera2D.CameraTargets[i].TargetOffset)));
                
                distanceMaxX = Mathf.Max(distanceMaxX, Mathf.Abs(Vector3H(distance)));
                distanceMaxY = Mathf.Max(distanceMaxY, Mathf.Abs(Vector3V(distance)));
            }

            // Zoom out
            if (distanceMaxX > ProCamera2D.ScreenSizeInWorldCoordinates.x * ZoomOutBorder * .5f ||
                distanceMaxY > ProCamera2D.ScreenSizeInWorldCoordinates.y * ZoomOutBorder * .5f)
            {
                if (ProCamera2D.IsCameraPositionHorizontallyBounded || ProCamera2D.IsCameraPositionVerticallyBounded)
                    return;

                if (distanceMaxX / ProCamera2D.ScreenSizeInWorldCoordinates.x >= distanceMaxY / ProCamera2D.ScreenSizeInWorldCoordinates.y)
                    _targetCamSize = (distanceMaxX / ProCamera2D.GameCamera.aspect) / ZoomOutBorder;
                else
                    _targetCamSize = distanceMaxY / ZoomOutBorder;
            }
            // Zoom in
            else if (distanceMaxX < ProCamera2D.ScreenSizeInWorldCoordinates.x * ZoomInBorder * .5f &&
                     distanceMaxY < ProCamera2D.ScreenSizeInWorldCoordinates.y * ZoomInBorder * .5f)
            {
                if (distanceMaxX / ProCamera2D.ScreenSizeInWorldCoordinates.x >= distanceMaxY / ProCamera2D.ScreenSizeInWorldCoordinates.y)
                    _targetCamSize = (distanceMaxX / ProCamera2D.GameCamera.aspect) / ZoomInBorder;
                else
                    _targetCamSize = distanceMaxY / ZoomInBorder;

            }

            if (ProCamera2D.IsCameraSizeBounded)
            {
                _targetCamSize = _gameCameraSize;
                _targetCamSizeSmoothed = _targetCamSize;
            }
            else
            {
                _minCameraSize = _initialCamSize / MaxZoomInAmount;
                _maxCameraSize = _initialCamSize * MaxZoomOutAmount;
                _targetCamSize = Mathf.Clamp(_targetCamSize, _minCameraSize, _maxCameraSize);
            }
        }

        protected void UpdateScreenSize()
        {
            _previousCamSize = _targetCamSizeSmoothed;
            _targetCamSizeSmoothed = Mathf.SmoothDamp(_targetCamSizeSmoothed, _targetCamSize, ref _velocity, _targetCamSize < _targetCamSizeSmoothed ? ZoomInSmoothness : ZoomOutSmoothness);

            if (Math.Abs(_targetCamSizeSmoothed - _previousCamSize) > .0001f)
            {
                ProCamera2D.UpdateScreenSize(_targetCamSizeSmoothed);
                _targetCamSizeSmoothed = _gameCameraSize;
            }
        }

        #if UNITY_EDITOR
        int _drawGizmosCounter;

        override protected void OnDrawGizmos()
        {
            // HACK to prevent Unity bug on startup: http://forum.unity3d.com/threads/screen-position-out-of-view-frustum.9918/
            _drawGizmosCounter++;
            if (_drawGizmosCounter < 5 && UnityEditor.EditorApplication.timeSinceStartup < 60f)
                return;

            base.OnDrawGizmos();

            if (_gizmosDrawingFailed)
                return;

            var gameCamera = ProCamera2D.GetComponent<Camera>();
            var cameraDimensions = gameCamera.orthographic ? Utils.GetScreenSizeInWorldCoords(gameCamera) : Utils.GetScreenSizeInWorldCoords(gameCamera, Mathf.Abs(Vector3D(transform.localPosition)));
            float cameraDepthOffset = Vector3D(ProCamera2D.transform.localPosition) + Mathf.Abs(Vector3D(transform.localPosition)) * Vector3D(ProCamera2D.transform.forward);
            var cameraCenter = VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset);


            var bordersColor = DisableWhenOneTarget && ProCamera2D.CameraTargets.Count <= 1 ? PrefsData.ZoomToFitColorValue * .75f : PrefsData.ZoomToFitColorValue;

            // Zoom out border
            Gizmos.color = Math.Abs(_targetCamSizeSmoothed - _maxCameraSize) < .01f ? Color.red : EditorPrefsX.GetColor(PrefsData.ZoomToFitColorKey, bordersColor);
            Gizmos.DrawWireCube(cameraCenter, VectorHV(cameraDimensions.x, cameraDimensions.y) * ZoomOutBorder);
            Utils.DrawArrowForGizmo(cameraCenter + VectorHV(cameraDimensions.x / 2 * ZoomOutBorder, cameraDimensions.y / 2 * ZoomOutBorder), VectorHV(.05f * cameraDimensions.y, .05f * cameraDimensions.y), .04f * cameraDimensions.y);
            Utils.DrawArrowForGizmo(cameraCenter - VectorHV(cameraDimensions.x / 2 * ZoomOutBorder, cameraDimensions.y / 2 * ZoomOutBorder), VectorHV(-.05f * cameraDimensions.y, -.05f * cameraDimensions.y), .04f * cameraDimensions.y);

            // Zoom in border
            Gizmos.color = Math.Abs(_targetCamSizeSmoothed - _minCameraSize) < .01f ? Color.red : EditorPrefsX.GetColor(PrefsData.ZoomToFitColorKey, bordersColor);
            Gizmos.DrawWireCube(cameraCenter, VectorHV(cameraDimensions.x, cameraDimensions.y) * ZoomInBorder);
            Utils.DrawArrowForGizmo(cameraCenter + VectorHV(cameraDimensions.x / 2 * ZoomInBorder, cameraDimensions.y / 2 * ZoomInBorder), VectorHV(-.05f * cameraDimensions.y, -.05f * cameraDimensions.y), .04f * cameraDimensions.y);
            Utils.DrawArrowForGizmo(cameraCenter - VectorHV(cameraDimensions.x / 2 * ZoomInBorder, cameraDimensions.y / 2 * ZoomInBorder), VectorHV(.05f * cameraDimensions.y, .05f * cameraDimensions.y), .04f * cameraDimensions.y);
        }
        #endif
    }
}