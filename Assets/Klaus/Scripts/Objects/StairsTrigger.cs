using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StairsTrigger : MonoBehaviour {
    static PhysicsMaterial2D friction;

    PhysicsMaterial2D currentMaterial;
    void Awake()
    {
        if(friction ==null )
            friction = (PhysicsMaterial2D)Resources.Load("Roce");

        Collider2D[] col = GetComponents<Collider2D>();
        for (int i = 0; i < col.Length; ++i)
        {
            col[i].enabled = false;
            col[i].sharedMaterial = friction;
            col[i].enabled = true;

        }
    }
    void OnDestroy()
    {
        if (friction != null)
        {
            Resources.UnloadAsset(friction);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (friction != other.collider.sharedMaterial)
            {
                other.collider.enabled = false;
                currentMaterial = other.collider.sharedMaterial;
                other.collider.sharedMaterial = friction;
                other.collider.enabled = true;
            }
          
        }


    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {

            if (currentMaterial != other.collider.sharedMaterial && currentMaterial != friction)
            {
                other.collider.enabled = false;
                other.collider.sharedMaterial = currentMaterial;
                other.collider.enabled = true;
            }
        }
    }
}
