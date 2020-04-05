using UnityEngine;
using System.Collections;

public class AS_InstantiatedObject : MonoBehaviour {

	public float delay;
	// Use this for initialization
	void Awake () {
        audio.playOnAwake = false;
	}
    void OnEnable() {
       audio.PlayDelayed(delay);
        StartCoroutine(RecycleThis(audio.clip.length+delay));

    }
	public AudioSource audio {
		get {
			if (_audio == null)
				_audio = GetComponent<AudioSource>();
			return _audio;
		}
	}
	private AudioSource _audio;
    IEnumerator RecycleThis(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.Recycle();

    }
}
