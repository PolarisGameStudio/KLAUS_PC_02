using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeLevelSelector : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Text timeAttackText, collectablesText;

    public Image background;
    public GameObject noise;
    public GameObject clock;
    public GameObject document;
    public bool back = false;
    public float duration;

    public void ResetSelector()
    {
        canvasGroup.alpha = 0;
    }

    public void Setup(RectTransform rectTransform, Color color, bool glitchEnabled, string timeAttackValue, string collectablesValue)
    {
        canvasGroup.alpha = 1;

        background.rectTransform.anchoredPosition = rectTransform.anchoredPosition;
        background.rectTransform.sizeDelta = rectTransform.sizeDelta;
        background.color = color;

        timeAttackText.text = timeAttackValue;
        collectablesText.text = collectablesValue;

        noise.SetActive(glitchEnabled);
    }

    public IEnumerator Blink()
    {
        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 1;
    }

    public void HideSprites()
    {
        if (clock.active == true)
        {
            clock.SetActive(false);
            document.SetActive(false);
        }

    }

    public void RestoreSprites()
    {
        if (clock.active == false)
        {
            clock.SetActive(true);
            document.SetActive(true);
        }

    }

}
