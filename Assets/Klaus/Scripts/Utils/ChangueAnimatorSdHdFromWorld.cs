using UnityEngine;
using System.Collections;
using System;

public class ChangueAnimatorSdHdFromWorld : MonoBehaviour
{

#if UNITY_EDITOR
    public RuntimeAnimatorController[] SdWorld;
    public RuntimeAnimatorController[] HdWorld;
#endif
    //   public string[] pathSdWorld;
    public string[] pathHdWorld;

    void Awake()
    {
        string nameLevel = Application.loadedLevelName.Substring(0, 1);
        int World = 0;
        if (nameLevel == "W")
        {
            string numberLevel = Application.loadedLevelName.Substring(1, 1);
            World = Convert.ToInt32(numberLevel) - 1;
            if (World == 5)
            {
                string newNumberLevel = Application.loadedLevelName.Substring(4, 1);
                int newWorld = Convert.ToInt32(newNumberLevel) - 1;
                World += newWorld;
            }
        }
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;
        RuntimeAnimatorController anim = null;

        /*#if UNITY_PSP2 && !(UNITY_EDITOR)

                if (World >= pathSdWorld.Length || World < 0)
                    World = 0;

                if (string.IsNullOrEmpty(pathSdWorld[World]))
                {
                    Debug.LogWarning("El path de: " + gameObject + " es nulo");

                }
                anim = Resources.Load<RuntimeAnimatorController>(pathSdWorld[World]);
        #else*/

        if (World >= pathHdWorld.Length || World < 0)
            World = 0;

        if (string.IsNullOrEmpty(pathHdWorld[World]))
        {
            Debug.LogWarning("El path de: " + gameObject + " es nulo");

        }
        anim = Resources.Load<RuntimeAnimatorController>(pathHdWorld[World]);
        //#endif

        if (anim == null)
            Debug.LogWarning("El animator: " + gameObject + " es nulo");

        animator.runtimeAnimatorController = anim;
        animator.enabled = true;

    }
}
