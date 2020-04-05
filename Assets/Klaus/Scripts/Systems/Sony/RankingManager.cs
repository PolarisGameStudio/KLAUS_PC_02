using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RankingManager : MonoSingleton<RankingManager>
{
    public struct ScoreRank
    {
        public int boardId;
        public int position;
        public float score;
        public string playerId;
    }

    public bool RefreshOwnRankIsBusy
    {
        get
        {
            return false;

        }
    }

    bool registerScoreIsBusy;

    protected override void Init()
    {
        base.Init();

    }

    #region Public Methods

    public Action<List<ScoreRank>> OnOwnRankObtanied;
    public Action<List<ScoreRank>> OnOwnFriendsRankObtanied;
    public Action<List<ScoreRank>> OnOwnGlobalRankObtanied;
    public Action OnError;
    public Action OnRegisterScoreError;

    public void RegisterScore(int boardID, ulong currentScore, string comment = "")
    {
        if (!SonyUserManager.Instance.HasProperAge)
        {
        }
        else
        {

        }
    }

    public void RefreshOwnRank(int boardID)
    {

    }

    public void RefreshFriendRank(int boardID)
    {

    }

    public void RefreshRankList(int boardID, int rankRangeStart, int rankRangeCount)
    {
    }

    #endregion

    #region Events

    void OnScoreRegistered()
    {
        Debug.Log("Score registered: ");
        if (SonyManager.Instance.logText) SonyManager.Instance.logText.text = "Score registered! Result: ";
    }

    void OnRankingGotOwnRank()
    {

        if (SonyManager.Instance.logText) SonyManager.Instance.logText.text = "Got your rank. Result: " + "Not ranked";

        if (OnOwnRankObtanied != null)
            OnOwnRankObtanied(CreateRanks());
    }

    void OnRankingGotFriendRank()
    {
        if (SonyManager.Instance.logText) SonyManager.Instance.logText.text = "Got your friend's rank. Total entries: ";

        if (OnOwnFriendsRankObtanied != null)
            OnOwnFriendsRankObtanied(CreateRanks());
    }

    void OnRankingGotRankList()
    {

        if (SonyManager.Instance.logText) SonyManager.Instance.logText.text = "Got global rank. Total entries: ";

        if (OnOwnGlobalRankObtanied != null)
            OnOwnGlobalRankObtanied(CreateRanks());
    }

    void OnRankingError()
    {
    }

    #endregion

    #region Utilities

    IEnumerator DummyObtainOwnRank(int boardId)
    {
        yield return new WaitForSeconds(1f);

        if (OnOwnRankObtanied != null)
            OnOwnRankObtanied(new List<ScoreRank>());
    }

    IEnumerator DummyObtainGlobalRank(int boardId, int totalEntries)
    {
        yield return new WaitForSeconds(1f);

        if (OnOwnGlobalRankObtanied != null)
            OnOwnGlobalRankObtanied(CreateDummyRanks(boardId, totalEntries));
    }

    List<ScoreRank> CreateDummyRanks(int boardId, int totalEntries = 10)
    {
        List<ScoreRank> result = new List<ScoreRank>();

        for (int i = 0; i != totalEntries; ++i)
        {
            result.Add(new ScoreRank()
            {
                playerId = "DummyID_" + i,
                boardId = boardId,
                position = i + 1,
                score = (float)i * 1000f
            }
            );
        }

        return result;
    }

    List<ScoreRank> CreateRanks()
    {
        List<ScoreRank> result = new List<ScoreRank>();


        return result;
    }


    #endregion
}
