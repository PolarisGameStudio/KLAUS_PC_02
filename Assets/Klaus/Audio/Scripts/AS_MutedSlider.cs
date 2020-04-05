using UnityEngine;
using System.Collections;

public class AS_MutedSlider : MonoBehaviour {

	public int cont = 0;
	public AudioSource audio1;
	// Use this for initialization
	public void OnValueChange (float value) 
	{
		cont += 1;
		if (cont > 1)
		{
			audio1.mute = false;
			Destroy(this);
		}
	}
}
