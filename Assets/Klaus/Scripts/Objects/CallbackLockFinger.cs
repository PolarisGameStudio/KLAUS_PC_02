using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CallbackLockFinger : MonoBehaviour
{

    public PressFingerGesture pressCallback;
    public Collider2D collide;
    public GameObject TriggerGhost;
    public float TimeToStartOpen = 0.5f;
    public float TimeToClose = 1.0f;
    public float TimeToOpen = 1.5f;

    Tweener twen;
    bool firstRun = true;

    // Use this for initialization
    void Awake()
    {
        pressCallback.callbackOpen += Open;
        pressCallback.callbackClose += Close;

    }
    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        ManagerStop.SubscribeOnStopGame(OnStopGame);
        firstRun = false;


    }
    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
            ManagerStop.SubscribeOnStopGame(OnStopGame);

        }
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
        ManagerStop.UnSubscribeOnStopGame(OnStopGame);

    }

    // Update is called once per frame
    protected virtual void Open()
    {
        if (twen != null)
            twen.Pause();

        StopCoroutine("ActiveKillGhost");
        TriggerGhost.SetActive(false);

        StopCoroutine("OpenScale");
        StartCoroutine("OpenScale");
    }
    IEnumerator OpenScale()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToStartOpen));
        twen = transform.DOScaleY(0, TimeToOpen);
    }
    IEnumerator ActiveKillGhost(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        TriggerGhost.SetActive(true);
    }
    protected virtual void Close()
    {
        if (twen != null)
            twen.Pause();
        twen = transform.DOScaleY(1, TimeToClose);
        StopCoroutine("ActiveKillGhost");
        StartCoroutine("ActiveKillGhost", TimeToClose * 0.25f);
    }
    bool isPause = false;
    bool isStop = false;
    public void OnPauseGame()
    {
        if (twen != null)
            twen.Pause();
        isPause = true;
    }
    public void OnStopGame(bool stop)
    {
        isStop = stop;
        if (stop)
        {
            if (twen != null)
            {
                if (twen.IsPlaying())
                {
                    twen.Pause();
                }
            }
        }
        else
        {
            if (twen != null)
            {
                if (!isPause)
                {
                    twen.Play();
                }
            }
        }

    }
    public void OnResumeGame()
    {
        if (twen != null && !isStop)
            twen.Play();

        isPause = false;
    }

}
