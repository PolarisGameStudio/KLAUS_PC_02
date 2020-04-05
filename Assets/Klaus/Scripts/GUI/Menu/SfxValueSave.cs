using UnityEngine;
using UnityEngine.UI;

public class SfxValueSave : MonoBehaviour
{
    public Slider slide;

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
            slide.value = SaveManager.Instance.dataKlaus.fxVolume;
    }

    public void SetValue(float value)
    {
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.fxVolume = value;
    }
}
