using UnityEngine;
using System.Collections;

public class BossEyes : MonoBehaviour
{
    public Color color;

    void OnEnable()
    {
        BossW6Animator.laserColor = color;
    }
}
