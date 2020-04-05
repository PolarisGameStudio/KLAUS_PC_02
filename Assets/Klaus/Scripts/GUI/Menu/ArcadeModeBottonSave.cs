using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ArcadeModeBottonSave : MonoBehaviour
{
    Button _button;

    public Button button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();
            return _button;
        }
    }
    void Start()
    {
        OnGameLoaded();
    }
    void OnEnable()
    {
        SaveManager.onGameLoaded += OnGameLoaded;
    }
    void OnDisable()
    {
        SaveManager.onGameLoaded -= OnGameLoaded;

    }
    void OnGameLoaded()
    {
        if (SaveManager.Instance.dataKlaus.isArcadeModeUnlock)
        {
            button.interactable = true;
        }
    }
}
