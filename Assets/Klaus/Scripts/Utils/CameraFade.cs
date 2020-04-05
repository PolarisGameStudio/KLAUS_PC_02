using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class CameraFade : MonoSingleton<CameraFade>
{
    public Image image
    {
        get
        {
            if (!_image)
                _image = GetComponent<Image>();
            return _image;
        }
    }

    Image _image;
    Color m_TargetColor;
    float m_FadeDelay;
    float m_FadeDuration;
    public Action m_OnFadeFinish;

    public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration, float fadeDelay = 0)
    {
        Color startColor = newScreenOverlayColor;
        Color targetColor = newScreenOverlayColor;

        startColor.a = isFadeIn ? 1f : 0f;
        targetColor.a = isFadeIn ? 0f : 1f;

        Instance.FadeColor(startColor, targetColor, fadeDuration, fadeDelay);
    }

    void FadeColor(Color startColor, Color targetColor, float fadeDuration, float fadeDelay = 0)
    {
        // Cancel all invokes
        StopAllCoroutines();

        // Setup parameters
        m_TargetColor = targetColor;
        m_FadeDuration = fadeDuration;
        m_FadeDelay = fadeDelay;

        // Set start color
        image.color = startColor;

        // After certain delay, invoke 
        StartCoroutine(InvokeIgnoringTimescale(StartFade, m_FadeDelay));
    }



    void StartFade()
    {
        StartCoroutine(TweenColor(m_TargetColor, m_FadeDuration));
        StartCoroutine(InvokeIgnoringTimescale(FadeEnded, m_FadeDuration));
    }

    void FadeEnded()
    {

        if (m_OnFadeFinish != null)
            m_OnFadeFinish();
    }

    IEnumerator TweenColor(Color color, float duration)
    {
        float timer = duration;
        Color startColor = image.color;

        while (timer > 0.0f)
        {
            timer -= Time.unscaledDeltaTime;
            image.color = Color.Lerp(color, startColor, timer / duration);
            yield return null;
        }

        image.color = color;
    }

    IEnumerator InvokeIgnoringTimescale(Action function, float duration)
    {
        float timer = duration;
        while (timer > 0.0f)
        {
            timer -= Time.unscaledDeltaTime;
            yield return null;
        }

        if (function != null)
            function();
    }
}