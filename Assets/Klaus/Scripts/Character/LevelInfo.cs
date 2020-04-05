//
// LevelInfo.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class LevelInfoKlaus : MonoBehaviour
{

	public static string getCurrentLevelName(){
		//N101,5,6,7,8
		return Application.loadedLevelName;
	}
	public static void ResetInfoLevel(string idLevel)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        PlayerPrefs.SetInt(idLevel,-1);
#else
		SonyVitaSavedKlaus.Instance.AutoSave(true);
#endif

	}public static void ResetLevels()
    {
#if UNITY_EDITOR ||UNITY_STANDALONE
        PlayerPrefs.DeleteAll();
#else
		SonyVitaSavedKlaus.Instance.AutoSave(true);
#endif
	}
	public static void SaveInfoLevel(string idLevel)
    {
#if UNITY_EDITOR ||UNITY_STANDALONE
        PlayerPrefs.SetInt(idLevel,-1);
#else
		SonyVitaSavedKlaus.Instance.data.setLevelData(idLevel,-1);
		SonyVitaSavedKlaus.Instance.AutoSave(false);
#endif
	}
	public static void SaveInfoLevel()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        PlayerPrefs.SetInt(getCurrentLevelName(),-1);
#else
		SonyVitaSavedKlaus.Instance.data.setLevelData(getCurrentLevelName(),-1);
		SonyVitaSavedKlaus.Instance.AutoSave(false);
#endif

    }
    public static int GetMoveCount(string idLevel)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return PlayerPrefs.GetInt(idLevel);
#else
		return SonyVitaSavedKlaus.Instance.data.getLevelData(idLevel);
#endif

	}
	public static int GetMoveCount(){
        //	SonyVitaSavedKlaus.Instance.AutoLoad();
#if UNITY_EDITOR ||UNITY_STANDALONE
        return PlayerPrefs.GetInt(getCurrentLevelName());

#else
		return SonyVitaSavedKlaus.Instance.data.getLevelData(getCurrentLevelName());
#endif
	}

	public static bool isNewGame(){
	
		//return GetMoveCount("N100") <= 0;
        return false;
	}

}

