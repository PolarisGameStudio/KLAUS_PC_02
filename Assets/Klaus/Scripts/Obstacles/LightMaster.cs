using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightMaster : MonoBehaviour
{

    public List<GameObject> scripts;

    void Awake()
    {
        OnBecameInvisible();
    }
    void OnBecameInvisible()
    {
        foreach (GameObject pist in scripts)
        {
            if (pist != null)
            {
                pist.SetActive(false);
            }
        }
    }


    void OnBecameVisible()
    {
        foreach (GameObject pist in scripts)
        {
            if (pist != null)
            {
                pist.SetActive(true);
            }
        }

    }
}
