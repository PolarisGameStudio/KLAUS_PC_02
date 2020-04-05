namespace SmartLocalization.Editor
{
    using UnityEngine.UI;
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    [CustomEditor(typeof(LocalizedTextMeshPro))]
    public class LocalizedTextMeshProInspector : Editor
    {
        private string selectedKey = null;

        void Awake()
        {
            LocalizedTextMeshPro textObject = ((LocalizedTextMeshPro)target);
            if (textObject != null)
            {
                selectedKey = textObject.localizedKey;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
		
            selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
		
            if (!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
            {
                LocalizedTextMeshPro textObject = ((LocalizedTextMeshPro)target);
                textObject.localizedKey = selectedKey;
            }
        }
	
    }
}