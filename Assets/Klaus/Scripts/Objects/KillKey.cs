using UnityEngine;
using System.Collections;
using System;

public class KillKey : KillObject, ICurrentPlatform
{
    public Animator anim;

    protected Vector3 startPos;
    protected Quaternion startRot;

    public Collider2D colliderKey;
    public GameObject[] childrens;

    public float TimeToRespawn = 2.0f;

    public Transform HeadCheck;
    public LayerMask whatIsGround;
    public float groundRadius = 0.04f;
    Collider2D[] result = new Collider2D[5];

    protected float storeGravityScale = 2;
    Rigidbody2D currentPlatform;
    bool firstRun = true;


    Rigidbody2D _rigidBd2D = null;
    public Vector2 MaxSpeed = new Vector2(100, 50);
    public Rigidbody2D rigidbody2D
    {

        get
        {

            if (_rigidBd2D == null)
                _rigidBd2D = GetComponent<Rigidbody2D>();

            return _rigidBd2D;
        }
    }
    public Action KillCallback;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;

        startPos = transform.position;
        startRot = transform.rotation;
        storeGravityScale = rigidbody2D.gravityScale;

    }

    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    void ResetTransform()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }

    void DisableKey()
    {
        anim.SetBool("Destroy", true);
        currentPlatform = null;
        colliderKey.enabled = false;
        rigidbody2D.isKinematic = true;
        rigidbody2D.gravityScale = 0;
        for (int i = 0; i < childrens.Length; ++i)
        {
            childrens[i].SetActive(false);
        }
    }

    void EnableKey()
    {
        StartCoroutine("GravityFall", 0.5f);
        ResetTransform();
        colliderKey.enabled = true;
        rigidbody2D.isKinematic = false;
        for (int i = 0; i < childrens.Length; ++i)
        {
            childrens[i].SetActive(true);
        }
        anim.SetBool("Destroy", false);

    }

    public override void Kill()
    {
        if (KillCallback != null)
            KillCallback();
        DisableKey();
        rigidbody2D.velocity = new Vector2(0f, 0f);
        StartCoroutine("Respawn", TimeToRespawn);
    }

    IEnumerator GravityFall(float time)
    {

        rigidbody2D.gravityScale = 0;
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        rigidbody2D.gravityScale = storeGravityScale;
    }

    IEnumerator Respawn(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        EnableKey();
    }

    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (colliderKey.enabled)
            {
                if (currentPlatform != null)
                {
                    float y = currentPlatform.velocity.y;
                    if (Mathf.Approximately(y, 0))
                    {
                        y = rigidbody2D.gravityScale * Physics2D.gravity.y;
                    }
                    else if (y < 0)
                    {
                        y += rigidbody2D.gravityScale * Physics2D.gravity.y;
                    }
                    float x = currentPlatform.velocity.x;
                    if (Mathf.Approximately(x, 0))
                    {
                        x = rigidbody2D.velocity.x;
                    }
                    rigidbody2D.velocity = new Vector2(x, y);

                }
                else
                {
                    float newY = rigidbody2D.velocity.y;

                    if (Mathf.Abs(rigidbody2D.velocity.y) > MaxSpeed.y)
                    {
                        newY = Mathf.Clamp(rigidbody2D.velocity.y, MaxSpeed.y * -1, MaxSpeed.y);
                    }
                    float newX = rigidbody2D.velocity.x;
                    if (Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed.x)
                    {
                        newX = Mathf.Clamp(rigidbody2D.velocity.x, MaxSpeed.x * -1, MaxSpeed.x);
                    }
                    rigidbody2D.velocity = new Vector2(newX, newY);
                }
                /*    Debug.DrawLine(HeadCheck.position, HeadCheck.position + groundRadius * Vector3.up, Color.black);
                    Debug.DrawLine(HeadCheck.position, HeadCheck.position + groundRadius * Vector3.up * -1, Color.black);
                    Debug.DrawLine(HeadCheck.position, HeadCheck.position + groundRadius * Vector3.left * -1, Color.black);
                    Debug.DrawLine(HeadCheck.position, HeadCheck.position + groundRadius * Vector3.left, Color.black);*/

                if (Physics2D.OverlapCircleNonAlloc(HeadCheck.position, groundRadius, result, whatIsGround) > 0)
                {
                    Kill();

                }
            }
        }
    }

    public void CurrentPlatformEnter(Rigidbody2D platform)
    {

        currentPlatform = platform;

    }

    public void CurrentPlatformExit(Rigidbody2D platform)
    {
        if (currentPlatform == platform)
        {
            currentPlatform = null;
        }
    }

    public Collider2D getLegsCollider()
    {
        return colliderKey;
    }
    public Rigidbody2D getOnPlatform()
    {
        return currentPlatform;

    }
    #region Pause:

    Vector2 velPause = Vector3.zero;
    bool kinematic = false;

    public void OnPauseGame()
    {
        velPause = rigidbody2D.velocity;
        rigidbody2D.velocity = Vector2.zero;
        kinematic = rigidbody2D.isKinematic;
        rigidbody2D.isKinematic = true;
    }

    public void OnResumeGame()
    {
        rigidbody2D.isKinematic = kinematic;
        rigidbody2D.velocity = velPause;
    }

    #endregion
}
