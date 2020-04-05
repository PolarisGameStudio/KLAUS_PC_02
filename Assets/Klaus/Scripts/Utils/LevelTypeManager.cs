using UnityEngine;
using System.Text.RegularExpressions;

public class LevelTypeManager : MonoBehaviour
{
    static string[] levelTypes =
    {
        "W\\dL\\d\\d-\\d",    // W0L00-0
        "W\\dBossFight",      // W0BossFight
        "W\\dIntro",          // W0Intro
        "W\\dEnding",         // W0Ending
        "W\\dVideoEnding",    // W0Ending
        "W\\dC\\d\\d",        // W0C00
        "W\\dC\\d\\d_Video",  // W0C00_Video
        "Collect\\d\\d-\\d",  // Collect00-0
        "Loading"             // Loading
    };

    static Regex[] regex;

    public static string GetWorld(string sceneName)
    {
        string result = string.Empty;

        if (sceneName[0] == 'W' && char.IsDigit(sceneName[1]))
            result += sceneName[1];

        return result;
    }

    public static string GetLevel(string sceneName)
    {
        string result = string.Empty;

        if (sceneName[0] == 'W' && sceneName[2] == 'L' && char.IsDigit(sceneName[4]))
            result += sceneName[4];
        
        return result;
    }

    public static string GetSection(string sceneName)
    {
        string result = string.Empty;

        if (sceneName[0] == 'W' && sceneName[2] == 'L' && sceneName[5] == '-' && char.IsDigit(sceneName[6]))
            result += sceneName[6];
        
        return result;
    }
}
