using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class AS_InstObjectDontDestroyDS4 : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
        AudioSource audio = GetComponent<AudioSource>();
		if(!audio.isPlaying)
		{
#if UNITY_PS4 && !(UNITY_EDITOR)
			
            audio.PlayOnDualShock4(PS4Input.PadGetUsersDetails(0).userId);
#else
            audio.Play();
#endif
		}
        Destroy(gameObject, audio.clip.length);
	}
}
