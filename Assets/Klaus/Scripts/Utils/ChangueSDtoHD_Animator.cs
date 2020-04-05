using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class ChangueSDtoHD_Animator : MonoBehaviour {

#if UNITY_EDITOR

    public RuntimeAnimatorController SD;
    public RuntimeAnimatorController HD;
#endif
    /*
    public string pathSD;
    public string pathHD;

    void Awake() {
        Animator animator =  GetComponent<Animator>();
        animator.enabled = false;
        RuntimeAnimatorController anim = null;

#if UNITY_PSP2
        if (string.IsNullOrEmpty(pathSD))
        {
            Debug.LogWarning("El path de: " + gameObject + " es nulo");

        }
        anim = Resources.Load<RuntimeAnimatorController>(pathSD);
#else
        
        if (string.IsNullOrEmpty(pathHD))
        {
            Debug.LogWarning("El path de: " + gameObject + " es nulo");

        }
        anim = Resources.Load<RuntimeAnimatorController>(pathHD);
#endif

        if (anim == null)
            Debug.LogWarning("El animator: " + gameObject + " es nulo");

        animator.runtimeAnimatorController = anim;
        animator.enabled = true;

    }*/
}
