using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(ChangueSpriteSdHdFromWorld))]
[CanEditMultipleObjects]
public class ChangueSpriteSdHdFromWorldEditor : Editor
{
    public static string getResourcesPath(string source)
    {
        string[] stringSeparators = new string[] { "/Resources/", "." };
        string[] result;

        result = source.Split(stringSeparators, StringSplitOptions.None);
        if (result.Length >= 2)
        {
            return result[1];
        }
        return "";
    }

    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();
        ChangueSpriteSdHdFromWorld changuer = ((ChangueSpriteSdHdFromWorld)target);
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
                    if (serializedObject.FindProperty("nameSdWorld").isArray)
                    {
                        if (serializedObject.FindProperty("nameSdWorld").arraySize <= i)
                            serializedObject.FindProperty("nameSdWorld").arraySize++;
                        serializedObject.FindProperty("nameSdWorld").GetArrayElementAtIndex(i).stringValue = changuer.SdWorld [i].name;

                    }
                    isChangued = true;
                }
            }

        }*/
        if (changuer.HdWorld != null)
        {
            for (int i = 0; i < changuer.HdWorld.Length; ++i)
            {
                string sd = getResourcesPath(AssetDatabase.GetAssetPath(changuer.HdWorld[i]));
                if (!string.IsNullOrEmpty(sd))
                {
                    if (serializedObject.FindProperty("pathHdWorld").isArray)
                    {
                        if (serializedObject.FindProperty("pathHdWorld").arraySize <= i)
                            serializedObject.FindProperty("pathHdWorld").arraySize++;
                        serializedObject.FindProperty("pathHdWorld").GetArrayElementAtIndex(i).stringValue = sd;

                    }
                    if (serializedObject.FindProperty("nameHdWorld").isArray)
                    {
                        if (serializedObject.FindProperty("nameHdWorld").arraySize <= i)
                            serializedObject.FindProperty("nameHdWorld").arraySize++;
                        serializedObject.FindProperty("nameHdWorld").GetArrayElementAtIndex(i).stringValue = changuer.HdWorld[i].name;

                    }
                    isChangued = true;
                }
            }

        }
        if (isChangued)
            serializedObject.ApplyModifiedProperties();

    }
}
