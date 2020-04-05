namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        public string localizedKey = "INSERT_KEY_HERE";
        Text textObject;
        public string ObjectText
        {
            get
            {

                if (textObject == null)
                    textObject = this.GetComponent<Text>();

                return textObject.text;
            }
            set
            {
                if (textObject == null)
                    textObject = this.GetComponent<Text>();

                textObject.text = value;
            }
        }
        void Start()
        {
            //Subscribe to the change language event
            LanguageManager languageManager = LanguageManager.Instance;
            languageManager.OnChangeLanguage += OnChangeLanguage;

            //Run the method one first time
            OnChangeLanguage(languageManager);
        }

        void OnDestroy()
        {
            if (LanguageManager.HasInstance)
            {
                LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
            }
        }

        void OnChangeLanguage(LanguageManager languageManager)
        {
            ObjectText = LanguageManager.Instance.GetTextValue(localizedKey) ?? localizedKey;
        }
        public void UpdateKey(string key)
        {
            localizedKey = key;

            OnChangeLanguage(LanguageManager.Instance);

        }
    }
}