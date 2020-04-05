using UnityEngine;
using System.Collections;

public class AS_LowPassFilter : MonoBehaviour {

	public AudioLowPassFilter wakeEffect;
	// Use this for initialization
	void Start () {

		Invoke ("Filter",6f);
	}
	
	// Update is called once per frame
	void Filter () {

		StartCoroutine("OpenFilter");
	}
	IEnumerator OpenFilter()
	{
		for(float f = 0f; f < 10000f; f+=10f)
		{
			wakeEffect.cutoffFrequency = f;
			yield return StartCoroutine( new TimeCallBacks().WaitForSecondsPauseStop(0.01f));
		}
		wakeEffect.cutoffFrequency = 22000f;
	}
}
