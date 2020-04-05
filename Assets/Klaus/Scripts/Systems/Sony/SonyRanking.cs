using System;

public class SonyRanking
{
    ulong currentScore = (ulong)0 << 32;
    int rankBoardID = 0;

    public SonyRanking()
    {
        Initialize();
    }

    public void Initialize()
    {

    }



    public void RegisterScore(ulong currentScore)
    {
    }

    public void RegisterScoreAndData(ulong score, byte[] additionaldata)
    {

    }

    public void RefreshOwnRank()
    {

    }

    public void RefreshFriendRank()
    {

    }

    public void RefreshRankList(int rankRangeStart, int rankRangeCount)
    {
    }

    void OnSomeEvent()
    {
    }

    void OnRegisteredNewBestScore()
    {
    }

    void LogRank()
    {
    }

    void OnRankingGotOwnRank()
    {
    }

    void OnRankingGotFriendRank()
    {
    }

    int LastRankDisplayed = 0;
    int LastRankingMaxCount = 999;

    void OnRankingGotRankList()
    {
        if (LastRankDisplayed >= LastRankingMaxCount) { LastRankDisplayed = 0; }
    }

    void OnRankingError()
    {
    }
}
