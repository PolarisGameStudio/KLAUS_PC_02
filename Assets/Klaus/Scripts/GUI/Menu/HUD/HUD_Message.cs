using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using TMPro;

public class HUD_Message : Singleton<HUD_Message> {

    public CanvasGroup canvas;
    public TextMeshProUGUI message;
    public RectTransform rect;
    public float MoveDown = 100;
    public float TimeShow = 3;
    public float TimeToReachShow = 0.5f;

    protected Tweener tween;
    bool showing = false;
    float timeAux = 0;
    bool isActive = false;
    public void Show(string text, float timeS = 0) {
        if (isActive)
            return;

        timeAux = TimeShow;
        if (timeS > 0)
            timeAux = timeS;

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -MoveDown);




        message.text = text;
        canvas.alpha = 1;
        isActive = true;
        showing = true;
        message.SetText(convertButtons(message.text));
        tween = DOTween.To(SetPos, -MoveDown, 0, TimeToReachShow);


        tween = tween.OnComplete(OnComplete);
    }

    public void ShowWithOutTime(string text) {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -MoveDown);
        message.text = text;
        message.SetText(convertButtons(message.text));
        canvas.alpha = 1;
        isActive = true;
        showing = true;
        tween = DOTween.To(SetPos, -MoveDown, 0, TimeToReachShow);
        tween = tween.OnComplete(OnCompleteStatic);
    }

    public void HideText() {
        showing = false;
        StopCoroutine("HideMessage");
        StartCoroutine("HideMessage");
    }

    void OnCompleteStatic() {
        tween.Kill();
        showing = false;
    }

    void OnComplete() {
        tween.Kill();

        if (showing) {
            HideText();
        } else {
            Hide();
        }
    }

    IEnumerator HideMessage() {
        yield return new WaitForSeconds(timeAux);
        tween = DOTween.To(SetPos, 0, -MoveDown, TimeToReachShow);
        tween = tween.OnComplete(OnComplete);
    }

    void SetPos(float value) {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, value);
    }

    void Hide() {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -MoveDown);
        message.text = "";
        canvas.alpha = 0;
        isActive = false;
    }

    public string convertButtons(string message)
    {
        if (message.Contains("sprite name=X") && InputEnum.GamePad.ToString() == "xbox 360")
        {
            message = (message.Replace("sprite name=X", "sprite name=X-A"));
        }

        if (message.Contains("sprite name=Square") && InputEnum.GamePad.ToString() == "xbox 360")
        {
            message = (message.Replace("sprite name=Square", "sprite name=X-X"));
        }

        if (message.Contains("sprite name=Triangle") && InputEnum.GamePad.ToString() == "xbox 360")
        {
            message = (message.Replace("sprite name=Triangle", "sprite name=X-Y"));
        }

        if (message.Contains("sprite name=Circle") && InputEnum.GamePad.ToString() == "xbox 360")
        {
            message = (message.Replace("sprite name=Circle", "sprite name=X-B"));
        }

        return message;

    }
}
