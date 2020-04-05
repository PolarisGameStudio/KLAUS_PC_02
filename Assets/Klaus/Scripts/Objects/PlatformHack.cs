using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SimbolSetter))]
public class PlatformHack : IHack
{
    public bool isForActive = true;
    public MonoBehaviour ai;
    public Animator anim;
    public Animator LightPrefab;
    Animator animLight;
    public float TimeToShowLight = 0.15f;
    public float TimeToActive = 0.2f;

    protected SimbolSetter Simbol;

    public float TimeToLook = 1.5f;

    public bool CameraFollowPaltform = true;

    public Action WarningDestroy;
    public float SpeedX = 1;
    public float SpeedY = 1;
    protected virtual void Awake()
    {

        Simbol = GetComponent<SimbolSetter>();

    }

    protected void Start()
    {
        if (isForActive)
        {
            ai.enabled = false;
            anim.enabled = false;

        }
        anim.SetBool("isCPU", true);//cambio el sprite de la plataforma
        SpriteRenderer spritePaltform = anim.GetComponent<SpriteRenderer>();
        Simbol.SetSimbols(spritePaltform.sortingOrder + 1, spritePaltform.sortingLayerName);

        if (isForActive)
        {
            Simbol.ActiveSimbol();
        }
        animLight = LightPrefab.Spawn(Simbol.spotTargetSimbol.position, Simbol.spotTargetSimbol.rotation);
        animLight.transform.parent = Simbol.spotTargetSimbol;
        if (anim.enabled)
        {
            animLight.SetTrigger("Show");
        }
    }

    public override void HackedSystem()
    {

        if (CameraFollowPaltform)
        {
            CameraFollow.Instance.ChangueTargetOnly(ai.transform, TimeToLook, SpeedX, SpeedY);
        }

        if (SaveManager.Instance.dataKlaus != null)
        {
            SaveManager.Instance.dataKlaus.AddHack_CPU();
        }
        StartCoroutine("ShowLight");
    }

    IEnumerator ShowLight()
    {
        yield return new WaitForSeconds(TimeToShowLight);
        if (isForActive)
        {
            animLight.SetTrigger("Show");
        }
        else
        {
            animLight.SetTrigger("Off");

        }
        yield return new WaitForSeconds(TimeToActive);

        if (isForActive)
        {
            ai.enabled = true;
            anim.enabled = true;
            Simbol.ActiveSimbol();
        }
        else
        {
            ai.GetComponent<PlatformAI>().DeActivePlatform();// TEner cuidado con esto, no es para nada elegante

            anim.enabled = false;
            Simbol.DeActiveSimbol();
        }

        if (WarningDestroy != null)
            WarningDestroy();
    }

    public override void ResetAll()
    {
        ai.enabled = false;
        anim.enabled = false;

        anim.Rebind();
        if (animLight != null)
            animLight.Rebind();
    }

}
