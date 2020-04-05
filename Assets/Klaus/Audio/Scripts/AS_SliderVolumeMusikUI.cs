using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AS_SliderVolumeMusikUI : MonoBehaviour
{
    public float ValueTExt { set { GameObject.Find("AS_MusikManager_Arrecho").GetComponent<AudioSource>().volume = value; } }

    public void Start()
    {
        var musikArrecho = GameObject.Find("AS_MusikManager_Arrecho");
        musikArrecho.GetComponent<AudioSource>().volume = SaveManager.Instance.dataKlaus.bgVolume;

        //this.gameObject.GetComponent<Slider>().value = GameObject.Find("AS_MusikManager_Arrecho").GetComponent<AudioSource>().volume;
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            GetComponent<Slider>().value = SaveManager.Instance.dataKlaus.bgVolume;

        if (GameObject.Find("AS_PrincipalMenuMusik") != null)
        {
            GameObject.Find("AS_PrincipalMenuMusik").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
        else if (GameObject.Find("AS_W1BossMusik") != null)
        {
            GameObject.Find("AS_W1BossMusik").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
        else if (GameObject.Find("AS_W4BossMusik") != null)
        {
            GameObject.Find("AS_W4BossMusik").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
        else if (GameObject.Find("MusikFloating") != null)
        {
            GameObject.Find("MusikFloating").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
        else if (GameObject.Find("AS_W6BossMusik") != null)
        {
            GameObject.Find("AS_W6BossMusik").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
        else if (GameObject.Find("AS_W5BossMusik") != null)
        {
            GameObject.Find("AS_W6BossMusik").GetComponent<AudioSource>().volume = this.gameObject.GetComponent<Slider>().value;
        }
    }

    public void OnValueChanged(float value)
    {
        if (SaveManager.HasInstance && SaveManager.Instance.dataKlaus != null)
            SaveManager.Instance.dataKlaus.bgVolume = value;
        var musikArrecho = GameObject.Find("AS_MusikManager_Arrecho");
        if (musikArrecho == null)
            return;
        musikArrecho.GetComponent<AudioSource>().volume = value;

        if (GameObject.Find("AS_W1BossMusik") != null)
        {
            AudioSource[] aSources = GameObject.Find("AS_W1BossMusik").GetComponents<AudioSource>();
            for (int i = 0; i < aSources.Length; i++)
            {
                aSources[i].volume = value;
            }
        }

        if (GameObject.Find("AS_W4BossMusik") != null)
        {
            AudioSource[] aSources = GameObject.Find("AS_W4BossMusik").GetComponents<AudioSource>();
            for (int i = 0; i < aSources.Length; i++)
            {
                aSources[i].volume = value;
            }
        }

        if (GameObject.Find("AS_W6BossMusik") != null)
        {
            AudioSource[] aSources = GameObject.Find("AS_W6BossMusik").GetComponents<AudioSource>();
            for (int i = 0; i < aSources.Length; i++)
            {
                aSources[i].volume = value;
            }
        }

        if (GameObject.Find("AS_W5BossMusik") != null)
        {
            AudioSource[] aSources = GameObject.Find("AS_W5BossMusik").GetComponents<AudioSource>();
            for (int i = 0; i < aSources.Length; i++)
            {
                aSources[i].volume = value;
            }
        }

        if (GameObject.Find("MusikFloating") != null)
        {
            GameObject.Find("MusikFloating").GetComponent<AudioSource>().volume = value;
        }

        if (GameObject.Find("AS_PrincipalMenuMusik") != null)
        {
            GameObject.Find("AS_PrincipalMenuMusik").GetComponent<AudioSource>().volume = value;
        }
    }
}
