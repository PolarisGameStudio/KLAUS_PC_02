using UnityEngine;
using System.Collections;

public class DemoGDC : Singleton<DemoGDC>
{


    public InputController KlausController;
    public Animator KlausAnim;

    public float TimeToWakeUp = 1.5f;
    public float TimeToMove = 1.5f;

    public TweenTextShow InitialText;
    public float TimeToShowText = 1.0f;
    public float TimeToReachZoom = 40.0f;
    public float StartZoom = 3;
    public float Zoom = 4;
    public GameObject Musik;
    public float TimeToPlayMusik = 15f;
    public AudioLowPassFilter wakeEffect;
    public float TimeToOpenFilter = 6f;
    public float speedFilter = 100f;
    public float speedVol = 0.1f;

    void Start()
    {
        CameraZoom.Instance.SetTargetSizeIndme(StartZoom);
        CameraFollow.Instance.SetTarget(KlausController.transform);
        KlausAnim.SetBool("isDead", true);
        KlausAnim.SetBool("firstIsDead", true);

        /////

        KlausController.GetComponent<MoveStateKlaus>().Stop();
        KlausController.enabled = false;
        CharacterManager.Instance.enabled = false;
        Camera.main.GetComponent<CameraMovement>().enabled = false;
        StartCoroutine("DontChangueForAnim");

        CameraZoom.Instance.SetTargetSize(Zoom, TimeToReachZoom);
        CameraFollow.Instance.SetTarget(KlausController.transform);
        /////
        StartCoroutine("StartDead", TimeToWakeUp);

        Musik.GetComponent<AudioSource>().PlayDelayed(TimeToPlayMusik);
        Invoke("OpenFilt", TimeToOpenFilter);
        StartCoroutine("VolDown");

    }

    public IEnumerator StartDead(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));

        KlausAnim.SetBool("WakeUp", true);
        KlausAnim.SetBool("isDead", false);
        StartCoroutine("ShowText", TimeToShowText);

        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToMove));
	
        KlausController.enabled = true;
        CharacterManager.Instance.enabled = true;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
        DynamicCameraManager.Instance.RemoveEspecialTargetForKlaus();
        Camera.main.GetComponent<AudioListener>().enabled = false;

    }

    IEnumerator ShowText(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        InitialText.InitText();
        KlausAnim.SetBool("WakeUp", false);
    }

    IEnumerator DontChangueForAnim()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.1f));
        KlausAnim.SetBool("firstIsDead", false);

    }

    void OpenFilt()
    {
        StartCoroutine("OpenFilter");
    }

    IEnumerator OpenFilter()
    {
        while (wakeEffect.cutoffFrequency < 11000f)
        {
            wakeEffect.cutoffFrequency += speedFilter * Time.deltaTime;
            yield return null;
        }
        wakeEffect.cutoffFrequency = 22000f;
    }

    IEnumerator VolDown()
    {
        GameObject musikIntro = GameObject.Find("AS_MusikIntro");
        if (musikIntro != null)
        {
            AudioSource musikIntroSRC = musikIntro.GetComponent<AudioSource>();
            while (musikIntroSRC.GetComponent<AudioSource>().volume > 0)
            {
                musikIntroSRC.GetComponent<AudioSource>().volume -= speedVol * Time.deltaTime;
                yield return null;
            }
        }
    }
	
}
