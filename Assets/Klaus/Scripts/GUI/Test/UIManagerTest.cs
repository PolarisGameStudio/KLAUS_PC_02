using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIManagerTest : PersistentSingleton<UIManagerTest>
{

    public Toggle PauseToggle;
    public Toggle HUDToggle;
    public GameObject firstbutton;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        ManagerPause.SubscribeOnPauseGame(onPauseGame);
        ManagerPause.SubscribeOnResumeGame(onResumeGame);
    }
    void OnLevelWasLoaded(int level)
    {
        onResumeGame();

        if (level > 1)
        {
            ManagerPause.SubscribeOnPauseGame(onPauseGame);
            ManagerPause.SubscribeOnResumeGame(onResumeGame);
        }
    }
    
    // Update is called once per frame
    public void onPauseGame()
    {
        HUDToggle.interactable = false;
        HUDToggle.isOn = false;


        PauseToggle.interactable = true;
        PauseToggle.isOn = true;
        EventSystem.current.SetSelectedGameObject(firstbutton);


    }
    public void onResumeGame()
    {
        PauseToggle.interactable = false;
        PauseToggle.isOn = false;
        HUDToggle.interactable = true;
        HUDToggle.isOn = true;
    }
}
