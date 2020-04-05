using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class AS_SliderSensitivity : MonoBehaviour
{

    public void OnValueChanged(float value)
    {
        // SaveManager.Instance.dataKlaus.controlSensitivity = value;

        SaveManager.Instance.dataKlaus.controlSensitivity = (-value + 1);
        // AudioListener.volume = value;
    }
}
