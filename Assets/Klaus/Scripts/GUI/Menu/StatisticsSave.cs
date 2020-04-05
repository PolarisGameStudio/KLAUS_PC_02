using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatisticsSave : MonoBehaviour
{
    public Text timePlayedText;
    public Text storyText;
    public Text deathsText;
    public Text piecesText;
    public Text memoriesCompletedText;
    public Text timeAttackText;
    public Text progressText;

    void OnEnable()
    {
        SaveManager.onGameLoaded += OnGameLoaded;
        OnGameLoaded();
    }

    void OnDisable()
    {
        SaveManager.onGameLoaded -= OnGameLoaded;
    }

    void OnGameLoaded()
    {
        // Get total time played
        timePlayedText.text = HUD_TimeAttack.FormatTime(SaveManager.Instance.dataKlaus.totalTimePlayed);

        // Get story progress
        float storyProgress = SaveManager.Instance.GetHistoryPercent();
        storyText.text = Mathf.FloorToInt(storyProgress) + "%";

        // Get total deaths
        deathsText.text = SaveManager.Instance.dataKlaus.deaths.ToString();

        // Get total memories
        int currentPieces = 0;
        int totalPieces = 0;
        for (int i = 0; i != SaveManager.Instance.dataKlaus.collectablesItems.Count; ++i)
        {
            totalPieces += SaveManager.Instance.dataKlaus.collectablesItems[i].item.Count;
            currentPieces += SaveManager.Instance.dataKlaus.collectablesItems[i].item.FindAll(x => x).Count;
        }

        piecesText.text = currentPieces.ToString("00") + "/" + totalPieces.ToString("00");

        // Get memories completed
        int memoriesCompleted = 0;
        int totalMemories = 6;

        for (int i = 1; i != 7; ++i)
            memoriesCompleted += CollectablesManager.isCollectableFull("W" + i) ? 1 : 0;

        memoriesCompletedText.text = memoriesCompleted.ToString() + "/" + totalMemories.ToString();

        // Get time attack progress
        float timeAttackProgress = 100f * (float)SaveManager.Instance.dataKlaus.GetBrokenCompanyRecords() / (float)SaveManager.Instance.dataKlaus.GetTotalCompanyRecords();
        timeAttackText.text = Mathf.FloorToInt(timeAttackProgress) + "%";

        // Get overall progress
        progressText.text = Mathf.FloorToInt(storyProgress * 0.5f + timeAttackProgress * 0.25f + 25f * (float)currentPieces / (float)totalPieces) + "%";
    }
}
