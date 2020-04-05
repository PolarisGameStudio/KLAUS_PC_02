using UnityEngine;
using System.Collections;

public class HUD_BlockCamera : MonoBehaviour
{

    public Animator animator;

    public void Show()
    {
        animator.SetBool("Show", true);
    }
    public void Hide()
    {
        animator.SetBool("Show", false);
    }
}
