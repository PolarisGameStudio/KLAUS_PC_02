using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GlitchEffect))]
public class StartSceneGlitch : MonoBehaviour
{
    public float TimeGlitching = 1.0f;
    public float Intensity = 2.0f;
    protected float storeIntensity = 0;
    GlitchEffect fx;
    // Use this for initialization
    void Awake()
    {  
        fx = GetComponent<GlitchEffect>();
        storeIntensity = fx.intensity;
        fx.intensity = Intensity;
        Invoke("StopGlitchEffect", TimeGlitching);
    }

    void StopGlitchEffect()
    {
        fx.intensity = storeIntensity;
    }
}
