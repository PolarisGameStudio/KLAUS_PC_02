using UnityEngine;
using System.Collections;

public class Hack_CPU_Trophy : TrophieLogic
{
    public int MinHack = 0;
    bool firstStart = true;
    IEnumerator Start()
    {
        while (true)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.hack_cpu_UpdateCallback += OnGetHack;
                firstStart = false;
                break;
            }
            yield return null;
        }

    }
    void OnEnable()
    {
        if (!firstStart)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.hack_cpu_UpdateCallback += OnGetHack;

            }
        }
    }

    void OnDisable()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.hack_cpu_UpdateCallback -= OnGetHack;
        }
    }

    void OnGetHack(int hack)
    {
        if (UnLock)
            return;
        if (hack >= MinHack)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
