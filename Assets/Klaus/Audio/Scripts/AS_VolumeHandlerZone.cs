using UnityEngine;
using System.Collections;

public class AS_VolumeHandlerZone : MonoBehaviour 
{
	public bool enter = false;
	public float vol;
	public float speedUp = 0.015f;
	public float speedDown = 0.03f;
    public float minVol = 0.1f;

    float totalVolume = 1f;
    float maxDistance;
    float maxVolumeReduction;
    AudioSource asource;

    void Awake()
    {
        asource = GetComponent<AudioSource>();
        maxVolumeReduction = totalVolume - minVol;
    }

	void  OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        vol = minVol;
        asource.volume = vol;
        maxDistance = Vector3.Distance(transform.position, other.transform.position);
	}


	void  OnTriggerExit2D(Collider2D other)
    {
        vol = 0f;
        asource.volume = vol;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        float distance = Vector3.Distance(transform.position, other.transform.position);
        float volumeReduction = distance * maxVolumeReduction / maxDistance;
        vol = totalVolume - volumeReduction;
        asource.volume = vol;
    }


}
