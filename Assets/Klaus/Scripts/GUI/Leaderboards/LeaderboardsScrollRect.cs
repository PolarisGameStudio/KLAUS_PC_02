using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsScrollRect : MonoBehaviour
{
    public RectTransform container, parent, playerEntry;

    public void ResetScroll()
    {
        container.anchoredPosition = Vector2.zero;
    }

    public void SetSelected(RectTransform rect)
    {
        // Get the Y limits
        float min, max;
        Vector3[] corners = new Vector3[4];

        parent.GetWorldCorners(corners);
        min = corners[0].y;
        max = corners[2].y;

        playerEntry.GetWorldCorners(corners);

        if (corners[2].y >= max)
        {
            max = corners[0].y;
        }
        else if (corners[0].y <= min)
        {
            min = corners[2].y;
        }

        // Set position
        if (min > rect.position.y || rect.position.y > max)
        {
            Vector2 position = container.anchoredPosition;
            position.y += rect.position.y > max ? -rect.sizeDelta.y : rect.sizeDelta.y;
            container.anchoredPosition = position;
        }
    }
}
