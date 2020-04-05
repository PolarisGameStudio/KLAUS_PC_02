using UnityEngine;
using System.Collections;

public class Jump_Trophy : TrophieLogic
{
    public int MinJump = 0;
    bool firstStart = true;
    IEnumerator Start()
    {
        while (true)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                SaveManager.Instance.dataKlaus.jumpUpdateCallback += OnGetJump;
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
                SaveManager.Instance.dataKlaus.jumpUpdateCallback += OnGetJump;

            }
        }
    }

    void OnDisable()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.jumpUpdateCallback -= OnGetJump;
        }
    }

    void OnGetJump(int jump)
    {
        if (UnLock)
            return;
        if (jump >= MinJump)
        {
            UnLock = true;
            enabled = false;
        }
    }
}
