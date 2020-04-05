using UnityEngine;
using System.Collections;

public class LookToPlayer : MonoBehaviour
{

    public BoolCircle2DOverlap playerNear;

    private Vector2 normalDir = Vector2.zero;

    public bool isPlayerNear
    {
        get
        {
            return playerNear.isCollide;
        }
    }

    public void ResetIsPlayerNear(bool value)
    {
        playerNear.isCollide = value;   
    }

    public Vector3 playerPosition
    {
        get
        {
            return playerNear.getFirstResultCollider().transform.position;
        }
    }

    public float DistanceToPlayer
    {
        get
        {
            if (isPlayerNear)
            {

                return Vector2.Distance(playerNear.getFirstResultCollider().transform.position, transform.position);

            } else
            {
                return int.MaxValue;
            }
        }
    }

    public Vector2 DirectionToPlayer
    {
        get
        {
            if (isPlayerNear)
            {

                Vector2 dir = playerNear.getFirstResultCollider().transform.position - transform.position;
                Debug.DrawLine(transform.position, playerNear.getFirstResultCollider().transform.position, Color.black);
                normalDir.Set(dir.normalized.x, 0);
                normalDir.Normalize();

                return normalDir;
            } else
            {
                normalDir = Vector2.zero;
                return normalDir;
            }
        }
    }

}
