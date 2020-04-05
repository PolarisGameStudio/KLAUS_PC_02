using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SpecialButtonSelect : MonoBehaviour
{
    private RectTransform rect;

    public Text text;
    public Color NormalText;
    public Color HighlightedText;
    public Color PressText;
    public Color DisableText;

    [SerializeField]
    protected Color managedColor, originalColor;

    void Start()
    {
        managedColor = ColorWorldManager.Instance.getColorScene();
    }


    public void SetNormalColor()
    {
        text.color = NormalText;
    }

    public void SetHighlightedColor()
    {
        text.color = HighlightedText * managedColor;
    }

    public void SetPressColor()
    {
        text.color = PressText * managedColor;
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
