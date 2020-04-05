using UnityEngine;
using System.Collections;
using Colorful;

public class W5ResetDead : MonoBehaviour
{
    public GameObject K1;
    DeadState deadKlaus;

    public DeadState deadKlausState
    {
        get
        {
            if (deadKlaus == null)
                deadKlaus = GameObject.FindObjectOfType <DeadState>();
            return deadKlaus;
        }
    }

    Glitch _fx_Glitch;
    RGBSplit _fx_RGB;

    public Glitch fx_Glitch
    {
        get
        {
            if (_fx_Glitch == null)
                _fx_Glitch = Camera.main.GetComponent<Glitch>();

            return _fx_Glitch;
        }
    }

    public RGBSplit fx_RGB
    {
        get
        {
            if (_fx_RGB == null)
                _fx_RGB = Camera.main.GetComponent<RGBSplit>();

            return _fx_RGB;
        }
    }

    public Vector2 EverySecondNew = new Vector2(0, 0.01f);
    public Vector2 ForSecondNew = new Vector2(0.1f, 0.2f);
    public Glitch.InterferenceSettings InterferenceSetting;
    public Glitch.TearingSettings TearingSetting;
    public float AmountRGB = 8;
    Vector2 store_EverySecondNew = new Vector2(0.1f, 0.3f);
    Vector2 store_ForSecondNew = new Vector2(0.1f, 0.2f);
    Glitch.InterferenceSettings store_InterferenceSetting;
    Glitch.TearingSettings store_TearingSetting;
    float store_AmountRGB = 8;

    protected float TimetoChange = 0.3f;


    // Use this for initialization
    void OnEnable()
    {

        deadKlausState.SuscribeOnStart(OnDead);
        //  deadKlausState.onRespawn += OnDead;

    }

    void OnDisable()
    {
        if (deadKlausState != null)
            deadKlausState.UnSuscribeOnStart(OnDead);
        //deadKlausState.onRespawn -= OnDead;
    }


    void OnDead()
    {
        store_EverySecondNew = fx_Glitch.RandomEvery;
        store_ForSecondNew = fx_Glitch.RandomDuration;
        store_InterferenceSetting = fx_Glitch.SettingsInterferences;
        store_TearingSetting = fx_Glitch.SettingsTearing;
        store_AmountRGB = fx_RGB.Amount;
   //     fx_Glitch.IsActive = false;
        fx_Glitch.RandomEvery = EverySecondNew;
        fx_Glitch.RandomDuration = ForSecondNew;
        fx_Glitch.SettingsInterferences = InterferenceSetting;
        fx_Glitch.SettingsTearing = TearingSetting;
  //      fx_Glitch.IsActive = true;
        fx_RGB.Amount = AmountRGB;
        Invoke("StopGlitchEffect", TimetoChange);

        K1.SetActive(false);
    }

    void StopGlitchEffect()
    {
        K1.SetActive(true);
    //    fx_Glitch.IsActive = false;
        fx_Glitch.RandomEvery = store_EverySecondNew;
        fx_Glitch.RandomDuration = store_ForSecondNew;
        fx_Glitch.SettingsInterferences = store_InterferenceSetting;
        fx_Glitch.SettingsTearing = store_TearingSetting;
    //    fx_Glitch.IsActive = true;
        fx_RGB.Amount = store_AmountRGB;


    }

}
