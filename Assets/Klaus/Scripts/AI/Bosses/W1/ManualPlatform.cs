using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ManualPlatform : MonoBehaviour
{
    public Transform spot1, spot2;
    public float upSpeed, downSpeed;
    public float downDelay;
    public Collider2D collider;

    void OnEnable()
    {
        ResetPlatform();
    }

    public Animator[] animators
    {
        get
        {
            if (_animators == null)
                _animators = GetComponentsInChildren<Animator>();
            return _animators;
        }
    }

    Animator[] _animators;

    public void ToggleAnimations(bool on)
    {
        foreach (Animator animator in animators)
            animator.SetBool("isCPU", !on);
    }

    public IEnumerator Shake(float duration, float strength, int vibrato)
    {
        transform.DOShakePosition(duration, Vector3.up * strength, vibrato, 0);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(duration));
    }

    public IEnumerator MoveUp()
    {
        ToggleAnimations(true);

        while (transform.localPosition != spot1.localPosition)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, spot1.localPosition, upSpeed * Time.deltaTime);
                collider.enabled = transform.localPosition.y > -2.22f;
            }

            yield return null;
        }

        ToggleAnimations(false);
    }

    public IEnumerator MoveDown()
    {
        ToggleAnimations(true);

        while (transform.localPosition != spot2.localPosition)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, spot2.localPosition, downSpeed * Time.deltaTime);
                collider.enabled = transform.localPosition.y > -2.22f;
            }

            yield return null;
        }

        if (downDelay != 0)
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(downDelay));

        ToggleAnimations(false);
    }

    public void ResetPlatform()
    {
        StopAllCoroutines();

        transform.localPosition = spot1.localPosition;
        collider.enabled = transform.localPosition.y > -2.22f;

        foreach (Animator animator in animators)
            animator.SetBool("isCPU", true);
    }
}
