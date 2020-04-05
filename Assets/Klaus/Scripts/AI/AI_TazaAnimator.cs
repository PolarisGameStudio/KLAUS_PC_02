using UnityEngine;
using System.Collections;

public class AI_TazaAnimator : MonoBehaviour {

    private Animator anim;

    public Animator animator
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }

    public GameObject Handler;

    void OnEnable()
    {
        animator.SetTrigger("Respawn");
    }
    public void DestroyAnim()
    {
        animator.SetTrigger("Destroy");
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(0.1f);
        Handler.Recycle();
    }
}
