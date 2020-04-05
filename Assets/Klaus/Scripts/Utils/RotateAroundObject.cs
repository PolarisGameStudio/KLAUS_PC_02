using UnityEngine;
using System.Collections;

public class RotateAroundObject : MonoBehaviour
{

    public float angleMaxSpeed = 200.0f;
    public float radio = 1.5f;
    public float PosRelative;
    Rigidbody2D _rig2D = null;
    protected float currentTime = 0;

    public Rigidbody2D _rigidbody2D
    {

        get
        {

            if (_rig2D == null)
            {
                _rig2D = GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }
    }

    void Start()
    {
        //radio = transform.localPosition.magnitude;
        currentTime = PosRelative;
        transform.localPosition = (new Vector3(Mathf.Sin((angleMaxSpeed * currentTime)), Mathf.Cos(angleMaxSpeed * currentTime), 0) * radio);
    }

    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            _rigidbody2D.velocity = (new Vector2(angleMaxSpeed * Mathf.Cos(angleMaxSpeed * (currentTime)),
                -1 * angleMaxSpeed * Mathf.Sin(angleMaxSpeed * (currentTime))) * radio);
            currentTime += Time.fixedDeltaTime;
        }
        else
            _rigidbody2D.velocity = Vector2.zero;

//        Debug.Log(Time.time);
    }

}
