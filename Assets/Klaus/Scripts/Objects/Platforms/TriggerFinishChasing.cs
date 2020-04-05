using UnityEngine;
using System.Collections;

public class TriggerFinishChasing : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            ChaseManager.Instance.FinishChase();
        }
    }
}
