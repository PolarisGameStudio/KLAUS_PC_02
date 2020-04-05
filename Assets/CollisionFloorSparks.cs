using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFloorSparks : MonoBehaviour
{
    public Collider2D[] startedCollision;
     ParticleSystem particle;
    public DeadState klausMuerto;

    private void Start()
    {
        particle = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      //  Debug.Log("There's collision");
        for(int i=0; i< startedCollision.Length; i++)
        { 
            if(startedCollision[i]==other)
            { 
                particle.Play();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < startedCollision.Length; i++)
        {
            if (startedCollision[i] == other)
            {
                particle.Stop();
            }
        }
    }

    private void Update()
    {
        if(klausMuerto.enabled)
        {
            particle.Stop();
        }
    }
}
