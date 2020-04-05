using UnityEngine;
using System.Collections;

public class AS_PositionCollision : MonoBehaviour {
	public GameObject AS_Target;
	void OnParticleCollision (GameObject other)
	{
		Vector3 result = new Vector3(AS_Target.transform.position.x, other.transform.position.y, AS_Target.transform.position.z);
		AS_Target.transform.position = result;
		this.enabled=false;
	}
}
