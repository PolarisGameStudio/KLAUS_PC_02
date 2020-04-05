using UnityEngine;
using System.Collections;
using System;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class PressFingerGesture : Input3DSelection
{

    float timeToUnlock = 1.0f;
    float timeToHide = .5f;

    bool isPressing = false;
    bool isWaitPress = false;
    public Action callbackOpen;
    public Action callbackClose;
    public Animator Mira;
    public AudioSource audio1;
    public SpriteRenderer MiraSpriteRenderer;

    bool touching = false;
    bool canTouch = true;

    public bool StartOpen = false;


    public override Vector3 Center
    {
        get
        {
            return MiraSpriteRenderer.transform.position;
        }
    }

    IEnumerator Start()
    {
        MiraSpriteRenderer.enabled = false;
        yield return null;
        if (StartOpen)
        {
            callbackOpen();
            isPressing = true;
        }
    }

    public override void SelectByInput()
    {
        if (!ManagerPause.Pause)
        {
            touching = true;
            if (!isWaitPress && canTouch)
            {
                canTouch = false;
                if (isPressing)
                {
                    callbackClose();
                }
                else
                {
                    callbackOpen();
                }
                Mira.SetTrigger("Start");
                MiraSpriteRenderer.enabled = true;
#if UNITY_PS4 && !(UNITY_EDITOR)
				
				audio1.PlayOnDualShock4(PS4Input.PadGetUsersDetails(0).userId);
#endif
                audio1.Play();
                isWaitPress = true;
                StartCoroutine(Pressing(timeToUnlock));
                isPressing = !isPressing;
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
        Mira.SetTrigger("Finish");
        yield return StartCoroutine(new TimeCallBacks().WaitPause(timeToHide));
        MiraSpriteRenderer.enabled = false;
        isWaitPress = false;
    }
}
