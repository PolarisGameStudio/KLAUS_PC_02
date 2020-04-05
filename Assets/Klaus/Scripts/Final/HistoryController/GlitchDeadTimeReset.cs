using UnityEngine;
using System.Collections;

public class GlitchDeadTimeReset : MonoBehaviour
{

    public float DeadTime
    {
        set
        {
            if (value >= 0)
            {
                StopCoroutine("DestroyThis");
                StartCoroutine("DestroyThis", value);
            }
        }
    }

    IEnumerator DestroyThis(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        this.Recycle();
    }
}
