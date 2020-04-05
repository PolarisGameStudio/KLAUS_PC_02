using UnityEngine;
using System.Collections;

public class Destroy_Mug_Trophy : TrophieLogic
{
    public int MinDestroyMug = 0;
    bool firstStart = true;
    IEnumerator Start()
    {
        while (true)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.destroy_mug_UpdateCallback += OnGetDestroy;
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
                SaveManager.Instance.dataKlaus.destroy_mug_UpdateCallback += OnGetDestroy;

            }
        }
    }

    void OnDisable()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.destroy_mug_UpdateCallback -= OnGetDestroy;
        }
    }

    void OnGetDestroy(int destroy)
    {
        if (destroy >= MinDestroyMug)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
