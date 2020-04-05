using UnityEngine;
using UnityEngine.UI;

public class CheckOptionVibration : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
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
            toggle.isOn = SaveManager.Instance.dataKlaus.Rumble;
    }

    public void OnToggleUpdate(bool value)
    {
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.Rumble = value;
    }
}
