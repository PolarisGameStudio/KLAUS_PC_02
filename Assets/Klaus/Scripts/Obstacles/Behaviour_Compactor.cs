using UnityEngine;
using System.Collections;

public class Behaviour_Compactor : StateMachineBehaviour
{
    public Compactor_Trigger compactorT;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        compactorT.Down();
    }
}
