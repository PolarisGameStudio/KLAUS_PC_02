using UnityEngine;
using System.Collections;
using SmartLocalization;
using System;
using TMPro;
public class KlausAscencionController : MonoBehaviour, ICompleteLevel
{


    public CharacterInputController Klaus;
    public Rigidbody2D klausRigidBody;
    public Animator BackgroundColors;
    public string ChangeColorsVar = "Start";

    public TextMeshPro TextSpot_Left_Top;
    public TextMeshPro TextSpot_Rigth_Down;

    float timer = 0;
    IEnumerator LevelCounterTime()
    {
        while (true)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void OnLevelWasLoaded(int level)
    {
        SaveManager.Instance.SaveCurrentLevel(Application.loadedLevelName);
    }

    public void SetCameraStrenght()
    {

        CameraShake.Instance.proCameraShake.Strength = new Vector2(0.6f,0.6f);
        CameraShake.Instance.proCameraShake.Vibrato = 130;

    }



    void Start()
    {
        CameraShake.Instance.proCameraShake.Duration=5;
       // RumbleManager.Instance.Stop();
        ManagerAnalytics.MissionStarted(Application.loadedLevelName, false);

        CallTrophy();
        StartCoroutine("LevelCounterTime");
        CameraShake.Instance.proCameraShake.Shake();
        StartShake(1);
        StartVibration();
        
        RumbleManager.Instance.Vibrate();

    }

    public void StartVibration()
    {
        RumbleManager.Instance.Vibrate();
        Debug.Log("Start Vibration");
    }

   public void EndVibration()
    {
        RumbleManager.Instance.Stop();
        Debug.Log("End Vibration");
    }

    public void StartBackgroundColors()
    {
        BackgroundColors.SetTrigger(ChangeColorsVar);
    }
    public void StartShake(int preset)
    {
        CameraShake.Instance.proCameraShake.Shake();
        Debug.Log("inicio el shake");
    }

    public void EndShake()
    {
        Debug.Log("Termino el shake");
        CameraShake.Instance.StopShake();
    }

    public void Update()
    {
    }

    public void LockKlaus()
    {
        Klaus.enabled = false;
        klausRigidBody.isKinematic = true;
    }
    public void UnlockKlaus()
    {
        Klaus.enabled = true;
        klausRigidBody.isKinematic = false;
    }
    public void PreLoadScene(string NextScene)
    {

        CompleteScene();
        LoadLevelManager.Instance.LoadLevel(NextScene, true);
    }
    public void ChangeScene()
    {
        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    string GetTextValue(string id)
    {
        return LanguageManager.Instance.GetTextValue(id) ?? "NOT LOCALIZED";
    }

    public void SetText_Spot_Left_Top_Null()
    {

        TextSpot_Left_Top.text = "";
    }
    public void SetText_Spot_Left_Top(string id)
    {
        StartShake(1);
        TextSpot_Left_Top.text = GetTextValue(id);
    }
    public void SetText_Spot_Rigth_Down_Null()
    {
        TextSpot_Rigth_Down.text = "";
    }
    public void SetText_Spot_Rigth_Down(string id)
    {
        StartShake(1);
        TextSpot_Rigth_Down.text = GetTextValue(id);
    }

    Action completeSceneCallback;
    Action completeLevelCallback;

    public void CompleteScene()
    {
        StopCoroutine("LevelCounterTime");
        SaveManager.Instance.AddPlayTime(timer);
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
    false, timer, 0, true);

        if (completeSceneCallback != null)
            completeSceneCallback();
    }
    public void CallTrophy()
    {
        CompleteScene_Trophy[] trophy = GameObject.FindObjectsOfType<CompleteScene_Trophy>();
        for (int i = 0; i < trophy.Length; ++i)
        {
            if (trophy[i] != null)
            {
                trophy[i].OnRegister(this);
            }
        }
    }
    public void CompleteLevel()
    {
        if (completeLevelCallback != null)
            completeLevelCallback();
    }

    public void RegisterCompleteLevel(Action callback)
    {
        completeLevelCallback += callback;
    }

    public void UnRegisterCompleteLevel(Action callback)
    {
        completeLevelCallback -= callback;
    }
    public void RegisterCompleteScene(Action callback)
    {
        completeSceneCallback += callback;
    }

    public void UnRegisterCompleteScene(Action callback)
    {
        completeSceneCallback -= callback;
    }
}
