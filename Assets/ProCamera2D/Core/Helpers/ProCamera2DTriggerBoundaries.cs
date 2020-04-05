using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DTriggerBoundaries : BaseTrigger
    {
        public bool AreBoundariesRelative = true;
        
        public bool UseTopBoundary = true;
        public float TopBoundary = 10;
        public bool UseBottomBoundary = true;
        public float BottomBoundary = -10;
        public bool UseLeftBoundary = true;
        public float LeftBoundary = -10;
        public bool UseRightBoundary = true;
        public float RightBoundary = 10;

        public float TransitionDuration = 1f;
        public EaseType TransitionEaseType;

        public bool SetAsStartingBoundaries;

        BoundariesAnimator _boundsAnim;

        float _targetTopBoundary;
        float _targetBottomBoundary;
        float _targetLeftBoundary;
        float _targetRightBoundary;

        protected override void Start()
        {
            base.Start();

            if(ProCamera2D == null)
                return;

            _boundsAnim = new BoundariesAnimator(ProCamera2D);
            _boundsAnim.OnTransitionStarted += () =>
            {
                if (ProCamera2D.OnBoundariesTransitionStarted != null)
                    ProCamera2D.OnBoundariesTransitionStarted();
            };

            _boundsAnim.OnTransitionFinished += () =>
            {
                if (ProCamera2D.OnBoundariesTransitionFinished != null)
                    ProCamera2D.OnBoundariesTransitionFinished();
            };

            GetTargetBoundaries();

            if (SetAsStartingBoundaries)
            {
                ProCamera2D.CurrentBoundariesTriggerID = _instanceID;

                ProCamera2D.UseLeftBoundary = UseLeftBoundary;
                if (UseLeftBoundary)
                    ProCamera2D.LeftBoundary = ProCamera2D.TargetLeftBoundary = _targetLeftBoundary;

                ProCamera2D.UseRightBoundary = UseRightBoundary;
                if (UseRightBoundary)
                    ProCamera2D.RightBoundary = ProCamera2D.TargetRightBoundary = _targetRightBoundary;

                ProCamera2D.UseTopBoundary = UseTopBoundary;
                if (UseTopBoundary)
                    ProCamera2D.TopBoundary = ProCamera2D.TargetTopBoundary = _targetTopBoundary;

                ProCamera2D.UseBottomBoundary = UseBottomBoundary;
                if (UseBottomBoundary)
                    ProCamera2D.BottomBoundary = ProCamera2D.TargetBottomBoundary = _targetBottomBoundary;
                    
                if (!UseTopBoundary && !UseBottomBoundary && !UseLeftBoundary && !UseRightBoundary)
                    ProCamera2D.UseNumericBoundaries = false;
                else
                    ProCamera2D.UseNumericBoundaries = true;
            }
        }

        protected override void EnteredTrigger()
        {
            base.EnteredTrigger();

            if (ProCamera2D.CurrentBoundariesTriggerID != _instanceID)
            {
                ProCamera2D.CurrentBoundariesTriggerID = _instanceID;
                Transition();
            }
        }

        void GetTargetBoundaries()
        {
            if (AreBoundariesRelative)
            {
                _targetTopBoundary = Vector3V(transform.position) + TopBoundary;
                _targetBottomBoundary = Vector3V(transform.position) + BottomBoundary;
                _targetLeftBoundary = Vector3H(transform.position) + LeftBoundary;
                _targetRightBoundary = Vector3H(transform.position) + RightBoundary;
            }
            else
            {
                _targetTopBoundary = TopBoundary;
                _targetBottomBoundary = BottomBoundary;
                _targetLeftBoundary = LeftBoundary;
                _targetRightBoundary = RightBoundary;
            }
        }

        void Transition()
        {
            if (!UseTopBoundary && !UseBottomBoundary && !UseLeftBoundary && !UseRightBoundary)
            {
                ProCamera2D.UseNumericBoundaries = false;
                return;
            }
                
            ProCamera2D.UseNumericBoundaries = true;
            
            GetTargetBoundaries();

            _boundsAnim.UseTopBoundary = UseTopBoundary;
            _boundsAnim.TopBoundary = _targetTopBoundary;
            _boundsAnim.UseBottomBoundary = UseBottomBoundary;
            _boundsAnim.BottomBoundary = _targetBottomBoundary;
            _boundsAnim.UseLeftBoundary = UseLeftBoundary;
            _boundsAnim.LeftBoundary = _targetLeftBoundary;
            _boundsAnim.UseRightBoundary = UseRightBoundary;
            _boundsAnim.RightBoundary = _targetRightBoundary;

            _boundsAnim.TransitionDuration = TransitionDuration;
            _boundsAnim.TransitionEaseType = TransitionEaseType;

            _boundsAnim.Transition();
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
            
            if(_gizmosDrawingFailed)
                return;

            float cameraDepthOffset = Vector3D(ProCamera2D.transform.position) + 5f * Vector3D(ProCamera2D.transform.forward);
            var cameraCenter = VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset);
            var cameraDimensions = Utils.GetScreenSizeInWorldCoords(ProCamera2D.GetComponent<Camera>());

            GetTargetBoundaries();

            Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset - .05f * Mathf.Sign(Vector3D(ProCamera2D.transform.position))), "ProCamera2D/gizmo_icon_bg.png", false);

            Gizmos.color = EditorPrefsX.GetColor(PrefsData.BoundariesTriggerColorKey, PrefsData.BoundariesTriggerColorValue);
            if (UseTopBoundary)
            {
                Gizmos.DrawRay(VectorHVD(Vector3H(transform.position) - cameraDimensions.x / 2, _targetTopBoundary, cameraDepthOffset), ProCamera2D.transform.right * cameraDimensions.x);
                Utils.DrawArrowForGizmo(cameraCenter, VectorHV(0, _targetTopBoundary - Vector3V(transform.position)));

                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_bound_top.png", false);
            }

            if (UseBottomBoundary)
            {
                Gizmos.DrawRay(VectorHVD(Vector3H(transform.position) - cameraDimensions.x / 2, _targetBottomBoundary, cameraDepthOffset), ProCamera2D.transform.right * cameraDimensions.x);
                Utils.DrawArrowForGizmo(cameraCenter, VectorHV(0, _targetBottomBoundary - Vector3V(transform.position)));

                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_bound_bottom.png", false);
            }

            if (UseRightBoundary)
            {
                Gizmos.DrawRay(VectorHVD(_targetRightBoundary, Vector3V(transform.position) - cameraDimensions.y / 2, cameraDepthOffset), ProCamera2D.transform.up * cameraDimensions.y);
                Utils.DrawArrowForGizmo(cameraCenter, VectorHV(_targetRightBoundary - Vector3H(transform.position), 0));

                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_bound_right.png", false);
            }

            if (UseLeftBoundary)
            {
                Gizmos.DrawRay(VectorHVD(_targetLeftBoundary, Vector3V(transform.position) - cameraDimensions.y / 2, cameraDepthOffset), ProCamera2D.transform.up * cameraDimensions.y);
                Utils.DrawArrowForGizmo(cameraCenter, VectorHV(_targetLeftBoundary - Vector3H(transform.position), 0));

                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_bound_left.png", false);
            }

            if(SetAsStartingBoundaries)
                Gizmos.DrawIcon(VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset), "ProCamera2D/gizmo_icon_start.png", false);
        }
        #endif
    }
}