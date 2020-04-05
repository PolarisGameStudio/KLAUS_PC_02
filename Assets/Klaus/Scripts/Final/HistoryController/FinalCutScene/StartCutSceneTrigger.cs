using UnityEngine;
using System.Collections;

public class StartCutSceneTrigger : TriggerHistory{

    public Animator realKlausController;
    public string firstVar = "First";
	// Use this for initialization
    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        realKlausController.SetTrigger(firstVar);
    }
}
