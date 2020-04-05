using UnityEngine;
using System.Collections;
using System;

public class CPUTrigger : MonoBehaviour
{

    public IHack hack;
    public IHack secondaryHack;

    public float timeTohack = 2.0f;
    public Animator anim;
    public GameObject activeSFX;
    CodeState[] codePlayer;
    bool isHacked = false;
    bool isIn = false;

    public Action onFinishHack;
    public Action onStartHack;
    public Action onCancelHack;

    public Collider2D hackTrigger;
    int posPStore = -1;

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

    void Awake()
    {
        codePlayer = GameObject.FindObjectsOfType<CodeState>();
        if (hack != null)
            hack.cpu = this;

        if (secondaryHack != null)
            secondaryHack.cpu = this;
    }

    int isCoderPlayer(int id)
    {
        for (int i = 0; i < codePlayer.Length; ++i)
        {
            if (codePlayer [i].gameObject.GetInstanceID() == id)
            {
                return i;
            }
        }
        return -1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!isHacked && !isIn)
        {
            if (other.CompareTag("Player"))
            {
                int posP = isCoderPlayer(other.gameObject.GetInstanceID());
                if (posP >= 0)
                {
                    posPStore = posP;
                    isIn = true;
                    codePlayer [posP].SetHack(timeTohack, OnStartCode, OnCancelCode, OnFinishCode);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (!isHacked && isIn)
        {

            if (other.CompareTag("Player"))
            {
                int posP = isCoderPlayer(other.gameObject.GetInstanceID());
                if (posP >= 0 && posP == posPStore)
                {
                    isIn = false;
                    codePlayer [posP].ClearHack(OnStartCode, OnCancelCode, OnFinishCode);
                    posPStore = -1;

                }
            }

        }
    }

    void OnFinishCode()
    {
        isHacked = true;
        anim.SetBool("HackingTrue", true);
        if (posPStore >= 0 && codePlayer [posPStore] != null)
            codePlayer [posPStore].ClearHack(OnStartCode, OnCancelCode, OnFinishCode);
        if (hack != null)
            hack.HackedSystem();

        if (secondaryHack != null)
            secondaryHack.HackedSystem();
        
        audio.Stop();
        activeSFX.Spawn(transform.position, transform.rotation);

        posPStore = -1;

        if (onFinishHack != null)
            onFinishHack();
    }

    void OnStartCode(float time)
    {
        anim.SetBool("Hacking", true);
        audio.Play();
        if (onStartHack != null)
            onStartHack();
    }

    void OnCancelCode()
    {
        anim.SetBool("Hacking", false);
        audio.Stop();
        if (onCancelHack != null)
            onCancelHack();

    }

    public void ResetCPU()
    {
        if (hack != null)
            hack.ResetAll();
        if (secondaryHack != null)
            secondaryHack.ResetAll();
        if (posPStore >= 0 && codePlayer[posPStore] != null)
        {
            Debug.Log("Clearing hack");
            codePlayer[posPStore].ClearHack(OnStartCode, OnCancelCode, OnFinishCode);
            codePlayer[posPStore].CancelAllCoding();
        }
        anim.Rebind();
        audio.Stop();
        isHacked = false;
        isIn = false;
    }

    public void EnableHacking(bool value)
    {
        if (hackTrigger)
            hackTrigger.enabled = value;
    }
}
