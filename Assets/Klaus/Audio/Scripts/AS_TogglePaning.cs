using UnityEngine;
using System.Collections;

public class AS_TogglePaning : MonoBehaviour {

	private bool m_isInMuteLoop;
	public AudioSource audio1;
	public float delay;
	// Use this for initialization
	void Start () {
		StartCoroutine ("panning", delay);
	}
	IEnumerator panning(float delay)
	{
		m_isInMuteLoop = true;
		while(m_isInMuteLoop)
		{
			if(audio1.panStereo == 1)
			{
				audio1.panStereo = -1;
			}else
				audio1.panStereo = 1;
			yield return new WaitForSeconds(delay);
		}
	}

}
