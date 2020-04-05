using UnityEngine;

public class MovementAlign : MonoBehaviour
{
    private Rigidbody2D _rigid;

    public Rigidbody2D rigidBody2D
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponentInChildren<Rigidbody2D>();
            }
            return _rigid;
        }
    }

    void Update()
    {
        if (rigidBody2D.velocity != Vector2.zero)
            transform.right = rigidBody2D.velocity;
    }
}
