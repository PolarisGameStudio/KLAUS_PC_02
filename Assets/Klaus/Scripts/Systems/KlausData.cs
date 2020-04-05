using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class SpeedLevelInfo
{
    public string level = "W1L01";
    public int id;

    public float time { get; set; }
}

[Serializable]
public class CollectablesInfo
{
    public int ID = -1;
    public List<bool> item = new List<bool>();
}


[Serializable]
public class KlausData
{
    const int kSaveDataMaxSize = 64 * 1024;

    public KlausData()
    {
        CreateLevels();

        isNewGame = true;
        world = 1;
        currentLevel = "W1L01-1";
        CompleteGame = false;
        totalTimePlayed = 0;
        deaths = 0;
        jumps = 0;
        destroy_mugclone = 0;
        destroy_clone = 0;
        destroy_objectUpper = 0;
        destroy_object = 0;
        hack_cpu = 0;
        Rumble = true;
        bgVolume = 1;
        fxVolume = 1;
        fullscreen = true;
        reswidth = 0;
        resheight = 0;
        nativeWidth = 0;
        nativeHeight = 0;
        controlSensitivity = 0.1f;

        List<bool> itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        collectablesItems = new List<CollectablesInfo>();
        CollectablesInfo collect1 = new CollectablesInfo();
        collect1.ID = 1;
        collect1.item = itemsPC;
        collectablesItems.Add(collect1);

        itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        CollectablesInfo collect2 = new CollectablesInfo();
        collect2.ID = 2;
        collect2.item = itemsPC;
        collectablesItems.Add(collect2);

        itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        CollectablesInfo collect3 = new CollectablesInfo();
        collect3.ID = 3;
        collect3.item = itemsPC;
        collectablesItems.Add(collect3);

        itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        CollectablesInfo collect4 = new CollectablesInfo();
        collect4.ID = 4;
        collect4.item = itemsPC;
        collectablesItems.Add(collect4);

        itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        CollectablesInfo collect5 = new CollectablesInfo();
        collect5.ID = 5;
        collect5.item = itemsPC;
        collectablesItems.Add(collect5);

        itemsPC = new List<bool>(6);
        for (int i = 0; i < 6; ++i)
            itemsPC.Add(false);
        CollectablesInfo collect6 = new CollectablesInfo();
        collect6.ID = 6;
        collect6.item = itemsPC;
        collectablesItems.Add(collect6);

    }

    public void CreateLevels()
    {
        if (worlds == null)
            return;

        for (int i = 0; i != worlds.Length; ++i)
        {
            for (int j = 0; j != worlds[i].levels.Length; ++j)
            {
                string name = worlds[i].levels[j].sceneName.Split('-')[0];
                levels.Add(
                    new SpeedLevelInfo()
                    {
                        level = name,
                        id = worlds[i].GetLevelId(j),
                        time = 0
                    });
            }
        }
    }

    public bool isNewGame = true;


    public int world = 1;
    public string currentLevel = "W1L01-1";
    public void SetCurrentLevel(string newLevel)
    {
        currentLevel = newLevel;
    }
    public string GetCurrentLevel()
    {
        return currentLevel;
    }
    public float totalTimePlayed = 0;

    #region Deaths

    public int deaths = 0;
    public int jumps = 0;
    public Action<int> jumpUpdateCallback;
    public int destroy_mugclone = 0;
    public Action<int> destroy_mug_UpdateCallback;
    public int destroy_clone = 0;
    public Action<int> destroy_clone_UpdateCallback;
    public int destroy_objectUpper = 0;
    public Action<int> destroy_objectUpper_UpdateCallback;
    public int destroy_object = 0;
    public Action<int> destroy_object_UpdateCallback;
    public int hack_cpu = 0;
    public Action<int> hack_cpu_UpdateCallback;

    public void AddDeath()
    {
        ++deaths;
        //Deberia ir el Analticia de la muerte
    }
    /*
	* Aqui hago update cada vez que anado un valor para dar el trofeo al momento
	*/

    public void AddJump()
    {
        ++jumps;
        UpdateJumps();
    }

    public void UpdateJumps()
    {
        if (jumpUpdateCallback != null)
            jumpUpdateCallback(jumps);
    }

    public void AddDestroy_MugClone()
    {
        ++destroy_mugclone;
        UpdateDestroy_MugClone();
    }

    public void UpdateDestroy_MugClone()
    {
        if (destroy_mug_UpdateCallback != null)
            destroy_mug_UpdateCallback(destroy_mugclone);
    }

    public void AddDestroy_Clone()
    {
        ++destroy_clone;
        UpdateDestroy_Clone();
    }

    public void UpdateDestroy_Clone()
    {
        if (destroy_clone_UpdateCallback != null)
            destroy_clone_UpdateCallback(destroy_clone);
    }

