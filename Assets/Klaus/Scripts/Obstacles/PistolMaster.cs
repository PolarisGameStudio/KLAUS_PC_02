using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PistolMaster : MonoBehaviour {

    public List<PistolHandler> pistols;
        
    void OnBecameInvisible()
    {
        for (int i = 0; i < pistols.Count; ++i)
        {
            if (pistols[i] != null)
            {
                pistols[i].Invisible();
            }
        }
    }


    void OnBecameVisible()
    {
        for (int i = 0; i < pistols.Count; ++i)
        {
            if (pistols[i] != null)
            {
                pistols[i].Visible();
            }
        }

    }
}
