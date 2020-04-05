using System;
using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
    public Action<Collider2D> onTriggerEnter;
    public Action<Collider2D> onTriggerStay;
    public Action<Collider2D> onTriggerExit;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter != null) onTriggerEnter(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onTriggerStay != null) onTriggerStay(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (onTriggerExit != null) onTriggerExit(other);
    }
}
