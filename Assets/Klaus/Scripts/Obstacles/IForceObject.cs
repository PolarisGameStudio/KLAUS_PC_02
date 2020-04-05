using UnityEngine;

interface IForceObject
{
    bool ApplyForce(Vector2 dir, float forceX, float forceY, bool isObject, float time = 0);
}