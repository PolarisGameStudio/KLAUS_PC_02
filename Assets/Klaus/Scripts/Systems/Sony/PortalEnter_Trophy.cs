using UnityEngine;
using System.Collections;

public class PortalEnter_Trophy : TrophieLogic
{
    public CollectablePortal portal;
    void OnEnable()
    {
        portal.EnterPortalCallback += OnEnterPortalCallback;
    }

    void OnDisable()
    {
        if (portal != null)
            portal.EnterPortalCallback -= OnEnterPortalCallback;

    }
    void OnEnterPortalCallback()
    {
        if (UnLock)
            return;
        UnLock = true;
        enabled = false;
    }
}
