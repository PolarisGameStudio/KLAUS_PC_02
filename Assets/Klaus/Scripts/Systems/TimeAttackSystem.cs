using UnityEngine;
using System.Collections;

public class TimeAttackSystem : MonoSingleton<TimeAttackSystem>
{
    public float timer { get; protected set; }

    public float bestTime { get; protected set; }

    public bool timerRunning { get; protected set; }

    public HUD_TimeAttack hud
    {
        get
        {
            if (_hud == null)
                _hud = GameObject.FindObjectOfType<HUD_TimeAttack>();
            return _hud;
        }
    }

    public TimeAttackTrigger[] triggers
    {
        get
        {
            if (_triggers == null)
                _triggers = GameObject.FindObjectsOfType<TimeAttackTrigger>();
            return _triggers;
        }
    }

    TimeAttackTrigger[] _triggers;
    HUD_TimeAttack _hud;
    public CompanyRecorTrophy trophy;

	public AudioSource audio1;
	public AudioSource audio2;
	public bool firstPlay = false;
	public bool firstStop = false;

    protected override void Init()
    {
        base.Init();
        Setup();
    }

    IEnumerator OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Loading")
        {
            audio1.Pause();
        }
        else
        {
            yield return null;
            Setup();
        }
    }

    void Setup()
    {
        _hud = null;
        _triggers = null;

        foreach (TimeAttackTrigger trigger in triggers)
            trigger.gameObject.SetActive(false);

        if (SaveManager.Instance.dataKlaus != null)//Luis Fix
            bestTime = SaveManager.Instance.dataKlaus.GetTime(Application.loadedLevelName);
        else
            bestTime = 0;

        if (CanEnableTimeAttackMode())
        {
            if (timerRunning)
            {
                audio1.UnPause();
                ShowHud();
                PlayTimer();
            }
            else
            {
                foreach (TimeAttackTrigger trigger in triggers)
                    trigger.gameObject.SetActive(!trigger.isGoal);

                StopTimer();
				audio1.Stop();
            }
        }
        else
        {
            StopTimer(false);
        }
    }

    void LateUpdate()
    {
        if (timerRunning && !ManagerPause.Pause)
        {
            timer += Time.deltaTime;
            hud.SetCurrentTime(timer);
        }
    }

    public void ShowHud()
    {
        if (hud == null)
            return;
        hud.SetBestTime(bestTime);
        hud.SetCurrentTime(timer);
        hud.Show();
    }

    public void HideHud()
    {
        hud.Hide();
    }

    public void PlayTimer()
    {
        enabled = true;
        timerRunning = true;
		audio1.Play ();
		if(!firstPlay)
		{
			firstPlay = true;
			firstStop = true;
			audio2.Play ();
			Debug.Log ("Audio2 Played");
		}

        // Prende triggers que terminan el evento, apaga triggers que lo inician
        foreach (TimeAttackTrigger trigger in triggers)
        {
            trigger.collider.enabled = trigger.isGoal;
            if (trigger.isGoal)
                trigger.gameObject.SetActive(true);
        }

        // Chequea errores en este punto para alertar al usuario
        HUD_ErrorMessages.Instance.CheckErrors();
    }

    public void PauseTimer()
    {
        enabled = false;
		//audio1.Stop();
    }

    public void ResetTimer()
    {
        timer = 0f;
        timerRunning = false;
        enabled = false;
		firstPlay = false;

        // Prende triggers que inician el evento, apaga triggers que lo terminan
        foreach (TimeAttackTrigger trigger in triggers)
            trigger.collider.enabled = !trigger.isGoal;
    }

    public void StopTimer(bool checkBestTime = true)
    {
        if (checkBestTime && timerRunning)
        {
            if (timer > 0 && (bestTime <= 0 || timer < bestTime))
            {
                SaveManager.Instance.dataKlaus.SetTime(Application.loadedLevelName, timer);
                hud.OnNewRecord();
            }
            //Show HUD
            //         if (timer < SaveManager.Instance.dataKlaus.worlds[0].levels[0].companyRecordSeconds)


            //Check Trophy

            if (SaveManager.Instance.dataKlaus.GetBrokenCompanyRecords() == SaveManager.Instance.dataKlaus.GetTotalCompanyRecords())
            {
                trophy.CompleteTrophy();
            }
        }

        timer = 0f;
        timerRunning = false;
        enabled = false;
		audio1.Stop();
		if (firstStop)
		{
			audio2.Play();
		}

        // Prende triggers que inician el evento, apaga triggers que lo terminan
        foreach (TimeAttackTrigger trigger in triggers)
            if (trigger != null)
                trigger.collider.enabled = !trigger.isGoal;
    }

    bool CanEnableTimeAttackMode()
    {
        // Chequear aca:
        // - Que la escena cargada sea un nivel valido (habra HUD de ser asi)
        // - Que el nivel venga cargado desde el arcade mode
        return hud != null && SaveManager.Instance.comingFromTimeArcadeMode;
    }
}
