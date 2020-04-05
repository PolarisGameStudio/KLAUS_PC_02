using UnityEngine;
using TMPro;

public class FontSetter : MonoBehaviour
{
    TextMeshPro textProObject
    {
        get
        {
            if (_proObj == null)
                _proObj = this.GetComponentInChildren<TextMeshPro>();
            return _proObj;
        }
    }

    TextMeshPro _proObj;

    TextMesh textMesh
    {
        get
        {
            if (_textMesh == null)
                _textMesh = this.GetComponentInParent<TextMesh>();
            return _textMesh;
        }
    }

    TextMesh _textMesh;

    FontsManager.FontType fontType;

    void Start()
    {
        if (FontsManager.Instance != null)
        {
            // Get font type
            fontType = FontsManager.Instance.GetFontType(textMesh != null && textMesh.font != null ? textMesh.font.name : string.Empty);

            // Get font
            FontsManager.Instance.onFontsUpdated += UpdateFont;
            UpdateFont();
        }
    }

    void OnDestroy()
    {
        if (FontsManager.InstanceExists())
            FontsManager.Instance.onFontsUpdated -= UpdateFont;
    }

    void UpdateFont()
    {
        if (FontsManager.Instance != null)
        {
            if (FontsManager.Instance.Fonts != null && FontsManager.Instance.Fonts.Length > (int)fontType)
                textProObject.font = FontsManager.Instance.Fonts[(int)fontType];

            if (FontsManager.Instance.RegularFonts != null && FontsManager.Instance.RegularFonts.Length > (int)fontType)
                textMesh.font = FontsManager.Instance.RegularFonts[(int)fontType];
        }
    }
}
