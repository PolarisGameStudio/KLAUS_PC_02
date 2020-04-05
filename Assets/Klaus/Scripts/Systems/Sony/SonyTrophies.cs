using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SonyTrophies
{
    int nextTrophyIndex = 1;
    // trophy 0 is the platinum trophy which we can't award, it gets awarded automatically when all other trophies have been awarded.
    Texture2D trophyIcon = null;
    Texture2D trophyGroupIcon = null;

    public SonyTrophies()
    {
        Initialize();
    }

    public void Initialize()
    {

    }

    public void RequestGroupInfo()
    {

    }

    public void RequestTrophyInfo()
    {

    }

    public void RequestTrophyProgress()
    {
    }

    public void AwardTrophy(int trophyID)
    {

    }

    public void RegisterTrophyPack()
    {

    }

    void OnSomeEvent()
    {
    }

    void DumpGameInfo()
    {

    }

    void OnTrophyGotGameInfo()
    {
        DumpGameInfo();
    }

    void OnTrophyGotGroupInfo()
    {
        Debug.Log("Got Group List!");

    }

    void OnTrophyGotTrophyInfo()
    {

    }

    void OnTrophyGotProgress()
    {

    }
}
