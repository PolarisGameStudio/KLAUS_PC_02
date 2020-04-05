using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class AS_SliderVolumeUI : MonoBehaviour
{
    public IEnumerator Start()
    {
        yield return null;

        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            AudioListener.volume = SaveManager.Instance.dataKlaus.fxVolume;
    }

    public void OnValueChanged(float value)
    {
        AudioListener.volume = value;
    }
}
