using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AreaEffector2D))]
public class VentiladorTrigger : MonoBehaviour
{

    AreaEffector2D _ae2D = null;

    public AreaEffector2D areaEffector2D
    {
	
        get
        {
            if (_ae2D == null)
                _ae2D = GetComponent<AreaEffector2D>();
            return _ae2D;
        }
    }

    Vector3 lastVector = Vector3.zero;

    void UpdateAreaEfector()
    {
	
        float angle = Mathf.Atan2(transform.up.y, transform.up.x);
        if (angle < 0)
        {
            angle += 2 * Mathf.PI;
        }
        areaEffector2D.forceAngle = Mathf.Rad2Deg * angle;
        lastVector = transform.up;
    }

    void Start()
    {
        UpdateAreaEfector();
    }

    void LateUpdate()
    {
        //a = mod(atan2(y,x),2*pi);
        if (transform.up != lastVector)
        {
            UpdateAreaEfector();
        }
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MoveState>().SetIsNONWhenJump();
        }
    }
    /*
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }*/

}
