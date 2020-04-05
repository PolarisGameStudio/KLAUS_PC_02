using UnityEngine;
using System.Collections;

public class TimesParticles : MonoBehaviour
{

    public ParticleSystem Psystem;
    public float TimeToStart = 1.0f;
    public float TimeOn = 2.0f;
    public float TimeOff = 1.0f;
    public float speed;
    public GameObject acidDeathSFX;

    bool firstRun = true;
    public bool useAcid = true;
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
        StartCoroutine("StartParticle", TimeToStart);
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    public void OnPauseGame()
    {
        if (Psystem.isPlaying)
        {
            Psystem.Pause();
        }
    }

    public void OnResumeGame()
    {
        if (Psystem.isPaused)
        {
            Psystem.Play();
        }
    }

    IEnumerator StartParticle(float time)
    {
        StopCoroutine("ParticleSwitch");
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        Psystem.Play();
        StartCoroutine("ParticleSwitch", TimeOn);
    }

    IEnumerator ParticleSwitch(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        if (Psystem.isPlaying)
        {
            Psystem.Stop();
            StartCoroutine("ParticleSwitch", TimeOff);
            GetComponent<AudioSource>().volume = 0;
             //   StartCoroutine("VolDown", TimeToStart);
        }
        else
        {
            Psystem.Play();
            StartCoroutine("ParticleSwitch", TimeOn);
            GetComponent<AudioSource>().volume = 1;
            //StartCoroutine("VolDown", TimeToStart);

            //  StartCoroutine("VolUp", TimeToStart);
        }
    }
    IEnumerator VolDown()
    {
        for (float g = 1; g >= 0f; g -= speed * Time.deltaTime)
        {
            GetComponent<AudioSource>().volume = g;
            yield return null;

        }

    }

    IEnumerator VolUp()
    {
        for (float g = 0; g <= 1f; g -= speed * Time.deltaTime)
        {
            GetComponent<AudioSource>().volume = g;
            yield return null;

        }

    }

    public void OnParticleCollision(GameObject other)
    {
        if (!ManagerPause.Pause)
        {
            if (other.transform.parent == null)
            {
                if (other.transform.CompareTag("Player"))
                {
                    if (useAcid)
                    {
                        other.transform.GetComponent<DeadState>().typeOfDead = DeadType.Acid;
                        other.transform.GetComponent<MoveState>().Kill(acidDeathSFX);
                    }

                }
            }
            else
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    if (useAcid)
                    { 
                        other.transform.parent.GetComponent<DeadState>().typeOfDead = DeadType.Acid;
                        other.transform.parent.GetComponent<MoveState>().Kill(acidDeathSFX);
                    }
                }
            }
        }
    }



}
