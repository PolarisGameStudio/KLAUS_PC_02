using UnityEngine;
using System.Collections;

public class AI_TazaHandler : MonoBehaviour
{

    private Rigidbody2D _rigid;

    public Rigidbody2D rigidBody2D
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody2D>();
            }
            return _rigid;
        }
    }

    bool firstRun = true;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;
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

    #region Pause:
    Vector2 velPause = Vector3.zero;
    public void OnPauseGame()
    {
        velPause = rigidBody2D.velocity;
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.isKinematic = true;
    }
    public void OnResumeGame()
    {
        rigidBody2D.isKinematic = false;
        rigidBody2D.velocity = velPause;
    }
    #endregion

    #region Bullet:

    public void Force(Vector2 force)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(force,ForceMode2D.Impulse);
    }
    #endregion

    public void Stop()
    {
        StopCoroutine("DestroyBullet");
        rigidBody2D.velocity = Vector2.zero;
    }
}
