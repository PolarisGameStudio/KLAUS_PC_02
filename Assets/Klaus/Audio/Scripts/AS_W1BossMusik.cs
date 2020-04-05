using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AS_W1BossMusik : MonoSingleton<AS_W1BossMusik>
{
    public float speed;
    public AudioClip musikIntro;
    public AudioClip musikFight;
    public AudioClip musikMuere;
    public AudioClip musikAcensor;
    public string scene;
    public float vol;
    public float timeToFight;
    public bool isAlive = true;
    public AudioSource firstClones;
    private AudioSource _audio;
    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }
    private AudioSource _audioMMArrecho;
    public AudioSource AS_Target
    {
        get
        {
            if (_audioMMArrecho == null)
            {
                GameObject aux = GameObject.Find("AS_MusikManager_Arrecho");
                if (aux != null)
                    _audioMMArrecho = aux.GetComponent<AudioSource>();
            }
            return _audioMMArrecho;
        }
    }
    public bool firstTime = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("Player") && !firstTime)
        {
            if (AS_Target != null)//para la musica
            {
                firstTime = true;
                vol = AS_Target.volume;
                StartCoroutine("Down");
            }
        }*/
    }
    /*
    void OnEnable()
    {
        ElevatorDoorManager test = GameObject.FindObjectOfType<ElevatorDoorManager>();
        test.onEnterElevator += MusikAcensor;
    }
    void OnDisable()
    {
        ElevatorDoorManager test = GameObject.FindObjectOfType<ElevatorDoorManager>();
        if (test != null)
            test.onEnterElevator -= MusikAcensor;
    }*/
    void Update()
    {
        if (audio.clip == musikFight && !audio.isPlaying)
        {
            audio.Play();
        }
        if (isAlive)
        {
#if UNITY_EDITOR
            if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
#endif
                audio.volume = SaveManager.Instance.dataKlaus.bgVolume;
        }
    }
    IEnumerator OnLevelWasLoaded()
    {
        yield return null;
        StopAllCoroutines();
        firstClones.Stop();
        firstClones.volume = SaveManager.Instance.dataKlaus.bgVolume;
        firstClones.Play();
        audio.Stop();
        audio.clip = null;
        audio.volume = SaveManager.Instance.dataKlaus.bgVolume;
        scene = SceneManager.GetActiveScene().name;
        if (scene == "W1BossFight")
        {
            yield return null;
            ElevatorDoorManager test = GameObject.FindObjectOfType<ElevatorDoorManager>();
            test.onEnterElevator += MusikAcensor;
        }
        else if (scene == "W1Ending")
        {

        }
        else if (scene == "W1VideoEnding")
        {

        }
        else
        {
            /*if (AS_Target != null)//vuelve a poner la musica
            {
                AS_Target.volume = vol;
                audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(3f);
            }*/
            Destroy(this.gameObject);
        }
    }
    //MUSIK
    IEnumerator Down()
    {
        for (float g = vol; g >= 0f; g -= (2 * speed) * Time.deltaTime)//musik
        {
            AS_Target.volume = g;
            yield return null;

        }
    }
    IEnumerator Down1()//bajo el volumen de la primera musika en la escena (clones)
    {
        vol = firstClones.volume;
        for (float g = vol; g >= 0f; g -= (2 * speed) * Time.deltaTime)//musik
        {
            firstClones.volume = g;
            yield return null;

        }
    }
    IEnumerator Down2()//bajo el volumen de la 2da musika al acabarse la pelea
    {
        for (float g = vol; g >= 0f; g -= (2 * speed) * Time.deltaTime)//musik
        {
            audio.volume = g;
            yield return null;

        }
    }
    public void MusikFight()
    {
        StartCoroutine("Down1");
        audio.clip = musikFight;
        audio.Play();
        audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Fight").TransitionTo(timeToFight);
    }
    public void MusikMuere()
    {
        isAlive = false;
        //audio.clip = musikMuere;
        //audio.Play();
        vol = audio.volume;
        StartCoroutine("Down2");
        //audio.Stop();
        //audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(0.0f);
    }
    public void MusikRevive()
    {
        audio.clip = musikMuere;
        audio.volume = vol;
        audio.Play();
        //audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(0.0f);
    }
    public void MusikAcensor()
    {
        //audio.clip = musikAcensor;
        //audio.Play();
        audio.Stop();
    }
    public void MusikIntro()
    {
        audio.clip = musikIntro;
        audio.Play();
    }
    public void EndFight()
    {
        audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(timeToFight);
    }
}
