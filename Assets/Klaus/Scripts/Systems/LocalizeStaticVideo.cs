using UnityEngine;
using System.Collections;
using SmartLocalization;

public class LocalizeStaticVideo : MonoBehaviour
{

    public string KeyID = "";
    public VideoPlayerScene videPlayer;
    // Use this for initialization
    void Awake()
    {
        videPlayer = GetComponent<VideoPlayerScene>();
        videPlayer.WaitForKey = true;
    }
    void Start()
    {
        OnChangeLanguage(LanguageManager.Instance);
    }

    void OnChangeLanguage(LanguageManager languageManager)
    {
        //Initialize all your language specific variables here
        videPlayer.videoName = LanguageManager.Instance.GetTextValue(KeyID) ?? KeyID;
        videPlayer.WaitForKey = false;
    }
}
