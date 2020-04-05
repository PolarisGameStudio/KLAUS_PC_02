using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Assertions;
using Steamworks;


public class SaveManager : PersistentSingleton<SaveManager>
{
    public KlausData dataKlaus= new KlausData();
    

    #region File Info

    public static string SaveFileName { get; protected set; } = "KLAUS_SAVE_DATA.sav";
    public static string SaveFileFullPath { get { return GetPath(SaveFileName); } }
    public string TitleSave = "-KLAUS- SaveGame";
    public string NewTitleSave = "-KLAUS- SaveGame";
    public string SubTitleSave = "The save file for -KLAUS-";
    public string DetailSave = "All data from -KLAUS-";
    public string dataCorruptedMessageKey = "The save data will be overwritten.";
    public static bool nvidia = false;
    #endregion

    public static Action onGameLoaded;
    #region Temporal Var:
    public bool comingFromTimeArcadeMode;
    public bool comingFromHistoryArcadeMode;
    public bool comingFromMemoryMode;

    public string lastArcadeLevel;
    public void SetHistory()
    {
        comingFromTimeArcadeMode = false;
        comingFromHistoryArcadeMode = false;
        comingFromMemoryMode = false;
    }
    public bool isComingFromArcade
    {
        get
        {
            return comingFromTimeArcadeMode || comingFromHistoryArcadeMode;
        }
    }

    public void AddPlayTime(float time)
    {
        if (dataKlaus != null)
        {
            dataKlaus.totalTimePlayed += time;
            dataKlaus.isNewGame = false;
            Save();
        }
    }
    #endregion
    #region Position para volver del Collectable:

    public bool comingFromCollectables = false;
    public Vector3 posKlaus;
    public Vector3 posK1;
    public Vector3 posK1Checkpoint;
    public float currentTimePlayLevel = 0;
    /// <summary>
    /// The level of where we came from.
    /// </summary>
    public string LevelToLoadCollectable = "";
    /// <summary>
    /// The level to be loaded after see the video
    /// </summary>
    public string LevelToLoadVideoCollectable = "";
    /// <summary>
    /// The collectable level where we came from
    /// </summary>
    public string LevelCollectableLoaded = "";
    #endregion

    #region HistorySaveVar:
    public string[] WorldOfHistory = {
        "W1L01-1",
        "W1L02-1",
        "W1L03-1",
        "W1L04-1",
        "W1L05-1",
        "W1L06-1",
        "W1BossFight",
        "W2L01-1",
        "W2L02-1",
        "W2L03-1",
        "W2L04-1",
        "W2L05-1",
        "W2L06-1",
        "W3L01-1",
        "W3L02-1",
        "W3L03-1",
        "W3L04-1",
        "W3L05-1",
        "W3L06-1",
        "W4L01-1",
        "W4L02-1",
        "W4L03-1",
        "W4L04-1",
        "W4L05-1",
        "W4L06-1",
        "W4BossFight",
        "W5L01-1",
        "W5L02-1",
        "W5L03-1",
        "W5L04-1",
        "W5L05-1",
        "W5L06-1",
        "W6L01-1",
        "W6L02-1",
        "W6L03-1",
        "W6L04-1",
        "W6L05-1",
        "W6L06-1",
    };

    bool CheckSaveUpdate = false;

    public void SaveCurrentLevel(string level)
    {
        if (!isComingFromArcade)
        {
            if (dataKlaus != null)
            {
                if (level.Substring(0, 1) == "W")
                {
                    //  SaveManager.Instance.dataKlaus.world = Convert.ToInt32(Application.loadedLevelName.Substring(1, 1));
                    string typeLevel = level.Substring(2, 1);
                    if (typeLevel == "L")
                    {
                        dataKlaus.SetCurrentLevel(level.Substring(0, 6) + "1");
                    }
                    else if (typeLevel == "B")
                    {
                        dataKlaus.SetCurrentLevel(level);
                    }
                }
                CheckSaveUpdate = true;
            }
        }
    }

    public float GetHistoryPercent()
    {
        if (dataKlaus != null)
        {
            if (!dataKlaus.isNewGame)
            {
                if (dataKlaus.CompleteGame)
                {
                    return 100f;
                }
                else if (dataKlaus.CompleteMainStory)
                {
                    return 97.368f;
                }
                else
                {
                    for (int i = 1; i < WorldOfHistory.Length; ++i)
                    {
                        if (WorldOfHistory[i] == dataKlaus.GetCurrentLevel())
                        {
                            return (i - 1) * (100.0f / WorldOfHistory.Length);
                        }
                    }
                }
            }
        }
        return 0;
    }

