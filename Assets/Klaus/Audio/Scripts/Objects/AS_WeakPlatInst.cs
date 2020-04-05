using UnityEngine;
using System.Collections;

public class AS_WeakPlatInst : MonoBehaviour
{

    public float delay;
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

    void OnEnable()
    {
        Invoke("PlayAudio", delay);

    }

    void OnDisable()
    {
        CancelInvoke("PlayAudio");
    }

    void PlayAudio()
    {
        audio.mute = false;
        audio.Play();
        StartCoroutine(RecycleThis(audio.clip.length));
    }

    IEnumerator RecycleThis(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.Recycle();
		
    }
}
