using UnityEngine;
using System.Collections;

public class HeadTrigger : MonoBehaviour {

    public MoveState move;

    protected float headRadius = 0.1f;
    Collider2D[] result = new Collider2D[5];
    public LayerMask whatIsGround;

    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (Physics2D.OverlapCircleNonAlloc(transform.position, headRadius, result, whatIsGround) > 0)
            {
                if (move.isGround && !move.isDead)
                {
                    Debug.Log("kill");
                    move.Kill();
                }
            }
        }
    }
}
