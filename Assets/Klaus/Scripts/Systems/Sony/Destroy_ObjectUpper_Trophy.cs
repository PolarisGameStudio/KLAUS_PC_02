using UnityEngine;
using System.Collections;

public class Destroy_ObjectUpper_Trophy : TrophieLogic
{
    public int MinDestroyObjectUpper = 0;
    bool firstStart = true;
    IEnumerator Start()
    {
        while (true)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.destroy_objectUpper_UpdateCallback += OnGetDestroy;
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
                SaveManager.Instance.dataKlaus.destroy_objectUpper_UpdateCallback += OnGetDestroy;

            }
        }
    }

    void OnDisable()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.destroy_objectUpper_UpdateCallback -= OnGetDestroy;
        }
    }

    void OnGetDestroy(int destroy)
    {
        if (destroy >= MinDestroyObjectUpper)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
