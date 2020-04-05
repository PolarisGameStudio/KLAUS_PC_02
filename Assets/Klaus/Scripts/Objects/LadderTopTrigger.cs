using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderTopTrigger : MonoBehaviour
{

    public Collider2D colliderOneTop;
    public LadderTrigger ParentTrigger;
    public Transform BasePos;
    Dictionary<int, MoveState> isIn = new Dictionary<int, MoveState>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!isIn.ContainsKey(id))
            {
                isIn.Add(id, other.GetComponent<MoveState>());
            }
            MoveState State = isIn[id];
            if (other != State.getLegsCollider())
                return;

            State.CanLadderDown = true;
            State.LadderPosTop = BasePos;
            State.LadderTop = this;
            State.Ladder = ParentTrigger;

            //NOSEPORQUE if (!State.canLadder)
            {
                if (ParentTrigger.Plataforma != null)
                    State.setOnPlatformLadder(ParentTrigger.Plataforma._rigidbody2D);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!isIn.ContainsKey(id))
            {
                isIn.Add(id, other.GetComponent<MoveState>());
            }
            MoveState State = isIn[id];
            if (other != State.getLegsCollider())
                return;

            State.CanLadderDown = true;
            State.LadderPosTop = BasePos;
            State.LadderTop = this;
            State.Ladder = ParentTrigger;

            //NOSEPORQUE if (!State.canLadder)
            {
                if (ParentTrigger.Plataforma != null)
                    State.setOnPlatformLadder(ParentTrigger.Plataforma._rigidbody2D);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            MoveState State = isIn[other.gameObject.GetInstanceID()];
            if (other != State.getLegsCollider())
                return;

            if (BasePos == State.LadderPosTop)
            {
                State.LadderTop = null;
                State.LadderPosTop = null;
                State.CanLadderDown = false;
                if (!State.canLadder)
                    State.Ladder = null;

                if (ParentTrigger.Plataforma != null)
                    State.exitOnPlatformLadder(ParentTrigger.Plataforma._rigidbody2D);
            }
        }
    }
}
