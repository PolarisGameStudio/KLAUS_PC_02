using UnityEngine;
using System.Collections;

public class AS_ParticleCollision : MonoBehaviour {
	public GameObject AS_Target;
	// Use this for initialization
	void OnParticleCollision (GameObject other)
	{
		AS_Target.transform.position = other.transform.position;
		AS_Target.GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
		AS_Target.GetComponent<AudioSource>().Play();
	}
}
