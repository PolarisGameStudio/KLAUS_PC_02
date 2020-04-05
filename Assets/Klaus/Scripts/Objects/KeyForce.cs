using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class KeyForce : MonoBehaviour, IForceObject
{

    Rigidbody2D _rigidBd2D = null;

    public Rigidbody2D rigidbody2D
    {
	
        get
        {
		
            if (_rigidBd2D == null)
                _rigidBd2D = GetComponent<Rigidbody2D>();

            return _rigidBd2D;
        }
    }

    bool isForcing = false;

    #region IForceObject implementation

    public bool ApplyForce(Vector2 dir, float forceX, float forceY, bool isObject, float time = 0f)
    {
        if (!isForcing)
        {
            isForcing = true;
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.AddForce(new Vector2(dir.x * forceX, dir.y * forceY));
            StartCoroutine(ResetIsForcing((time == 0 ? 0.1f : time)));
            return true;
        }
        return false;
    }

    IEnumerator ResetIsForcing(float time)
    {
	
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isForcing = false;
    }

    #endregion

}
