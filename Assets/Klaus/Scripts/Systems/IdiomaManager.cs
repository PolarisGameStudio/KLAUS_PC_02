using UnityEngine;
using SmartLocalization;
using System.Collections.Generic;

public class IdiomaManager : MonoBehaviour
{
    private LanguageManager languageManager;
    private Dictionary<string,string> currentLanguageValues;
    private List<SmartCultureInfo> availableLanguages;

    void Start()
    {
        languageManager = LanguageManager.Instance;

        SmartCultureInfo systemLanguage = languageManager.GetSupportedSystemLanguage();
        if (systemLanguage != null)
        {
            languageManager.ChangeLanguage(systemLanguage); 
        }

        if (languageManager.NumberOfSupportedLanguages > 0)
        {
            currentLanguageValues = languageManager.RawTextDatabase;
            availableLanguages = languageManager.GetSupportedLanguages();
        } else
        {
            Debug.LogError("No languages are created!, Open the Smart Localization plugin at Window->Smart Localization and create your language!");
        }

        LanguageManager.Instance.OnChangeLanguage += OnLanguageChanged;
    }

    void OnDestroy()
    {
        if (LanguageManager.HasInstance)
        {
            LanguageManager.Instance.OnChangeLanguage -= OnLanguageChanged;
        }
    }

    void OnLanguageChanged(LanguageManager languageManager)
    {
        currentLanguageValues = languageManager.RawTextDatabase;
    }
}
