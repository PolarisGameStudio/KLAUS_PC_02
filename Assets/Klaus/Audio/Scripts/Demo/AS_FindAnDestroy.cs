using UnityEngine;
using System.Collections;

public class AS_FindAnDestroy : MonoBehaviour {
	public string objName;
	private GameObject objToDestroy;
	// Use this for initialization
	void Awake () {
		objToDestroy = GameObject.Find (objName);
		Destroy (objToDestroy);
	}
}
