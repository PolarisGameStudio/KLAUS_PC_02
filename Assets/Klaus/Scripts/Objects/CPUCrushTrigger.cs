using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlatformHack))]

public class CPUCrushTrigger : BlockCrush
{
    PlatformHack hack;
    public GameObject Remain;
    public GameObject Simbolo;
    public float TimeWatchingDestroy = 1.0f;
    protected void Awake()
    {
        hack = GetComponent<PlatformHack>();
    }

    protected override void StartDestroy()
    {
        OffSimbol();
        base.StartDestroy();
        StopCoroutine("WatchDestroy");
        StartCoroutine("WatchDestroy", TimeWatchingDestroy);
    }

    IEnumerator WatchDestroy(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        hack.HackedSystem();
        hack.WarningDestroy += BaseDestroy;
    }

    public void OffSimbol()
    {
        Simbolo.SetActive(false);
    }

    protected override void DestroyBox()
    {
        if (Remain != null)
        {
            Instantiate(Remain, transform.position, transform.rotation);
        }
    }

    public void BaseDestroy()
    {
        base.DestroyBox();
    }
}
