using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeakPlatform : MonoBehaviour
{
    public Renderer[] rendertoDestroy;
    public Behaviour[] monotoDestroy;
    public Animator anim;
    public float TimeToDestroy = 0.5f;
    public float TimeToRespawn = 1.0f;
    public GameObject SFXDestroy;
    Dictionary<int, MoveState> isIn = new Dictionary<int, MoveState>();

    bool isRespawn = false;
    bool isDetroy = false;

    private AudioSource _audio;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    void Awake()
    {
        SFXDestroy.CreatePool(2);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isRespawn && !isDetroy)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!isIn.ContainsKey(other.gameObject.GetInstanceID()))
                {
                    isIn.Add(other.gameObject.GetInstanceID(), other.gameObject.GetComponent<MoveState>());
                }
                /*/
                if (isIn [other.gameObject.GetInstanceID()].getLegsCollider() == other.collider)
                {
                    isDetroy = true;
                    StartCoroutine("Destroy", TimeToDestroy);

                }
				/*/
                for (int i = 0; i < isIn [other.gameObject.GetInstanceID()].colliders.Length; i++)
                {
                    if (isIn [other.gameObject.GetInstanceID()].colliders [i] == other.collider)
                    {
                        isDetroy = true;
                        StartCoroutine("DestroyPlatform", TimeToDestroy);
                        SFXDestroy.Spawn(transform.position, transform.rotation);
                    }
                }
            }

        }
    }

    void ManagerDestroyMono(bool value)
    {
        isRespawn = !value;
        for (int i = 0; i < monotoDestroy.Length; ++i)
            monotoDestroy [i].enabled = value;

    }

    void ManagerDetroyRender(bool value)
    {
        for (int i = 0; i < rendertoDestroy.Length; ++i)
            rendertoDestroy [i].enabled = value;
    }

    IEnumerator DestroyPlatform(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isDetroy = false;
        ManagerDestroyMono(false);
        anim.SetTrigger("Destroy");
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.4f));
        ManagerDetroyRender(false);
        StartCoroutine("Respawn", TimeToRespawn);
    }

    IEnumerator Respawn(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        audio.Play();
        ManagerDetroyRender(true);
        anim.SetTrigger("Reset");
        yield return StartCoroutine(new TimeCallBacks().WaitPause(0.25f));
        ManagerDestroyMono(true);
    }

    public void ResetPlatform()
    {
        ManagerDetroyRender(true);
        ManagerDestroyMono(true);
        anim.Rebind();
    }
}
