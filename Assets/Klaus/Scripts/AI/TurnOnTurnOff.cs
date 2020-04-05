using UnityEngine;
using System.Collections;

public class TurnOnTurnOff : MonoBehaviour
{

    public Behaviour[] script;
    public Renderer[] renderes;
    public ParticleSystem[] particles;

    public Renderer[] renderesRever;


    public float TimeTurnOn = 1.0f;
    public float TimeTurnOff = 1.0f;

    public bool StartOn = true;
    bool helperTurn = false;

    public AudioClip vent;
    public AudioClip electric;
    public AudioSource ventA;
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
    void TurnOnOff(bool value)
    {
        for (int i = 0; i < script.Length; ++i)
            script[i].enabled = value;
        for (int i = 0; i < renderes.Length; ++i)
            renderes[i].enabled = value;
        for (int i = 0; i < particles.Length; ++i)
        {
            if (value)
            {
                particles[i].Play();
            }
            else
            {
                particles[i].Pause();
            }
        }
        for (int i = 0; i < renderesRever.Length; ++i)
            renderesRever[i].enabled = !value;
    }

    void StartTurn()
    {
        if (helperTurn)
        {
            StartCoroutine("Turn", TimeTurnOn);
            if (audio != null)
            {
                audio.clip = vent;
				audio.volume = 1;
                audio.Play();

            }
            if (ventA)
                ventA.enabled = true;

        }
        else
        {
            StartCoroutine("Turn", TimeTurnOff);
            if (audio != null)
            {
                audio.clip = electric;
				audio.volume = 0.5f;
                audio.Play();
            }
            if (ventA)
                ventA.enabled = false;

        }
    }

    void OnEnable()
    {
        StopCoroutine("Turn");

        if (StartOn)
        {
            TurnOnOff(true);
        }
        else
        {
            TurnOnOff(false);

        }

        helperTurn = StartOn;
        StartTurn();

    }

    void OnDisable()
    {
        StopCoroutine("Turn");

        TurnOnOff(false);

    }

    IEnumerator Turn(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));

        for (int i = 0; i < script.Length; ++i)
            script[i].enabled = !script[i].enabled;
        for (int i = 0; i < renderes.Length; ++i)
            renderes[i].enabled = !renderes[i].enabled;
        for (int i = 0; i < particles.Length; ++i)
        {

            if (particles[i].isPaused)
            {
                particles[i].Play();
            }
            else
            {
                particles[i].Pause();
            }
        }
        for (int i = 0; i < renderesRever.Length; ++i)
            renderesRever[i].enabled = !renderesRever[i].enabled;
        helperTurn = !helperTurn;
        StartTurn();
    }

}
