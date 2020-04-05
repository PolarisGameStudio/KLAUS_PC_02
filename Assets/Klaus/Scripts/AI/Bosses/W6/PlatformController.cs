using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    public Action playerOnPlatform;
    public Action playerOffPlatform;

    public Collider2D collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<Collider2D>();
            return _collider;
        }
    }

    Collider2D _collider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform ground = other.transform.Find("Ground");
            if (ground != null && playerOnPlatform != null)
                playerOnPlatform();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform ground = other.transform.Find("Ground");
            if (ground != null && playerOffPlatform != null)
                playerOffPlatform();
        }
    }
}
