using UnityEngine;
using System.Collections;

public class CompleteKeyDead_Trophy : CompleteScene_Trophy
{
    public string[] ScenesID;
    KillKey[] keys;
    protected override bool CanRegister()
    {
        if (UnLock)
            return false;

        for (int i = 0; i < ScenesID.Length; ++i)
        {
            if (Application.loadedLevelName == ScenesID[i])
                return true;

        }
        return false;
    }

    protected override void OnStart(ICompleteLevel objCompelte)
    {
        base.OnStart(objCompelte);
        //Aqui me registro a las key de las escenas
        KillKey[] keys = GameObject.FindObjectsOfType<KillKey>();
        for (int i = 0; i < keys.Length; ++i)
        {
            keys[i].KillCallback += OnKillKey;
        }
    }
    public void OnKillKey()
    {
        UnRegisterCompelteScene();
    }
}
