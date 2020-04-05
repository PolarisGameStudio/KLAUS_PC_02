using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;

public class AS_MusikManager : MonoSingleton<AS_MusikManager>
{
    public string scene;
    public string sc;
    public string sc2;
    public string sc3;
    public string sc4;
    public string sc5;
    public string scCollectible;
    public string scMemories;

    //public GameObject glitchMusik;
    public string path_glitchMusik;
    // public AudioClip[] tracks;
    public string[] tracks = new string[16];
    public bool played = false;
    public bool firstRun;
    int nextClip = -1;
    public string lastScene;
    public bool destroyMe = false;
    bool resetAll = false;
    public AudioMixer mainMixer;
    public AudioMixer klausMixer;
    public float delayTime;
    [Header("Ducking")]
    public float vol;
    public float attackSpeed;
    public float releaseSpeed;
    public bool isDucking = false;
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

    bool loadingAudio = false;
    protected override void Init()
    {
        DontDestroyOnLoad(this);
        audio.ignoreListenerVolume = true;
        nextClip = -1;

    }

    public void OnLevelWasLoaded(int level)
    {
        resetAll = true;
    }

    void LateUpdate()
    {
        if (resetAll)
        {
            resetAll = false;
            CambiarMusica();
        }
    }

    void CambiarMusica()
    {
        scene = Application.loadedLevelName;
        sc = scene.Substring(0, 2);
        sc2 = scene.Substring(4, 1);
        scCollectible = scene.Substring(2, 1);
        scMemories = scene.Substring(0, 1);

        // Trata de obtener el nivel actual (W_L_0-_)
        int s2Parse = 0;
        int.TryParse(LevelTypeManager.GetLevel(scene), out s2Parse);

        if (scCollectible != "C")
        {
            sc3 = scene.Substring(6, 1);
        }
        //Compruebo que glitch no exista para borrarlo de una
        if (sc != "Lo" && sc != "W5")
        {
            GameObject glitch = GameObject.Find("AS_GlitchMusik");
            if (!object.ReferenceEquals(glitch, null))
            {
                glitch.Recycle();
            }
        }

        switch (sc)
        {
            case "Pr":
                nextClip = -1;
                audio.Stop();
                UnloadAssetClip();
                audio.clip = null;
                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                //mainMixer.FindSnapshot("LevelCompleted").TransitionTo(2*Time.deltaTime);
                //LoadClip(12);
                /*if(nextClip != tracks[12]){
                    nextClip = tracks[12];
                    GetComponent<AudioSource>().clip = nextClip;
                    GetComponent<AudioSource>().PlayDelayed (2f);
                }*/
                break;

            case "Co"://Memories
                sc4 = scene.Substring(8, 1);
                sc5 = scene.Substring(10, 1);
                if (int.Parse(sc4) <= 4)
                {
                    LoadClip(21);
                    if (int.Parse(sc5) == 2)
                    {
                        LoadClip(23);
                    }
                }
                else if (int.Parse(sc4) >= 5)
                {
                    LoadClip(22);
                    if (int.Parse(sc5) == 2)
                    {
                        LoadClip(23);
                    }
                }
                break;
            case "Lo":
                //mainMixer.FindSnapshot("CollectOUT").TransitionTo(0.1f * Time.deltaTime);
                break;

            case "Me":
                //   LoadClip(0);
                break;

            case "W1":

                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);
                    //Debug.Log ("Jodete!");

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(21);
                }
                else if (s2Parse == 1)//W1L01->1-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(0);
                }
                else if (s2Parse == 2)//W1L01->1-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(1);
                }
                else if (s2Parse == 3)//W1L01->1-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(2);
                }
                else if (s2Parse == 4)//W1L01->1-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(3);
                }
                else if (s2Parse == 5)//W1L01->1-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(0);
                }
                else if (s2Parse == 6)//W1L02->1-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(4);
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                break;

            case "W2":
                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(21);
                }
                else if (s2Parse == 1)//W2L01->2-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(5);
                }
                else if (s2Parse == 2)//W2L02->2-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(6);
                }
                //else if (s2Parse == 3 || s2Parse == 6)//W2L03->2-03 
                else if (s2Parse == 3)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(7);
                }
                else if (s2Parse == 4)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(5);
                }
                else if (s2Parse == 5)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(6);
                    if (int.Parse(sc3) == 3)
                    {
                        LoadClip(8);
                    }
                }
                else if (s2Parse == 6)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(7);
                    if (int.Parse(sc3) == 2)
                    {
                        LoadClip(27);
                    }
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                break;

            case "W3":
                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                klausMixer.FindSnapshot("LPGround").TransitionTo(0.1f * Time.deltaTime);
                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(21);

                }
                else if (s2Parse == 1)//W3L01->3-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(9);
                }
                else if (s2Parse == 2)//W3l02->3-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(10);
                }
                else if (s2Parse == 3)//W3l03->3-03
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(11);
                    if (int.Parse(sc3) == 2)
                    {
                        LoadClip(12);
                    }
                }
                else if (s2Parse == 4)//W3l03->3-03
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(9);
                }
                else if (s2Parse == 5)//W3l03->3-03
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(10);
                }
                else if (s2Parse == 6)//W3l03->3-03
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(11);
                    if (int.Parse(sc3) == 2)
                    {
                        LoadClip(27);
                    }
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                break;

            case "W4":

                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(21);

                }
                else if (s2Parse == 1 || s2Parse == 3)//W4L01->4-01
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(13);
                }
                else if (s2Parse == 2 || s2Parse == 4)//W4L02->4-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(14);
                }
                else if (s2Parse == 5)//W4L02->4-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(15);
                }
                else if (s2Parse == 6)//W4L02->4-02
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(16);
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                break;

            case "W5":
                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                GameObject glitch = GameObject.Find("AS_GlitchMusik");
                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(22);
                }
                else if (s2Parse != 6)
                {
                    LoadClip(26);
                }
                else if (s2Parse == 6)
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                    if (int.Parse(sc3) == 2)
                    {
                        LoadClip(27);
                    }
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                /*if (object.ReferenceEquals(glitch, null))
                {
                    nextClip = -1;
                    audio.Stop();
                    Resources.UnloadAsset(audio.clip);
                    audio.clip = null;
                    Resources.Load<GameObject>(path_glitchMusik).Spawn(transform.position, transform.rotation);
                }*/

                break;

            case "W6":
                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                if (scCollectible == "C")
                {
                    mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                    LoadClip(24);

                }
                else if (scMemories == "C")
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    LoadClip(22);

                }
                else if (s2Parse == 1)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(17);
                }
                else if (s2Parse == 2 || s2Parse == 3)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(18);
                }
                else if (s2Parse == 4)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(19);
                }
                else if (s2Parse == 5)
                {
                    mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                    klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                    LoadClip(20);
                    if (int.Parse(sc3) == 1)
                    {
                        LoadClip(23);
                    }
                }

                else if (s2Parse == 6)
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                else if (s2Parse == 0)//NUEVO!!
                {
                    nextClip = -1;
                    audio.Stop();
                    UnloadAssetClip();
                    audio.clip = null;
                }
                /*/	else if (s2Parse == 6)
              {


                  mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                  klausMixer.FindSnapshot("Normal").TransitionTo(0.1f * Time.deltaTime);
                  nextClip = -1;
                  audio.Stop();
                  Resources.UnloadAsset(audio.clip);
                  audio.clip = null;
                  audio.Stop();
                  if(int.Parse(sc3) == 2)
                  {
                      //LoadClip(25);
                  }


        }
          /*/
                break;

            case "Fi":
                //klausMixer.FindSnapshot("Normal").TransitionTo(0.1f);
                LoadClip(5);
                break;

            case "En"://EndingScene
                mainMixer.FindSnapshot("MUTE").TransitionTo(0.1f * Time.deltaTime);
                break;

            default:
                mainMixer.FindSnapshot("Unpaused").TransitionTo(6 * Time.deltaTime);
                //klausMixer.FindSnapshot("Normal").TransitionTo(0.1f);
                nextClip = -1;
                audio.Stop();
                UnloadAssetClip();
                audio.clip = null;
                audio.Stop();
                break;
        }

    }

    void LoadClip(int newclip)
    {
        if (nextClip != newclip)
        {
            loadingAudio = false;
            audio.Stop();
            nextClip = newclip;
            UnloadAssetClip();
            StopCoroutine("loadClipFromResources");
            loadingAudio = true;
            StartCoroutine("loadClipFromResources", tracks[nextClip]);
            /*
            audio.clip = Resources.Load<AudioClip>(tracks[nextClip]);
            audio.PlayDelayed(delayTime);*/
        }
    }
    IEnumerator loadClipFromResources(string name)
    {
        loadingAudio = true;
        ResourceRequest request = Resources.LoadAsync<AudioClip>(name);
        while (!request.isDone)
        {
            yield return null;
        }
        audio.clip = request.asset as AudioClip;
        audio.PlayDelayed(delayTime);
        loadingAudio = false;
    }
    void Update()
    {
        if (nextClip < 0)
            return;
        if (loadingAudio)
            return;

        if (!audio.isPlaying)
        {
            if (scCollectible == "C" || scMemories == "C")
            {
                mainMixer.FindSnapshot("Collectable").TransitionTo(6 * Time.deltaTime);
                //audio.volume = SaveManager.Instance.dataKlaus.fxVolume;
                audio.Play();
            }
            else
            {
                audio.PlayDelayed(0.1f);
            }
        }
        if (audio.isPlaying && audio.volume == 0 && !isDucking)
        {
            audio.volume = SaveManager.Instance.dataKlaus.bgVolume;
        }

        if (audio.isPlaying && audio.volume != SaveManager.Instance.dataKlaus.bgVolume && !isDucking)
        {
            audio.volume = SaveManager.Instance.dataKlaus.bgVolume;
        }

        if (!audio.isPlaying && audio.clip != null && audio.volume != 0)
        {
            audio.Play();
        }

    }

    public void StopMusik()
    {
        nextClip = -1;
        audio.Stop();
        UnloadAssetClip();
        audio.clip = null;
        audio.Stop();
    }

    public void DuckingAttack(float time)
    {
        if (isDucking == false)
        {
            vol = audio.volume;
            StartCoroutine("VolDown");
            Invoke("DuckingRelease", time);
            isDucking = true;
        }
    }

    public void DuckingRelease()
    {
        StartCoroutine("VolUp");
    }

    IEnumerator VolDown()
    {
        for (float i = vol; i >= 0f; i -= attackSpeed * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;
        }
    }

    IEnumerator VolUp()
    {
        for (float i = 0; i <= vol; i += releaseSpeed * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;
        }
        isDucking = false;
    }

    void UnloadAssetClip()
    {
        if (audio.clip != null)
            Resources.UnloadAsset(audio.clip);
    }

    private void Start()
    {
        audio.volume = SaveManager.Instance.dataKlaus.bgVolume;
    }
}


