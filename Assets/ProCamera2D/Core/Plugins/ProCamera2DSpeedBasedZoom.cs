using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DSpeedBasedZoom : BasePlugin
    {
        [Tooltip("The speed at which the camera will reach it's max zoom out.")]
        public float SpeedForZoomOut = 5f;
        [Tooltip("Below this speed the camera zooms in. Above this speed the camera will start zooming out.")]
        public float SpeedForZoomIn = 2f;

        [Tooltip("Represents how smooth the zoom in of the camera should be. The lower the number the quickest the zoom is. A number too low might cause some stuttering.")]
        public float ZoomInSmoothness = 5f;
        [Tooltip("Represents how smooth the zoom out of the camera should be. The lower the number the quickest the zoom is. A number too low might cause some stuttering.")]
        public float ZoomOutSmoothness = 3f;

        [Tooltip("Represents the maximum amount the camera should zoom in when the camera speed is below SpeedForZoomIn")]
        public float MaxZoomInAmount = 2f;
        [Tooltip("Represents the maximum amount the camera should zoom out when the camera speed is equal to SpeedForZoomOut")]
        public float MaxZoomOutAmount = 2f;

        float _velocity;

        float _initialCamSize;
        float _targetCamSize;
        float _targetCamSizeSmoothed;
        float _previousCamSizeSmoothed;

        Vector3 _previousCameraPosition;

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

            _previousCameraPosition = ProCamera2D.CameraPosition;

            _initialCamSize = _gameCameraSize;
            _targetCamSize = _initialCamSize;
            _targetCamSizeSmoothed = _targetCamSize;
            _previousCamSizeSmoothed = _targetCamSizeSmoothed;
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
            var deltaTime = (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime;
            if(Time.deltaTime < .0001f)
                return;

            UpdateTargetOrthoSize(deltaTime);

            UpdateScreenSize(deltaTime);

            _previousCameraPosition = ProCamera2D.CameraPosition;
        }

        void UpdateTargetOrthoSize(float deltaTime)
        {
            float currentVel = 0f;
            if (!ProCamera2D.IsCameraPositionHorizontallyBounded && !ProCamera2D.IsCameraPositionVerticallyBounded)
            {
                currentVel = (_previousCameraPosition - ProCamera2D.CameraPosition).magnitude / deltaTime;
            }
            else if (ProCamera2D.IsCameraPositionHorizontallyBounded)
            {
                currentVel = Mathf.Abs(Vector3V(_previousCameraPosition) - Vector3V(ProCamera2D.CameraPosition)) / deltaTime;
            }
            else if (ProCamera2D.IsCameraPositionVerticallyBounded)
            {
                currentVel = Mathf.Abs(Vector3H(_previousCameraPosition) - Vector3H(ProCamera2D.CameraPosition)) / deltaTime;
            }

            // Zoom out
            if (currentVel > SpeedForZoomIn)
            {
                var speedPercentage = (currentVel - SpeedForZoomIn) / (SpeedForZoomOut - SpeedForZoomIn);
                var newSize = _initialCamSize * (1 + (MaxZoomOutAmount - 1) * Mathf.Clamp01(speedPercentage));

                if (newSize > _targetCamSizeSmoothed)
                    _targetCamSize = newSize;
            }
            // Zoom in
            else
            {
                var speedPercentage = (1 - (currentVel / SpeedForZoomIn)).Remap(0f, 1f, .5f, 1f);
                var newSize = _initialCamSize / (MaxZoomInAmount * speedPercentage);

                if (newSize < _targetCamSizeSmoothed)
                    _targetCamSize = newSize;
            }

            if (ProCamera2D.IsCameraSizeBounded)
            {
                _targetCamSize = _gameCameraSize;
                _targetCamSizeSmoothed = _targetCamSize;
            }
        }

        protected void UpdateScreenSize(float deltaTime)
        {
            _previousCamSizeSmoothed = _targetCamSizeSmoothed;
            _targetCamSizeSmoothed = Mathf.SmoothDamp(_targetCamSizeSmoothed, _targetCamSize, ref _velocity, _targetCamSize < _targetCamSizeSmoothed ? ZoomInSmoothness : ZoomOutSmoothness);

            if (Math.Abs(_targetCamSizeSmoothed - _previousCamSizeSmoothed) > .0001f)
            {
                ProCamera2D.UpdateScreenSize(_targetCamSizeSmoothed);
                _targetCamSizeSmoothed = _gameCameraSize;
            }
        }
    }
}