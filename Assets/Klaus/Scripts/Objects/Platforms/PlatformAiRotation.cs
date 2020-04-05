using UnityEngine;
using System.Collections;

public class PlatformAiRotation : MonoBehaviour
{

    public float AngleRotation = 180;
    public float speedRotation = 10.0f;
    public float rotationDir = 0;
    public float TimeToStart = 1.0f;
    float rotationSaveDir = 0;
    public float TimeStop = 2.0f;
    public bool played = false;
    public GameObject platAIStopSFX;
    bool reaching = false;
    Vector3 pivote = Vector3.zero;

    AudioSource _audio = null;
    public AudioSource audio
    {

        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    void Awake()
    {
        if (platAIStopSFX)
            platAIStopSFX.CreatePool(10);
    }

    void Start()
    {
        rotationSaveDir = rotationDir;
        reaching = false;
    }

    void OnEnable()
    {
        if (!reaching)
        {
            StartCoroutine("RotateAgain", TimeToStart);
        }

    }

    void OnDisable()
    {
        StopCoroutine("RotateAgain");
        reaching = false;
        rotationSaveDir = rotationDir;
        rotationDir = 0;
    }
    Vector3 storePivot;
    protected float storeAngle = 0;

    void ResumeRotation()
    {
        storeAngle = 0;
        pivote = transform.up;
        storePivot = pivote;
        reaching = true;

    }

    void FixedUpdate()
    {
        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            if (reaching)
            {
                transform.Rotate(Vector3.forward, rotationDir * speedRotation);

            }
        }
    }
    void LateUpdate()
    {
        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            if (reaching)
            {
                storeAngle += Vector2.Angle(pivote, transform.up);
                pivote = transform.up;
                if (storeAngle >= AngleRotation) {
                    reaching = false;
                    transform.Rotate(Vector3.forward, rotationDir * (AngleRotation - storeAngle));
                    //Fix
                    float angle = Vector2.Angle(storePivot, transform.up);
                    transform.Rotate(Vector3.forward, rotationDir * (AngleRotation - angle));

                    storeAngle = 0;
                    rotationSaveDir = rotationDir;
                    rotationDir = 0;
                    StartCoroutine("PitchDown");
                    if (platAIStopSFX) {

                        platAIStopSFX.Spawn(transform.position, transform.rotation);
                    }
                    StartCoroutine("RotateAgain", TimeStop);
                }
            }
        }

    }

    IEnumerator RotateAgain(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));

        StartCoroutine("PitchUp");
        if (!played)
        {
            audio.Play();
            played = true;
        }
        rotationDir = rotationSaveDir;
        ResumeRotation();
    }

    IEnumerator PitchUp()
    {
        for (float f = 0.7f; f <= 1; f += 0.03f)
        {
            audio.pitch = f;
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.001f));
        }
    }

    IEnumerator PitchDown()
    {
        for (float g = 1f; g >= 0.6f; g -= 0.045f)
        {//baja pitch
            audio.pitch = g;
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.001f));

        }
        audio.Stop();
        played = false;

    }
}
