using UnityEngine;
using System.Collections;

public class AS_GlitchMusik : MonoBehaviour
{

    public string[] tracks;
    AudioClip[] tracksClips;
    public int[] rndDistribution;
    public int nGlitch;
    public float tGlitch;
    public float Timer;
    public float tMax;
    public float tMin;
    public float getTime;
    private int pitchBend;
    private float panTimer;
    private float panTimerL;
    private float panTimerR;
    private bool panLeft;
    private bool sube;
    public bool pitchBendF;
    public bool paning;
    public bool LPF;
    public bool echoF;
    public bool repeat;
    private bool gotTime;
    private int nRepeat;
    public bool change;
    private float changeT;
    public bool changeB;
    private int saveNGlitch;
    protected int currentPos = -1;
    protected int lastPos = -1;
    AudioSource _audio;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    AudioLowPassFilter _lowPass;

    public AudioLowPassFilter lowPas
    {
        get
        {
            if (_lowPass == null)
                _lowPass = GetComponent<AudioLowPassFilter>();
            return _lowPass;
        }
    }

    AudioEchoFilter _echoPass;

    public AudioEchoFilter echoPass
    {
        get
        {
            if (_echoPass == null)
                _echoPass = GetComponent<AudioEchoFilter>();
            return _echoPass;
        }
    }

    protected int nextPos = -1;

    AudioClip LoadClip()
    {
        if (nextPos == -1)
        {
            currentPos = Random.Range(0, tracks.Length);
            nextPos = Random.Range(0, tracks.Length);
        }
        else
        {
            currentPos = nextPos;
            nextPos = Random.Range(0, tracks.Length);
        }
        AudioClip currentAudio = LoadClip(currentPos);
        //  StartCoroutine("LoadClipAsync", nextPos);
        return currentAudio;

    }

    IEnumerator LoadClipAsync(int pos)
    {
        if (tracksClips[pos] == null)
        {
            ResourceRequest request = Resources.LoadAsync<AudioClip>(tracks[pos]);
            yield return request;
            tracksClips[pos] = request.asset as AudioClip;

        }
    }

    AudioClip LoadClip(int pos)
    {
        //audio.Stop();
        // Resources.UnloadAsset(audio.clip);
        if (tracksClips[pos] == null)
        {
            tracksClips[pos] = Resources.Load<AudioClip>(tracks[pos]);
        }
        return tracksClips[pos];

    }

    // Use this for initialization
    void Start()
    {
        if (tracksClips == null)
            tracksClips = new AudioClip[tracks.Length];//Fix para probar
        //Selecciona el track randomly
        audio.clip = LoadClip();
        for (int i = 0; i < tracks.Length; ++i)
        {
            if (i != currentPos)
                StartCoroutine(LoadClipAsync(i));

        }
        //Reproduce el track graciosamente
        audio.timeSamples = audio.clip.samples - 1;
        audio.pitch = -1;
        audio.Play();
        //Seteo el tiempo para el proximo efecto
        tGlitch = Random.Range(tMax, tMin);
        //Doy el valor del primer caso
        Invoke("Rnd", 10);
    }

    void Rnd()
    {

        nGlitch = rndDistribution[Random.Range(0, rndDistribution.Length)];
        if (saveNGlitch == 5 || saveNGlitch == 3 || saveNGlitch == 7)
        {
            nGlitch = rndDistribution[Random.Range(0, rndDistribution.Length)];
        }
        Invoke("Glitches", tGlitch);
    }

