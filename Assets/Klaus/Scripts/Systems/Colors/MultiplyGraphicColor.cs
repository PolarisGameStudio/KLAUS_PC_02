using UnityEngine;

public class MultiplyGraphicColor : SetColorImage
{
    public override void SetColor(bool original)
    {
        if (original)
        {
            graphic.color = originalColor;
            if (mask) mask.enabled = false;
            if (noise) noise.SetActive(false);
        }
        else
        {
            graphic.color = managedColor * originalColor;

            Color glitchColor = ColorWorldManager.Instance.getGlitchColor();
            if (mask) mask.enabled = glitchColor == managedColor;
            if (noise) noise.SetActive(glitchColor == managedColor);
        }

        if (mantainOriginalAlpha)
        {
            Color color = graphic.color;
            color.a = originalColor.a;
            graphic.color = color;
        }
    }
}
