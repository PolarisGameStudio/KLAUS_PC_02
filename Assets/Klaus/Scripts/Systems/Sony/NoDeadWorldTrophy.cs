using UnityEngine;
using System.Collections.Generic;
using System;

public class NoDeadWorldTrophy : CompleteScene_Trophy
{
    public string[] LevelsW1;
    public string[] LevelsW2;
    public string[] LevelsW3;
    public string[] LevelsW4;
    public string[] LevelsW5;
    public string[] LevelsW6;

    List<bool> checkLevelZeroDead = new List<bool>();

    int currentW = -1;
    int currentDeads = -1;
    int currentScene = -1;

    protected override bool CanRegister()
    {
        if (UnLock)
            return false;

        return true;
    }

    protected override void OnStart(ICompleteLevel objCompelte)
    {
        if (Application.loadedLevelName.Substring(0, 1) != "W")
            return;

        int newCurrentLevel = Convert.ToInt32(Application.loadedLevelName.Substring(1, 1));
        if (newCurrentLevel == currentW)
        {
            currentScene = getLevelDead();
        }
        else //Cambiamos de mundo
        {
            currentW = newCurrentLevel;
            RefillList();
            currentScene = getLevelDead();

        }
        if (SaveManager.Instance.dataKlaus != null)
        {
            currentDeads = SaveManager.Instance.dataKlaus.deaths;
        }
        else
            currentDeads = -1;
        base.OnStart(objCompelte);
    }

    protected override void OnCompleteScene()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            int deaths = SaveManager.Instance.dataKlaus.deaths - currentDeads;
            Debug.Log("deaths: " + deaths);
            if (deaths == 0)
            {
                if (AddLevelDead())
                {
                    base.OnCompleteScene();
                }
            }
            else
            {
                currentW = -1;
            }
        }
    }

    bool AddLevelDead()
    {
        Debug.Log("currentScene: " + currentScene + " checkLevelZeroDead: " + checkLevelZeroDead.Count);

        if (currentScene < 0 || currentScene >= checkLevelZeroDead.Count)
            return false;

        checkLevelZeroDead[currentScene] = true;
        bool getAllLevel = true;
        for (int i = 0; i < checkLevelZeroDead.Count; ++i)
        {
            getAllLevel = getAllLevel && checkLevelZeroDead[i];
        }
        return getAllLevel;
    }

    int getLevelDead()
    {
        switch (currentW)
        {
            case 1:
                for (int i = 0; i < LevelsW1.Length; ++i)
                    if (LevelsW1[i] == Application.loadedLevelName)
                        return i;
                break;
            case 2:
                for (int i = 0; i < LevelsW2.Length; ++i)
                    if (LevelsW2[i] == Application.loadedLevelName)
                        return i;
                break;
            case 3:
                for (int i = 0; i < LevelsW3.Length; ++i)
                    if (LevelsW3[i] == Application.loadedLevelName)
                        return i;
                break;
            case 4:
                for (int i = 0; i < LevelsW4.Length; ++i)
                    if (LevelsW4[i] == Application.loadedLevelName)
                        return i;
                break;
            case 5:
                for (int i = 0; i < LevelsW5.Length; ++i)
                    if (LevelsW5[i] == Application.loadedLevelName)
                        return i;
                break;
            case 6:
                for (int i = 0; i < LevelsW6.Length; ++i)
                    if (LevelsW6[i] == Application.loadedLevelName)
                        return i;
                break;
        }
        return -1;
    }

    void RefillList()
    {
        checkLevelZeroDead.Clear();
        switch (currentW)
        {
            case 1:
                for (int i = 0; i < LevelsW1.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
            case 2:
                for (int i = 0; i < LevelsW2.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
            case 3:
                for (int i = 0; i < LevelsW3.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
            case 4:
                for (int i = 0; i < LevelsW4.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
            case 5:
                for (int i = 0; i < LevelsW5.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
            case 6:
                for (int i = 0; i < LevelsW6.Length; ++i)
                    checkLevelZeroDead.Add(false);
                break;
        }
    }
}