    #endregion

    bool _isB = false;
    bool isBusy
    {
        set
        {
            _isB = value;

            if (SaveMessagesManager.Instance != null)
            {
                if (_isB)
                    SaveMessagesManager.Instance.ShowSave();
                else
                    SaveMessagesManager.Instance.HideSave();
            }
        }
    }

    public bool isSaving
    {
        get
        {
            return _isB;
        }
    }

    void Awake()
    {
        Screen.SetResolution(800, 600, false);
        DontDestroyOnLoad(this);

        if (RankingManager.Instance != null)
            RankingManager.Instance.OnOwnRankObtanied += LoadedOwnRank;

        if (dataKlaus != null)
        {
            dataKlaus.CreateLevels();
            LoadRanksFromServer();

        }

        CreateDataKlaus();

    }

    void LoadedOwnRank(List<RankingManager.ScoreRank> ranks)
    {
        if (dataKlaus != null)
            dataKlaus.LoadedOwnRank(ranks);
    }


    public void LoadRanksFromServer()
    {
        /// TODO: PC: Fix this for steam
       /*/ if (dataKlaus != null)
            StartCoroutine("DownloadRanks");/*/
    }

    IEnumerator DownloadRanks()
    {
        // If we are not signed in and we are not trying to sign in, exit
        if (!SonyUserManager.Instance.IsSignedIn || !SonyUserManager.Instance.IsSigninIn)
            yield break;

        // If we are here, we are trying to sign in
        while (!SonyUserManager.Instance.IsSignedIn)
            yield return null;

        // Try to download every score from the server
        foreach (SpeedLevelInfo info in dataKlaus.levels)
        {
            while (RankingManager.Instance.RefreshOwnRankIsBusy)
                yield return null;

            RankingManager.Instance.RefreshOwnRank(info.id);
        }
    }

    void Start()
    {
        StartCoroutine("LoadOrSave");
    }

    public static string GetPath(string filename)
    {
        string path = "";
        if (SteamManager.Initialized)
        {
            path = Path.Combine(Application.persistentDataPath, SteamUser.GetSteamID().ToString());

            if (!Directory.Exists(path))//if the directory doesn't exist. it's created
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(Application.persistentDataPath, SteamUser.GetSteamID().ToString());
        }

        else
        {
            path = Application.persistentDataPath;

        }
        return Path.Combine(path, filename);
    }

    public static bool Exists(string filename)
    {
        return File.Exists(GetPath(filename));//PlayerPrefs.HasKey("isNewGame");
    }

    /// <summary>
    /// To know in which time I csaved the file
    /// </summary>
    protected DateTime m_lastModification;

