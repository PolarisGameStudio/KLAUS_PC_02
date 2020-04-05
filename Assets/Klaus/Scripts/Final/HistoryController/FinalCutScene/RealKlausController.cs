using UnityEngine;
using System.Collections;
using SmartLocalization;
using TMPro;

public class RealKlausController : MonoBehaviour
{

    public CharacterInputController Klaus;
    public Animator KlausOriginal;
    public string FirstMeetVar = "FirstMeetKlaus";
    public string WalkVar = "Walk";
    public string FirstGestureVar = "FirstGesture";
    public string TalkVar = "FirstGesture";
    public string WaitVar = "FirstGesture";

    public TextMeshPro TextSpot_First;
    public TextMeshPro TextSpot_First_Klaus;


    public TextMeshPro TextSpot_Second;
    public TextMeshPro TextSpot_Third;
    public TextMeshPro TextSpot_Fourt;
    public TextMeshPro TextSpot_Fifth;
    public TextMeshPro TextSpot_Final;

    public void InitTween(TextMeshPro text)
    {
        if (text.text.Contains("1984"))
        {
            Debug.Log("Reconozco que tienes 1984");
            text.text = text.text.Replace("1984", SaveManager.Instance.dataKlaus.deaths.ToString());
        }


        TweenTextShowCutScene tween = text.gameObject.GetComponent<TweenTextShowCutScene>();
        tween.InitText();
    }

    public void HideTween(TextMeshPro text)
    {
        TweenTextShowCutScene tween = text.gameObject.GetComponent<TweenTextShowCutScene>();
        tween.HideText();
    }

    public void ResetAnimator_KlausOriginal()
    {
        KlausOriginal.Rebind();
    }
    public void LockKlaus()
    {
        Klaus.enabled = false;
    }
    public void UnlockKlaus()
    {
        Klaus.enabled = true;

    }
    public void FirstMeet_Klaus_KlausOriginal()
    {
        KlausOriginal.SetTrigger(FirstMeetVar);
    }
    public void FirstGesture_Klaus_KlausOriginal()
    {
        KlausOriginal.SetTrigger(FirstGestureVar);
    }
    public void StartWalk_KlausOriginal()
    {
        KlausOriginal.SetTrigger(WalkVar);
    }
    public void StartTalk_KlausOriginal()
    {
        KlausOriginal.SetTrigger(TalkVar);
    }
    public void StartWait_KlausOriginal()
    {
        KlausOriginal.SetTrigger(WaitVar);
    }

    string GetTextValue(string id)
    {
        return LanguageManager.Instance.GetTextValue(id) ?? "NOT LOCALIZED";
    }

    public void SetText_Spot_First_Null()
    {
        TextSpot_First.text = "";
        HideTween(TextSpot_First);

    }

    public void SetText_Spot_First(string id)
    {
        TextSpot_First.text = GetTextValue(id);
        InitTween(TextSpot_First);
    }

    public void SetText_Spot_First_Klaus_Null()
    {
        TextSpot_First_Klaus.text = "";
        HideTween(TextSpot_First_Klaus);
    }

    public void SetText_Spot_First_Klaus(string id)
    {
        TextSpot_First_Klaus.text = GetTextValue(id);
        InitTween(TextSpot_First_Klaus);
    }


    public void SetText_Spot_02_Null()
    {
        TextSpot_Second.text = "";
        HideTween(TextSpot_Second);
    }
    public void SetText_Spot_02(string id)
    {
        TextSpot_Second.text = GetTextValue(id);
        InitTween(TextSpot_Second);

    }
    public void SetText_Spot_03_Null()
    {
        TextSpot_Third.text = "";
        HideTween(TextSpot_Third);
    }
    public void SetText_Spot_03(string id)
    {
        TextSpot_Third.text = GetTextValue(id);
        InitTween(TextSpot_Third);

    }
    public void SetText_Spot_04_Null()
    {
        TextSpot_Fourt.text = "";
        HideTween(TextSpot_Fourt);
    }
    public void SetText_Spot_04(string id)
    {
        TextSpot_Fourt.text = GetTextValue(id);
        InitTween(TextSpot_Fourt);

    }
    public void SetText_Spot_05_Null()
    {
        TextSpot_Fifth.text = "";
        HideTween(TextSpot_Fifth);
    }
    public void SetText_Spot_05(string id)
    {
        TextSpot_Fifth.text = GetTextValue(id);
        InitTween(TextSpot_Fifth);


    }
    public void SetText_Spot_Final_Null()
    {
        TextSpot_Final.text = "";
        HideTween(TextSpot_Final);
    }
    public void SetText_Spot_Final(string id)
    {
        TextSpot_Final.text = GetTextValue(id);
        InitTween(TextSpot_Final);

    }
}
