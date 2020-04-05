using UnityEngine;

public class ArcadeLevelsInfo : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public string sceneName;
        public bool hasTimeAttack = true;
        public string title;
        public Sprite picture;
        public string[] sectionNames;
        public float companyRecordSeconds = 600;
    }

    public int worldID;
    public string Title;
    public Level[] levels;

    public int GetLevelId(int levelId)
    {
        return int.Parse((worldID + 1) + string.Empty + (levelId + 1));
    }
}
