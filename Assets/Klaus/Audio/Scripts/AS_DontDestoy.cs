using UnityEngine;
using System.Collections;

public class AS_DontDestoy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}

}
