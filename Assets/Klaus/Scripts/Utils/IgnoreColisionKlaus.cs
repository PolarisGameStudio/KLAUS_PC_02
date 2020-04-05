using UnityEngine;
using System.Collections;

public class IgnoreColisionKlaus : MonoBehaviour {

    // Use this for initialization
    void Start() {
        MoveState[] moves = GameObject.FindObjectsOfType<MoveState>();
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        for (int j = 0; j < moves.Length; ++j) {
            for (int z = 0; z < moves[j].colliders.Length; ++z) {
                for (int i = 0; i < colliders.Length; ++i) {
                    Physics2D.IgnoreCollision(colliders[i], moves[j].colliders[z], true);
                }
            }
        }
    }
}
