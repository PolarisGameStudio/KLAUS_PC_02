using UnityEngine;
using System.Collections;

public class Destroy_Clone_Trophy : TrophieLogic
{
    public int MinDestroyClone = 0;
    bool firstStart = true;
    IEnumerator Start()
    {
        while (true)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.destroy_clone_UpdateCallback += OnGetDestroy;
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
                SaveManager.Instance.dataKlaus.destroy_clone_UpdateCallback += OnGetDestroy;

            }
        }
    }

    void OnDisable()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.destroy_clone_UpdateCallback -= OnGetDestroy;
        }
    }

    void OnGetDestroy(int destroy)
    {
        if (destroy >= MinDestroyClone)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
