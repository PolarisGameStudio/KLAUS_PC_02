using UnityEngine;
using System.Collections;

public class PlatformFreeMovement : MonoBehaviour, IMovePlatform
{

    AudioSource _audio = null;

    public AudioSource audio
    {

        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    Rigidbody2D rgb2D = null;

    public Rigidbody2D rigidbody2D
    {

        get
        {
            if (rgb2D == null)
                rgb2D = GetComponent<Rigidbody2D>();
            return rgb2D;
        }
    }

    #region IMovePlatform implementation

    public void SetMovement(Vector2 move)
    {
        Vector2 vel = new Vector2(move.x * maxSpeedX, move.y * maxSpeedY);
        rigidbody2D.velocity = vel;
    }

    #endregion

    [Range(0, 4)]
    public float maxSpeedY = 4.0f;
    public float maxSpeedX = 8.0f;

    void OnEnable()
    {
        rigidbody2D.isKinematic = false;
    }

    void OnDisable()
    {
        rigidbody2D.isKinematic = true;

    }
}
