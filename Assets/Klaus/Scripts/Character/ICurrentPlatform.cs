using UnityEngine;

interface ICurrentPlatform
{
    void CurrentPlatformEnter(Rigidbody2D platform);
    void CurrentPlatformExit(Rigidbody2D platform);
    Collider2D getLegsCollider();

    Rigidbody2D getOnPlatform();
}

