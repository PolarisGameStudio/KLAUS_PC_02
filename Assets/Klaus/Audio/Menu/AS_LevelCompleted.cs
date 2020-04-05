using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class AS_LevelCompleted : MonoBehaviour {

	public AudioSource flash;
	public AudioSource noise;
	public AudioSource musik;
    public AudioMixer mixer;
    public AudioSource whoosh;
	[Range(0.0f, 1.0f)]
	public float step;
	private AudioSource AS_Target;
	private float vol;
	// Use this for initialization
	void Complete()
	{
		if (GameObject.Find("AS_MusikManager_Arrecho") != null)//para la musica
		{
            mixer.FindSnapshot("LevelCompleted").TransitionTo(0.1f);
			AS_MusikManager.Instance.StopMusik ();
            //AS_Target = GameObject.Find("AS_MusikManager_Arrecho").GetComponent<AudioSource>();
			//AS_Target.Stop ();
		}
		if (GameObject.Find("AS_W5BossMusik") != null)//para la musica
		{
			AudioSource [] aSources = GameObject.Find("AS_W5BossMusik").GetComponents<AudioSource>();
			for(int i=0; i<aSources.Length; i++)
			{
				aSources[i].Stop ();
			}
		}
	}
	void PlayWhoosFlash()
	{
		flash.time = 0;
		flash.Play ();
	}
	void PlayFlash()
	{
		flash.time = step;
		flash.Play ();
	}
	void PlayNoise()
	{
		noise.Play ();
	}
	void PlayMusik()
	{
        musik.Play ();
	}
    void PlayWhoosh()
    {
        whoosh.Play();
    }
    void Out ()
    {
        mixer.FindSnapshot("Unpaused").TransitionTo(3f);
    }
}
