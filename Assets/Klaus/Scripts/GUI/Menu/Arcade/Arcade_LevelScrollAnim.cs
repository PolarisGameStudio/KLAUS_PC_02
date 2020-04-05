using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Arcade_LevelScrollAnim : MonoBehaviour
{
    public RectTransform panel;
    public Image leftArrow, rightArrow;

    [HideInInspector]
    public bool forceReset;

    public void ResetScroll()
    {
        // Reset arrows
        StopAllCoroutines();
        SetAlpha(leftArrow, 0);
        SetAlpha(rightArrow, 1);

        // Set position
        float chunkSize = panel.sizeDelta.x / panel.transform.childCount;

        Vector2 position = panel.anchoredPosition;
        position.x = -chunkSize * 0.5f;
        panel.anchoredPosition = position;

        forceReset = false;
    }

    public void SetNewNormalPos(int positionInList)
    {
        if (forceReset)
        {
            ResetScroll();
            return;
        }

        // Animate position
        float chunkSize = panel.sizeDelta.x / panel.transform.childCount;
        Vector2 position = panel.anchoredPosition;
        position.x = -chunkSize * (positionInList + 0.5f);

        if (position != panel.anchoredPosition) {
            panel.DOAnchorPos(position, 0.2f).OnComplete(CheckLimits);
            
            // Animate arrows
            StopAllCoroutines();
            if (position.x > panel.anchoredPosition.x)
            {
                SetAlpha(rightArrow, 1);
                StartCoroutine(Animate(leftArrow));
            }
            else
            {
                SetAlpha(leftArrow, 1);
                StartCoroutine(Animate(rightArrow));
            }
        }
    }

    void CheckLimits()
    {
        float chunkSize = panel.sizeDelta.x / panel.transform.childCount;

        // In left limit
        if (panel.anchoredPosition.x >= -chunkSize * 0.5f - 0.1f)
        {
            StopAllCoroutines();
            SetAlpha(leftArrow, 0);
        }

        // In right limit
        if (panel.anchoredPosition.x <= -chunkSize * (panel.transform.childCount - 0.5f) + 0.1f)
        {
            StopAllCoroutines();
            SetAlpha(rightArrow, 0);
        }
    }

    IEnumerator Animate(Image image)
    {
        int blinks = 5;
        float time = 0.075f;
        float alpha = image.color.a;

        for (int i = 0; i != blinks; ++i)
        {
            alpha = alpha == 1 ? 0 : 1;
            SetAlpha(image, alpha);
            yield return new WaitForSeconds(time);
        }

        SetAlpha(image, 1);
    }

    void SetAlpha(Image image, float a)
    {
        Color color = image.color;
        color.a = a;
        image.color = color;
    }
}
