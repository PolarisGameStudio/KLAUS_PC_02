using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIt : MonoBehaviour {

	public float Speed = 200.0f;
	void Update()
	{
		transform.Rotate(Vector3.forward * Time.deltaTime * Speed);
	}
}
