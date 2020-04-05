using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(ChangueAnimatorSdHdFromWorld))]
[CanEditMultipleObjects]
public class ChangueAnimatorSdHdFromWorldEditor : Editor
{
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

        ChangueAnimatorSdHdFromWorld changuer = ((ChangueAnimatorSdHdFromWorld)target);
        bool isChangued = false;
        /*
        if (changuer.SdWorld != null)
        {
            for (int i = 0; i < changuer.SdWorld.Length; ++i)
            {
                string sd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.SdWorld [i]));
                if (!string.IsNullOrEmpty(sd))
                {
                    if (serializedObject.FindProperty("pathSdWorld").isArray)
                    {
                        if (serializedObject.FindProperty("pathSdWorld").arraySize <= i)
                        {
                            serializedObject.FindProperty("pathSdWorld").arraySize++;
                        }

                        serializedObject.FindProperty("pathSdWorld").GetArrayElementAtIndex(i).stringValue = sd;

                    }
                    isChangued = true;
                }
            }

        }*/

        if (changuer.HdWorld != null)
        {
            for (int i = 0; i < changuer.HdWorld.Length; ++i)
            {
                string sd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.HdWorld [i]));
                if (!string.IsNullOrEmpty(sd))
                {
                    if (serializedObject.FindProperty("pathHdWorld").isArray)
                    {
                        if (serializedObject.FindProperty("pathHdWorld").arraySize <= i)
                            serializedObject.FindProperty("pathHdWorld").arraySize++;
                        serializedObject.FindProperty("pathHdWorld").GetArrayElementAtIndex(i).stringValue = sd;

                    }
                    isChangued = true;
                }
            }

        }
        if (isChangued)
            serializedObject.ApplyModifiedProperties();
    }
}
