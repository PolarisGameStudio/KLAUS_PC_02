using UnityEngine;
using System.Collections;

public class AS_VolumeRollOff : MonoBehaviour
{

    Rigidbody2D _rig2D = null;

    public Rigidbody2D _rigidbody2D
    {

        get
        {

            if (_rig2D == null && moveState != null)
            {
                _rig2D = moveState.GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }

    }

    public bool adentro = false;
    public float maxVolume;
    public float rollOnFactor;
    public float rollOffFactor;
    public MoveState moveState;
    // Update is called once per frame

    void Update()
    {

        if (_rigidbody2D == null)
            return;
        if (adentro)
        {
            if (_rigidbody2D.velocity.x != 0 || _rigidbody2D.velocity.y != 0)
            {
                if (GetComponent<AudioSource>().volume < maxVolume)
                {
                    GetComponent<AudioSource>().volume += rollOnFactor;
                }
            }
        }
        if (!adentro)
        {
            if (_rigidbody2D.velocity.x != 0 || _rigidbody2D.velocity.y != 0)
            {
                if (GetComponent<AudioSource>().volume > 0f && _rigidbody2D.velocity.x != 0)
                {
                    GetComponent<AudioSource>().volume -= rollOffFactor;
                }
            }
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            adentro = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            adentro = false;
        }
    }
}
