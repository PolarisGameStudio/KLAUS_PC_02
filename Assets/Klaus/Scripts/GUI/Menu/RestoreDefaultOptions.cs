using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class RestoreDefaultOptions : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public Toggle vibrationToggle, fullscreen;
    public SetResolutionDropdown resolution;

    public AudioSource audioSource
    {
        get
        {
            if (_audioSource == null)
                _audioSource = GameObject.Find("AS_ButtonDown").GetComponent<AudioSource>();
            return _audioSource;
        }
    }

    AudioSource _audioSource;


    void Update()
    {
        if (IsPressed())
        {
            audioSource.Play();
            RestoreOptions();
        }
    }

    public void RestoreOptions()
    {
        musicSlider.value = 1;
        sfxSlider.value = 1;
        vibrationToggle.isOn = true;
        fullscreen.isOn = true;
        resolution.SetDefault();
        resolution.SetResolution();
       // Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true, 60);
    }

    public bool IsPressed()
    {
        return ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputActionOld.UI_Restore));
    }
}