    public void AddDestroy_ObjectUpper()
    {
        ++destroy_objectUpper;
        UpdateDestroy_ObjectUpper();
    }

    public void UpdateDestroy_ObjectUpper()
    {
        if (destroy_objectUpper_UpdateCallback != null)
            destroy_objectUpper_UpdateCallback(destroy_objectUpper);
    }

    public void AddDestroy_Object()
    {
        ++destroy_object;
        UpdateDestroy_Object();
    }

    public void UpdateDestroy_Object()
    {
        if (destroy_object_UpdateCallback != null)
            destroy_object_UpdateCallback(destroy_object);
    }

    public void AddHack_CPU()
    {
        ++hack_cpu;
        UpdateHack_CPU();
    }

    public void UpdateHack_CPU()
    {
        if (hack_cpu_UpdateCallback != null)
            hack_cpu_UpdateCallback(hack_cpu);
    }

    #endregion

    #region Time Attack

    [SerializeField]
    public ArcadeLevelsInfo[] worlds;
    [HideInInspector]
    public List<SpeedLevelInfo> levels = new List<SpeedLevelInfo>();

    public void SetTime(string sceneName, float time)
    {
        string name = sceneName.Split('-')[0];

        foreach (SpeedLevelInfo info in levels)
            if (info.level.Equals(name))
            {
                info.time = time;
                break;
            }

        UpdateScoresOnServer();
    }

    public int GetBrokenCompanyRecords()
    {
        int result = 0;
        for (int i = 0; i != worlds.Length; ++i)
        {
            for (int j = 0; j != worlds[i].levels.Length; ++j)
            {
                string sceneName = worlds[i].levels[j].sceneName.Split('-')[0];
                float companyRecord = worlds[i].levels[j].companyRecordSeconds;
                float currentRecord = SaveManager.Instance.dataKlaus.GetTime(sceneName);

                if (worlds[i].levels[j].hasTimeAttack && currentRecord > 0f && currentRecord < companyRecord)
                    ++result;
            }
        }

        return result;
    }

    public int GetTotalCompanyRecords()
    {
        int result = 0;
        for (int i = 0; i != worlds.Length; ++i)
            result += Array.FindAll<ArcadeLevelsInfo.Level>(worlds[i].levels, x => x.hasTimeAttack).Length;

        return result;
    }

    public float GetTime(string sceneName)
    {
        string name = sceneName.Split('-')[0];

        foreach (SpeedLevelInfo info in levels)
            if (info.level.Equals(name))
                return info.time;

        return 0;
    }

    public float TotalWorldTime(string worldID)
    {
        float sum = 0f;
        string id = worldID.ToUpper();

        foreach (SpeedLevelInfo info in levels)
            if (info.level.StartsWith(id))
                sum += info.time;

        return sum;
    }

    public float TotalWorldTime(int worldID)
    {
        return TotalWorldTime("W" + worldID);
    }

    public float TotalSpeedrunTime()
    {
        float sum = 0f;

        foreach (SpeedLevelInfo info in levels)
            sum += info.time;

        return sum;
    }

    public void LoadedOwnRank(List<RankingManager.ScoreRank> ranks)
    {
        if (ranks == null)
            return;

        for (int i = 0; i != ranks.Count; ++i)
        {
            int boardId = ranks[i].boardId;
            float time = ranks[i].score;

            foreach (SpeedLevelInfo info in levels)
            {
                if (info.id == boardId && (info.time <= 0 || time < info.time))
                {
                    info.time = time;
                    break;
                }
            }
        }
    }

    public void UpdateScoresOnServer()
    {
        foreach (SpeedLevelInfo info in levels)
            if (info.time != 0)
                RankingManager.Instance.RegisterScore(info.id, (ulong)(info.time * 1000f));
    }

    public void UpdateData()
    {
        UpdateJumps();
        UpdateDestroy_MugClone();
        UpdateDestroy_Clone();
        UpdateDestroy_ObjectUpper();
        UpdateDestroy_Object();
        UpdateHack_CPU();
    }

    #endregion

    #region History
    public List<CollectablesInfo> collectablesItems = new List<CollectablesInfo>();
    public bool CompleteGame = false;
    public bool CompleteMainStory = false;

    #endregion
    public bool Rumble = true;
    public float bgVolume = 1;
    public float fxVolume = 1;
    public string language = "en";
    public bool fullscreen = true;
    public int reswidth = 0;
    public int resheight = 0;
    public int nativeWidth = 0;
    public int nativeHeight = 0;
    public float controlSensitivity = 0;
    public string[][] gamepadController = new string[][]
{
    new string[] {"LT","SelectBothCharacters"},
    new string[] {"L","SwitchCharacters"},
    new string[] {"RT","Target"},
    new string[] {"R","MoveCamera"},
    new string[] {"Y","Throw"},
    new string[] {"B","Hack/Punch"},
    new string[] {"R","Jump"},
    new string[] {"X","Run"},
};

