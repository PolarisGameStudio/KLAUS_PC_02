using UnityEngine;
using System.Collections;

public class AI_KillEnemyTrigger : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<MoveState>().Kill();
        }
    }
}
