using UnityEngine;
using System.Collections;

public class CollectableCPU : MonoBehaviour
{

    public string World = "W";
    // Use this for initialization
    void Awake()
    {
        if (CollectablesManager.isCollectableFull(World) == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            CollectableCpuMaster.Instance.AddCPU();
        }
    }




}
