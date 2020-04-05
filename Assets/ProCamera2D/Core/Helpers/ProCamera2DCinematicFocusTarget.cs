using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DCinematicFocusTarget : BasePlugin
    {
        public bool IgnoreBoundaries;

        public float EaseInDuration = 3f;
        public float EaseOutDuration = 3f;
        public EaseType TransitionEaseType;

        [HideInInspector]
        public bool IsActive;

        Coroutine _applyInfluenceRoutine;
        Coroutine _disableInfluenceRoutine;

        Vector3? _targetExclusiveFocusPoint;

        BoundariesAnimator _boundsAnimator;
        bool _reenableGeometryBoundaries;

        public void Enable()
        {
            if (IsActive)
                return;
            
            IsActive = true;
            if(_disableInfluenceRoutine != null)
            {
                StopCoroutine(_disableInfluenceRoutine);
                _disableInfluenceRoutine = null;
            }

            if(_applyInfluenceRoutine == null)
                _applyInfluenceRoutine = StartCoroutine(ApplyInfluenceRoutine());

            if(ProCamera2D.OnExclusiveFocusStarted != null)
                ProCamera2D.OnExclusiveFocusStarted();
        }

        public void Disable()
        {
            if (!IsActive)
                return;
            
            IsActive = false;
            if(_applyInfluenceRoutine != null)
            {
                StopCoroutine(_applyInfluenceRoutine);
                _applyInfluenceRoutine = null;
            }

            if(_disableInfluenceRoutine == null)
                _disableInfluenceRoutine = StartCoroutine(DisableInfluenceRoutine());
        }

        IEnumerator ApplyInfluenceRoutine()
        {
            var initialExclusiveFocusPointH = Vector3H(_targetExclusiveFocusPoint.HasValue ? _targetExclusiveFocusPoint.Value : ProCamera2D.CameraPosition);
            var initialExclusiveFocusPointV = Vector3V(_targetExclusiveFocusPoint.HasValue ? _targetExclusiveFocusPoint.Value : ProCamera2D.CameraPosition);

            // Save the current boundaries so we can enable then later again
            if(IgnoreBoundaries && ProCamera2D.UseNumericBoundaries)
            {
                ProCamera2D.UseNumericBoundaries = false;

                _boundsAnimator = new BoundariesAnimator(ProCamera2D);
                _boundsAnimator.UseTopBoundary = ProCamera2D.UseTopBoundary;
                _boundsAnimator.TopBoundary = ProCamera2D.TopBoundary;
                _boundsAnimator.UseBottomBoundary = ProCamera2D.UseBottomBoundary;
                _boundsAnimator.BottomBoundary = ProCamera2D.BottomBoundary;
                _boundsAnimator.UseLeftBoundary = ProCamera2D.UseLeftBoundary;
                _boundsAnimator.LeftBoundary = ProCamera2D.LeftBoundary;
                _boundsAnimator.UseRightBoundary = ProCamera2D.UseRightBoundary;
                _boundsAnimator.RightBoundary = ProCamera2D.RightBoundary;

                ProCamera2D.MoveCameraInstantlyToPosition(VectorHV(initialExclusiveFocusPointH, initialExclusiveFocusPointV));
            }
            else if(IgnoreBoundaries && ProCamera2D.UseGeometryBoundaries)
            {
                ProCamera2D.UseGeometryBoundaries = false;
                _reenableGeometryBoundaries = true;

                ProCamera2D.MoveCameraInstantlyToPosition(VectorHV(initialExclusiveFocusPointH, initialExclusiveFocusPointV));
            }

            // Ease in
            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / EaseInDuration;

                var targetExclusiveFocusPointH = Utils.EaseFromTo(initialExclusiveFocusPointH, Vector3H(_transform.position), t, TransitionEaseType);
                var targetExclusiveFocusPointV = Utils.EaseFromTo(initialExclusiveFocusPointV, Vector3V(_transform.position), t, TransitionEaseType);

                _targetExclusiveFocusPoint = VectorHV(targetExclusiveFocusPointH, targetExclusiveFocusPointV);

                ProCamera2D.ExclusiveTargetPosition = _targetExclusiveFocusPoint;

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            // Follow this
            while(true)
            {
                ProCamera2D.ExclusiveTargetPosition = VectorHV(Vector3H(_transform.position), Vector3V(_transform.position));

                _targetExclusiveFocusPoint = ProCamera2D.ExclusiveTargetPosition.Value;

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }
        }

        IEnumerator DisableInfluenceRoutine()
        {
            var initialExclusiveFocusPointH = Vector3H(_targetExclusiveFocusPoint.Value);
            var initialExclusiveFocusPointV = Vector3V(_targetExclusiveFocusPoint.Value);

            // Ease out
            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / EaseOutDuration;

                var targetExclusiveFocusPointH = Utils.EaseFromTo(initialExclusiveFocusPointH, Vector3H(ProCamera2D.CameraTargetPositionDuringExclusive), t, TransitionEaseType);
                var targetExclusiveFocusPointV = Utils.EaseFromTo(initialExclusiveFocusPointV, Vector3V(ProCamera2D.CameraTargetPositionDuringExclusive), t, TransitionEaseType);

                _targetExclusiveFocusPoint = VectorHV(targetExclusiveFocusPointH, targetExclusiveFocusPointV);

                ProCamera2D.ExclusiveTargetPosition = _targetExclusiveFocusPoint;

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            _targetExclusiveFocusPoint = null;

            // Restore boundaries if existed
            if(_boundsAnimator != null)
                _boundsAnimator.Transition();

            _boundsAnimator = null;

            if(_reenableGeometryBoundaries)
            {
                ProCamera2D.UseGeometryBoundaries = true;
                _reenableGeometryBoundaries = false;
            }

            if(ProCamera2D.OnExclusiveFocusFinished != null)
                ProCamera2D.OnExclusiveFocusFinished();
        }

        #if UNITY_EDITOR
        override protected void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if(_gizmosDrawingFailed)
                return;

            float cameraDepthOffset = Vector3D(ProCamera2D.transform.position) + 5f * Vector3D(ProCamera2D.transform.forward);
            Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_exclusive.png", false);
        }
        #endif
    }
}