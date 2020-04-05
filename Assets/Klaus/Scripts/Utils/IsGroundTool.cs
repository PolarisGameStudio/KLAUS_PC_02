using UnityEngine;
using System.Collections;
using System;

public class IsGroundTool : MonoBehaviour
{

    public LayerMask whatIsGround;
    public float groundRadius = 0.04f;
    public Action GroundAction;
    Collider2D[] result = new Collider2D[5];
    bool isG = false;
    const int factorUpIsGround = 3;
    public bool isGround
    {
        get
        {
            return isG;
        }
    }

    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            float groundRadius2 = groundRadius;
            if (isG)
                groundRadius2 *= factorUpIsGround;
            Debug.DrawLine(transform.position, transform.position + transform.up * -1 * groundRadius2, Color.black);
            Debug.DrawLine(transform.position, transform.position + transform.right * -1 * groundRadius2, Color.black);

            if (Physics2D.OverlapCircleNonAlloc(transform.position, groundRadius2, result, whatIsGround) > 0)
            {

                if (!isG)
                {
                    isG = true;
                    if (GroundAction != null)
                    {
                        GroundAction();
                    }
                }
            }
            else
            {
                isG = false;
            }

        }
    }
}
