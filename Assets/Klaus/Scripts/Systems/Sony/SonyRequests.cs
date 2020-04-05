using UnityEngine;
using System;

public class SonyRequests : MonoSingleton<SonyRequests>
{
    public Action OnError;
    public Action<int, bool, bool> OnParentalControlReturned;

    bool npChecked;

    protected override void Init()
    {
        base.Init();

    }

    public void CheckNpAvailability()
    {

    }

    public void GetParentalControlInfo()
    {
        if (!SonyUserManager.Instance.IsSignedIn)
        {
            if (OnError != null)
                OnError();
        }


    }

    void OnParentalControlResult()
    {
        int age = 25;
        bool chatRestriction = false;
        bool ugcRestriction = false;

        OnParentalControlReturned?.Invoke(age, chatRestriction, ugcRestriction);
    }
}
