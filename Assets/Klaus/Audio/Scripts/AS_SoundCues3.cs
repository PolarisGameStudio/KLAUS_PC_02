using UnityEngine;
using System.Collections;

public class AS_SoundCues3 : MonoBehaviour {

	public AudioSource audio1;
	public bool played;
	// Use this for initialization
	void Awake () 
	{
		audio1 = GetComponent<AudioSource>();
		played = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(audio1.isPlaying && !played)
		{
			played = true;
			AS_MusikManager.Instance.DuckingAttack (audio1.clip.length);
		}
	}
}
