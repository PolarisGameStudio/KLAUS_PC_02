using UnityEngine;
using System.Collections;

public class AS_DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
