using UnityEngine;
using System.Collections;
using DG.Tweening;
public class PlatformRotate : MonoBehaviour, IMovePlatform {

    public float AngleRotation = 45;
    public float speedRotation = 10.0f;
    public float TimeToReachAngleRotation = 1;
    float rotationDir = 0;
    public bool played = false;
    public RotateObject gear;
    public PlatformArrowAnim arrows;

    bool reaching = false;
    Vector3 pivote = Vector3.zero;
    Vector3 storePivot;

    Vector2 MoveStore = Vector2.zero;

    protected float storeAngle = 0;
    AudioSource _audio = null;
    public AudioSource audio {

        get {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    protected Tween twenner;
    /*void FixedUpdate()
    {
        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            if (reaching) {
                transform.Rotate(Vector3.forward, rotationDir * speedRotation);
            }
        }
    }*/
    /*void LateUpdate()
    {
        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            if (reaching)
            {
                storeAngle += Vector2.Angle(pivote, transform.up);
                pivote = transform.up;
                if (storeAngle >= AngleRotation)
                {
                    reaching = false;

           //         transform.Rotate(Vector3.forward, rotationDir * (AngleRotation - storeAngle));
                    //Fix
                    float angle = Vector2.Angle(storePivot, transform.up);
                    transform.Rotate(Vector3.forward, rotationDir * (AngleRotation - angle));


                    storeAngle = 0;
                    rotationDir = 0;
                    StopRotate();
                }
            }
        }
    }*/
    bool firstRun = true;

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

    public void OnPauseGame() {
        if (twenner != null) {
            twenner.Pause();
        }
    }

    public void OnResumeGame() {
        if (twenner != null) {
            twenner.Play();
        }
    }


    void StopRotate() {
        reaching = false;
        rotationDir = 0;
        if (arrows != null)
            arrows.ResetMove();
        gear.enabled = false;
        if (played) {
            played = false;
            audio.Stop();
        }
        twenner = null;
        SetMovement(MoveStore);

    }
    public void SetMovement(Vector2 move) {
        MoveStore = move;
        if (!reaching) {
            rotationDir = 0;
            if (move == Vector2.zero) {
                //  StopRotate();
                return;
            }

            pivote = transform.up;
            storePivot = pivote;
            storeAngle = 0;
            reaching = true;

            float value = Mathf.Atan2(move.y, move.x);
            if (value != 0) {
                value /= Mathf.Abs(value);
                rotationDir = value;

            } else {
                rotationDir = -1;

            }
            if (rotationDir < 0) {
                if (arrows != null)
                    arrows.PositiveMove();
                gear.enabled = true;
                gear.angleMaxSpeed = Mathf.Abs(gear.angleMaxSpeed);
                if (!played) {
                    played = true;
                    audio.Play();
                }
            } else if (rotationDir > 0) {
                if (arrows != null)
                    arrows.NegativeMove();
                gear.enabled = true;
                gear.angleMaxSpeed = -1 * Mathf.Abs(gear.angleMaxSpeed);
                if (!played) {
                    played = true;
                    audio.Play();
                }
            }
            twenner = transform.DORotate(new Vector3(0, 0, rotationDir * AngleRotation + transform.rotation.eulerAngles.z), TimeToReachAngleRotation, RotateMode.Fast)
                .SetEase(Ease.Linear)
                .OnComplete(StopRotate);
        }

    }
}
