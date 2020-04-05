using UnityEngine;
using System;
using System.Collections;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class BoundariesAnimator
    {
        public Action OnTransitionStarted;
        public Action OnTransitionFinished;

        public ProCamera2D ProCamera2D;

        public bool UseTopBoundary;
        public float TopBoundary;
        public bool UseBottomBoundary;
        public float BottomBoundary;
        public bool UseLeftBoundary;
        public float LeftBoundary;
        public bool UseRightBoundary;
        public float RightBoundary;

        public float TransitionDuration = 1f;
        public EaseType TransitionEaseType;

        Func<Vector3, float> Vector3H;
        Func<Vector3, float> Vector3V;

        bool _hasFiredTransitionFinished;

        public BoundariesAnimator(ProCamera2D proCamera2D)
        {
            ProCamera2D = proCamera2D;

            switch (ProCamera2D.Axis)
            {
                case MovementAxis.XY:
                    Vector3H = vector => vector.x;
                    Vector3V = vector => vector.y;
                    break;
                case MovementAxis.XZ:
                    Vector3H = vector => vector.x;
                    Vector3V = vector => vector.z;
                    break;
                case MovementAxis.YZ:
                    Vector3H = vector => vector.z;
                    Vector3V = vector => vector.y;
                    break;
            }
        }

        public void Transition()
        {
            if (OnTransitionStarted != null)
                OnTransitionStarted();

            _hasFiredTransitionFinished = false;

            ProCamera2D.UseNumericBoundaries = true;

            if (UseLeftBoundary)
            {
                ProCamera2D.UseLeftBoundary = true;

                if (ProCamera2D.LeftBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.LeftBoundaryAnimRoutine);

                ProCamera2D.LeftBoundaryAnimRoutine = ProCamera2D.StartCoroutine(LeftTransitionRoutine(TransitionDuration));
            }
            else if (!UseLeftBoundary && ProCamera2D.UseLeftBoundary && UseRightBoundary && RightBoundary < ProCamera2D.TargetLeftBoundary) 
            {
                ProCamera2D.UseLeftBoundary = true;
                UseLeftBoundary = true;

                LeftBoundary = RightBoundary - ProCamera2D.ScreenSizeInWorldCoordinates.x * 100f;

                if (ProCamera2D.LeftBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.LeftBoundaryAnimRoutine);

                ProCamera2D.LeftBoundaryAnimRoutine = ProCamera2D.StartCoroutine(LeftTransitionRoutine(TransitionDuration, true));
            }

            if (UseRightBoundary)
            {
                ProCamera2D.UseRightBoundary = true;

                if (ProCamera2D.RightBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.RightBoundaryAnimRoutine);

                ProCamera2D.RightBoundaryAnimRoutine = ProCamera2D.StartCoroutine(RightTransitionRoutine(TransitionDuration));
            }
            else if (!UseRightBoundary && ProCamera2D.UseRightBoundary && UseLeftBoundary && LeftBoundary > ProCamera2D.TargetRightBoundary) 
            {
                ProCamera2D.UseRightBoundary = true;
                UseRightBoundary = true;

                RightBoundary = LeftBoundary + ProCamera2D.ScreenSizeInWorldCoordinates.x * 100f;

                if (ProCamera2D.RightBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.RightBoundaryAnimRoutine);

                ProCamera2D.RightBoundaryAnimRoutine = ProCamera2D.StartCoroutine(RightTransitionRoutine(TransitionDuration, true));
            }

            if (UseTopBoundary)
            {
                ProCamera2D.UseTopBoundary = true;

                if (ProCamera2D.TopBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.TopBoundaryAnimRoutine);

                ProCamera2D.TopBoundaryAnimRoutine = ProCamera2D.StartCoroutine(TopTransitionRoutine(TransitionDuration));
            }
            else if (!UseTopBoundary && ProCamera2D.UseTopBoundary && UseBottomBoundary && BottomBoundary > ProCamera2D.TargetTopBoundary) 
            {
                ProCamera2D.UseTopBoundary = true;
                UseTopBoundary = true;

                TopBoundary = BottomBoundary + ProCamera2D.ScreenSizeInWorldCoordinates.y * 100f;

                if (ProCamera2D.TopBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.TopBoundaryAnimRoutine);

                ProCamera2D.TopBoundaryAnimRoutine = ProCamera2D.StartCoroutine(TopTransitionRoutine(TransitionDuration, true));
            }

            if (UseBottomBoundary)
            {
                ProCamera2D.UseBottomBoundary = true;

                if (ProCamera2D.BottomBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.BottomBoundaryAnimRoutine);

                ProCamera2D.BottomBoundaryAnimRoutine = ProCamera2D.StartCoroutine(BottomTransitionRoutine(TransitionDuration));
            }
            else if (!UseBottomBoundary && ProCamera2D.UseBottomBoundary && UseTopBoundary && TopBoundary < ProCamera2D.TargetBottomBoundary) 
            {
                ProCamera2D.UseBottomBoundary = true;
                UseBottomBoundary = true;

                BottomBoundary = TopBoundary - ProCamera2D.ScreenSizeInWorldCoordinates.y * 100f;

                if (ProCamera2D.BottomBoundaryAnimRoutine != null)
                    ProCamera2D.StopCoroutine(ProCamera2D.BottomBoundaryAnimRoutine);

                ProCamera2D.BottomBoundaryAnimRoutine = ProCamera2D.StartCoroutine(BottomTransitionRoutine(TransitionDuration, true));
            }
        }

        IEnumerator LeftTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
        {
            var initialLeftBoundary = Vector3H(ProCamera2D.CameraPosition) - ProCamera2D.ScreenSizeInWorldCoordinates.x / 2;

            ProCamera2D.TargetLeftBoundary = LeftBoundary;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / duration;

                if (UseLeftBoundary && UseRightBoundary && LeftBoundary < initialLeftBoundary)
                    ProCamera2D.LeftBoundary = Utils.EaseFromTo(initialLeftBoundary, Mathf.Clamp(Vector3H(ProCamera2D.CameraTargetPosition) - ProCamera2D.ScreenSizeInWorldCoordinates.x / 2, LeftBoundary, RightBoundary - ProCamera2D.ScreenSizeInWorldCoordinates.x), t, TransitionEaseType);
                else if (UseLeftBoundary)
                    ProCamera2D.LeftBoundary = Utils.EaseFromTo(initialLeftBoundary, LeftBoundary, t, TransitionEaseType);

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            if (turnOffBoundaryAfterwards)
            {
                ProCamera2D.UseLeftBoundary = false;
                UseLeftBoundary = false;
            }

            ProCamera2D.LeftBoundary = LeftBoundary;

            if (!_hasFiredTransitionFinished && OnTransitionFinished != null)
                OnTransitionFinished();
        }

        IEnumerator RightTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
        {
            var initialRightBoundary = Vector3H(ProCamera2D.CameraPosition) + ProCamera2D.ScreenSizeInWorldCoordinates.x / 2;

            ProCamera2D.TargetRightBoundary = RightBoundary;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / duration;

                if (UseRightBoundary && UseLeftBoundary && RightBoundary > initialRightBoundary)
                    ProCamera2D.RightBoundary = Utils.EaseFromTo(initialRightBoundary, Mathf.Clamp(Vector3H(ProCamera2D.CameraTargetPosition) + ProCamera2D.ScreenSizeInWorldCoordinates.x / 2, LeftBoundary + ProCamera2D.ScreenSizeInWorldCoordinates.x, RightBoundary), t, TransitionEaseType);
                else if (UseRightBoundary)
                    ProCamera2D.RightBoundary = Utils.EaseFromTo(initialRightBoundary, RightBoundary, t, TransitionEaseType);

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            if (turnOffBoundaryAfterwards)
            {
                ProCamera2D.UseRightBoundary = false;
                UseRightBoundary = false;
            }

            ProCamera2D.RightBoundary = RightBoundary;

            if (!_hasFiredTransitionFinished && OnTransitionFinished != null)
                OnTransitionFinished();
        }

        IEnumerator TopTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
        {
            var initialTopBoundary = Vector3V(ProCamera2D.CameraPosition) + ProCamera2D.ScreenSizeInWorldCoordinates.y / 2;

            ProCamera2D.TargetTopBoundary = TopBoundary;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / duration;

                if (UseTopBoundary && UseBottomBoundary && TopBoundary > initialTopBoundary)
                    ProCamera2D.TopBoundary = Utils.EaseFromTo(initialTopBoundary, Mathf.Clamp(Vector3V(ProCamera2D.CameraTargetPosition) + ProCamera2D.ScreenSizeInWorldCoordinates.y / 2, BottomBoundary + ProCamera2D.ScreenSizeInWorldCoordinates.y, TopBoundary), t, TransitionEaseType);
                else if (UseTopBoundary)
                    ProCamera2D.TopBoundary = Utils.EaseFromTo(initialTopBoundary, TopBoundary, t, TransitionEaseType);

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            if (turnOffBoundaryAfterwards)
            {
                ProCamera2D.UseTopBoundary = false;
                UseTopBoundary = false;
            }

            ProCamera2D.TopBoundary = TopBoundary;

            if (!_hasFiredTransitionFinished && OnTransitionFinished != null)
                OnTransitionFinished();
        }

        IEnumerator BottomTransitionRoutine(float duration, bool turnOffBoundaryAfterwards = false)
        {
            var initialBottomBoundary = Vector3V(ProCamera2D.CameraPosition) - ProCamera2D.ScreenSizeInWorldCoordinates.y / 2;

            ProCamera2D.TargetBottomBoundary = BottomBoundary;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var t = 0f;
            while (t <= 1.0f)
            {
                t += (ProCamera2D.UpdateType == UpdateType.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime) / duration;

                if (UseBottomBoundary && UseTopBoundary && BottomBoundary < initialBottomBoundary)
                    ProCamera2D.BottomBoundary = Utils.EaseFromTo(initialBottomBoundary, Mathf.Clamp(Vector3V(ProCamera2D.CameraTargetPosition) - ProCamera2D.ScreenSizeInWorldCoordinates.y / 2, BottomBoundary, TopBoundary - ProCamera2D.ScreenSizeInWorldCoordinates.y), t, TransitionEaseType);
                else if (UseBottomBoundary)
                    ProCamera2D.BottomBoundary = Utils.EaseFromTo(initialBottomBoundary, BottomBoundary, t, TransitionEaseType);

                yield return (ProCamera2D.UpdateType == UpdateType.FixedUpdate) ? waitForFixedUpdate : null;
            }

            if (turnOffBoundaryAfterwards)
            {
                ProCamera2D.UseBottomBoundary = false;
                UseBottomBoundary = false;
            }

            ProCamera2D.BottomBoundary = BottomBoundary;

            if (!_hasFiredTransitionFinished && OnTransitionFinished != null)
                OnTransitionFinished();
        }
    }
}