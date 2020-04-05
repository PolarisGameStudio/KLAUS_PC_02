using UnityEngine;
using UnityEngine.UI;

public class AS_AudioListenerVol : MonoBehaviour
{
    public void OnEnable()
    {
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            AudioListener.volume = SaveManager.Instance.dataKlaus.fxVolume;
    }
}