    void Glitches()
    {

        switch (nGlitch)
        {
            case 1://PitchBend
                pitchBend = 1;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                Timer = 0;
                saveNGlitch = nGlitch;
                break;
            case 2://PitchBend doble
                pitchBend = 2;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                Timer = 1;
                saveNGlitch = nGlitch;
                break;
            case 3://Pa lante
                pitchBendF = true;
                sube = true;
                Timer = 15;
                tGlitch = 20;
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 4://Pan2D
                paning = true;
                panLeft = false;
                Timer = 5;
                panTimerL = 2;
                panTimerR = 2;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 5://LowPassFilter
                Timer = 5;
                LPF = true;
                tGlitch = Random.Range(tMax, Timer);
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 6://EchoFilter
                Timer = 2;
                echoF = true;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 7://Repeat
                repeat = true;
                gotTime = false;
                change = false;
                changeB = false;
                Timer = 0.5f;
                nRepeat = Random.Range(2, 5);
                ;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 8://ChangeTrack
                change = true;
                repeat = true;
                changeB = false;
                gotTime = false;
                Timer = 0.5f;
                nRepeat = 4;
                tGlitch = Random.Range(tMax, tMin);
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
            case 9://ChangeTrackNBack
                change = true;
                repeat = true;
                gotTime = false;
                changeB = true;
                changeT = 3;
                Timer = 0.5f;
                nRepeat = 2;
                tGlitch = Random.Range(tMax, (tMin + changeT));
                Invoke("Rnd", 0);
                saveNGlitch = nGlitch;
                break;
        }
    }

    void LateUpdate()
    {

        //PITCH BEND
        if (pitchBend > 0 && !pitchBendF)
        {

            Timer -= 0.1f;

            if (Timer <= 0)
            {
                audio.pitch += 0.04f;
                if (audio.pitch >= -0.5)
                {
                    audio.pitch = -1f;
                    pitchBend -= 1;//Para hacerlo doble	
                    Timer = 1;
                }
            }
        }

        //PALANTE
        if (pitchBendF)
        {
            if (sube)
            {
                audio.pitch += 0.1f;
            }
            if (audio.pitch >= 1)
            {
                sube = false;
                audio.pitch = 1;
                Timer -= Time.deltaTime;
            }
            if (Timer <= 0)
            {
                audio.pitch -= 0.1f;
                if (audio.pitch <= -1)
                {
                    audio.pitch = -1;
                    pitchBendF = false;
                }
            }
        }
        //PANING
        if (paning && Timer > 0)
        {
            Timer -= 0.1f;
            if (panLeft == true)
            {
                panTimerL -= 0.9f;
                audio.panStereo = -1;
                if (panTimerL < 0)
                {
                    panLeft = false;
                    panTimerR = 2;
                }
            }
            if (panLeft == false)
            {
                panTimerR -= 0.9f;
                audio.panStereo = 1;
                if (panTimerR < 0)
                {
                    panLeft = true;
                    panTimerL = 2;
                }
            }
        }
        else
        {
            paning = false;
            audio.panStereo = 0;
        }

        //LOW PASS FILTER
        if (LPF && Timer > 0)
        {
            Timer -= Time.deltaTime;
            lowPas.cutoffFrequency = 700;


            if (Timer <= 0)
            {
                lowPas.cutoffFrequency = 22000;
                LPF = false;
            }
        }
        //ECHO FILTER
        if (echoF && Timer > 0)
        {
            Timer -= Time.deltaTime;
            echoPass.wetMix = 1;
            echoPass.dryMix = 0;
        }
        else
        {
            echoPass.wetMix = 0;
            echoPass.dryMix = 1;
            echoF = false;
        }

        //BEAT REPEAT
        if (repeat)
        {
            Timer -= Time.deltaTime;
            if (!gotTime)
            {
                //getTime = audio.time;
                getTime = Random.Range(10f, 60f);
                gotTime = true;
            }


            if (Timer <= 0 && nRepeat > 0)
            {
                audio.time = getTime;
                //audio.time =0;
                audio.Play();
                Timer = 0.5f;
                nRepeat -= 1;

                if (change && nRepeat == 1)//BEAT REPEAT + CHANGE TRACK
                {
                    lastPos = currentPos;
                    audio.clip = LoadClip();

                    if (currentPos == lastPos)
                    {
                        currentPos++;
                        currentPos = currentPos % tracks.Length;
                    }
                    audio.time = getTime;
                    //audio.time =0;
                    audio.Play();
                    change = false;

                }
                if (nRepeat == 0 && !changeB)
                {
                    repeat = false;
                    gotTime = false;
                }
            }
            if (changeB && changeT <= 0)// CHANGE AND BACK
            {
                audio.clip = LoadClip(lastPos);
                currentPos = lastPos;
                nRepeat = 1;
                changeB = false;
            }
            if (!change && changeT > 0)//Timer to change back
            {
                changeT -= Time.deltaTime;
            }
        }
    }
}
