using UnityEngine;
using System.Collections;

public class AS_ButtonDown : MonoBehaviour {

	public string sc;
	public string sc1;
	public string scene;
	public int count;
	// Use this for initialization
	void awake()
	{
		count =0;
	}
	void Start () 
	{
		DontDestroyOnLoad(this);
		scene = Application.loadedLevelName;
		sc = scene.Substring(0, 2);
	}
	public void OnLevelWasLoaded()
	{
		count +=1;
		scene = Application.loadedLevelName;
		sc1 = scene.Substring(0, 2);
		if (sc != sc1 && count>1) 
		{
			Destroy(gameObject,1f);
		}
	}
}
