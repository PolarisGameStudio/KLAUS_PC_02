using SmartLocalization.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class LeaderboardsManager : UIEntryList<LeaderboardEntry>
{
    public LeaderboardEntry playerEntry;

    public enum BoardType
    {
        Global = 0,
        Friends = 1
    }

    public int entriesToLoad = 100;
    public BoardType boardType = BoardType.Global;
    public int worldIndex;
    public int levelIndex;

    public CanvasGroup canvasGroup, messageCanvas, boardCanvas;
    public LocalizedTextMeshPro messageLabel;
    public LocalizedTextMeshPro worldLabel;
    public LocalizedTextMeshPro levelLabel;
    public LocalizedTextMeshPro boardLabel;
    public string friendsKey = "UI.Leaderboards.FriendsScores", allTimeKey = "UI.Leaderboards.AllTimeScores";

    public ArcadeLevelsInfo[] levelsInfo;
    public SetColorImage[] images;

    public float colorLerpSpeed = 3f;
    public bool automaticWorldLevelSetup = true;

    bool initialized;

    public int currentBoard { get { return levelsInfo[worldIndex].GetLevelId(levelIndex); } }

    public BackButtonMenu back
    {
        get
        {
            if (_back == null)
                _back = GetComponent<BackButtonMenu>();
            return _back;
        }
    }

    public LeaderboardsScrollRect rectScroll
    {
        get
        {
            if (_rectScroll == null)
                _rectScroll = GameObject.FindObjectOfType<LeaderboardsScrollRect>();
            return _rectScroll;
        }
    }

    BackButtonMenu _back;
    LeaderboardsScrollRect _rectScroll;

    #region Setup

    void Awake()
    {
        InitPools(50);

        RankingManager.Instance.OnOwnFriendsRankObtanied += OnScoresObtained;
        RankingManager.Instance.OnOwnGlobalRankObtanied += OnScoresObtained;
        RankingManager.Instance.OnOwnRankObtanied += OnPlayerScoreObtained;
        // RankingManager.Instance.OnError += OnScoresFailed;

        //    SonyUserManager.Instance.onSignedOut += OnError;
        //  SonyUserManager.Instance.onSignInError += OnError;
        // SonyManager.Instance.onConnectionDown += OnError;
    }

    void OnDestroy()
    {
        if (RankingManager.InstanceExists())
        {
            RankingManager.Instance.OnOwnFriendsRankObtanied -= OnScoresObtained;
            RankingManager.Instance.OnOwnGlobalRankObtanied -= OnScoresObtained;
            RankingManager.Instance.OnOwnRankObtanied -= OnPlayerScoreObtained;
            //  RankingManager.Instance.OnError -= OnScoresFailed;
        }

        if (SonyUserManager.InstanceExists())
        {
            //      SonyUserManager.Instance.onSignedOut -= OnError;
            //       SonyUserManager.Instance.onSignInError -= OnError;
        }

        if (SonyManager.InstanceExists())
        {
            // SonyManager.Instance.onConnectionDown -= OnError;
        }
    }

    void OnEnable()
    {
        OnPanelShowing();
        back.enabled = true;
    }

    void OnDisable()
    {
        back.enabled = false;
    }

    public void Setup()
    {
        messageCanvas.alpha = 1;
        boardCanvas.alpha = 0;
        boardCanvas.interactable = false;

        worldLabel.UpdateKey("Wname." + (worldIndex + 1).ToString("00"));

        string key = "LName" + (worldIndex + 1).ToString("00") + "." + (levelIndex + 1).ToString("00");

        if (key == "LName01.07")
            key = "W1Boss.01";
        else if (key == "LName04.07")
            key = "W6LBOSS-28";
        else if (key == "LName06.06")
            key = "W6LBOSS-19";

        levelLabel.UpdateKey(key);

        if (boardType == BoardType.Global)
            boardLabel.UpdateKey(friendsKey);
        else
            boardLabel.UpdateKey(allTimeKey);

        Color worldColor = ColorWorldManager.Instance.getWorldColor(worldIndex + 1);
        foreach (SetColorImage image in images)
            image.UpdateManagedColor(worldColor, colorLerpSpeed);

        StopAllCoroutines();
        StartCoroutine("DelayToRefreshScores");
    }

    #endregion

    #region Refresh Scores

    bool loadingScores;

    IEnumerator DelayToRefreshScores()
    {
        loadingScores = true;

        messageCanvas.alpha = 1;
        boardCanvas.alpha = 0;
        boardCanvas.interactable = false;

        messageLabel.UpdateKey("UI.Leaderboards.Loading");

        RemoveAll();
        rectScroll.ResetScroll();
        EventSystem.current.SetSelectedGameObject(null);
        playerEntry.Clear();
        playerEntry.gameObject.SetActive(false);

        // Check connection status
        if (!SonyManager.Instance.IsConnected)
        {
            loadingScores = false;
            messageLabel.UpdateKey("UI.Leaderboards.NP_ERR_NOT_CONNECTED");
            yield break;
        }

        // Wait for sign in (in case it is connecting)
        if (!SonyUserManager.Instance.IsSignedIn)
        {
            while (SonyUserManager.Instance.IsSigninIn)
            {
                messageLabel.UpdateKey("UI.Leaderboards.LoggingIn");
                yield return null;
            }
        }

        // Check if signed in
        if (!SonyUserManager.Instance.IsSignedIn)
        {
            loadingScores = false;
            messageLabel.UpdateKey("UI.Leaderboards.NP_ERR_NOT_SIGNED_IN");
            yield break;
        }
        // Check if signed in and not underage
        else if (!SonyUserManager.Instance.HasProperAge)
        {
            loadingScores = false;
            messageLabel.UpdateKey("UI.Leaderboards.NP_UNDERAGE");
            yield break;
        }

        // If NP failed, print it here
        /*   ErrorCode npAvailability = SonyRequests.Instance.CheckNpAvailability();
           if (npAvailability != ErrorCode.NP_OK)
           {
               loadingScores = false;
               messageLabel.UpdateKey("UI.Leaderboards." + npAvailability);
               Debug.LogError("NP AVAILABILITY ERROR: " + npAvailability);
               yield break;
           }*/

        messageLabel.UpdateKey("UI.Leaderboards.Loading");
        yield return null;
        RefreshScores();
    }

    void RefreshScores()
    {
        if (!loadingScores) return;
        if (!enabled)
        {
            OnScoresFailed();
            return;
        }

        switch (boardType)
        {
            case BoardType.Friends:
                RankingManager.Instance.RefreshFriendRank(currentBoard);
                break;

            case BoardType.Global:
                // TODO: Need to be able to refresh next entries
                RankingManager.Instance.RefreshRankList(currentBoard, 0, entriesToLoad);
                break;
        }
    }

    void OnScoresObtained(List<RankingManager.ScoreRank> result)
    {
        if (!loadingScores) return;

        if (!enabled)
        {
            OnScoresFailed();
            return;
        }

        AddScores(result);

        if (playerEntry.gameObject.activeSelf)
            StartCoroutine(ScoresLoaded());
        else
            RankingManager.Instance.RefreshOwnRank(currentBoard);
    }

    void OnPlayerScoreObtained(List<RankingManager.ScoreRank> result)
    {
        if (!loadingScores) return;

        if (!enabled)
        {
            StartCoroutine(ScoresLoaded());
            return;
        }

        string sceneName = levelsInfo[worldIndex].levels[levelIndex].sceneName.Split('-')[0];
        float localScore = SaveManager.Instance.dataKlaus.GetTime(sceneName);
        Color worldColor = ColorWorldManager.Instance.getWorldColor(worldIndex + 1);

        for (int i = 0; i != result.Count; ++i)
        {
            if (SonyUserManager.Instance.PlayerID == result[i].playerId && localScore > result[i].score)
            {
                localScore = result[i].score;
                AddEntry(result[i].boardId, result[i].playerId, result[i].position, result[i].score, worldColor);
            }
        }

        if (!playerEntry.gameObject.activeSelf)
        {
            // Si no hay score local, agrega el score del player al final de todo
            if (localScore <= 0)
            {
                AddEntry(currentBoard, SonyUserManager.Instance.PlayerID, 0, localScore, worldColor);
            }
            // Si hay score local, averigua primero en donde tiene que ir
            else
            {
                // Agrega la entrada a la lista
                AddEntry(currentBoard, SonyUserManager.Instance.PlayerID, 0, localScore, worldColor);

                // Busca la posicion a la que pertenece el score
                for (int i = 0; i != currentItems.Count - 1; ++i)
                {
                    if (currentItems[i].time > localScore)
                    {
                        currentItems[currentItems.Count - 1].transform.SetSiblingIndex(i);
                        break;
                    }
                }

                for (int i = 0; i != currentItems.Count; ++i)
                    currentItems[i].SetPosition(currentItems[i].transform.GetSiblingIndex() + 1);
            }
        }

        StartCoroutine(ScoresLoaded());
    }

    void OnScoresFailed()
    {
        if (!loadingScores) return;
        StartCoroutine(ScoresLoaded());
    }

    void AddScores(List<RankingManager.ScoreRank> entries)
    {
        Color worldColor = ColorWorldManager.Instance.getWorldColor(worldIndex + 1);

        for (int i = 0; i != entries.Count; ++i)
            AddEntry(entries[i].boardId, entries[i].playerId, entries[i].position, entries[i].score, worldColor);
    }

    IEnumerator ScoresLoaded()
    {
        if (!loadingScores) yield break;
        loadingScores = false;

        if (SonyUserManager.Instance.HasProperAge)
        {
            if (currentItems.Count == 0)
            {
                messageLabel.UpdateKey("UI.Leaderboards.Empty");
                messageCanvas.alpha = 1;
                boardCanvas.alpha = 0;
                boardCanvas.interactable = false;
            }
            else
            {
                yield return null;
                AdjustContent();
                yield return new WaitForSeconds(0.5f);

                EventSystem.current.SetSelectedGameObject(currentItems.Count != 0 ? currentItems[0].gameObject : null);
                boardCanvas.interactable = true;

                messageCanvas.alpha = 0;
                boardCanvas.alpha = 1;
            }
        }
        else
        {
            OnError();
        }
    }

    void OnError()
    {
        StopAllCoroutines();

        if (enabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        //   ErrorCode npAvailability = SonyRequests.Instance.CheckNpAvailability();

        // If not connected, notify it first
        if (!SonyManager.Instance.IsConnected)
        {
            messageLabel.UpdateKey("UI.Leaderboards.NP_ERR_NOT_CONNECTED");
        }
        // Check now if signed in (normally and to PSN)
        else if (!SonyUserManager.Instance.IsSignedIn)
        {
            messageLabel.UpdateKey("UI.Leaderboards.NP_ERR_NOT_SIGNED_IN");
        }
        // Check if underage
        else if (!SonyUserManager.Instance.HasProperAge)
        {
            messageLabel.UpdateKey("UI.Leaderboards.NP_UNDERAGE");
        }
        // If no returned error, or the error is the generic one, and no NP availability
        /*  else if ((error == ErrorCode.NP_OK || error == ErrorCode.NP_ERR_FAILED) && npAvailability != ErrorCode.NP_OK)
          {
              messageLabel.UpdateKey("UI.Leaderboards." + npAvailability);
              Debug.LogError("NP AVAILABILITY ERROR: " + npAvailability);
          }*/
        // Every other case, fallback here
        else
        {
            messageLabel.UpdateKey("UI.Leaderboards.");
        }

        messageCanvas.alpha = 1;
        boardCanvas.alpha = 0;
        boardCanvas.interactable = false;
        loadingScores = false;
    }

    #endregion

    #region Scores management

    public void AddEntry(int boardId, string playerId, int position, float score, Color color)
    {
        if (boardId != currentBoard)
            return;

        LeaderboardEntry entry = prefab.Spawn<LeaderboardEntry>();
        if (playerId == SonyUserManager.Instance.PlayerID)
        {
            playerEntry.gameObject.SetActive(true);
            entry.clone = playerEntry;
        }

        entry.Setup(playerId, position, score, color);
        Add(entry);
    }

    public override void ClearEntry(LeaderboardEntry entry)
    {
        base.ClearEntry(entry);

        if (entry.playerId == SonyUserManager.Instance.PlayerID)
            playerEntry.gameObject.SetActive(false);

        entry.Clear();
        entry.Recycle<LeaderboardEntry>();
    }

    #endregion

    #region Login

    IEnumerator Login()
    {
        RemoveAll();
        messageCanvas.alpha = 1;
        boardCanvas.alpha = 0;

        if (!SonyUserManager.Instance.IsSignedIn)
        {
            SonyManager.Instance.SignIn();
            messageLabel.UpdateKey("UI.Leaderboards.LoggingIn");

            while (SonyUserManager.Instance.IsSigninIn)
                yield return null;
        }

        Setup();
    }

    #endregion

    #region UI interactions

    Vector2 lastAxisValue;

    void Update()
    {
        bool updateBoard = false;

        // If a message is showing and it doesn't says LOADING...
        if (messageCanvas.alpha != 0)
        {
            if (messageLabel.textObject.text.Contains("<sprite=Square") && ReInput.players.GetPlayer(0).GetButtonDown("Square"))
            {
                StartCoroutine("Login");
            }
        }
        else
        {
            if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputActionOld.Info)))
            {
                updateBoard = true;
                OnChangeType();
            }

            if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputActionOld.NextPage_Left)))
            {
                updateBoard = true;
                OnLevelLeft();
            }
            else if (ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputActionOld.NextPage_Right)))
            {
                updateBoard = true;
                OnLevelRight();
            }

            if (ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.NextPage_Left_2)) >= 0.9f && lastAxisValue.x < 0.9f)
            {
                updateBoard = true;
                OnWorldLeft();
            }
            else if (ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.NextPage_Right_2)) >= 0.9f && lastAxisValue.y < 0.9f)
            {
                updateBoard = true;
                OnWorldRight();
            }
        }

        lastAxisValue.x = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.NextPage_Left_2));
        lastAxisValue.y = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.NextPage_Right_2));

        if (updateBoard)
            Setup();
    }

    public void OnPanelShowing()
    {
        if (automaticWorldLevelSetup)
        {
            worldIndex = 0;
            levelIndex = 0;
            boardType = BoardType.Global;

            // If this level is a game level
            if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
            {
                string sceneName = Application.loadedLevelName;
                if (sceneName.Contains("-"))
                    sceneName = sceneName.Split('-')[0] + "-1";

                for (int i = 0; i != levelsInfo.Length; ++i)
                {
                    for (int j = 0; j != levelsInfo[i].levels.Length; ++j)
                    {
                        if (levelsInfo[i].levels[j].sceneName == sceneName)
                        {
                            worldIndex = i;

                            if (levelsInfo[i].levels[j].hasTimeAttack)
                                levelIndex = j;

                            initialized = false;
                            Setup();
                            return;
                        }
                    }
                }
            }
        }
        else if (!levelsInfo[worldIndex].levels[levelIndex].hasTimeAttack)
        {
            levelIndex = 0;
        }

        initialized = false;
        Setup();
    }

    public void OnWorldLeft()
    {
        worldIndex = (int)Mathf.Repeat((int)worldIndex - 1, levelsInfo.Length);
        levelIndex = 0;
    }

    public void OnWorldRight()
    {
        worldIndex = (int)Mathf.Repeat((int)worldIndex + 1, levelsInfo.Length);
        levelIndex = 0;
    }

    public void OnLevelLeft()
    {
        int lvl = levelIndex;

        do
        {
            lvl = (int)Mathf.Repeat((int)lvl - 1, levelsInfo[worldIndex].levels.Length);
        } while (lvl != levelIndex && !levelsInfo[worldIndex].levels[lvl].hasTimeAttack);


        levelIndex = lvl;
    }

    public void OnLevelRight()
    {
        int lvl = levelIndex;

        do
        {
            lvl = (int)Mathf.Repeat((int)lvl + 1, levelsInfo[worldIndex].levels.Length);
        } while (lvl != levelIndex && !levelsInfo[worldIndex].levels[lvl].hasTimeAttack);


        levelIndex = lvl;
    }

    public void OnChangeType()
    {
        int current = (int)Mathf.Repeat((int)boardType + 1, Enum.GetNames(typeof(BoardType)).Length);
        boardType = (BoardType)Enum.ToObject(typeof(BoardType), current);
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        back.enabled = true;
        back.enabled = true;

        enabled = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        back.enabled = false;
        back.enabled = false;

        enabled = false;
    }

    #endregion
}
