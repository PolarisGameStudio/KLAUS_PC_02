using UnityEngine;
using System.Collections;

public class CollectableCpuMaster : Singleton<CollectableCpuMaster>
{

    public int CPUNeed = 6;
    protected int currentCpuNeed = 0;

    //  public Animator CPU_Anim;
    public PlatformController platformController;

    public PlatformAISingleRute PlatformAI;
    public Transform SpotA;
    public Transform SpotB;

    public void AddCPU()
    {
        ++currentCpuNeed;
    }
    void OnEnable()
    {
        if (platformController != null)
            platformController.playerOnPlatform += StartMovePlatform;
    }
    void OnDisable()
    {
        if (platformController != null)
            platformController.playerOnPlatform -= StartMovePlatform;
    }
    IEnumerator Start()
    {
        yield return null;
        if (currentCpuNeed != 6)
        {
            PlatformAI.gameObject.SetActive(false);
        }
    }


    public void StartMovePlatform()
    {
        PlatformAI.enabled = true;
    }

}
