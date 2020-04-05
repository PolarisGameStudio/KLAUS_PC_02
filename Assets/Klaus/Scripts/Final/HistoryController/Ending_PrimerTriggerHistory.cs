using UnityEngine;
using System.Collections;

public class Ending_PrimerTriggerHistory : TriggerHistory
{
    public string NextSceneHistory = "";
    public string NextSceneArcadeMode = "";

    public EnterDoorManager door;
    protected override void Start()
    {


    }

    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);

        float timer = door.StopTimerAndReturn();
        SaveManager.Instance.AddPlayTime(timer);
        SaveManager.Instance.dataKlaus.CompleteMainStory = true;
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
        false, timer, 0, false);

        LoadLevelManager.Instance.LoadLevel(SaveManager.Instance.comingFromTimeArcadeMode ? NextSceneArcadeMode : NextSceneHistory, false);
    }
}
