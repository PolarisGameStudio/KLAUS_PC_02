using UnityEngine;
using System.Collections;

public class AS_Sweep1B6 : Singleton<AS_Sweep1B6>
{
	public void PlaySweep ()
    {
        if(gameObject.activeSelf == true && GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }

    }
}
