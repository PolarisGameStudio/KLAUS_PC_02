using UnityEditor;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
	[CustomEditor(typeof(ProCamera2DTriggerInfluence))]
	[CanEditMultipleObjects]
    public class ProCamera2DTriggerInfluenceEditor : Editor
    {
        void OnEnable()
        {
            var proCamera2DTriggerInfluence = (ProCamera2DTriggerInfluence)target;

            if(proCamera2DTriggerInfluence.ProCamera2D == null && Camera.main != null)
                proCamera2DTriggerInfluence.ProCamera2D = Camera.main.GetComponent<ProCamera2D>();
        }

        public override void OnInspectorGUI()
        {
            var proCamera2DTriggerInfluence = (ProCamera2DTriggerInfluence)target;

            if(proCamera2DTriggerInfluence.ProCamera2D == null)
                EditorGUILayout.HelpBox("ProCamera2D is not set.", MessageType.Error, true);

            DrawDefaultInspector();

            if (proCamera2DTriggerInfluence.UpdateInterval <= 0.01f)
                proCamera2DTriggerInfluence.UpdateInterval = 0.01f;
            
            if (proCamera2DTriggerInfluence.InfluenceSmoothness < 0f)
                proCamera2DTriggerInfluence.InfluenceSmoothness = 0f;
        }
    }
}