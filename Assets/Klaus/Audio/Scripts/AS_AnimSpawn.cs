using UnityEngine;
using System.Collections;

public class AS_AnimSpawn : MonoBehaviour {

	public GameObject objectSFX;
	// Use this for initialization
	void SpawnSFX () 
	{
		objectSFX.Spawn(transform.position, transform.rotation);
	}
}