    public bool isArcadeModeUnlock = false;

    #region Save Logic
    public void WriteToBuffer(ref BinaryFormatter bf, ref FileStream file)
    {
        bf.Serialize(file, isNewGame);
        bf.Serialize(file, world);
        bf.Serialize(file, currentLevel);
        bf.Serialize(file, deaths);
        bf.Serialize(file, totalTimePlayed);

        foreach (SpeedLevelInfo level in levels)
            bf.Serialize(file, level.time);

        for (int i = 0; i != collectablesItems.Count; ++i)
            for (int j = 0; j != collectablesItems[i].item.Count; ++j)
                bf.Serialize(file, collectablesItems[i].item[j]);

        bf.Serialize(file, Rumble);
        bf.Serialize(file, bgVolume);
        bf.Serialize(file, fxVolume);
        bf.Serialize(file, isArcadeModeUnlock);
        bf.Serialize(file, language);
        //new values
        if(reswidth==0)
        {
            reswidth = Screen.width;
            resheight = Screen.height;
        }
        bf.Serialize(file, fullscreen);
        bf.Serialize(file, reswidth);
        bf.Serialize(file, resheight);
        //end of new values
        bf.Serialize(file, jumps);
        bf.Serialize(file, destroy_mugclone);
        bf.Serialize(file, destroy_clone);
        bf.Serialize(file, destroy_objectUpper);
        bf.Serialize(file, destroy_object);
        bf.Serialize(file, hack_cpu);
        bf.Serialize(file, CompleteGame);
        bf.Serialize(file, CompleteMainStory);
        bf.Serialize(file, controlSensitivity);
        Debug.Log("I'm saving control sensitivity " + controlSensitivity);
    }

    static void ReadInt(ref int value, ref BinaryFormatter bf, ref FileStream file)
    {
        value = Convert.ToInt32(bf.Deserialize(file));
    }
    static void ReadString(ref string value, ref BinaryFormatter bf, ref FileStream file)
    {
        value = bf.Deserialize(file) as string;
    }
    static void ReadBoolean(ref bool value, ref BinaryFormatter bf, ref FileStream file)
    {
        value = Convert.ToBoolean(bf.Deserialize(file));
    }
    static void ReadSingle(ref float value, ref BinaryFormatter bf, ref FileStream file)
    {

        value = Convert.ToSingle(bf.Deserialize(file));
    }

    public void ReadFromBuffer(ref BinaryFormatter bf, ref FileStream file)
    {
        ReadBoolean(ref isNewGame, ref bf, ref file);
        ReadInt(ref world, ref bf, ref file);
        ReadString(ref currentLevel, ref bf, ref file);
        ReadInt(ref deaths, ref bf, ref file);
        ReadSingle(ref totalTimePlayed, ref bf, ref file);

        foreach (SpeedLevelInfo level in levels)
            level.time = Convert.ToSingle(bf.Deserialize(file));

        for (int i = 0; i != collectablesItems.Count; ++i)
            for (int j = 0; j != collectablesItems[i].item.Count; ++j)
                collectablesItems[i].item[j] = Convert.ToBoolean(bf.Deserialize(file));

        ReadBoolean(ref Rumble, ref bf, ref file);
        ReadSingle(ref bgVolume, ref bf, ref file);
        ReadSingle(ref fxVolume, ref bf, ref file);
        ReadBoolean(ref isArcadeModeUnlock, ref bf, ref file);
        ReadString(ref language, ref bf, ref file);
        //new values
        ReadBoolean(ref fullscreen, ref bf, ref file);
        ReadInt(ref reswidth, ref bf, ref file);
        ReadInt(ref resheight, ref bf, ref file);
        //end of new values
        ReadInt(ref jumps, ref bf, ref file);
        ReadInt(ref destroy_mugclone, ref bf, ref file);
        ReadInt(ref destroy_clone, ref bf, ref file);
        ReadInt(ref destroy_objectUpper, ref bf, ref file);
        ReadInt(ref destroy_object, ref bf, ref file);
        ReadInt(ref hack_cpu, ref bf, ref file);

        ReadBoolean(ref CompleteGame, ref bf, ref file);
        ReadBoolean(ref CompleteMainStory, ref bf, ref file);
       
       try
       { 
            ReadSingle(ref controlSensitivity, ref bf, ref file);

       }
        catch (SerializationException se)
        {
            Debug.Log("Error opening this file due to serialization "+se.Source);

        }

        Debug.Log("I'm loading control sensitivity " + controlSensitivity);


        // Always try to update any scores missing on the server
        UpdateScoresOnServer();
    }
    #endregion
}