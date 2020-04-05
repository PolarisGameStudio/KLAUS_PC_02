using UnityEngine;

public class SpecialBlink : Blink
{
    public Renderer[] renderers;

    public override Color GetColor()
    {
        return renderers[0].sharedMaterial.color;
    }

    public override void SetColor(Color color)
    {
        foreach (Renderer renderer in renderers)
            renderer.sharedMaterial.color = color;
    }
}
