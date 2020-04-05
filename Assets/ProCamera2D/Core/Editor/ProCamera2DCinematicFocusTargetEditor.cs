using UnityEditor;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
	[CustomEditor(typeof(ProCamera2DCinematicFocusTarget))]
	[CanEditMultipleObjects]
    public class ProCamera2DCinematicFocusTargetEditor : Editor
    {
        void OnEnable()
        {
            var proCamera2DCinematicFocusTarget = (ProCamera2DCinematicFocusTarget)target;

            if(proCamera2DCinematicFocusTarget.ProCamera2D == null && Camera.main != null)
                proCamera2DCinematicFocusTarget.ProCamera2D = Camera.main.GetComponent<ProCamera2D>();
        }

        public override void OnInspectorGUI()
        {
            var proCamera2DCinematicFocusTarget = (ProCamera2DCinematicFocusTarget)target;

            if(proCamera2DCinematicFocusTarget.ProCamera2D == null)
                EditorGUILayout.HelpBox("ProCamera2D is not set.", MessageType.Error, true);

            DrawDefaultInspector();

            if (proCamera2DCinematicFocusTarget.EaseInDuration <= 0f)
                proCamera2DCinematicFocusTarget.EaseInDuration = 0f;
            
            if (proCamera2DCinematicFocusTarget.EaseOutDuration < 0f)
                proCamera2DCinematicFocusTarget.EaseOutDuration = 0f;
        }
    }
}