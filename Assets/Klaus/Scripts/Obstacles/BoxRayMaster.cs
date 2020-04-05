using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxRayMaster : MonoBehaviour {

    public List<RayBox> rayos;

    void OnBecameInvisible()
    {
        foreach (RayBox pist in rayos)
        {
            if (pist != null)
            {
                pist.Invisible();
            }
        }
    }


    void OnBecameVisible()
    {
        foreach (RayBox pist in rayos)
        {
            if (pist != null)
            {
                pist.Visible();
            }
        }

    }
}
