using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveState))]
public class Glitch_AAAA : MonoBehaviour
{

    protected float scaleXM = -1;
    protected Vector3 Rotation = new Vector3(0, 0, 180);
    protected float gravityM = -1;
    Rigidbody2D _rig2D = null;

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

    MoveState _move = null;

    public MoveState MoveS
    {

        get
        {

            if (_move == null)
            {
                _move = GetComponent<MoveState>();
            }

            return _move;
        }

    }

    bool isActive = false;
    // Use this for initialization
    void OnEnable()
    {
        MoveS.SuscribeOnJump(OnJump);
    }
        

    // Update is called once per frame
    void OnJump(float percent)
    {
        if (!isActive)
        {
            SetAllVar(Rotation, scaleXM, gravityM);
        } else
        {
            SetAllVar(Vector3.zero, scaleXM, gravityM);
        }
        isActive = !isActive;

    }

    void SetAllVar(Vector3 rot, float ScaleX, float GraviM)
    {
        transform.rotation = Quaternion.Euler(rot);
        transform.localScale = new Vector3(transform.localScale.x * ScaleX, transform.localScale.y, transform.localScale.z);
        _rigidbody2D.gravityScale *= GraviM;
        MoveS.GraviyScaleForUp *= GraviM;
        MoveS.GraviyScaleForDown *= GraviM;
    }
}
