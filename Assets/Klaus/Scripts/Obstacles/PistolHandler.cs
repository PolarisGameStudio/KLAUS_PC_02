//
// PistolHandler.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;

public class PistolHandler : MonoBehaviour
{

    public float TimeToFirstShoot = 0.1f;
    public float timeToRepetShoot = 3.0f;

    public float timeLiveBullet = 5.0f;
    protected BulletInfo info;
    public BulletHandler bullet;

    public GameObject bulletSFX;

    public bool useMasterPistol = true;
    void Awake()
    {
        bullet.CreatePool(2);
        bulletSFX.CreatePool(2);
    }
    void OnEnable()
    {
        info = new BulletInfo(transform.up, timeLiveBullet);
        if (!useMasterPistol)
        {
            StopCoroutine("Shoot");
            StartCoroutine("Shoot", TimeToFirstShoot);
        }
    }


    IEnumerator Shoot(float time)
    {

        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        bullet.Spawn(transform.position).SetDirection(info);
        bulletSFX.Spawn(transform.position, transform.rotation);
        StartCoroutine("Shoot", timeToRepetShoot);
    }
    public void Invisible()
    {
        // enabled = false;
        StopCoroutine("Shoot");
    }


    public void Visible()
    {

        //enabled = true;
        StopCoroutine("Shoot");
        if (gameObject.activeInHierarchy)
            StartCoroutine("Shoot", TimeToFirstShoot);
    }

    public void StopPistol()
    {
        StopCoroutine("Shoot");
    }
}
