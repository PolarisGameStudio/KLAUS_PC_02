using UnityEngine;
using SmartLocalization;
using TMPro;
using System;

public class FontsManager : MonoSingleton<FontsManager>
{
    public enum FontType
    {
        Klaus = 0,
        K1 = 1,
        OriginalKlaus = 2,
        MainMenu = 3,
        LevelCompleted = 4,
        Arcade = 5,
        CollectiblesAndSpecials = 6
    }

    public TMP_FontAsset[] Fonts { get; protected set; }

    public Font[] RegularFonts { get; protected set; }

    public int TotalFonts { get { return Enum.GetNames(typeof(FontType)).Length; } }

    public Action onFontsUpdated;

    static string basePath = "Fonts/";
    static string baseKey = "Dummy.Font.";
    static string textMeshProTerminal = " SDF";

    static string[] baseFonts = new string[]
    {
        "BebasNeue02",
        "BIG JOHN",
        "Hand_Of_Sean_Demo",
        "HelveticaNeueLTStd-Md",
        "HelveticaNeueLT-ExtBlackCond",
        "windows_command_prompt",
        "Futura",
        "Haettenschweiler",
        "PT Sans Narrow",
        "basis33",
        "BadScript-Regular"
    };

    static string[] defaultFonts = new string[]
    {
        "BebasNeue02",
        "Eagle",
        "Hand_Of_Sean_Demo",
        "HelveticaNeueLTStd-Md",
        "HelveticaNeueLT-ExtBlackCond",
        "windows_command_prompt",
        "Futura",
        "Haettenschweiler",
        "PT Sans Narrow",
        "basis33",
        "BadScript-Regular"
    };

    void Start()
    {
        Fonts = new TMP_FontAsset[TotalFonts];
        RegularFonts = new Font[TotalFonts];

        // Run the method one first time
        OnChangeLanguage(LanguageManager.Instance);
    }

    public void OnChangeLanguage(LanguageManager languageManager)
    {
        int totalTypes = Enum.GetNames(typeof(FontType)).Length;

        for (int i = 0; i != totalTypes; ++i)
            Fonts[i] = GetProFont(languageManager, baseKey + ((FontType)i).ToString(), defaultFonts[i]);

        for (int i = 0; i != totalTypes; ++i)
            RegularFonts[i] = GetFont(languageManager, baseKey + ((FontType)i).ToString(), defaultFonts[i]);

        if (onFontsUpdated != null)
            onFontsUpdated();
    }

    TMP_FontAsset GetProFont(LanguageManager languageManager, string key, string defaultValue)
    {
        string path = languageManager.GetTextValue(key);

        if (string.IsNullOrEmpty(path))
            Debug.LogError("FontsManager: Path for font empty. Key = " + key + ", Default = " + defaultValue);

        TMP_FontAsset font = Resources.Load<TMP_FontAsset>(basePath + (string.IsNullOrEmpty(path) ? defaultValue : path) + textMeshProTerminal);

        if (font == null)
            Debug.LogError("FontsManager: ProFont " + (basePath + (string.IsNullOrEmpty(path) ? defaultValue : path) + textMeshProTerminal) + " null");

        return font;
    }

    Font GetFont(LanguageManager languageManager, string key, string defaultValue)
    {
        string path = languageManager.GetTextValue(key);

        if (string.IsNullOrEmpty(path))
            Debug.LogError("FontsManager: Path for font empty, " + path);

        Font font = Resources.Load<Font>(basePath + (string.IsNullOrEmpty(path) ? defaultValue : path));

        if (font == null)
            Debug.LogError("FontsManager: Font " + (basePath + (string.IsNullOrEmpty(path) ? defaultValue : path)) + " null");

        return font;
    }

    public FontType GetFontType(string baseFont)
    {
        int index = Array.FindIndex<string>(baseFonts, x => baseFont.Contains(x));
        return index != -1 ? (FontType)index : FontType.Klaus;
    }
}
