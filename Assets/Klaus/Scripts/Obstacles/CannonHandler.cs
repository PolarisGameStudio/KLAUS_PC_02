using UnityEngine;

public class CannonHandler : BulletHandler
{
    public void SetMass(float value)
    {
        rigidBody2D.mass = value;
    }

    public override void SetDirection(BulletInfo dir)
    {
        direction = dir.direction;
        StartCoroutine("DestroyBullet", dir.timeLive);
        rigidBody2D.AddForce(direction * maxSpeed, ForceMode2D.Impulse);
    }
}
