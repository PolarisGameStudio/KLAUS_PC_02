using UnityEngine;
using System.Collections;
using System;

public class PressPlatformRespawn : Input3DSelection
{

    public float timeToUnlock = 1.0f;
    bool isWaitPress = false;
    public Action callbackOpen;
    public Action callbackClose;
    public Animator Mira;
    public SpriteRenderer MiraSpriteRenderer;

    bool touching = false;
    bool canTouch = true;


    public override void SelectByInput()
    {
        if (!ManagerPause.Pause)
        {
            touching = true;
            if (!isWaitPress && canTouch)
            {
                canTouch = false;
                callbackOpen();

                Mira.SetTrigger("Start");
                MiraSpriteRenderer.enabled = true;
                GetComponent<AudioSource>().Play();
                isWaitPress = true;
                StartCoroutine("Pressing", timeToUnlock);
            }
        }
    }

    void LateUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (!touching)
            {
                canTouch = true;
            }
            touching = false;
        }
    }

    IEnumerator Pressing(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        MiraSpriteRenderer.enabled = false;
        isWaitPress = false;
        yield return StartCoroutine(new TimeCallBacks().WaitPause(1.0f));
        callbackClose();
    }
}
