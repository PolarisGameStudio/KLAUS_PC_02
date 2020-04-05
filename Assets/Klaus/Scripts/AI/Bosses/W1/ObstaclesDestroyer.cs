using System.Collections;
using UnityEngine;

public class ObstaclesDestroyer : MonoBehaviour
{
    public Animator animator;
    public PistolHandler pistol;
    public PlatformInputController platform;
    public PlatformMovement platformMovement;
    public PlatformAI platformAI;
    public CPUTrigger cpuTrigger;
    public WeakPlatform weakPlatform;
    public Animator[] spikes;
	//public AudioSource aSource;
    public Rigidbody2D cachedRigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();
            return _rigidbody;
        }
    }

    public bool enableOnGround;

    Rigidbody2D _rigidbody;
    bool toggled;
    bool positionInitialized;

    Vector2 initialPosition;

    void OnEnable()
    {
        ResetObject();
        StopAllCoroutines();
        StartCoroutine("ToggleOnGround");
    }

    void OnDisable()
    {
        ResetObject();
        StopAllCoroutines();
    }

    IEnumerator ToggleOnGround()
    {
        yield return new WaitForSeconds(0.5f);

        if (enableOnGround)
        {
            while (cachedRigidbody != null && cachedRigidbody.velocity != Vector2.zero)
                yield return null;
        }
		/*if (aSource != null)
		{
			aSource.Play ();
		}*/
        Toggle(true);
    }

    public void ResetObject()
    {
        if (!positionInitialized)
        {
            initialPosition = transform.localPosition;
            positionInitialized = true;
        }

        transform.localPosition = initialPosition;

        Toggle(false);

        if (animator) animator.Rebind();
        foreach (Animator spike in spikes) spike.Rebind();
        if (platformAI != null) platformAI.ResetPlatform();
        if (cpuTrigger != null) cpuTrigger.ResetCPU();
        if (cpuTrigger != null) cpuTrigger.EnableHacking(true);
        if (platformMovement != null) platformMovement.ResetToSpot(false);
        if (weakPlatform != null) weakPlatform.ResetPlatform();
    }

    public void Toggle(bool on)
    {
        toggled = on;
        if (cpuTrigger != null) cpuTrigger.ResetCPU();
        if (cpuTrigger != null) cpuTrigger.EnableHacking(on);
        if (platformAI != null) platformAI.enabled = false;

        foreach (Animator spike in spikes)
            spike.SetBool("Showing", on);

        if (pistol != null)
        {
            pistol.enabled = on;
            if (!on) pistol.StopPistol();
        }

        if (platform != null)
        {
            ManagerPlatform.Instance.AddPlatform(platform);
            ManagerPlatform.Instance.DeselectAll();
            platform.ToggleControl(on);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "FightLimit") return;
        Toggle(false);

        gameObject.SetActive(false);
    }
}
