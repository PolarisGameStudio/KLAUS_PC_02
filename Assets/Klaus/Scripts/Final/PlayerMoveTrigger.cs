using UnityEngine;
using System.Collections;

public class PlayerMoveTrigger : MonoBehaviour {

    public PlayerMoveAI AIMove;
    bool isShow = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShow)
        {
            if (CompareDefinition(other))
            {
                isShow = true;
                AIMove.enabled = true;
            }

        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player") ;
    }
}
