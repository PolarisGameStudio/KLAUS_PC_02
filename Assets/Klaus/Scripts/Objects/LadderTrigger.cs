using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderTrigger : MonoBehaviour
{
    Dictionary<int, MoveState> isInMove = new Dictionary<int, MoveState>();

    [Tooltip("Agrega aqui el script currentPlatform de la plataforma si esta dentro de este.")]
    public CurrentPlatform Plataforma;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!isInMove.ContainsKey(id))
            {
                isInMove[id] = other.GetComponent<MoveState>();
            }
            if (other == isInMove[id].getLegsCollider())
                return;

            isInMove[id].canLadder = true;
            isInMove[id].Ladder = this;
            if (Plataforma != null)
                isInMove[id].setOnPlatformLadder(Plataforma._rigidbody2D);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!isInMove.ContainsKey(id))
            {
                isInMove[id] = other.GetComponent<MoveState>();
            }
            if (other == isInMove[id].getLegsCollider())
                return;

            isInMove[id].canLadder = true;
            isInMove[id].Ladder = this;
            if (Plataforma != null)
                isInMove[id].setOnPlatformLadder(Plataforma._rigidbody2D);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (other == isInMove[id].getLegsCollider())
                return;

            isInMove[id].canLadder = false;
            isInMove[id].Ladder = null;
            if (Plataforma != null)
                isInMove[id].exitOnPlatformLadder(Plataforma._rigidbody2D);
        }
    }
}
