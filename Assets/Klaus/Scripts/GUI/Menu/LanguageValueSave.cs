using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LanguageValueSave : MonoBehaviour
{
    public Slider slide;
    public GameObject left, right;
    public CanvasGroup loading;
    public InGameSubPanel panel;

    static string[] languages = { "en", "fr", "de", "it", "pt", "ru", "la", "po", "es" };
    //static string[] languages = { "en", "fr", "de", "it", "pt-PT", "ru", "es", };

    void Start()
    {
        slide.minValue = 0;
        slide.maxValue = languages.Length - 1;

        string language = languages[0];

        // Si existen las instancias, usa el lenguaje en memoria
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            language = ConvertToValidLanguage(SaveManager.Instance.dataKlaus.language);

        slide.value = Array.FindIndex<string>(languages, x => x == language);

        // Set values
        int index = Mathf.Clamp((int)slide.value, 0, languages.Length);
        SaveManager.Instance.dataKlaus.language = languages[index];

        left.SetActive(index > 0);
        right.SetActive(index < languages.Length - 1);
    }

    public void SetValue(float value)
    {
        int index = Mathf.Clamp((int)value, 0, languages.Length);
        SaveManager.Instance.dataKlaus.language = languages[index];

        left.SetActive(index > 0);
        right.SetActive(index < languages.Length - 1);

        if (panel.canvasGroup.interactable)
            StartCoroutine("ChangeLanguage");
    }

    IEnumerator ChangeLanguage()
    {
        loading.alpha = 1f;
        panel.ToggleInteractions(false);
        yield return null;

        UpdateLanguage();

        yield return null;
        panel.ToggleInteractions(true);
        loading.alpha = 0f;
    }

    public static void UpdateLanguage()
    {
        string language = ConvertToValidLanguage(SaveManager.Instance.dataKlaus.language);

        // Si existen las instancias adecuadas, guarda el lenguaje
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.language = language;
        
        SmartLocalization.LanguageManager.Instance.ChangeLanguage(language);
    }

    public static void InitLanguage()
    {
        // Si existen las instancias adecuadas, guarda el lenguaje
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.language = ConvertToValidLanguage(SmartLocalization.LanguageManager.Instance.GetSupportedSystemLanguageCode());
    }

    public static string ConvertToValidLanguage(string language)
    {
        // Si el lenguaje es vacio, retorna el lenguaje por defecto (en)
        if (string.IsNullOrEmpty(language))
            return languages[0];

        // Normaliza el lenguaje
        language = language.ToLower();

        // Si es portugues, haz que sea el portugues que esta disponible
        /*/
        if (language.Contains("pt"))
            language = languages[4];

        // Si es espanol, que sea el espanol disponible
        else if (language.Contains("es"))
            language = languages[6];
            /*/

        // Si tras todo esto el lenguaje no se encuentra en la lista de los permitidos, pon el lenguaje por defecto (en)
        if (string.IsNullOrEmpty(Array.Find<string>(languages, x => x.Equals(language))))
            language = languages[0];

        return language;
    }
}
