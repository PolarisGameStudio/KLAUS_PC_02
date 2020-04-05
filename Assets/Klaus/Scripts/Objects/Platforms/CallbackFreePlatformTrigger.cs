using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CallbackFreePlatformTrigger : MonoBehaviour
{

    public PressPlatformRespawn pressCallback;
    public Transform posToRespawn;
    public Collider2D freePlatform;

    public Collider2D collide;
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
        Physics2D.IgnoreCollision(freePlatform, collide);
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

    // Update is called once per frame
    protected virtual void Open()
    {
        if (twen != null)
            twen.Pause();

        StopCoroutine("OpenScale");
        StartCoroutine("OpenScale");
    }

    IEnumerator OpenScale()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToStartOpen));
        twen = transform.DOScaleY(0, TimeToOpen);

        freePlatform.transform.position = posToRespawn.position;
    }

    protected virtual void Close()
    {
        if (twen != null)
            twen.Pause();
        twen = transform.DOScaleY(1, TimeToClose);
    }

    public void OnPauseGame()
    {
        if (twen != null)
            twen.Pause();
    }

    public void OnResumeGame()
    {
        if (twen != null)
            twen.Play();
    }
}
