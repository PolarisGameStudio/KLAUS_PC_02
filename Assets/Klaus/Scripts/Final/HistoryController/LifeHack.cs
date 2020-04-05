using UnityEngine;
using System.Collections;
using DG.Tweening;
using SmartLocalization;
using Colorful;

public class LifeHack : IHack
{
    public int PresetShake = 0;

    public Transform BarHack;
    protected Vector3 storeScaleBarHack;

    Tweener tween = null;
    bool firstRun = true;

    public GameObject BackgroundOff;
    public string KeyLocalized_UnlockMessage = "AQUI VA EL KEY";
    public float TimeShowMessageUnlock = 10.0f;
    string MessageUnlock = "NOT LOCALIZED";

    public Glitch glitchFX;
    Tweener tween2 = null;

    public AudioSource hackSFX;
    public UnlockArcade_Trophy trophy;
    void Start()
    {

        if (SaveManager.Instance.dataKlaus.isArcadeModeUnlock)
        {
            cpu.gameObject.SetActive(false);
            BarHack.gameObject.SetActive(false);
            VisualHack();
            gameObject.SetActive(false);
            return;
        }
        else
        {
            cpu.onStartHack += OnStart;
            cpu.onCancelHack += OnCancel;
        }
        storeScaleBarHack = BarHack.localScale;
        BarHack.gameObject.SetActive(false);

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;

        LanguageManager languageManager = LanguageManager.Instance;
        languageManager.OnChangeLanguage += OnChangeLanguage;

        //Run the method one first time
        OnChangeLanguage(languageManager);
    }
    void OnDestroy()
    {
        if (LanguageManager.HasInstance)
        {
            LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
        }
    }

    void OnChangeLanguage(LanguageManager languageManager)
    {
        MessageUnlock = LanguageManager.Instance.GetTextValue(KeyLocalized_UnlockMessage) ?? MessageUnlock;
    }


    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
            ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
        }
    }
    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }
    void VisualHack()
    {
        BackgroundOff.SetActive(false);
    }
    // Use this for initialization
    public override void HackedSystem()
    {
        StopEffectHack();
        Debug.LogError("ArcadeMode Unlock");
        SaveManager.Instance.dataKlaus.isArcadeModeUnlock = true;
        SaveManager.Instance.Save();
        //Doy Trofeo
        trophy.UnlockArcade();
        VisualHack();
        HUD_Message.Instance.Show(MessageUnlock, TimeShowMessageUnlock);
    }

    public override void ResetAll()
    {
    }

    public void OnStart()
    {
        Debug.Log("Empezo a hack");
        CameraShake.Instance.StartShake(PresetShake);
        BarHack.gameObject.SetActive(true);
        tween = BarHack.DOScaleX(0.05f, cpu.timeTohack);
        tween2 = DOTween.To(SetGlitchNewValue, 0, 1, cpu.timeTohack);
        glitchFX.enabled = true;
        hackSFX.Play();

    }
    void SetGlitchNewValue(float value)
    {
        glitchFX.SettingsTearing.Intensity = value;
        
        
    }
    public void OnCancel()
    {
        Debug.Log("Cancelo a hack");
        StopEffectHack();
        hackSFX.Stop();
    }

    void StopEffectHack()
    {
        tween.Pause();
        tween.Kill();
        tween = null;
        tween2.Pause();
        tween2.Kill();
        tween2 = null;
        CameraShake.Instance.StopShake();
        BarHack.gameObject.SetActive(false);
        BarHack.localScale = storeScaleBarHack;
        glitchFX.enabled = false;
        SetGlitchNewValue(0);
    }

    #region Pause:
    public void OnPauseGame()
    {
        if (tween != null)
            tween.Pause();

        if (tween2 != null)
            tween2.Pause();
    }
    public void OnResumeGame()
    {
        if (tween != null)
            tween.Play();

        if (tween2 != null)
            tween2.Play();
    }
    #endregion
}
