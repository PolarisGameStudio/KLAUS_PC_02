using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class LightBarManager : PersistentSingleton<LightBarManager>
{

    public void SetLightColor(Color color)
    {


#if UNITY_PS4 && !(UNITY_EDITOR)
                        PS4Input.PadSetLightBar(0,
                        Mathf.RoundToInt(color.r * 255),
                        Mathf.RoundToInt(color.g * 255),
                        Mathf.RoundToInt(color.b * 255));
#endif
    }

    public void ResetColor()
    {
#if UNITY_PS4 && !(UNITY_EDITOR)
        PS4Input.PadResetLightBar(PS4Input.PadGetUsersDetails(0).userId);
#endif

    }
}
