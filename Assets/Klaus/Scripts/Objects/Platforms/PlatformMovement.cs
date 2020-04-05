//
// PlatformMovement.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public enum DirectionPlatform
{
    Horizontal = 0, // Use this transition to represent a non-existing transition in your system
    Vertical = 1,
}

public class PlatformMovement : MonoBehaviour, IMovePlatform
{

    /// <summary>
    /// If is horizontal o Vertical
    /// </summary>
    public bool isV = false;
    public bool played = false;
    public Transform spotPositive;
    public Transform spotNegative;
    public GameObject startSFX;
    public GameObject stopSFX;

    [Range(1f, 3f)]
    public float speedMultiplier = 1f;

    private static float maxSpeed = 3f;

    /// <summary>
    /// Input direction for move
    /// </summary>
	protected Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// Direcction to the positive
    /// </summary>
	protected Vector2 dirToA = Vector2.zero;

    protected const float minDistToReachPoint = 0.15f;

    /// <summary>
    /// Gear Rotation when move
    /// </summary>
    public RotateObject gear;
    public PlatformArrowAnim arrows;

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
    // Use this for initialization
    void Start()
    {
        if (spotPositive == null)
        {
            Debug.Log("No hay Spot Asignados");
            enabled = false;
            return;
        }

        if (!isV)
        {
            spotPositive.position = new Vector3(spotPositive.position.x, transform.position.y, spotPositive.position.z);
            spotNegative.position = new Vector3(spotNegative.position.x, transform.position.y, spotNegative.position.z);

        }
        else
        {
            spotPositive.position = new Vector3(transform.position.x, spotPositive.position.y, spotPositive.position.z);
            spotNegative.position = new Vector3(transform.position.x, spotNegative.position.y, spotNegative.position.z);

        }


        dirToA = (spotPositive.position - transform.position).normalized;
    }

    /// <summary>
    /// Check if Paltform stop or move
    /// </summary>
	protected void CheckStop()
    {
        if (Mathf.Approximately(Vector2.Angle(dirToA, inputDirection), 0.0f))
        {
            //Si voy hacia A
            if (Vector2.Distance(spotPositive.position, transform.position) < minDistToReachPoint)
            {
                inputDirection = Vector2.zero;

            }
        }
        else
        {
            //si voy hacia B
            if (Vector2.Distance(spotNegative.position, transform.position) < minDistToReachPoint)
            {
                inputDirection = Vector2.zero;
            }
        }

    }
    /// <summary>
    /// Cosas que debo hacer cuando voy a mover
    /// </summary>
    void OtherFuncionalityForSetMovement()
    {
        if (inputDirection.x > 0 || inputDirection.y > 0)
        {
            if (arrows != null)
                arrows.PositiveMove();
            //Animacion del Gear(quitar de aqui)
            gear.enabled = true;
            gear.angleMaxSpeed = Mathf.Abs(gear.angleMaxSpeed);
            if (!played)
            {
                played = true;
                startSFX.Spawn(transform.position, transform.rotation);
                GetComponent<AudioSource>().Play();
            }

        }
        else if (inputDirection.x < 0 || inputDirection.y < 0)
        {
            if (arrows != null)
                arrows.NegativeMove();
            gear.enabled = true;
            gear.angleMaxSpeed = -1 * Mathf.Abs(gear.angleMaxSpeed);
            if (!played)
            {
                played = true;
                startSFX.Spawn(transform.position, transform.rotation);
                audio.Play();
            }

        }
        else
        {
            if (arrows != null)
                arrows.ResetMove();
            gear.enabled = false;
            if (played)
            {

                played = false;
                stopSFX.Spawn(transform.position, transform.rotation);
                GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void SetMovement(Vector2 move)
    {
        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            inputDirection = move;

            CheckStop();

            OtherFuncionalityForSetMovement();

            rigidbody2D.velocity = inputDirection * maxSpeed * speedMultiplier;
            if (rigidbody2D.velocity == Vector2.zero && rigidbody2D.isKinematic == false)
                rigidbody2D.isKinematic = true;
            else if (rigidbody2D.velocity != Vector2.zero && rigidbody2D.isKinematic == true)
                rigidbody2D.isKinematic = false;
        }
        else
        {
            inputDirection = Vector2.zero;
            rigidbody2D.velocity = Vector2.zero;
            OtherFuncionalityForSetMovement();

        }
    }

    public void ResetToSpot(bool positive)
    {
        rigidbody2D.isKinematic = true;
        inputDirection = Vector2.zero;
        rigidbody2D.velocity = Vector2.zero;
        OtherFuncionalityForSetMovement();

        transform.position = positive ? spotPositive.position : spotNegative.position;
    }

}
