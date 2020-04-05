using UnityEngine;
using System.Collections;

public class AS_InvisibleVolRollOff : MonoBehaviour {
	
	public bool visible;
	public PlatformAI platformAI;
	// Use this for initialization
	void OnBecameInvisible()
	{
		platformAI.visible = false;
	}
	void OnBecameVisible()
	{
		platformAI.visible = true;
	}
}
