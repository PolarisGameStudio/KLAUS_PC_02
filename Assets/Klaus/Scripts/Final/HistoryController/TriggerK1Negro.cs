using UnityEngine;
using System.Collections;

public class TriggerK1Negro : TriggerHistory {
    public float TimeStop = 0.3f;
    public PlayerMoveAI AIMove;

    protected override void Start() {
        if (SaveManager.Instance.comingFromTimeArcadeMode) {
            AIMove.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
    }

    protected override void OnEnterAction(Collider2D other) {
        base.OnEnterAction(other);
        CharacterManager.Instance.FreezeAllWithTimer(TimeStop);
        AIMove.enabled = true;

    }
}
