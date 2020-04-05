using UnityEngine;
using System.Collections;

public class ColliderKlausActivateTrigger : MonoBehaviour
{
    public Collider2D colliders;
    public Rigidbody2D rigidbody;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            Debug.Log("AQUI: " + other.name);
            colliders.enabled = false;
            rigidbody.isKinematic = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (CompareDefinition(other))
        {
            colliders.enabled = true;
            rigidbody.isKinematic = false;

        }

    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player");
    }
}
