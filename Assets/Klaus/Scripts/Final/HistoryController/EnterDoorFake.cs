using UnityEngine;
using System.Collections;
using Colorful;

public class EnterDoorFake : MonoBehaviour
{

    public float TimeGlitching = 0.5f;

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
    //public float Interference_Speed = 28;
    //public float Interference_Density = 22;
    //public float Interference_MaxDisplacement = 16;
    //public float Tearing_Speed = 0.8f;
    //public bool AllowFlipping = true;
    //public float Tearing_Intensity = 0.75f;
    //public float Tearing_MaxDisplacement = 0.35f;

    Vector2 store_EverySecondNew = new Vector2(0.1f, 0.3f);
    Vector2 store_ForSecondNew = new Vector2(0.1f, 0.2f);
    Glitch.InterferenceSettings store_InterferenceSetting;
    Glitch.TearingSettings store_TearingSetting;
    float store_AmountRGB = 8;


    bool isIn = false;
    public bool isFinalDoor = true;
    public GameObject RealDoor;
    public float TimeToLook = 1.5f;

    public bool needCollectAll = false;
    EnterDoorFake[] doors;

    void OnEnable()
    {
        store_EverySecondNew = fx_Glitch.RandomEvery;
        store_ForSecondNew = fx_Glitch.RandomDuration;
        store_InterferenceSetting = fx_Glitch.SettingsInterferences;
        store_TearingSetting = fx_Glitch.SettingsTearing;
        store_AmountRGB = fx_RGB.Amount;
        fx_Glitch.RandomActivation=false;
    }

    void StopGlitchEffect()
    {
   //     fx_Glitch.IsActive = false;
        fx_Glitch.RandomEvery = store_EverySecondNew;
        fx_Glitch.RandomDuration = store_ForSecondNew;
        fx_Glitch.SettingsInterferences = store_InterferenceSetting;
        fx_Glitch.SettingsTearing = store_TearingSetting;
        fx_RGB.Amount = store_AmountRGB;
 //       fx_Glitch.IsActive = true;

        RevealRealDoor();
    }

    void RevealRealDoor()
    {
        if (needCollectAll)
        {
            if (doors == null)
            {
                doors = GameObject.FindObjectsOfType<EnterDoorFake>();
            }
            bool canShow = true;
            for (int i = 0; i < doors.Length; ++i)
            {
                if (doors [i].gameObject.activeSelf && doors [i] != this)
                {
                    canShow = canShow && !doors [i].gameObject.activeSelf;
                }
            }
            if (canShow)
            {
                RealDoor.SetActive(true);
                CameraFollow.Instance.ChangueTargetOnly(RealDoor.transform, TimeToLook);
            }
        } else
        {
            RealDoor.SetActive(true);
            if (isFinalDoor)
            {
                CameraFollow.Instance.ChangueTargetOnly(RealDoor.transform, TimeToLook);
            }
            
        }

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ManageIn(other);
    }

    void ManageIn(Collider2D other)
    {
        if (!isIn && gameObject.activeSelf)
        {
            if (CompareDefinition(other))
            {

     //           fx_Glitch.IsActive = false;
                fx_Glitch.RandomEvery = EverySecondNew;
                fx_Glitch.RandomDuration = ForSecondNew;
                fx_Glitch.SettingsInterferences = InterferenceSetting;
                fx_Glitch.SettingsTearing = TearingSetting;
                fx_RGB.Amount = AmountRGB;
         //       fx_Glitch.IsActive = true;

                Invoke("StopGlitchEffect", TimeGlitching);
                isIn = true;
            }
        }
    }

    protected virtual bool CompareDefinition(Collider2D other)
    {
        return other.CompareTag("Player");
    }
}
