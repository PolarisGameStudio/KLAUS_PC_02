namespace SmartLocalization.Editor
{
    using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetLanguageDropDown : MonoBehaviour {

    Dropdown dropdown;
    List<string> m_DropOptions;
    static string[] languages = {"de", "en", "es", "es-MX", "fr", "it", "pt", "pt-PT" };
    public CanvasGroup option;

    // Use this for initialization
    void Start () {

            m_DropOptions = new List<string> {"DEUTSCH", "ENGLISH", "ESPANOL", "ESPANOL (LATAM)", "FRANÇAIS", "ITALIANO", "PORTUGUÊS", "PORTUGUÊS (BR)"};


            dropdown = gameObject.GetComponent(typeof(Dropdown)) as Dropdown;
            dropdown.AddOptions(m_DropOptions);
            SetDefaultValue();

            dropdown.interactable = false;

        }

    void SetDefaultValue()
        {
            SmartCultureInfo deviceCulture = LanguageManager.Instance.GetDeviceCultureIfSupported();
            string currentLanguage = deviceCulture.languageCode;
            Debug.Log("Current Language " + currentLanguage);

            int value = 0;

            for(int i=0; i<languages.Length;i++)
            {
                if (languages[i] == currentLanguage)
                    value = i;

            }

            dropdown.value = value;

        }

    public void SetLanguage()
    {
            SaveManager.Instance.dataKlaus.language = languages[dropdown.value];
            SaveManager.Instance.Save();
            LanguageManager.Instance.ChangeLanguage(languages[dropdown.value]);
            dropdown.interactable = false;


        }
	
	// Update is called once per frame
	void Update () {
    


        }

        public void SetInteractable()
        {
            dropdown.interactable = true;
        }


        public static string ConvertToValidLanguage(string language)
        {
            // Si el lenguaje es vacio, retorna el lenguaje por defecto (en)
            if (string.IsNullOrEmpty(language))
                return languages[0];

            // Normaliza el lenguaje
            language = language.ToLower();

            // Si es portugues, haz que sea el portugues que esta disponible
            if (language.Contains("pt"))
                language = languages[4];

            // Si es espanol, que sea el espanol disponible
            else if (language.Contains("es"))
                language = languages[6];

            // Si tras todo esto el lenguaje no se encuentra en la lista de los permitidos, pon el lenguaje por defecto (en)
            if (string.IsNullOrEmpty(Array.Find<string>(languages, x => x.Equals(language))))
                language = languages[0];

            return language;
        }
    }
}
