using UnityEngine;
using UnityEditor;

public class LevelInfoAsset
{
    [MenuItem("Assets/Create/ArcadeLevelsInfo")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ArcadeLevelsInfo>();
    }
}