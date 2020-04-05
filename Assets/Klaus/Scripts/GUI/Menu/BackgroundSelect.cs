using UnityEngine;
using System.Collections;
using DG.Tweening;
[RequireComponent(typeof(RectTransform))]
public class BackgroundSelect : MonoBehaviour
{
    public bool isH = false;
    public bool forceTargetWhileNavigating;
    public bool adjustWidth;
    public bool adjustHeight;
    public bool initXisScreenRelative;
    public float initX = -330;
    [HideInInspector]
    public float initY = -9.784f;
    public float TargetX = -9.784f;
    public float durationX = 0.3f;
    public float durationY = 0;
    public float durationOUT = 0.3f;

    float temporaryTarget;

    public RectTransform reference;

    private RectTransform rect;

    public RectTransform rectTransform
    {
        get
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            return rect;
        }
    }

    IEnumerator Start()
    {
        yield return null;

        initY = reference.anchoredPosition.y;

        if (isH)
            initX = reference.anchoredPosition.x;

        else if (initXisScreenRelative)
            initX = initX * ManagerMenuUI.Instance.GetSize().x;

        Vector2 sizeDelta = rectTransform.sizeDelta;
        if (adjustWidth) sizeDelta.x = reference.sizeDelta.x;
        if (adjustHeight) sizeDelta.y = reference.sizeDelta.y;
        rectTransform.sizeDelta = sizeDelta;

        Reset();
    }

    public void SelectY(float posY)
    {
        rectTransform.anchoredPosition = new Vector2(forceTargetWhileNavigating ? temporaryTarget : initX, posY);
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(new Vector2(TargetX, posY), durationX);
        if (forceTargetWhileNavigating) temporaryTarget = TargetX;
    }

    public virtual void SelectX(float posX)
    {
        rectTransform.anchoredPosition = new Vector2(posX, initY);
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(new Vector2(posX, initY), durationY);
    }

    public virtual void Out()
    {
        if (isH)
        {
          //  rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, OutX), durationX).OnComplete(Reset);

        }
        else
        {
            float target = ManagerMenuUI.Instance.GetSize().x;
            rectTransform.DOAnchorPos(new Vector2(target, rectTransform.anchoredPosition.y), durationX).OnComplete(Reset);
        }
    }

    public void Reset()
    {
        rectTransform.anchoredPosition = new Vector2(initX, initY);
        if (forceTargetWhileNavigating) temporaryTarget = initX;
    }
}
