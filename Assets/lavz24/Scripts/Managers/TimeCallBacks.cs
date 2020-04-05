using UnityEngine;
using System.Collections;


public class TimeCallBacks : YieldInstruction
{

    public IEnumerator WaitForSecondsPauseStop(float duration)
    {
        while (duration > 0) //check time and listen for keypress
        {
            if (!ManagerStop.Stop && !ManagerPause.Pause)
            {
                duration -= Time.deltaTime; //deduce time passed this frame.
            }

            yield return null; //yield for one(1) frame.
        }
    }

    public IEnumerator WaitPause(float duration)
    {

        while (duration > 0) //check time and listen for keypress
        {
            if ( !ManagerPause.Pause)
            {
                duration -= Time.deltaTime; //deduce time passed this frame.
            }

            yield return null; //yield for one(1) frame.
        }
    }
}
