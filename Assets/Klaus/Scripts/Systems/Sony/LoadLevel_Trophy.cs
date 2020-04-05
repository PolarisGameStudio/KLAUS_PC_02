using UnityEngine;
using System.Collections;

public class LoadLevel_Trophy : TrophieLogic
{
    public string SceneID = "";

    public void Start()
    {
        if (UnLock || (Application.loadedLevelName != SceneID))
            return;

        UnLock = true;
        enabled = false;
    }
}
