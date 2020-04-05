using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Other/TVShader")]
public class TVShader : ImageEffectBase {
    [Range(0, 1)]
    public float verts_force = 0.0f;
    [Range(0, 1)]
    public float verts_force_2 = 0.0f;
    [Range(-3, 20)]
    public float contrast = 0.0f;
    [Range(-200, 200)]
    public float brightness = 0.0f;
    [Range(0, 1)]
    public float scanColorLines = 0.0f;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {

        material.SetFloat("_VertsColor", 1 - verts_force);
        material.SetFloat("_VertsColor2", 1 - verts_force_2);
        material.SetFloat("_Contrast", contrast);
        material.SetFloat("_Br", brightness);
        material.SetFloat("_ScansColor", 1 - scanColorLines);

        Graphics.Blit(source, destination, material);
    }
}
