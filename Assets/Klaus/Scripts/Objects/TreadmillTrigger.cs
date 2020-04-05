using UnityEngine;
using System.Collections;

public class TreadmillTrigger : MonoBehaviour {

    public Collider2D Colliders;

    void Start()
    {
        MoveState[] moves = GameObject.FindObjectsOfType<MoveState>();
        for (int i = 0; i < moves.Length; ++i)
        {
             for (int j = 0; j <  moves[i].colliders.Length; ++j){
                 if(moves[i].colliders[j] != moves[i].getLegsCollider()){
                     Physics2D.IgnoreCollision(Colliders, moves[i].colliders[j]);

                 }
             }
        
        }
    }
}
