//
// CurrentPlatform.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurrentPlatform : MonoBehaviour
{

    Dictionary<int, ICurrentPlatform> playersOn = new Dictionary<int, ICurrentPlatform>();

    Rigidbody2D _rig2D = null;

    public Rigidbody2D _rigidbody2D
    {

        get
        {

            if (_rig2D == null)
            {
                _rig2D = transform.parent.GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        int id = other.gameObject.GetInstanceID();
        if (!playersOn.ContainsKey(id))
        {
            ICurrentPlatform iCurrent = ((ICurrentPlatform)other.GetComponent(typeof(ICurrentPlatform)));
            if (iCurrent == null)
                return;
            if (iCurrent.getLegsCollider() != other)
                return;
            playersOn[id] = iCurrent;
        }
        if (playersOn[id].getLegsCollider() != other)
            return;

        playersOn[id].CurrentPlatformEnter(_rigidbody2D);

    }
    void OnTriggerStay2D(Collider2D other)
    {
        int id = other.gameObject.GetInstanceID();
        if (!playersOn.ContainsKey(id))
        {
            ICurrentPlatform iCurrent = ((ICurrentPlatform)other.GetComponent(typeof(ICurrentPlatform)));
            if (iCurrent == null)
                return;
            if (iCurrent.getLegsCollider() != other)
                return;
            playersOn[id] = iCurrent;
        }
        if (playersOn[id].getLegsCollider() != other)
            return;
        /* if (playersOn[id].getOnPlatform() != null)
             return;*/
        playersOn[id].CurrentPlatformEnter(_rigidbody2D);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        int id = other.gameObject.GetInstanceID();
        if (!playersOn.ContainsKey(id))
            return;
        /* if (playersOn[id].getOnPlatform() == null)
             return;*/
        if (playersOn[id].getLegsCollider() != other)
            return;
        playersOn[id].CurrentPlatformExit(_rigidbody2D);
    }
}
