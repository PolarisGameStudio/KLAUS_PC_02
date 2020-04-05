using UnityEngine;
using System.Collections;
using SmartLocalization.Editor;
using UnityEngine.UI;

public class SaveMessagesManager : MonoSingleton<SaveMessagesManager>
{
    public float[] positions;
    public Toggle[] gears;
    public float time = 0.2f;
    public float timeToShowMessage = 5f;

    public LocalizedTextMeshPro text
    {
        get
        {
            if (_text == null)
                _text = GetComponentInChildren<LocalizedTextMeshPro>();
            return _text;
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

    RectTransform _rectTransform;
    LocalizedTextMeshPro _text;
    int position = 0;
    bool important;

    [ContextMenu("ShowSave")]
    public void ShowSave()
    {
        gears[0].isOn = true;
        gears[1].isOn = false;

        if (position == 0)
            MoveToPosition(1);
    }
    [ContextMenu("HideSave")]
    public void HideSave()
    {
        if (position == 1)
            MoveToPosition(0);
        else
        {
            gears[1].isOn = true;
            gears[0].isOn = false;
        }
    }

    public void ShowMessage(string localizationKey, bool isImportant = false)
    {
        if (important && !isImportant)
            return;

        CancelInvoke("HideMessage");

        important = isImportant;
        text.UpdateKey(localizationKey);

        if (position == 0)
        {
            gears[1].isOn = true;
            gears[0].isOn = false;
        }

        MoveToPosition(2);

        Invoke("HideMessage", timeToShowMessage);
    }

    void HideMessage()
    {
        important = false;
        MoveToPosition(gears[0].isOn ? 1 : 0);
    }

    void MoveToPosition(int value)
    {
        position = Mathf.Clamp(value, 0, positions.Length - 1);
        LeanTween.moveX(rectTransform, positions[position], time);
    }
}
