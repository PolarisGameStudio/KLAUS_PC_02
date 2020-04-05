using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class TutorialHardware : MonoBehaviour
{
    [SerializeField]
    string m_tutorialPath = "move";
    static string m_parentFolder = "HD";
    static string m_childFolder = "Tutorial";

    IEnumerator Start()
    {
        string CompletePath = Path.Combine(m_parentFolder, InputEnum.GamePad);
        CompletePath = Path.Combine(CompletePath, m_childFolder);
        CompletePath = Path.Combine(CompletePath, m_tutorialPath);
        var asyncLoad = Resources.LoadAsync<RuntimeAnimatorController>(CompletePath);
        yield return asyncLoad;
        Assert.IsNotNull(asyncLoad.asset);
        Animator anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = asyncLoad.asset as RuntimeAnimatorController;
    }
}
