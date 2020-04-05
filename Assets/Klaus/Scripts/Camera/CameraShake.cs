using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class CameraShake : Singleton<CameraShake>
{
    public bool Vibration = true;

    public ProCamera2DShake proCameraShake
    {
        get
        {
            if (_proCameraShake == null)
                _proCameraShake = GetComponent<ProCamera2DShake>();

            return _proCameraShake;
        }
    }


    public void Update()
    {
    }

    ProCamera2DShake _proCameraShake;
    bool firstRun = true;
    public bool isShaking;

    public void StartShakeBy(float time, int preset = -1)
    {
        StopShake();
        StartCoroutine(ShakeWithTime(time, preset));
    }


    public void StartShakeWithDelay(Vector2 time)
    {
        StopShake();
        StartCoroutine("ShakeWithDelay", time);
    }

    public void StartShake(int preset = -1)
    {
        isShaking = true;
        if (preset >= 0)
        {
            proCameraShake.ShakeUsingPreset(preset);
        }
        else
            proCameraShake.Shake();
        RumbleManager.Instance.Vibrate();

    }

    public void StartShakeNoVibration(int preset = -1)
    {
        isShaking = true;
        if (preset >= 0)
        {
            proCameraShake.ShakeUsingPreset(preset);
        }
        else
            proCameraShake.Shake();

    }

    public void StartShakeNoVibrationNonStop()
    {
      
        isShaking = true;
            proCameraShake.Shake();
    }

    public void StopShake()
    {
        isShaking = false;
        StopAllCoroutines();
        proCameraShake.StopShaking();
        RumbleManager.Instance.Stop();
    }


    IEnumerator ShakeWithTime(float time, int preset)
    {
        StartShake(preset);
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        StopShake();
    }

    IEnumerator ShakeWithDelay(Vector2 time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time.y));
        yield return StartCoroutine(ShakeWithTime(time.x, -1));
    }

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
    }

    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    public void OnPauseGame()
    {
        if (isShaking)
        {
            proCameraShake.StopShaking();
            RumbleManager.Instance.Stop();
        }
    }

    public void OnResumeGame()
    {
        if (isShaking && Vibration)
        {
            proCameraShake.Shake();
            RumbleManager.Instance.Vibrate();
        }
    }
}


