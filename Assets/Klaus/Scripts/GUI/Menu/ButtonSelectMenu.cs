using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonSelectMenu : MonoBehaviour
{

    public BackgroundSelect BackgroundSelect;
    private RectTransform rect;

    public Text text;
    public Color NormalText;
    public Color HighlightedText;
    public Color PressText;
    public Color DisableText;
    public bool disabledIsColorScene = true;

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
        text.color = disabledIsColorScene ? ColorWorldManager.Instance.getColorScene() : DisableText;
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

    public virtual void SelectThis()
    {
        BackgroundSelect.SelectY(rectTransform.anchoredPosition.y);
      //  Debug.Log("You selected Something");
    }

    void OnMouseOver()
    {

    //    Debug.Log("Dude, you're on me");
        

    }

    private void Update()
    {
      // OnMouseOver();
    }
}
