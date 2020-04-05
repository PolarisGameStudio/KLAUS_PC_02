using TMPro;


namespace SmartLocalization.Editor
{
    using UnityEngine;
    using System.Collections;

    public class LocalizedTextMesh : MonoBehaviour
    {
        public string localizedKey = "INSERT_KEY_HERE";
        TextMesh textObject
        {
            get 
            {
                if (_obj == null)
                    _obj = this.GetComponent<TextMesh>();
                return _obj;
            }
        }
        TextMesh _obj;

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
            if (textObject) textObject.text = LanguageManager.Instance.GetTextValue(localizedKey) ?? textObject.text;
            if (textProObject) textProObject.text = LanguageManager.Instance.GetTextValue(localizedKey) ?? textProObject.text;
        }

        public void UpdateKey(string key)
        {
            localizedKey = key;
            OnChangeLanguage(LanguageManager.Instance);
        }
    }
}