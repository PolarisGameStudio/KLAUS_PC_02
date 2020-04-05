using UnityEngine;
using System.Collections;
using Colorful;

public class Glitch_DoorMimic : KillObject, ICurrentPlatform
{
    public Vector2 MaxSpeed = new Vector2(15, 15);

    public DeadState KlausDead;
    public MoveState KlausMove;

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


    Rigidbody2D currentPlatform;

    public Collider2D colliderDoor;


    Rigidbody2D _rig2D = null;

    public Rigidbody2D _rigidbody2D
    {

        get
        {

            if (_rig2D == null)
            {
                _rig2D = GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }

    }

    public static bool DeadByGamePlay = false;
    protected float currentTimePlayLevel = 0;
    public EnterDoorManager Door;
    PausaMenu_PopUp pausa;

    void Awake()
    {
        if (DeadByGamePlay)
        {
            Door.isStartLevel = false;
            Door.canStartAnalitic = false;
            Door.SetLevelCounterTimer(currentTimePlayLevel);
            DeadByGamePlay = false;
            currentTimePlayLevel = 0;
        }
        else
        {
            currentTimePlayLevel = 0;
        }
    }

    void Start()
    {
        Door.callbackEnterDoor = ResetValues;
        pausa = GameObject.FindObjectOfType<PausaMenu_PopUp>();
        pausa.callbackChangeLevel = ResetValues;
    }

    public void ResetValues()
    {
        DeadByGamePlay = false;
        currentTimePlayLevel = 0;
    }

    void OnEnable()
    {
        if (object.ReferenceEquals(KlausDead, null))
        {
            KlausDead = GameObject.FindObjectOfType<DeadState>();
            KlausMove = GameObject.FindObjectOfType<MoveState>();

        }
        KlausDead.onRespawn += OnDead;
        KlausMove.SuscribeOnJump(OnJump);
    }

    void OnDead(Vector3 pos)
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
    }

    void OnJump(float percent)
    {
        _rigidbody2D.velocity = new Vector2(KlausMove.initialVelOffGround /*+ KlausMove.initialVelocityImpulse * Time.fixedDeltaTime*/, 0);
        _rigidbody2D.AddForce(new Vector2(0, KlausMove.ForceImpulse.y * percent * transform.up.y), ForceMode2D.Impulse);
        currentPlatform = null;
    }

    void StopGlitchEffect()
    {
   //     fx_Glitch.IsActive = false;
        fx_Glitch.RandomEvery = store_EverySecondNew;
        fx_Glitch.RandomDuration = store_ForSecondNew;
        fx_Glitch.SettingsInterferences = store_InterferenceSetting;
        fx_Glitch.SettingsTearing = store_TearingSetting;
   //     fx_Glitch.IsActive = true;
        fx_RGB.Amount = store_AmountRGB;

        //Tengo que avisar al sistema que no mande un mensaje de nuevo para la puerta
        DeadByGamePlay = true;
        currentTimePlayLevel = Door.TimePlayingLevel;

        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.0f);
        CameraFade.Instance.m_OnFadeFinish += RestartScene;

    }
    void OnDestroy()
    {
        CameraFade.Instance.m_OnFadeFinish -= RestartScene;
    }
    void RestartScene()
    {
        LoadLevelManager.Instance.RestartCurrentLevel();
    }
    public override void Kill()
    {
        // OnDead(Vector3.zero);
        KlausMove.Kill();
    }

    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (colliderDoor.enabled)
            {

                float speedH = KlausMove.inputDirection.x * KlausMove.speedInX;
                //Aqui veo mimica de lso input d eklaus



                if (!object.ReferenceEquals(currentPlatform, null))
                {

                    float valueX = 0;
                    float valueY = 0;

                    valueY = currentPlatform.velocity.y;

                    if ((currentPlatform.velocity.x > 0 && speedH > 0)
                        || (currentPlatform.velocity.x < 0 && speedH < 0))
                    {
                        valueX = speedH * 0.5f + currentPlatform.velocity.x;
                    }
                    else if ((currentPlatform.velocity.x > 0 && speedH < 0)
                             || (currentPlatform.velocity.x < 0 && speedH > 0))
                    {
                        valueX = speedH * 0.5f + currentPlatform.velocity.x;
                    }
                    else
                    {
                        valueX = speedH + currentPlatform.velocity.x;
                    }

                    _rigidbody2D.velocity = new Vector2(valueX, valueY);

                }
                else
                {
                    float newY = _rigidbody2D.velocity.y;
                    if (Mathf.Abs(_rigidbody2D.velocity.y) > MaxSpeed.y)
                    {
                        newY = Mathf.Clamp(_rigidbody2D.velocity.y, MaxSpeed.y * -1, MaxSpeed.y);
                    }
                    _rigidbody2D.velocity = new Vector2(speedH, newY);
                }
            }
        }
    }

    #region ICurrentPlatform implementation

    public void CurrentPlatformEnter(Rigidbody2D platform)
    {
        currentPlatform = platform;
    }

    public void CurrentPlatformExit(Rigidbody2D platform)
    {
        if (currentPlatform == platform)
        {
            currentPlatform = null;
        }
    }

    public Collider2D getLegsCollider()
    {
        return colliderDoor;
    }

     public Rigidbody2D getOnPlatform()
    {
        return currentPlatform;

    }


    #endregion
}
