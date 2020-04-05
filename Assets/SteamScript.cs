using UnityEngine;
using System.Collections;
using Steamworks;
using UnityEngine.UI;
using System;

public class SteamScript : MonoBehaviour
{
    public Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    public CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
    
    static string[] languages = { "en", "fr", "de", "it", "pt", "ru", "la", "po", "es" };

    private bool m_bInitialized;



    void Start()
    {

            m_bInitialized = SteamAPI.Init();


        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }


        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
            
            string SteamLanguage = SteamApps.GetCurrentGameLanguage();
            Debug.Log("pase por aca");
            Debug.Log("Steam Language " + SteamLanguage);

            if(SteamLanguage=="english")
            {
                SaveManager.Instance.dataKlaus.language = "en";
            }

            if (SteamLanguage == "french")
            {
                SaveManager.Instance.dataKlaus.language = "fr";
            }

            if (SteamLanguage == "italian")
            {
                SaveManager.Instance.dataKlaus.language = "it";
            }

            if (SteamLanguage == "german")
            {
                SaveManager.Instance.dataKlaus.language = "de";
            }

            if (SteamLanguage == "spanish")
            {
                SaveManager.Instance.dataKlaus.language = "es";
            }

            if (SteamLanguage == "portuguese")
            {
                SaveManager.Instance.dataKlaus.language = "po";
                Debug.Log("It's set up to " + SaveManager.Instance.dataKlaus.language);
            }

            if (SteamLanguage == "brazilian")
            {
                SaveManager.Instance.dataKlaus.language = "pt";
            }

            if (SteamLanguage == "russian")
            {
                SaveManager.Instance.dataKlaus.language = "ru";
            }

            if (SteamLanguage == "latam")
            {
                SaveManager.Instance.dataKlaus.language = "la";
                Debug.Log("deberia ser latam");
            }



            string language = ConvertToValidLanguage(SaveManager.Instance.dataKlaus.language);

            // Si existen las instancias adecuadas, guarda el lenguaje
            if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
                SaveManager.Instance.dataKlaus.language = language;

            SmartLocalization.LanguageManager.Instance.ChangeLanguage(language);


            //   SaveManager.Instance.dataKlaus.language = languages[dropdown.value];
            SaveManager.Instance.Save();
          
        }
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

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
        }
      
    }

    private void Update()
    {/*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
            m_NumberOfCurrentPlayers.Set(handle);
            Debug.Log("Called GetNumberOfCurrentPlayers()");
            Debug.Log("It's set up to " + SaveManager.Instance.dataKlaus.language);
            SmartLocalization.LanguageManager.Instance.ChangeLanguage(SaveManager.Instance.dataKlaus.language);
            SaveManager.Instance.dataKlaus.language = "po";
        }
        /*/

    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");


              //  ManagerPause.Pause = true;
            
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");


            //    ManagerPause.Pause = false;
            
        }
    }

    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }
}
