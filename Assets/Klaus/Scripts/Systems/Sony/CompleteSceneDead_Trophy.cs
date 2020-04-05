using UnityEngine;
using System.Collections;

public class CompleteSceneDead_Trophy : CompleteScene_Trophy
{
    public int MinDead = 0;
    protected int currentDeads = -1;

    protected override void OnStart(ICompleteLevel objCompelte)
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            currentDeads = SaveManager.Instance.dataKlaus.deaths;
            base.OnStart(objCompelte);
        }
    }
    protected override void OnCompleteScene()
    {
        if (SaveManager.Instance.dataKlaus != null)
        {
            int deaths = SaveManager.Instance.dataKlaus.deaths - currentDeads;
            if (deaths == MinDead)
            {
                Debug.Log("Complete Trophy Dead");
                base.OnCompleteScene();
            }
        }
    }

}
