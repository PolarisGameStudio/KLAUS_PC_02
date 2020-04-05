using UnityEngine;
using System.Collections;

public class CompleteLevelDead_Trophy : CompleteLevel_Trophy
{
    public string SceneStartID = "";
    protected int currentDeads = -1;
    public int MinDead = 0;

    protected override bool CanRegister()
    {
        if (UnLock)
            return false;

        return true;
    }

    protected override void OnStart(ICompleteLevel objCompelte)
    {
        if (Application.loadedLevelName == SceneID)
        {
            base.OnStart(objCompelte);
        }
        else if (Application.loadedLevelName == SceneStartID)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                currentDeads = SaveManager.Instance.dataKlaus.deaths;
            }
        }
    }

    protected override void OnCompleteLevel()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            int deaths = SaveManager.Instance.dataKlaus.deaths - currentDeads;
            if (deaths == MinDead)
            {
                base.OnCompleteLevel();
            }
        }

    }
}
