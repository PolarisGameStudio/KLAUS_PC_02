using UnityEngine;
using System.Collections;

public class CompleteLevel_Trophy : CompleteScene_Trophy
{

    ICompleteLevel test;

    protected override void OnStart(ICompleteLevel objCompelte)
    {
        test = objCompelte;
        test.RegisterCompleteLevel(OnCompleteLevel);
    }

    protected virtual void OnCompleteLevel()
    {
        if (UnLock)
        {
            if (test != null)
                test.UnRegisterCompleteLevel(OnCompleteScene);
            test = null;
            return;
        }

        UnLock = true;
        test.UnRegisterCompleteLevel(OnCompleteLevel);
        test = null;
        enabled = false;
    }
}
