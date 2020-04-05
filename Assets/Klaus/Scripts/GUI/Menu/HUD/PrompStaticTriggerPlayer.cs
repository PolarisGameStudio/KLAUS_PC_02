using UnityEngine;
using System.Collections;

public class PrompStaticTriggerPlayer : PrompTriggerPlayer {

    protected override void OnEnterAction(Collider2D other) {
        HUD_Message.Instance.ShowWithOutTime(TextToShow);
    }

    void OnTriggerExit2D(Collider2D other) {

        if (CompareDefinition(other)) {
            HUD_Message.Instance.HideText();
        }
    }
}
