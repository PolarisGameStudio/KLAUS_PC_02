using UnityEngine;

public class K1TakeDamage : KillObject
{
    public BossW1Controller controller;

    public override void Kill()
    {
        controller.Kill();
    }
}
