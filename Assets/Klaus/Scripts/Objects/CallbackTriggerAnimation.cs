using UnityEngine;
using System.Collections;

public class CallbackTriggerAnimation : MonoBehaviour {

    public PressFingerGesture pressCallback;
    public Animator anim;

    bool firstRun = true;
	public float pitchUp;
	public float pitchDown;
	protected AudioSource _aud;
	
	public AudioSource audio
	{
		get
		{
			if (_aud == null)
				_aud = GetComponent<AudioSource>();
			return _aud;
		}
	}
    // Use this for initialization
    void Awake() {
        pressCallback.callbackOpen += Open;
        pressCallback.callbackClose += Close;
    }
    void Start() {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;


    }
    void OnEnable() {
        if (!firstRun) {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    void OnDisable() {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }
    // Update is called once per frame
    protected virtual void Open()
    {
        anim.SetBool("Open",true);
		audio.pitch = pitchUp;
		audio.PlayDelayed(0.5f);
    }

    protected virtual void Close()
    {
        anim.SetBool("Open", false);
		audio.pitch = pitchDown;
		audio.Play();
    }

    public void OnPauseGame() {
        anim.speed = 0;
    }

    public void OnResumeGame() {
        anim.speed = 1;

    }
}
