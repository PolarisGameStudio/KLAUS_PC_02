using UnityEngine;
using System.Collections;

public class AI_IsVisibleSprite : MonoBehaviour {

    public bool isVisible { get; protected set; }
    public void OnBecameInvisible()
    {
        // enabled = false;
        isVisible = false;
    }


    public void OnBecameVisible()
    {
        isVisible = true;
    }
}