    IEnumerator SaveData(KlausData data, string fileName)
    {
        while (isSaving)
        {
            yield return null;
        }
        isBusy = true;
        m_lastModification = DateTime.Now;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetPath(fileName));
        bf.Serialize(file, m_lastModification);
        data.WriteToBuffer(ref bf, ref file);
        file.Flush();
        file.Close();
        isBusy = false;

    }


    public void Save()
    {
        StartCoroutine(SaveData(dataKlaus, SaveFileName));//we don't stop the courutine it will handle for itslef

        // Always try to update any scores missing on the server
        dataKlaus.UpdateScoresOnServer();

        //   Debug.LogError("Saved Game");
        //  Debug.LogError("Current level " + dataKlaus.currentLevel);
        TrophiesManager.Instance.CheckLastTwoTrophies();
    }


    void emptyDataKlaus()
    {
        if (dataKlaus == null)
        {
            dataKlaus = new KlausData();
            dataKlaus.CreateLevels();
            LoadRanksFromServer();
        }
    }

    IEnumerator Load(string filename)
    {
        while (isSaving)
            yield return null;

        isBusy = true;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(GetPath(filename), FileMode.Open);
        object dateValue = bf.Deserialize(file);
        Assert.IsNotNull(dateValue);
        m_lastModification = Convert.ToDateTime(dateValue);

        // Read the game data from the buffer.
        Debug.LogError("This is loading");
        dataKlaus.ReadFromBuffer(ref bf, ref file);
        file.Close();
        // Set settings
        LanguageValueSave.UpdateLanguage();
        ColorWorldManager.Instance.LoadWorldColor();
        if (onGameLoaded != null)
            onGameLoaded();
        isBusy = false;

        Debug.LogError("This is the loaded resolution " + dataKlaus.reswidth.ToString() + "x" + dataKlaus.resheight.ToString());
        Debug.LogError("This is the loaded fullscreen " + dataKlaus.fullscreen);

        TrophiesManager.Instance.CheckLastTwoTrophies();
    }

    void CreateDataKlaus()
    {
        Resolution[] resolutions = Screen.resolutions;

        //line for all the resolutions available
      //  Debug.LogError("Resolution availble");

        for (int i = 0; i < resolutions.Length; i++)
        {
       //     Debug.LogError("Res " + i.ToString() + " " + resolutions[i].width + "x" + resolutions[i].height);
        }
        

        dataKlaus.nativeWidth = resolutions[resolutions.Length-1].width;
        dataKlaus.nativeHeight = resolutions[resolutions.Length - 1].height;

        emptyDataKlaus();



        if (Exists(SaveFileName))
        {
           StartCoroutine(Load(SaveFileName));

            Screen.SetResolution(dataKlaus.reswidth, dataKlaus.resheight, dataKlaus.fullscreen);
            Debug.LogError("I just setted up this "+dataKlaus.reswidth+"x"+dataKlaus.resheight+" "+dataKlaus.fullscreen);

        }
        else
        {
            Screen.SetResolution(dataKlaus.nativeWidth, dataKlaus.nativeHeight, true);
            LanguageValueSave.InitLanguage();
            Save();
            Debug.LogError("First time data");
        }
    }

    IEnumerator LoadOrSave()
    {
        yield return new WaitForEndOfFrame();
        CreateDataKlaus();
    }

    public void StartSaveManager()
    {
        StopCoroutine("LoadOrSave");
        CreateDataKlaus();

    }

    void Update()
    {
        if (CheckSaveUpdate)
        {
            CheckSaveUpdate = false;
            Save();
        }

        if(SaveManager.Instance.dataKlaus.fxVolume!= AudioListener.volume)
        {
            AudioListener.volume = SaveManager.Instance.dataKlaus.fxVolume;
        }

        
    }


    #region logic for downlaod save form server
    /* LOGIC FOR DOWNLOAD FROM SERVER
     public void SetSaveFile (string filename, byte[] data, Action onSaveFinish)
    {
        DateTime dateLocal;
        DateTime dateRemote;

        FileStream file2 = File.Create (GetPath (filename + "temp"));
        file2.Write (data, 0, data.Length);
        file2.Flush ();
        file2.Close ();

        BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = new FileStream (GetPath (filename + "temp"), FileMode.Open);
        object dateValue = bf.Deserialize (file);
        Assert.IsNotNull (dateValue);
        dateRemote = Convert.ToDateTime (dateValue);
        file.Close ();

        if (isSaveFileExists (filename)) {
            file = new FileStream (GetPath (filename), FileMode.Open);
            dateValue = bf.Deserialize (file);
            Assert.IsNotNull (dateValue);
            dateLocal = Convert.ToDateTime (dateValue);
            file.Close ();
            Debug.Log ("Save FileLocal: " + dateLocal + " > Remote: " + dateRemote + " :" + (dateLocal >= dateRemote) + " Diff:" + (dateLocal - dateRemote).TotalSeconds);

            if (dateLocal != dateRemote) {
                //Show Poup and wait for answer 
                m_tempFilename = filename;
                m_onSaveFinish = onSaveFinish;
                SavePopup.Instance.Show (dateLocal.ToString (), dateRemote.ToString (), DeleteRemote);
                return;
                //DeleteRemote (dateLocal >= dateRemote);
            } else {
                File.Delete (GetPath (m_tempFilename + "temp"));
            }
            //
        } else {
            File.Move (GetPath (filename + "temp"), GetPath (filename));
        }
        if (onSaveFinish != null)
            onSaveFinish ();
    }

    [NonSerialized]
    string m_tempFilename = "";
    [NonSerialized]
    Action m_onSaveFinish;

    public void DeleteRemote (bool value)
    {
        if (value) {
            // Prompt message you have a local file more recent
            File.Delete (GetPath (m_tempFilename + "temp"));
        } else {
            //
            File.Delete (GetPath (m_tempFilename));
            File.Move (GetPath (m_tempFilename + "temp"), GetPath (m_tempFilename));
        }  
        if (m_onSaveFinish != null)
            m_onSaveFinish.Invoke ();
        m_tempFilename = "";
    }
     */
    #endregion
}
