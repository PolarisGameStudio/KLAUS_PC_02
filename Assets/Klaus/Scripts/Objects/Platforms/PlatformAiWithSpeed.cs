using UnityEngine;
using System.Collections;

public class PlatformAiWithSpeed : PlatformAI
{
    public float[] pathSpeed;

    protected override float getSpeed()
    {
        if (pathSpeed.Length == 0)
            return base.getSpeed();
        return pathSpeed[currentPath];
    }
}
