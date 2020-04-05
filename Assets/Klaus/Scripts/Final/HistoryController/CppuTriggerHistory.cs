using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CPUTrigger))]
public class CppuTriggerHistory : MonoBehaviour
{
    public TextTrigger texTrigg;

    protected CPUTrigger _cpu;

    public CPUTrigger cpu
    {
        get
        {
            if (_cpu == null)
                _cpu = GetComponent<CPUTrigger>();
            return _cpu;
        }
    }

    public float TimeStopping = 0.5f;
    public float timeToStart = 2.0f;
    public int ShakePreset = 1;
    // Use this for initialization
    void OnEnable()
    {
        cpu.onFinishHack += Finish;
    }

    void OnDisable()
    {
        if (cpu != null)
            cpu.onFinishHack -= Finish;
    }
    // Update is called once per frame
    public void Finish()
    {
        StartCoroutine("WaitingAll", timeToStart);

    }

    IEnumerator WaitingAll(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        ManagerStop.Instance.StopAll(TimeStopping);
        RumbleManager.Instance.VibrateByTime(TimeStopping);
        if (CameraShake.Instance != null)
            CameraShake.Instance.StartShakeBy(TimeStopping, ShakePreset);

        //Sonido
        texTrigg.OnEnterAction();
    }
}
