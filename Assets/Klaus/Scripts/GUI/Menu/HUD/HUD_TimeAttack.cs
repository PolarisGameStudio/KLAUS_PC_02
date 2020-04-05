using System;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization.Editor;

public class HUD_TimeAttack : MonoBehaviour
{
    public Animator animator;
    public Text[] currentTimeTexts;
    public Text[] bestTimeTexts;
    public CanvasGroup[] newRecords;

    public void Show()
    {
        animator.SetBool("Show", true);
    }

    public bool ShowLevelCompleted()
    {
        return animator.GetBool("Show");
    }

    public void Hide()
    {
        animator.SetBool("Show", false);
    }

    public void OnNewRecord()
    {
        for (int i = 0; i != newRecords.Length; ++i)
            newRecords[i].alpha = 1;
    }

    public void SetBestTime(float time)
    {
        for (int i = 0; i != bestTimeTexts.Length; ++i)
            SetTime(bestTimeTexts[i], time, false);
    }

    public void SetCurrentTime(float time)
    {
        for (int i = 0; i != currentTimeTexts.Length; ++i)
            SetTime(currentTimeTexts[i], time, true);
    }

    void SetTime(Text text, float time, bool canBeZero)
    {
        if (time == 0f && !canBeZero)
        {
            text.text = "-";
            return;
        }

        text.text = time == 0f && !canBeZero ? "-" : FormatTime(time);
    }

    public static string FormatTime(float time)
    {
        string result;

        TimeSpan t = TimeSpan.FromSeconds(time);
        result = string.Format("{0:D2}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds);

        if (t.Hours != 0)
            result = string.Format("{0:D2}:", t.Hours) + result;

        return result;
    }
}
