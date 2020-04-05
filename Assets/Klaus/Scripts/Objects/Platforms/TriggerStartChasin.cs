using UnityEngine;
using System.Collections;

public class TriggerStartChasin : MonoBehaviour
{

    bool isStart = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !isStart)
        {
            isStart = true;
            ChaseManager.Instance.StartChase();
        }
    }
}
