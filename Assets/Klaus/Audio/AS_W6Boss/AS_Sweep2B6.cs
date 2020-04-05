using UnityEngine;
using System.Collections;

public class AS_Sweep2B6 : Singleton<AS_Sweep2B6>
{
	public void PlaySweep ()
    {
        if(gameObject.activeSelf == true && GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
