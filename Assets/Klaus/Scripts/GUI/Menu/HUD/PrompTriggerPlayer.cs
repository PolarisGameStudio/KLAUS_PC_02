using UnityEngine;
using System.Collections;
using SmartLocalization;

public class PrompTriggerPlayer : MonoBehaviour
{

    public string ID_TextToShow = "";
    protected string TextToShow = "";
    public float TimeShow = 0;

    void Start()
    {
        //Subscribe to the change language event
        LanguageManager languageManager = LanguageManager.Instance;
        languageManager.OnChangeLanguage += OnChangeLanguage;

        //Run the method one first time
        OnChangeLanguage(languageManager);

    }
    void OnChangeLanguage(LanguageManager languageManager)
    {
        TextToShow = LanguageManager.Instance.GetTextValue(ID_TextToShow) ?? "NOT LOCALIZED";
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (CompareDefinition(other))
        {

            OnEnterAction(other);
        }


    }
    protected virtual void OnEnterAction(Collider2D other)
    {
        HUD_Message.Instance.Show(TextToShow, TimeShow);
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player");
    }
}
