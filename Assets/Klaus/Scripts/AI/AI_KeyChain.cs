using UnityEngine;
using System.Collections;

public class AI_KeyChain : MonoBehaviour
{
    [HideInInspector]
    public KeyTrigger currentKey;

    public bool hasKey
    {
        get
        {
            return currentKey != null;
        }
    }

    public bool SetCurrentKey(KeyTrigger key)
    {
        if (currentKey == null)
        {
            currentKey = key;
            currentKey.transform.parent.parent = transform;
            currentKey.transform.parent.localPosition = Vector3.zero;
            currentKey.transform.parent.GetComponent<Collider2D>().enabled = false;
            currentKey.transform.parent.GetComponent<Rigidbody2D>().isKinematic = true;

            return true;
        }
        return false;
    }


    public void DropKey()
    {
        if (currentKey != null)
        {
            currentKey.transform.parent.parent = null;
            currentKey.transform.parent.GetComponent<Collider2D>().enabled = true;
            currentKey.transform.parent.GetComponent<Rigidbody2D>().isKinematic = false;

            currentKey.ResetAI();
            currentKey = null;
        }

    }

    public void OpenDoor()
    {
        currentKey.transform.parent.parent = null;
        currentKey.OpenDoor();
        currentKey = null;
    }
}
