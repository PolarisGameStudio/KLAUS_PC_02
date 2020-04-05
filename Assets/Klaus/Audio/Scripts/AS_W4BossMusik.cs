using UnityEngine;
using System.Collections;

public class AS_W4BossMusik : MonoSingleton<AS_W4BossMusik>
{
    public float speed;
    public AudioClip musikIntro;
    public AudioClip musikFight;
    public string scene;
    public float vol;
    public float timeToFight;
    public AudioSource firstClones;
    public AudioSource musik;
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

    /*void Start()
    {
        firstClones.volume = SaveManager.Instance.dataKlaus.bgVolume;
        musik.Stop();

    }*/
    void OnTriggerEnter2D(Collider2D other)
    {
       /* if (other.CompareTag("Player") && !firstTime)
        {
            if (AS_Target != null)//para la musica
            {
                firstTime = true;
                vol = AS_Target.volume;
                StartCoroutine("Down");
            }
        }*/
    }
    IEnumerator OnLevelWasLoaded()
    {
        yield return null;
        StopAllCoroutines();
        firstClones.Stop();
        firstClones.volume = SaveManager.Instance.dataKlaus.bgVolume;
        firstClones.Play();
        musik.mute = true;
        musik.Stop();
        musik.mute = false;
        musik.volume = SaveManager.Instance.dataKlaus.bgVolume;
        scene = Application.loadedLevelName;
        if (scene == "W4VideoEnding")
        {
            StartCoroutine("Down1");
           /* if (AS_Target != null)//vuelve a poner la musica
            {
                AS_Target.volume = vol;
                audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(3f);
            }*/
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
        for (float g = vol; g >= 0f; g -= (speed * 2) * Time.deltaTime)//musik
        {
            firstClones.volume = g;
            yield return null;

        }
        firstClones.Stop();
        if (scene == "W4VideoEnding")
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator Down2()//bajo el volumen de la 2da musika al acabarse la pelea
    {
        for (float g = vol; g >= 0f; g -= speed * Time.deltaTime)//musik
        {
            musik.volume = g;
            yield return null;

        }
    }
    public void MusikClonesDown()
    {
        musik.outputAudioMixerGroup.audioMixer.FindSnapshot("Fight").TransitionTo(timeToFight + 3);
        vol = firstClones.volume;
        StartCoroutine("Down1");
    }
    public void MusikFight()
    {
        musik.clip = musikFight;
        musik.Play();
    }
    public void MusikMuere()
    {
        vol = musik.volume;
        StartCoroutine("Down2");

    }
    public void MusikRevive()
    {
        firstClones.PlayDelayed(timeToFight);
        firstClones.volume = 1;
        audio.outputAudioMixerGroup.audioMixer.FindSnapshot("Unpaused").TransitionTo(timeToFight);
    }
}
