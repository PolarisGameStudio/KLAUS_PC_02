using UnityEngine;
using System.Collections;

public class CompleteScene_Trophy : TrophieLogic
{
    public string SceneID = "";
    ICompleteLevel test;

    public void OnRegister(ICompleteLevel objCompelte)
    {
        if (!CanRegister())
            return;

        OnStart(objCompelte);
    }
    protected virtual bool CanRegister()
    {
        if (UnLock || (Application.loadedLevelName != SceneID))
            return false;

        return true;
    }
    protected virtual void OnStart(ICompleteLevel objCompelte)
    {

        test = objCompelte;
        test.RegisterCompleteScene(OnCompleteScene);
    }
    protected void UnRegisterCompelteScene()
    {
        if (test != null)
        {
            test.UnRegisterCompleteScene(OnCompleteScene);
            test = null;
        }
    }
    protected virtual void OnCompleteScene()
    {
        if (UnLock)
        {
            if (test != null)
                test.UnRegisterCompleteScene(OnCompleteScene);
            test = null;
            return;
        }
        UnLock = true;
        test.UnRegisterCompleteScene(OnCompleteScene);
        test = null;
        enabled = false;
    }

}
