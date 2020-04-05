using UnityEngine;
using System.Collections;

public class KeyGotBehaviour : StateMachineBehaviour {

    public KeyTrigger keyT;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        keyT.Got();
    }
}
