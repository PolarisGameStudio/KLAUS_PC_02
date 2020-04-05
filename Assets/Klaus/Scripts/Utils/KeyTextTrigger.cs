using UnityEngine;
using System.Collections;

public class KeyTextTrigger : TextTrigger
{

    protected override bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Key");
    }
}
