using System;
using UnityEngine;

public class StartFightTrigger : MonoBehaviour
{
    public Action onEnterTrigger;
    public Action onExitTrigger;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Player") && onEnterTrigger != null)
            onEnterTrigger();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Contains("Player") && onExitTrigger != null)
            onExitTrigger();
    }
}
