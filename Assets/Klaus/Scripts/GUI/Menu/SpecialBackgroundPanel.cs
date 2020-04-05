using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class SpecialBackgroundPanel : BackgroundSelect
{
    public Image image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
        }
    }

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    Image _image;
    RectTransform _rectTransform;

    public override void SelectX(float x)
    {
        image.CrossFadeAlpha(1, 0, true);

        Vector2 target = reference.anchoredPosition;
        target.x = x;
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(target, durationX);
    }

    public override void Out()
    {
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        image.CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(durationOUT);

        image.CrossFadeAlpha(1, 0, true);
        yield return new WaitForSeconds(durationOUT);

        image.CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(durationOUT);

        image.CrossFadeAlpha(1, 0, true);
        yield return new WaitForSeconds(durationOUT);

        image.CrossFadeAlpha(0, 0, true);
        yield return new WaitForSeconds(durationOUT);

        image.CrossFadeAlpha(1, 0, true);
    }
}
