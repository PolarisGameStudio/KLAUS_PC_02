using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityStandardAssets.ImageEffects;
using Colorful;
using UnityEngine.Assertions;
using Steamworks;


public class ManagerHudUI : MonoSingleton<ManagerHudUI>
{
    public Animator HUDMenu;
    public Animator PauseMenu;
    public HUD_LevelCompletedMenu LevelCompleted;
    public BackButtonMenu backHud, backPause;
    public bool fight;
    public AudioMixer mainMixer;

    public Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    /*public AudioSource cachedAudioSource {
        get {
            if (_audioSource == null)
            {
                GameObject obj = GameObject.Find("AS_MusikManager_Arrecho");

                if (obj != null)
                    _audioSource = obj.GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }

    AudioSource _audioSource;*/
    public void FightMix()
    {
        fight = true;
    }
    public void NormalMix()
    {
        fight = false;
    }
    public NoiseAndGrain noise
    {
        get
        {
            if (_noise == null)
                _noise = GetComponentInChildren<NoiseAndGrain>();
            return _noise;
        }
    }

    public LookupFilter3D lookup
    {
        get
        {
            if (_lookup == null)
                _lookup = GetComponentInChildren<LookupFilter3D>();
            return _lookup;
        }
    }

    public FastVignette vignette
    {
        get
        {
            if (_vignette == null)
                _vignette = GetComponentInChildren<FastVignette>();
            return _vignette;
        }
    }

    public Glitch glitch
    {
        get
        {
            if (_glitch == null)
                _glitch = GetComponentInChildren<Glitch>();
            return _glitch;
        }
    }

    public void ActivateMouse()
    {
     //   Debug.Log(InputEnum.CONFIG);

        /*/
        if(!InputEnum.GetInputString(InputAction.Jump).Contains("xbox") && !Cursor.visible)
        {
            Cursor.visible = true;

        }
        /*/
    }

    public void Update()
    {
        

    }


    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {

            if (SteamManager.Initialized)
            {
              if (!ManagerPause.Pause)
                {
                    if (backHud.Callback != null)
                        backHud.Callback();
                }
                
            }

            Debug.LogError("Steam Overlay has been activated");
            Cursor.visible = true;

        }
        else
        {
            Debug.LogError("Steam Overlay has been closed");
        }
    }

    NoiseAndGrain _noise;
    LookupFilter3D _lookup;
    FastVignette _vignette;
    Glitch _glitch;

    float noiseInitialValue;
    float lookupInitialValue;
    float vignetteInitialValue;
    float _effectsMultiplier;

    public float effectsMultiplier
    {
        get { return _effectsMultiplier; }
        set
        {
            _effectsMultiplier = Mathf.Clamp01(value);

            noise.intensityMultiplier = _effectsMultiplier * noiseInitialValue;
            /*#if UNITY_EDITOR
                        if(lookup == null || vignette == null)
                            return;
            #endif*/
            Assert.IsNotNull(lookup);
            lookup.Amount = _effectsMultiplier * lookupInitialValue;
            Assert.IsNotNull(vignette);
            vignette.Darkness = _effectsMultiplier * vignetteInitialValue;
        }
    }

    public bool glitchEnabled
    {
        get
        {
            Assert.IsNotNull(glitch);

            return glitch.enabled;
        }
        set
        {
            Assert.IsNotNull(glitch);

            glitch.enabled = value;
        }
    }

    protected override void Init()
    {
        /*/
        base.Init();
        noiseInitialValue = noise.intensityMultiplier;
        effectsMultiplier = 0f;
        glitchEnabled = false;
        Assert.IsNotNull(lookup);
        lookupInitialValue = lookup.Amount;
        Assert.IsNotNull(vignette);
        vignetteInitialValue = vignette.Darkness;
        /*/
    }

    void Start()
    {
        backHud.OtherInput.Add(InputActionOld.UI_Pause);
        backHud.Callback = delegate ()
        {
            ManagerPause.Pause = true;
        };

        ChangueToHUD();
    }


#if UNITY_PS4 && !UNITY_EDITOR
    void Update()
    {
        if (UnityEngine.PS4.Utility.isInBackgroundExecution)
        {
            if (!ManagerPause.Pause)
            {
                if (backHud.Callback != null)
                    backHud.Callback();
            }
        }
    }
#endif
    void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }

        ManagerPause.SubscribeOnPauseGame(onPauseGame);
        ManagerPause.SubscribeOnResumeGame(onResumeGame);
    }

    bool isHUD = false;
    public void ChangueToHUD()
    {
        if (!isHUD)
        {
            backHud.enabled = true;
            backPause.enabled = false;
            HUDMenu.SetTrigger("In");
            isHUD = true;

            if (fight)
            {
                mainMixer.FindSnapshot("Fight").TransitionTo(0.1f * Time.deltaTime);
            }
            else
                mainMixer.FindSnapshot("Unpaused").TransitionTo(0.1f * Time.deltaTime);
        }
    }

    public void UnloadHUD()
    {
        if (isHUD)
        {
            HUDMenu.SetTrigger("Out");
            isHUD = false;
        }
    }
    bool isPauseMenu = false;
    public void ChangueToPauseMenu()
    {

        if (!isPauseMenu)
        {
            backHud.enabled = false;
            backPause.enabled = true;
            PauseMenu.SetTrigger("In");
            isPauseMenu = true;
            mainMixer.FindSnapshot("Paused").TransitionTo(0.1f * Time.deltaTime);

        }
    }

    public void UnloadPauseMenu()
    {
        if (isPauseMenu)
        {
            PauseMenu.SetTrigger("Out");
            isPauseMenu = false;
        }

    }

    public void onPauseGame()
    {
        if (showLevelCompleted)
            return;
        UnloadHUD();
        ChangueToPauseMenu();
    }

    public void onResumeGame()
    {
        if (showLevelCompleted)
            return;
        UnloadPauseMenu();
        ChangueToHUD();

    }

    public void ChangueToLevelCompletedMenu()
    {
        backHud.enabled = false;
        backPause.enabled = false;
        LevelCompleted.anim.SetTrigger("In");
    }

    public void UnloadLevelCompletedMenu()
    {
        LevelCompleted.anim.SetTrigger("Out");

    }
    bool showLevelCompleted = false;
    public void ShowLevelCompleted(string level, bool preloadScene = true)
    {
        showLevelCompleted = true;
        UnloadHUD();
        ChangueToLevelCompletedMenu();
        LevelCompleted.Show(level, preloadScene);
    }

}
