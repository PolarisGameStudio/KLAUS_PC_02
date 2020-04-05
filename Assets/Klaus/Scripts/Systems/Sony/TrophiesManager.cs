using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class TrophiesManager : MonoSingleton<TrophiesManager>
{
    bool unlockTest = false;

    List<bool> TrophiesUnlocks = new List<bool>();
    bool waitForInit = true;
    public bool clearAllAchievements = false;
    public bool addAllAchievementsButTwo = false;
    public int TestAchievementUnlock = -1;
    public bool debugInput = false;

    protected override void Init()
    {
        //Debug.Log("Achievement name "+SteamUserStats.GetAchievementName(0));
       // SteamUserStats.SetAchievement("achievement_05");

        
        if (clearAllAchievements)
            ClearAchievements();
        /*/
        if (addAllAchievementsButTwo)
            AddAchievementsButTwo();
            /*/

        if (TestAchievementUnlock!=-1)
        {
            AwardTrophy(TestAchievementUnlock);
        }

        Debug.Log("I initiated the Trophies");

        
        base.Init();

    }
    bool isBusyAward = false;

    public void CheckLastTwoTrophies()
    {

        string ID = "";
        int counter = 0;
        for (int i = 0; i < 41; i++)
        {
            ID = "achievement_" + (i < 10 ? "0" : "") + i.ToString();
            if (SteamManager.Initialized)
            {
                bool achieved;

                bool ret = SteamUserStats.GetAchievement(ID, out achieved);
                if (achieved)
                {
                    counter++;
                }

            }
        }

        Debug.Log("Counter "+counter);

        if (counter==39 || counter == 40)
        {
            Debug.Log("I'm unlocking last two achievements");
            SteamUserStats.SetAchievement("achievement_38");
            SteamUserStats.SetAchievement("achievement_00");
            SteamUserStats.StoreStats();
        }
        counter = 0;

    }

    public void ClearAchievements()
    {
        string ID = "";
        for (int i = 0; i < 40; i++)
        {
            ID = "achievement_" + (i < 10 ? "0" : "") + i.ToString();
            if (SteamManager.Initialized)
            {
                Debug.Log("I'm truing to clear achievements");
                Debug.Log("I'm removing "+ ID);
                SteamUserStats.ClearAchievement(ID);
            }
            SteamUserStats.ClearAchievement("achievement_40");
        }
            SteamUserStats.StoreStats();

    }

    public void AddAchievementsButTwo()
    {
        string ID = "";
        for (int i = 1; i < 38; i++)
        {
            ID = "achievement_" + (i < 10 ? "0" : "") + i.ToString();
            if (SteamManager.Initialized)
            {
                Debug.Log("I'm adding achievements "+ID);
                SteamUserStats.SetAchievement(ID);
            }
        }

        SteamUserStats.SetAchievement("achievement_39");
        Debug.Log("I'm adding achievement_39");
        SteamUserStats.SetAchievement("achievement_40");
        Debug.Log("I'm adding achievement_40");
            SteamUserStats.StoreStats();

        }

    public void DebugInputSteam()
    {
        if(debugInput)
        {
            string ID = "achievement_" + (TestAchievementUnlock < 10 ? "0" : "") + TestAchievementUnlock.ToString();

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (SteamManager.Initialized)
                {
                    SteamUserStats.SetAchievement(ID);
                    Debug.Log("KLAUS: Trophy WIN CHECK: " + ID.ToString());
                    SteamUserStats.StoreStats();
                }

                TestAchievementUnlock++;
            }
        }


    }

    public void AwardTrophy(int trophyID)
    {
        string ID = "achievement_"+(trophyID < 10 ? "0" : "") + trophyID.ToString();

        TestSteamAchievement(ID);
        if (!unlockTest)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement(ID);
                Debug.Log("KLAUS: Trophy WIN CHECK: " + ID.ToString());
                SteamUserStats.StoreStats();
            }
        }
     //   StartCoroutine(AwatedTrophyCheck(trophyID));
    }
    IEnumerator AwatedTrophyCheck(int trophyID)
    {


        Debug.Log("KLAUS: Trophy WIN CHECK: " + trophyID);
        yield return null;

    }

    public void DEBUG_LockSteamAchievement(string ID)
    {
        if (SteamManager.Initialized)
        {
            TestSteamAchievement(ID);
            if (!unlockTest)
            {
                SteamUserStats.ClearAchievement(ID);
            }

        }
    }

    void TestSteamAchievement(string ID)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.GetAchievement(ID, out unlockTest);
        }
    }
    

    public void GiftAllTrophy()
    {
        for (int i = 1; i <= 40; ++i)
        {
            if (i != 38)
                AwardTrophy(i);
        }
    }

}

