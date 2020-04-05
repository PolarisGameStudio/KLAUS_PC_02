using UnityEngine;
using System.Collections;

public class AnimatorFxControllerW1Boss : AnimatorFxController
{

    public BossW1Controller boss;
    // Use this for initialization
    protected override void OnEnable()
    {
        boss.onJump = Jump2;
        speedStore = 0;
    }

    protected override void OnDisable()
    {
        boss.onJump = Jump2;
    }

    void Jump2()
    {
        Jump(0);
    }
}
