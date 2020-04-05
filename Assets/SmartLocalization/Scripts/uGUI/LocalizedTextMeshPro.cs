
namespace SmartLocalization.Editor
{
    using UnityEngine;
    using System.Collections;
    using TMPro;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextMeshPro : MonoBehaviour
    {
        public string localizedKey = "INSERT_KEY_HERE";
        public TextMeshProUGUI textObject
        {
            get 
            {
                if (_obj == null)
                    _obj = this.GetComponent<TextMeshProUGUI>();
                return _obj;
            }
        }
        TextMeshProUGUI _obj;

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
            textObject.text = LanguageManager.Instance.GetTextValue(localizedKey) ?? localizedKey;


            if (textObject.text.Contains("sprite name=X") && InputEnum.GamePad.ToString() == "xbox 360")
            {
                textObject.SetText(textObject.text.Replace("sprite name=X", "sprite name=X-A"));
            }

            if (textObject.text.Contains("sprite name=Square") && InputEnum.GamePad.ToString() == "xbox 360")
            {
                textObject.SetText(textObject.text.Replace("sprite name=Square", "sprite name=X-X"));
            }

            if (textObject.text.Contains("sprite name=Triangle") && InputEnum.GamePad.ToString() == "xbox 360")
            {
                textObject.SetText(textObject.text.Replace("sprite name=Triangle", "sprite name=X-Y"));
            }

            if (textObject.text.Contains("sprite name=Circle") && InputEnum.GamePad.ToString() == "xbox 360")
            {
                textObject.SetText(textObject.text.Replace("sprite name=Circle", "sprite name=X-B"));
            }

        }

        public void UpdateKey(string key)
        {
            localizedKey = key;
            OnChangeLanguage(LanguageManager.Instance);
        }
    }
}