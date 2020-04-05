using UnityEngine;
using System.Collections;

public class AS_PlayRndTime : MonoBehaviour {

    private float step;
    void Awake ()
    {
        step = Random.Range(0f, 0.6f);
        GetComponent<AudioSource>().time = step;
        GetComponent<AudioSource>().Play();

	}

}
