namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(LocalizedTextMesh))]

    public class LocalizedTextMeshInspector : Editor
    {

        private string selectedKey = null;

        void Awake()
        {
            LocalizedTextMesh textObject = ((LocalizedTextMesh)target);
            if (textObject != null)
            {
                selectedKey = textObject.localizedKey;
            }
        }

        /// <summary>
        /// Override of the OnInspectorGUI method
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
//            LocalizedTextMesh changuer = ((LocalizedTextMesh)target);

            selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
            bool isChangued = false;

            if (!Application.isPlaying && GUILayout.Button("Use Key", GUILayout.Width(70)))
            {
                // LocalizedTextMesh textObject = ((LocalizedTextMesh)target);       
                // textObject.localizedKey = selectedKey;
                serializedObject.FindProperty("localizedKey").stringValue = selectedKey;
                isChangued = true;

            }

            if (isChangued)
                serializedObject.ApplyModifiedProperties();
        }
    }
}
