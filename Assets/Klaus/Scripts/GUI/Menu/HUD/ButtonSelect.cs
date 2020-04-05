using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ButtonSelect : MonoBehaviour
{

    private RectTransform rect;

    public Text text;
    public Color NormalText;
    public Color HighlightedText;
    public Color PressText;
    public Color DisableText;

    public void SetNormalColor()
    {
        text.color = NormalText;
    }

    public void SetHighlightedColor()
    {
        text.color = HighlightedText;
    }

    public void SetPressColor()
    {
        text.color = PressText;
    }

    public void SetDisableColor()
    {
        text.color = DisableText;
    }

    public RectTransform rectTransform
    {
        get
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            return rect;
        }
    }

}
