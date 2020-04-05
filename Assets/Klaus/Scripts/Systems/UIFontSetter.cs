using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIFontSetter : MonoBehaviour
{
    TextMeshProUGUI textProObject
    {
        get 
        {
            if (_proObj == null)
                _proObj = this.GetComponentInChildren<TextMeshProUGUI>();
            return _proObj;
        }
    }

    Text textUI
    {
        get 
        {
            if (_textUI == null)
                _textUI = this.GetComponentInChildren<Text>();
            return _textUI;
        }
    }

    Text _textUI;
    TextMeshProUGUI _proObj;
    FontsManager.FontType fontType;

    void Awake()
    {
        // Get font type
        if (textProObject != null)
            fontType = FontsManager.Instance.GetFontType(textProObject.font.name);
        else if (textUI != null)
            fontType = FontsManager.Instance.GetFontType(textUI.font.name);

        // Get font
        FontsManager.Instance.onFontsUpdated += UpdateFont;
        UpdateFont();
    }

    void OnDestroy()
    {
        if (FontsManager.InstanceExists())
            FontsManager.Instance.onFontsUpdated -= UpdateFont;
    }

    void UpdateFont()
    {
        if (textProObject != null && FontsManager.Instance.Fonts != null && FontsManager.Instance.Fonts.Length > (int)fontType)
            textProObject.font = FontsManager.Instance.Fonts[(int)fontType];
        
        if (textUI && FontsManager.Instance.RegularFonts != null && FontsManager.Instance.RegularFonts.Length > (int)fontType)
            textUI.font = FontsManager.Instance.RegularFonts[(int)fontType];
    }

    public void ChangeFontType(FontsManager.FontType type)
    {
        if (type != fontType)
        {
            fontType = type;
            UpdateFont();
        }
    }
}
