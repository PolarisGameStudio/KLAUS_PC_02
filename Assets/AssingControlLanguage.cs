using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired.UI.ControlMapper;

public class AssingControlLanguage : MonoBehaviour
{
    public LanguageData english;
    public LanguageData spanish;
    public LanguageData german;
    public LanguageData italian;
    public LanguageData french;
    public LanguageData portuguese;
    public LanguageData russian;
    string language = "en";
    public ControlMapper controlMapper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(controlMapper!=null)
        { 
            if(SaveManager.Instance.dataKlaus.language!=language)
            {
                if(SaveManager.Instance.dataKlaus.language.Contains("en"))
                {
                    controlMapper.language = english;
                }

                else if (SaveManager.Instance.dataKlaus.language.Contains("de"))
                {
                    controlMapper.language = german;
                }

                else if (SaveManager.Instance.dataKlaus.language.Contains("it"))
                {
                    controlMapper.language = italian;
                }

                else if (SaveManager.Instance.dataKlaus.language.Contains("fr"))
                {
                    controlMapper.language = french;
                }

                else if (SaveManager.Instance.dataKlaus.language.Contains("es"))
                {
                    controlMapper.language = spanish;
                }

                else if (SaveManager.Instance.dataKlaus.language.Contains("pt"))
                {
                    controlMapper.language = portuguese;
                }
                else if (SaveManager.Instance.dataKlaus.language.Contains("ru"))
                {
                    controlMapper.language = russian;
                }

                language = SaveManager.Instance.dataKlaus.language;

            }
        }
    }
}
