using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(ChangueSDtoHD_Animator))]
[CanEditMultipleObjects]
public class ChangueSDtoHD_AnimatorEditor : Editor
{  /*
    public static string getResourcesPath(string source) {
        string[] stringSeparators = new string[] { "/Resources/", "." };
        string[] result;

        result = source.Split(stringSeparators, StringSplitOptions.None);
        if (result.Length >= 2) {
            return result[1];
        }
        return "";
    }

    public override void OnInspectorGUI()
    {



        base.OnInspectorGUI();

        ChangueSDtoHD_Animator changuer = ((ChangueSDtoHD_Animator)target);
        bool isChangued = false;
        string sd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.SD));
        if (!string.IsNullOrEmpty(sd))
        {
      //      Debug.Log(sd);
            serializedObject.FindProperty("pathSD").stringValue = sd;
            isChangued = true;
        }
        string hd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.HD));
        if (!string.IsNullOrEmpty(hd))
        {
            serializedObject.FindProperty("pathHD").stringValue = hd;
            isChangued = true;
        }
        if (isChangued)
            serializedObject.ApplyModifiedProperties();
    }

    */
}
