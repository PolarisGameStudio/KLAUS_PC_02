using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Graphic))]
public class SetColorImage : MonoBehaviour
{
    public Graphic graphic;
    public TextMeshProUGUI text;
    public GameObject noise;
    public Mask mask;

    public bool mantainOriginalAlpha;
    public bool startOnManaged = true;

    protected Color managedColor, originalColor;

    void Start()
    {
        if (graphic)
            originalColor = graphic.color;
        else if (text)
            originalColor = text.color;
        managedColor = ColorWorldManager.Instance.getColorScene();

        if (startOnManaged)
            SetColor(false);
    }

    public void SetOriginalColor()
    {
        SetColor(true);
    }

    public void SetManagedColor()
    {
        SetColor(false);
    }

    public void UpdateManagedColor(Color newColor, float speed)
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateColor(newColor, speed));
        }
        else
        {
            managedColor = newColor;
            SetManagedColor();
        }
    }

    IEnumerator UpdateColor(Color newColor, float speed)
    {
        Color startColor = managedColor;
        float delta = 0;

        while (delta <= 1)
        {
            delta += Time.unscaledDeltaTime * speed;
            managedColor = Color.Lerp(startColor, newColor, delta);
            SetManagedColor();
            yield return null;
        }
    }

    public virtual void SetColor(bool original)
    {
        if (original)
        {
            if (graphic)
                graphic.color = originalColor;
            if (text)
                text.color = originalColor;
            
            if (mask)
                mask.enabled = false;
            if (noise)
                noise.SetActive(false);
        }
        else
        {
            if (graphic)
                graphic.color = managedColor;
            if (text)
                text.color = managedColor;

            Color glitchColor = ColorWorldManager.Instance.getGlitchColor();
            if (mask)
                mask.enabled = glitchColor == managedColor;
            if (noise)
                noise.SetActive(glitchColor == managedColor);
        }

        if (mantainOriginalAlpha)
        {
            Color color = graphic.color;
            color.a = originalColor.a;
            graphic.color = color;
        }
    }
}
