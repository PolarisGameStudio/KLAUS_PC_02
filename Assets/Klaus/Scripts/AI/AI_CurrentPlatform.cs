using UnityEngine;
using System.Collections;

public class AI_CurrentPlatform : MonoBehaviour, ICurrentPlatform
{
    #region ICurrentPlatform implementation

    public Collider2D LegCollider;
    [HideInInspector]
    public bool isMovingInPaltformY = false;

    Rigidbody2D plat_rigidbody2D;


    public Rigidbody2D getOnPlatform()
    {

        return plat_rigidbody2D;

    }

    public bool isInPlatform
    {
        get
        {
            return getOnPlatform() != null;
        }
    }

    public void CurrentPlatformEnter(Rigidbody2D platform)
    {
        plat_rigidbody2D = platform;

    }

    public void CurrentPlatformExit(Rigidbody2D platform)
    {
        if (plat_rigidbody2D == platform)
        {
            ResetPlatform();
        }
    }

    public void ResetPlatform()
    {
        isMovingInPaltformY = false;
        plat_rigidbody2D = null;
    }

    public Collider2D getLegsCollider()
    {
        return LegCollider;
    }

    #endregion



}
