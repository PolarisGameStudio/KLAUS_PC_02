using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class RumbleManager : PersistentSingleton<RumbleManager>
{
    const int IntensityMayor = 200;
    const int IntensityMinor = 200;
    const float fixValueTime = 0.8f;
    static float startRumble;

    public void VibrateByTime(float time)
    {
        if (SaveManager.Instance.dataKlaus != null)
        if (!SaveManager.Instance.dataKlaus.Rumble)
            return;
        StopCoroutine("Vibrate");
        StartCoroutine("Vibrate", time * fixValueTime);

        if (SaveManager.Instance.dataKlaus.Rumble && InputEnum.GamePad.ToString() == "xbox 360")
        {
            GamePad.SetVibration(0, 1f, 1f);
            startRumble = Time.deltaTime;
            Debug.Log("StartRumble");
        }

        if ((Time.deltaTime - startRumble) >= time)
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            Debug.Log("StopRumble");
        }
    }

    public void Vibrate()
    {
        if (SaveManager.Instance.dataKlaus.Rumble && InputEnum.GamePad.ToString() == "xbox 360")
        {
            GamePad.SetVibration(0, 0.6f, 0.6f);

        }


            if (SaveManager.Instance.dataKlaus != null && !SaveManager.Instance.dataKlaus.Rumble)
            return;

        StopCoroutine("Vibrate");
        #if UNITY_PS4 && !(UNITY_EDITOR)
        PS4Input.PadSetVibration(0, IntensityMayor, IntensityMinor);
        #endif
    }

    public void Stop()
    {
        if (InputEnum.GamePad.ToString() == "xbox 360")
        {
            //Handheld.Vibrate();
            GamePad.SetVibration(0, 0f, 0f);
            Debug.Log("StopRumble");
        }

        StopCoroutine("Vibrate");
        #if UNITY_PS4 && !(UNITY_EDITOR)
        PS4Input.PadSetVibration(0, 0, 0);
        #endif
    }

    IEnumerator Vibrate(float time)
    {
        while (time > 0)
        {
            if (!ManagerPause.Pause)
            {/*/
                if(InputEnum.GamePad.ToString() == "xbox 360")
                {
                    Handheld.Vibrate();

                }
                /*/
#if UNITY_PS4 && !(UNITY_EDITOR)
                PS4Input.PadSetVibration(0, IntensityMayor, IntensityMinor);
#endif
                time -= Time.deltaTime;
            }
            yield return null;
        }

#if UNITY_PS4 && !(UNITY_EDITOR)
        PS4Input.PadSetVibration(0, 0, 0);

#endif
    }
}
