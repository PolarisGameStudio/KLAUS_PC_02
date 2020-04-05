using UnityEngine;
using System.Collections;

public class OffGameObjectPSVITA : MonoBehaviour
{

#if UNITY_PSP2
     void Start()
    {
        gameObject.SetActive(false);

    }
#endif


}
