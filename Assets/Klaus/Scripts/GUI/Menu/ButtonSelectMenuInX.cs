using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ButtonSelectMenuInX : ButtonSelectMenu
{
    public override void SelectThis()
    {
        BackgroundSelect.SelectX(rectTransform.anchoredPosition.x);
    }
}
