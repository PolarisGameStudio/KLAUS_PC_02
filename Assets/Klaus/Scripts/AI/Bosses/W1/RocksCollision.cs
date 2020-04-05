using UnityEngine;

public class RocksCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        GetComponentInParent<MoveState>().Kill();
    }
}
