using UnityEngine;
using UnityEngine.UI;

public class LevelAnimator : MonoBehaviour
{
    public Graphic[] graphics;
    public Mask[] masks;
    public GameObject[] noises;
    public Color blackColor = Color.black;
    public Color whiteColor = Color.white;

    Color managedColor;

    void Start()
    {
        managedColor = ColorWorldManager.Instance.getColorScene();
    }

    public void SetSceneColor()
    {
        SetColor(managedColor);
    }

    public void SetWhiteColor()
    {
        SetColor(whiteColor);
    }

    public void SetBlackColor()
    {
        SetColor(blackColor);
    }

    public void SetPlayersToBlackColor()
    {
        foreach (Graphic graphic in graphics)
            if (graphic.name.Contains("K1") || graphic.name.Contains("Klaus"))
                graphic.color = blackColor;
        
        foreach (Mask mask in masks)
            if (mask.name.Contains("K1") || mask.name.Contains("Klaus"))
                mask.enabled = false;
        
        foreach (GameObject noise in noises)
            if (noise.transform.parent.name.Contains("K1") || noise.transform.parent.name.Contains("Klaus"))
                noise.SetActive(false);
    }

    public virtual void SetColor(Color color)
    {
        foreach (Graphic graphic in graphics)
            graphic.color = color;

        // Turn on/off glitch
        if (color == managedColor) {
            Color glitchColor = ColorWorldManager.Instance.getGlitchColor();
            foreach (Mask mask in masks) mask.enabled = glitchColor == managedColor;
            foreach (GameObject noise in noises) noise.SetActive(glitchColor == managedColor);
        } else {
            foreach (Mask mask in masks) mask.enabled = false;
            foreach (GameObject noise in noises) noise.SetActive(false);
        }
    }
}
