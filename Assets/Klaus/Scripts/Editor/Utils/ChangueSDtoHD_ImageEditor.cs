using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System;

[CustomEditor(typeof(ChangueSDtoHD_Image))]
[CanEditMultipleObjects]
public class ChangueSDtoHD_ImageEditor : Editor
{
    /*
    public static string getResourcesPath(string source)
    {
        string[] stringSeparators = new string[] { "/Resources/", "." };
        string[] result;

        result = source.Split(stringSeparators, StringSplitOptions.None);
        if (result.Length >= 2)
        {
            return result [1];
        }
        return "";
    }

    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();
        ChangueSDtoHD_Image changuer = ((ChangueSDtoHD_Image)target);
        bool isChangued = false;
        string sd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.SD));
        if (!string.IsNullOrEmpty(sd))
        {
            serializedObject.FindProperty("pathSD").stringValue = sd;
            serializedObject.FindProperty("nameSD").stringValue = changuer.SD.name;
            isChangued = true;
        }
        string hd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.HD));
        if (!string.IsNullOrEmpty(hd))
        {
            serializedObject.FindProperty("pathHD").stringValue = hd;
            serializedObject.FindProperty("nameHD").stringValue = changuer.HD.name;

            isChangued = true;
        }
        if (isChangued)
            serializedObject.ApplyModifiedProperties();

    }
    */

}
