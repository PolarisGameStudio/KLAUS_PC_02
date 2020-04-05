using UnityEngine;
using System.Collections;

public class AI_ForceApply : MonoBehaviour,IForceObject
{
    private Rigidbody2D _rig2D = null;

    public Rigidbody2D rigidBody2D
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

    private AI_CurrentPlatform currPlatform = null;

    public AI_CurrentPlatform currentPlatform
    {

        get
        {

            if (currPlatform == null)
            {
                currPlatform = GetComponent<AI_CurrentPlatform>();
            }

            return currPlatform;
        }
    }

    #region IForceObject implementation

    public bool isForcing{ get; protected set; }

    public float TimeForceAgainIfNull = 0.4f;

    public virtual bool ApplyForce(Vector2 dir, float forceX, float forceY, bool isObject, float time = 0f)
    {
        if (enabled)
        {
            isForcing = true;
            currentPlatform.ResetPlatform();
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.AddForce(new Vector2(dir.x * forceX, dir.y * forceY),ForceMode2D.Impulse);
            transform.position += Vector3.up * 0.2f;
            StartCoroutine(ResetIsForcing((time == 0 ? TimeForceAgainIfNull : time)));
            return true;
        }
        return false;
    }

    protected IEnumerator ResetIsForcing(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isForcing = false;
    }

    #endregion
}
