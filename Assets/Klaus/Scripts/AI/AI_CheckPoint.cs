using UnityEngine;
using System.Collections;

public class AI_CheckPoint : MonoBehaviour
{
    public RotateObject rotate;
    public float timeRotating = 1f;

    void Start()
    {
        
    }

    public void RotateArrow()
    {
        StopCoroutine("DontRotateArrow");
        rotate.enabled = true;

        StartCoroutine("DontRotateArrow", timeRotating);
    }

    IEnumerator DontRotateArrow(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        rotate.enabled = false;

    }
}
