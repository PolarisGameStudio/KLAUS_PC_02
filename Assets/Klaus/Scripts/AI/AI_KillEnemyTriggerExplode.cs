using UnityEngine;
using System.Collections;

public class AI_KillEnemyTriggerExplode : MonoBehaviour
{
    public AI_ExplodeTrigger explode;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            explode.Explode(col.gameObject.GetComponent<MoveState>());
        }
    }
}
