using UnityEngine;
using UnityEngine.UI;

public class CheckOptionFullScreen : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
   //     Debug.LogError("Fullscreen " + SaveManager.Instance.dataKlaus.fullscreen.ToString());
        OnGameLoaded();
    }

    void OnEnable()
    {
        if (SaveManager.HasInstance)
            SaveManager.onGameLoaded += OnGameLoaded;
    }

    void OnDisable()
    {
        if (SaveManager.HasInstance)
            SaveManager.onGameLoaded -= OnGameLoaded;
    }

    void OnGameLoaded()
    {
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            toggle.isOn = SaveManager.Instance.dataKlaus.fullscreen;
    }

    public void ToggleUpdate()
    {
            bool value = toggle.isOn;

          //  Debug.Log("Toggle is "+toggle.isOn.ToString());
            SaveManager.Instance.dataKlaus.fullscreen = value;

            Screen.SetResolution(Screen.width, Screen.height, value);
        //    Debug.LogError("Fullscreen "+SaveManager.Instance.dataKlaus.fullscreen);

    }
}
