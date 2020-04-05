using UnityEngine;
using System.Collections;

public class AnimatorFx : MonoBehaviour
{

    public Animator anim;

    public void OnFinish()
    {
        anim.Recycle();
    }
}
