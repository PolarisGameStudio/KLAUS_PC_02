using System;
using System.Collections;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class BaseTrigger : BasePlugin
    {
        [TooltipAttribute("Every X seconds detect collision. Smaller frequencies are more precise but also require more processing.")]
        public float UpdateInterval = .1f;

        public TriggerShape TriggerShape;

        protected float _exclusiveInfluencePercentage;

        Coroutine _testTriggerRoutine;

        protected bool _insideTrigger;
        protected Vector2 _vectorFromPointToCenter;

        protected int _instanceID;

        override protected void Start()
        {
            base.Start();

            if (ProCamera2D == null)
                return;

            _instanceID = GetInstanceID();

            // Small random time offset to avoid having all the triggers calculatations on the same frame
            UpdateInterval += UpdateInterval * UnityEngine.Random.Range(.001f, .002f);

            // Start update routine
            Toggle(true);
        }

        public void Toggle(bool value)
        {
            if (value)
            {
                if (_testTriggerRoutine == null)
                    _testTriggerRoutine = StartCoroutine(TestTriggerRoutine());
            }
            else
            {
                if (_testTriggerRoutine != null)
                {
                    StopCoroutine(_testTriggerRoutine);
                    _testTriggerRoutine = null;
                }
            }
        }

        protected virtual void EnteredTrigger()
        {
            _insideTrigger = true;
        }

        protected virtual void ExitedTrigger()
        {
            _insideTrigger = false;
        }

        IEnumerator TestTriggerRoutine()
        {
            yield return new WaitForEndOfFrame();

            var waitForSeconds = new WaitForSeconds(UpdateInterval);
            while (true)
            {
                if (TriggerShape == TriggerShape.RECTANGLE &&
                    IsInsideRectangle(Vector3H(_transform.position), Vector3V(_transform.position), Vector3H(_transform.localScale), Vector3V(_transform.localScale), Vector3H(ProCamera2D.TargetsMidPoint), Vector3V(ProCamera2D.TargetsMidPoint)))
                {
                    if (!_insideTrigger)
                        EnteredTrigger();
                }
                else if (TriggerShape == TriggerShape.CIRCLE &&
                         IsInsideCircle(Vector3H(_transform.position), Vector3V(_transform.position), (Vector3H(_transform.localScale) + Vector3V(_transform.localScale)) * .25f, Vector3H(ProCamera2D.TargetsMidPoint), Vector3V(ProCamera2D.TargetsMidPoint)))
                {
                    if (!_insideTrigger)
                        EnteredTrigger();
                }
                else
                {
                    if (_insideTrigger)
                        ExitedTrigger();
                }
                yield return waitForSeconds;
            }
        }

        protected float GetDistanceToCenterPercentage(Vector2 point)
        {
            _vectorFromPointToCenter = point - new Vector2(Vector3H(_transform.position), Vector3V(_transform.position));
            if (TriggerShape == TriggerShape.RECTANGLE)
            {
            	var distancePercentageH = Vector3H(_vectorFromPointToCenter) / (Vector3H(_transform.localScale) * .5f);
            	var distancePercentageV = Vector3V(_vectorFromPointToCenter) / (Vector3V(_transform.localScale) * .5f);
            	var distancePercentage = (Mathf.Max(Mathf.Abs(distancePercentageH), Mathf.Abs(distancePercentageV))).Remap(_exclusiveInfluencePercentage, 1, 0, 1);
            	return distancePercentage;
            }
            else
            {
            	var distancePercentage = (_vectorFromPointToCenter.magnitude / ((Vector3H(_transform.localScale) + Vector3V(_transform.localScale)) * .25f)).Remap(_exclusiveInfluencePercentage, 1, 0, 1);
            	return distancePercentage;
            }
        }

        bool IsInsideRectangle(float x, float y, float width, float height, float pointX, float pointY)
        {
            if (pointX >= x - width * .5f &&
                pointX <= x + width * .5f &&
                pointY >= y - height * .5f &&
                pointY <= y + height * .5f)
                return true;

            return false;
        }

        bool IsInsideCircle(float x, float y, float radius, float pointX, float pointY)
        {
            return (pointX - x) * (pointX - x) + (pointY - y) * (pointY - y) < radius * radius;
        }

        #if UNITY_EDITOR
        override protected void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(_gizmosDrawingFailed)
                return;

            float cameraDepthOffset = Vector3D(ProCamera2D.transform.localPosition) + Mathf.Abs(Vector3D(ProCamera2D.transform.localPosition)) * Vector3D(ProCamera2D.transform.forward);
            var cameraCenter = VectorHVD(Vector3H(transform.position), Vector3V(transform.position), cameraDepthOffset);

            Gizmos.color = EditorPrefsX.GetColor(PrefsData.TriggerShapeColorKey, PrefsData.TriggerShapeColorValue);

            if (TriggerShape == TriggerShape.RECTANGLE)
            {
                Gizmos.DrawWireCube(cameraCenter, VectorHVD(Vector3H(transform.localScale), Vector3V(transform.localScale), 0f));

                if(_exclusiveInfluencePercentage > 0)
                    Gizmos.DrawWireCube(cameraCenter, VectorHVD(Vector3H(transform.localScale) * _exclusiveInfluencePercentage, Vector3V(transform.localScale) * _exclusiveInfluencePercentage, 0f));
            }
            else
            {
                Gizmos.DrawWireSphere(cameraCenter, ((Vector3H(transform.localScale) + Vector3V(transform.localScale)) * .25f));

                if(_exclusiveInfluencePercentage > 0)
                    Gizmos.DrawWireSphere(cameraCenter, ((Vector3H(transform.localScale) + Vector3V(transform.localScale)) * .25f) * _exclusiveInfluencePercentage);
            }
        }
        #endif
    }

    public enum TriggerShape
    {
        CIRCLE,
        RECTANGLE
    }
}