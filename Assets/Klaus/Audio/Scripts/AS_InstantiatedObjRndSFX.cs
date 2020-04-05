using UnityEngine;
using System.Collections;

public class AS_InstantiatedObjRndSFX : MonoBehaviour
{

    public AudioClip[] sfx;
    protected AudioSource _aud;

    public AudioSource audio
    {
        get
        {
            if (_aud == null)
                _aud = GetComponent<AudioSource>();
            return _aud;
        }
    }
    // Use this for initialization
    void Awake()
    {
        audio.playOnAwake = false;
    }

    void OnEnable()
    {
        audio.clip = sfx[Random.Range(0, sfx.Length)];
        audio.Play();
        StartCoroutine(RecycleThis(audio.clip.length));
    }

    IEnumerator RecycleThis(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));

        gameObject.Recycle();
    }
}
