using UnityEngine;
using System.Collections;

public class RigidBodyPause : MonoBehaviour
{

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

    Vector2 helperSpeed = Vector2.zero;
    float helperAngularSpeed = 0;
    bool isKinematic = false;
    public void OnPauseGame()
    {
        helperSpeed = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        isKinematic = _rigidbody2D.isKinematic;
        _rigidbody2D.isKinematic = true;
        helperAngularSpeed = _rigidbody2D.angularVelocity;
        _rigidbody2D.angularVelocity = 0;
    }

    public void OnResumeGame()
    {
        _rigidbody2D.isKinematic = isKinematic;
        _rigidbody2D.velocity = helperSpeed;
        _rigidbody2D.angularVelocity = helperAngularSpeed;
    }
}
