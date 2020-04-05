using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;


public class ManagerAnalytics : MonoSingleton<ManagerAnalytics>
{
    public static void MissionStarted(string missionName, bool isTutorial)
    {
        string worldID = getWorldID(missionName);
        string levelID = getLevelID(worldID, missionName);
        string sectionID = getSection(levelID, missionName);
        string missionType = getMissionType();

        Analytics.CustomEvent("missionStarted", new Dictionary<string, object>
         {
            { "missionName", missionName },
            { "missionType", missionType },
            { "worldID", worldID },
            { "levelID", levelID },
            { "sectionID", sectionID },
            { "isTutorial", isTutorial }
        });
    }

    public static void MissionFailed(string missionName,
        bool isTutorial, string reason = "Leave", float playTime = 0)
    {
        string worldID = getWorldID(missionName);
        string levelID = getLevelID(worldID, missionName);
        string sectionID = getSection(levelID, missionName);
        string missionType = getMissionType();

        Analytics.CustomEvent("missionFailed", new Dictionary<string, object>
         {
            { "missionName", missionName },
            { "missionType", missionType },
            { "worldID", worldID },
            { "levelID", levelID },
            { "sectionID", sectionID },
            { "isTutorial", isTutorial },
            { "terminationReason", reason },
            { "playTime", playTime }

        });
    }

    public static void MissionCompleted(string missionName,
        bool isTutorial, float playTime, float arcadeTime = 0, bool isFinalStage = false)
    {
        string worldID = getWorldID(missionName);
        string levelID = getLevelID(worldID, missionName);
        string sectionID = getSection(levelID, missionName);
        string missionType = getMissionType();

        Analytics.CustomEvent("missionCompleted", new Dictionary<string, object>
         {
            { "missionName", missionName },
            { "missionType", missionType },
            { "worldID", worldID },
            { "levelID", levelID },
            { "sectionID", sectionID },
            { "isTutorial", isTutorial },
            { "isFinalStage", isFinalStage },
            { "playTime", playTime },
            { "arcadeTime", arcadeTime }
        });
    }

    static string CheckNameCharacter(string character)
    {
        if (character.Contains("Klaus"))
        {
            character = "Klaus";
        }
        else if (character.Contains("K1"))
        {
            character = "K1";
        }
        else
        {
            character = "Klaus";

        }
        return character;
    }
    static string CheckMissionType(string missionType)
    {
        switch (missionType)
        {
            case "Story":
                break;
            case "ArcadeStory":
                break;
            case "ArcadeTime":
                break;
            case "Collectable":
                break;
            default:
                missionType = "Story";
                break;
        }
        return missionType;
    }
    static string getMissionType()
    {
        if (SaveManager.Instance.comingFromHistoryArcadeMode)
        {
            return "ArcadeStory";
        }
        else
        if (SaveManager.Instance.comingFromTimeArcadeMode)
        {
            return "ArcadeTime";
        }
        else if (SaveManager.Instance.comingFromCollectables)
        {
            return "Collectible";
        }

        return "Story";
    }

    static string getWorldID(string missionName)
    {
        bool isW = missionName.Substring(0, 1) == "W";
        if (isW)
        {
            return missionName.Substring(0, 2);
        }
        return "NONWORLD";
    }
    static string getLevelID(string world, string missionName)
    {

        if (world.Substring(0, 1) == "W")
        {
            string isLevel = missionName.Substring(2, 1);
            switch (isLevel)
            {
                case "L":
                    return missionName.Substring(2, 3);
                case "B":
                    return isLevel;
                case "C":
                    return missionName.Substring(2, 3);
            }
        }
        return "NONLEVEL";
    }
    static string getSection(string level, string missionName)
    {
        string isL = level.Substring(0, 1);
        switch (isL)
        {
            case "L":
                return missionName.Split('-')[1];
        }
        return "NONSECTION";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="missionName"></param>
    /// <param name="missionID"></param>
    /// <param name="isArcadeMode"></param>
    /// <param name="characterKilled">Klaus || K1</param>
    public static void CharacterDied(string missionName, string characterKilled,
        float coordinateX, float coordinateY, string reason = "")
    {

        string worldID = getWorldID(missionName);
        string levelID = getLevelID(worldID, missionName);
        string sectionID = getSection(levelID, missionName);
        string missionType = getMissionType();
        //  Debug.Log(worldID + levelID + sectionID);

        characterKilled = CheckNameCharacter(characterKilled);
        missionType = CheckMissionType(missionType);

        var dict = new Dictionary<string, object>
         {
            { "missionName", missionName },
            { "missionType", missionType },
            { "worldID", worldID },
            { "levelID", levelID },
            { "sectionID", sectionID },
            { "coordinateX", coordinateX },
            { "coordinateY", coordinateY }
        };
        if (!string.IsNullOrEmpty(reason))
            dict.Add("reason", reason);

        Analytics.CustomEvent("characterDied", dict);
    }
}
