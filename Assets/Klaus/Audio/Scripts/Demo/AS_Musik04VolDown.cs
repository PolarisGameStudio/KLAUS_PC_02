using UnityEngine;
using System.Collections;

public class AS_Musik04VolDown : MonoBehaviour {

	public float speedVol =0.1f;

	void OnTriggerEnter2D(Collider2D other)
	{
		StartCoroutine("VolDown");
	}
	IEnumerator VolDown()
	{
        GameObject musik04 = GameObject.Find("AS_Musik04");
        if (musik04 != null) {
            AudioSource musik04Src = musik04.GetComponent<AudioSource>();
            while (musik04Src.GetComponent<AudioSource>().volume > 0) {
                musik04Src.GetComponent<AudioSource>().volume -= speedVol * Time.deltaTime;
                yield return null;
            }
        }
	}
}
