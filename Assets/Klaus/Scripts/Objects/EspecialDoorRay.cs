using UnityEngine;
using System.Collections;

public class EspecialDoorRay : MonoBehaviour
{
    public Animator anim;
    public float TimeToDestroy = 0.5f;
    bool isDetroy = false;
    //public GameObject destorySFX;
	public AudioSource audio1;

    public void Kill()
    {
        if (!isDetroy)
        {
			audio1.Play();
            anim.SetBool("Destroy", true);
            isDetroy = true;
            StartCoroutine("DestroyDoor", TimeToDestroy);
        }
    }

    IEnumerator DestroyDoor(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        //destorySFX.Spawn(transform.position, transform.rotation);
        gameObject.SetActive(false);
        GetComponent<AudioSource>().Stop();
    }
}
