using UnityEngine;
using System.Collections;
using System;

public class CollectableUI : MonoBehaviour
{
    public float timeChange = 0.3f;
    public Animator animCollect;
    public Action onFinish;

    public void SetCollect(int number)
    {
        animCollect.SetInteger("Collect", number);
		GetComponent<AudioSource>().PlayDelayed (8f*Time.deltaTime);
        StartCoroutine(ShowNewDoor());
    }

    IEnumerator ShowNewDoor()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timeChange));
        animCollect.SetTrigger("Update");
    }

    public void OnFinishTransition()
    {
        if (onFinish != null)
            onFinish();
    }
}
