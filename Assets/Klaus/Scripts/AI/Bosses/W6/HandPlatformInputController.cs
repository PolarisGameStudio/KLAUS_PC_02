using UnityEngine;

public class HandPlatformInputController : PlatformFreeInputController
{
    public Rigidbody2D body
    {
        get
        {
            if (_body == null)
                _body = GetComponent<Rigidbody2D>();
            return _body;
        }
    }

    Rigidbody2D _body;

    protected override void OnDisable()
    {
        base.OnDisable();
        body.isKinematic = false;
    }
}
