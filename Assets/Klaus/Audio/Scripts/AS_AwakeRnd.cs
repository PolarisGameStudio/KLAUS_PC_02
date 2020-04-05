using UnityEngine;
using System.Collections;

public class AS_AwakeRnd : MonoBehaviour {
    public float minPitch;
    public float maxPitch;
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
    void Awake ()
    {
        audio.pitch = Random.Range(minPitch, maxPitch);
    }
}
