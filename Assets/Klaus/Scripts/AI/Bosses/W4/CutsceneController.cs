using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public TextTrigger[] texts;
    public GameObject transition;
    public GameObject[] FXs;
    public float timeBetweenEffects;
	public AudioSource[] shieldSFX;
	public AudioSource[] preSFX;
	public AudioSource activShieldSFX;
	public AudioSource electricSFX;
	public AudioClip electicSFX2;
    public Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }

    Animator _animator;

    public void AddTarget(Transform target, float zoom, float timeToTarget, float timeToZoom)
    {
        RemoveTarget();
        DynamicCameraManager.Instance.ChangueEspecialTargetForK1(target, timeToTarget, zoom, timeToZoom);
    }

    public void StartAnimation()
    {
        animator.SetTrigger("Start");
    }

    public void EnableText(int id)
    {
        if (texts != null && 0 <= id && id <= texts.Length)
            texts[id].OnEnterAction();
    }

    public void DisableText(int id)
    {
        if (texts != null && 0 <= id && id <= texts.Length)
            texts[id].HideAction();
    }

    public void EnableTransition()
    {
        transition.SetActive(true);
    }

    public void EnableEffects()
    {
        StartCoroutine("EnableFXSequence");
    }

    IEnumerator EnableFXSequence()
    {
        for (int i = 0; i != FXs.Length; ++i)
        {
            FXs[i].SetActive(true);
            yield return StartCoroutine(new TimeCallBacks().WaitPause(timeBetweenEffects));
        }
    }

    public void ShakeCamera()
    {
        CameraShake.Instance.StartShake();
    }

    public void StopCamera()
    {
        CameraShake.Instance.StopShake();
    }

    public void RemoveTarget()
    {
        DynamicCameraManager.Instance.RemoveEspecialTargetForK1(1f);
    }

    public void Freeze()
    {
        CharacterManager.Instance.FreezeAll();
    }

    public void Unfreeze()
    {
        CharacterManager.Instance.UnFreezeAll();
    }
	public void ShieldSFXON()
	{
		for(int i = 0; i != shieldSFX.Length; i++)
		{
			shieldSFX[i].mute = false;
		}
	}
	public void PreSFXOFF()
	{
		for(int i = 0; i != preSFX.Length; i++)
		{
			preSFX[i].mute = true;
		}
	}
	public void ActiveShieldSFX()
	{
		activShieldSFX.Play ();
	}
	public void ElectrikSFX()
	{
		electricSFX.Play ();
	}
	public void ElectrikSFX2()
	{
		electricSFX.clip = electicSFX2;
		electricSFX.Play ();
	}
}
