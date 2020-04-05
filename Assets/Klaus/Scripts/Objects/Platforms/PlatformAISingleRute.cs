using UnityEngine;

public class PlatformAISingleRute : PlatformAI
{
    public Animator[] animators;

    protected override void Awake()
    {
        base.Awake();
        foreach (Animator animator in animators) animator.SetBool("isCPU", !enabled);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (Animator animator in animators) animator.SetBool("isCPU", !enabled);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (Animator animator in animators) animator.SetBool("isCPU", !enabled);
    }

    protected override void ChangeSpot()
    {
        if (isGoing)
        {
            ++currentPath;
            if (currentPath == Path.Length)
            {
                currentPath = Path.Length - 1;
                isGoing = false;
                enabled = false;
                StopPlatform();
            }
        }
        else
        {
            --currentPath;
            if (currentPath == -1)
            {
                currentPath = 0;
                isGoing = true;
                enabled = false;
                StopPlatform();
            }
        }

        if (enabled)
        {
            if (TimeStop.Length > currentPath)
            {
                StartCoroutine("ResumeMovement", TimeStop[currentPath]);
            }
            else
            {
                StartCoroutine("ResumeMovement", DefaultTimeStop);
            }
        }
    }
}
