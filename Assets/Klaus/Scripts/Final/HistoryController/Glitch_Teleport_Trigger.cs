using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Glitch_Teleport_Trigger : MonoBehaviour
{
    public float TimeTeleporting = 0.5f;
    public bool isVertical = false;
    public Transform Pair;

    Dictionary<int, bool> isObjectTeleporting = new Dictionary<int, bool>();

    void Teleport(Collider2D other)
    {
        if (isVertical)
        {
            other.transform.position = new Vector2(other.attachedRigidbody.position.x, Pair.position.y);
        } else
        {
            other.transform.position = new Vector2(Pair.position.x, other.attachedRigidbody.position.y);
        }
        isObjectTeleporting [other.gameObject.GetInstanceID()] = Glitch_ManagerTelepor.Instance.Teleport(other.gameObject.GetInstanceID(), TimeTeleporting);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            if (Glitch_ManagerTelepor.CanTeleportStatic(other.gameObject.GetInstanceID()))
            {
                Teleport(other);
            } else
            {
                isObjectTeleporting [other.gameObject.GetInstanceID()] = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            if (Glitch_ManagerTelepor.CanTeleportStatic(other.gameObject.GetInstanceID()))
            {
                Teleport(other);
            } else
            {
                isObjectTeleporting [other.gameObject.GetInstanceID()] = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (CompareDefinition(other))
        {
            isObjectTeleporting [other.gameObject.GetInstanceID()] = false;   
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return true;
    }
}
