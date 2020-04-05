using UnityEngine;
using System.Collections;

public class BoolCircle2DOverlap : MonoBehaviour
{
    private bool collide = false;

    public bool isCollide
    {

        get
        {
            return collide;
        }
        set
        {
            collide = value;
        }
    }

    public float objectRadius = 0.09f;

    public Transform objectCheck;

    public LayerMask whatIsCollide;

    Collider2D[] result = new Collider2D[5];


    public bool useArea = false;
    public Vector2 pointA = new Vector2(-1, -1);
    public Vector2 pointB = new Vector2(1, 1);

    public Collider2D getFirstResultCollider()
    {
        return result [0];
    }

    public Collider2D[] getCollidersIn()
    {
        return result;
    }


    public void RemoveLayerToWhatisGround(string layer)
    {
        whatIsCollide &= ~(1 << LayerMask.NameToLayer(layer));
    }

    public void AddLayerToWhatisGround(string layer)
    {
        whatIsCollide |= (1 << LayerMask.NameToLayer(layer));
    }

    void FixedUpdate()
    {
        if (!ManagerPause.Pause && !ManagerStop.Stop)
        {
            if (!useArea)
                isCollide = Physics2D.OverlapCircleNonAlloc(objectCheck.position, objectRadius, result, whatIsCollide) > 0;
            else
            {
                isCollide = Physics2D.OverlapAreaNonAlloc(new Vector2(pointA.x + objectCheck.position.x, pointA.y + objectCheck.position.y),
                    new Vector2(pointB.x + objectCheck.position.x, pointB.y + objectCheck.position.y),
                    result, whatIsCollide) > 0;

            }
        }

    }

    public void NotUpdate(bool value)
    {
        enabled = false;
        isCollide = value;
        for (int i = 0; i < result.Length; ++i)
            result [i] = null;
    }

    public void StarUpdate(bool value)
    {
        enabled = true;
        isCollide = value;

    }
}
