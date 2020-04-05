using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class RayBox : MonoBehaviour
{
    public float TimeToStart = 0.3f;
    public float TimeToResetRay = 0.5f;
    public float DurationLoad = 1.0f;
    public float DurationRay = 2.0f;
    public LayerMask layerForRay;

    public float maxDitanceRay = 100.0f;

    public Animator animator;

    public Transform trailRen;
    public Transform trailRenOpaco;
    public Transform ParticleShock;

    List<Transform> SpawnedTrailRen = new List<Transform>();
    List<Transform> SpawnedShock = new List<Transform>();

    List<Tweener> mover = new List<Tweener>();

    Vector3 offSetZ;

    #region Rebote:

    public LayerMask layerRebote;
    public float OffSetRebote = 0.3f;
    public float LifeTimeTrail = 0.15f;

    #endregion

    public int MaxTrailNumber = 5;

    public Transform RayOriginPos;

    public bool AlwaysLaunchRay = false;
    public bool played = false;
    public GameObject electricDeathSFX;
    public AudioSource c01;
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

    Vector3 testQuat = new Vector3(0, 0, 90);
    void Awake()
    {
        //  trailRen.CreatePool(MaxTrailNumber);
        electricDeathSFX.CreatePool(1);
        //   ParticleShock.CreatePool(MaxTrailNumber);
        offSetZ = new Vector3(0, 0, trailRen.transform.position.z);

    }

    // Update is called once per frame
    void OnEnable()
    {
        //   StartCoroutine("StartLaunchRay");
    }

    IEnumerator StartLaunchRay()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeToStart));
        StopCoroutine("LoadRay");
        StopCoroutine("LaunchRay");
        StopCoroutine("IdleRay");
        StartCoroutine("IdleRay", TimeToResetRay);
    }

    void OnDisable()
    {
        StopCoroutine("StartLaunchRay");
        StopCoroutine("LoadRay");
        StopCoroutine("LaunchRay");
        StopCoroutine("IdleRay");
        //audio.Stop();
    }

    bool CheckForKill(Collider2D other, bool canKill)
    {
        if (other.CompareTag("Player"))
        {
            if (canKill)
            {
                other.GetComponent<DeadState>().typeOfDead = DeadType.Ray;
                other.GetComponent<MoveState>().Kill(electricDeathSFX);
            }
        }
        else if (other.CompareTag("Player2"))
        {
            return true;
        }
        else if (other.CompareTag("DoorEspecial"))
        {
            if (canKill)
                other.GetComponent<EspecialDoorRay>().Kill();
        }
        else
        {
            KillObject obj = other.GetComponent<KillObject>();
            if (obj != null)
            {
                if (canKill)
                    obj.Kill();
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    Collider2D[] outPutCol = new Collider2D[5];
    public float FactorToSizeTrail = 0.42f;

    void RayCastRay(Vector3 origin, Vector3 Direction, int firstSpawn, bool kill = true)
    {
        RaycastHit2D finalHit = new RaycastHit2D();
        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, Direction, maxDitanceRay, layerForRay);
        float distanceMax = maxDitanceRay;
        bool isFinalHitChange = false;
        for (int i = 0; i < hit.Length; ++i)
        {
            bool isKilleable = CheckForKill(hit[i].collider, kill);

            finalHit = hit[i];
            isFinalHitChange = true;
            if (!isKilleable)
            {
                break;
            }
        }
        if (isFinalHitChange)
        {
            if (!finalHit.collider.isTrigger)
            {
                isFinalHitChange = false;

                distanceMax = finalHit.distance;
                if (SpawnedShock.Count > firstSpawn)
                    SpawnedShock[firstSpawn].transform.position = finalHit.point;
            }
        }
        //        Debug.DrawRay(origin, Direction * distanceMax, Color.red);
        SpawnedTrailRen[firstSpawn].localScale = new Vector3((distanceMax / FactorToSizeTrail) / SpawnedTrailRen[firstSpawn].parent.transform.localScale.y, SpawnedTrailRen[firstSpawn].localScale.y, SpawnedTrailRen[firstSpawn].localScale.z);

        //NuevaFuncionalidad
        bool LaunchAnotherRay = false;
        Vector3 newOrigin = Vector3.zero;
        Vector3 newDir = Vector3.zero;
        Transform newParent = transform;

        if (hit.Length > 0)
        {
            int resorteNumb = 0;
            if ((resorteNumb = Physics2D.OverlapCircleNonAlloc(finalHit.point, OffSetRebote, outPutCol, layerRebote)) > 0)
            {
                for (int i = 0; i < resorteNumb; i++)
                {
                    if (outPutCol[i].CompareTag("Resorte"))
                    {
                        newParent = outPutCol[i].transform;
                        newDir = outPutCol[i].transform.up;
                        newOrigin = outPutCol[i].transform.position + newDir * OffSetRebote;
                        LaunchAnotherRay = true;
                        break;
                    }
                }
            }
        }
        bool spawnSamePlace = false;
        //Llamada recursiva
        if (LaunchAnotherRay)
        {
            if ((firstSpawn + 1) <= MaxTrailNumber)
            {
                if (SpawnedTrailRen.Count > (firstSpawn + 1))
                {
                    if (SpawnedTrailRen[firstSpawn + 1] != null)
                    {
                        if (!SpawnedTrailRen[firstSpawn + 1].gameObject.activeSelf)
                        {
                            spawnSamePlace = true;
                        }
                    }
                    else
                    {
                        spawnSamePlace = true;
                    }
                    if (spawnSamePlace)
                    {
                        Transform trailNew;
                        if (kill)
                        {
                            trailNew = trailRen.Spawn(newOrigin + offSetZ, transform.rotation);
                            if (SpawnedShock[firstSpawn + 1] != null)
                            {
                                SpawnedShock[firstSpawn + 1].Recycle();
                            }
                            SpawnedShock[firstSpawn + 1] = ParticleShock.Spawn(newOrigin + offSetZ);
                        }
                        else
                        {
                            trailNew = trailRenOpaco.Spawn(newOrigin + offSetZ, transform.rotation);
                        }

                        if (SpawnedTrailRen[firstSpawn + 1] != null)
                        {
                            SpawnedTrailRen[firstSpawn + 1].Recycle();
                        }

                        SpawnedTrailRen[firstSpawn + 1] = trailNew;
                        SpawnedTrailRen[firstSpawn + 1].SetParent(newParent);//nbew hack
                        trailNew.localEulerAngles = testQuat;
                    }
                }
                else
                {
                    Transform trailNew;
                    if (kill)
                    {
                        trailNew = trailRen.Spawn(newOrigin + offSetZ, transform.rotation);
                        SpawnedShock.Add(ParticleShock.Spawn(newOrigin + offSetZ));
                    }
                    else
                    {
                        trailNew = trailRenOpaco.Spawn(newOrigin + offSetZ, transform.rotation);
                    }

                    trailNew.SetParent(newParent);//new hack
                    trailNew.localEulerAngles = testQuat;
                    SpawnedTrailRen.Add(trailNew);
                }

                RayCastRay(newOrigin, newDir, firstSpawn + 1, kill);
            }
        }
        else
        {
            for (int i = firstSpawn + 1; i <= MaxTrailNumber && i < SpawnedTrailRen.Count; ++i)
            {
                if (SpawnedTrailRen[i] != null)
                {
                    SpawnedTrailRen[i].Recycle();
                    if (i < SpawnedShock.Count && SpawnedShock[i] != null)
                    {
                        SpawnedShock[i].Recycle();
                    }
                }
            }
        }
    }

    IEnumerator IdleRay(float duration)
    {
        if (played)
        {
            audio.Stop();
            played = false;
        }
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(duration));
        for (int i = 0; i < SpawnedTrailRen.Count; ++i)
        {
            if (SpawnedTrailRen[i] != null)
            {
                SpawnedTrailRen[i].Recycle();
            }

        }
        SpawnedTrailRen.Clear();
        StopCoroutine("LoadRay");
        StartCoroutine("LoadRay", DurationLoad);

    }

    IEnumerator LoadRay(float duration)
    {
        c01.Play();
        animator.SetTrigger("Load");
        Transform trailNew = trailRenOpaco.Spawn(RayOriginPos.position + offSetZ, transform.rotation);
        trailNew.SetParent(transform);//hack Luis
        if (SpawnedTrailRen.Count == 0)
        {
            SpawnedTrailRen.Add(trailNew);
        }
        else
            SpawnedTrailRen[0] = trailNew;
        while (duration > 0) //check time and listen for keypress
        {
            if (!ManagerPause.Pause)
            {
                RayCastRay(RayOriginPos.position, transform.right, 0, false);
                duration -= Time.deltaTime; //deduce time passed this frame.
            }

            yield return null; //yield for one(1) frame.
        }

        for (int i = 0; i < SpawnedTrailRen.Count; ++i)
        {
            if (SpawnedTrailRen[i] != null)
                SpawnedTrailRen[i].Recycle();

        }
        SpawnedTrailRen.Clear();

        StopCoroutine("LaunchRay");
        StartCoroutine("LaunchRay", DurationRay);
    }

    IEnumerator LaunchRay(float duration)
    {
        if (!played)
        {
            audio.Play();
            played = true;
        }
        animator.SetBool("Launch", true);
        Transform trailNew = trailRen.Spawn(RayOriginPos.position + offSetZ, transform.rotation);

        trailNew.SetParent(transform);//hackluis

        for (int i = 0; i < SpawnedShock.Count; ++i)
        {

            if (SpawnedShock[i] != null)
            {
                SpawnedShock[i].Recycle();
            }
        }
        SpawnedShock.Clear();
        SpawnedShock.Add(ParticleShock.Spawn(RayOriginPos.position + offSetZ));


        if (SpawnedTrailRen.Count == 0)
        {
            SpawnedTrailRen.Add(trailNew);
        }
        else
        {
            SpawnedTrailRen[0] = trailNew;
        }

        while (duration > 0) //check time and listen for keypress
        {
            if (!ManagerPause.Pause)
            {
                RayCastRay(RayOriginPos.position, transform.right, 0);
                if (!AlwaysLaunchRay)
                    duration -= Time.deltaTime; //deduce time passed this frame.
            }

            yield return null; //yield for one(1) frame.
        }


        animator.SetBool("Launch", false);
        for (int i = 0; i < SpawnedShock.Count; ++i)
        {

            if (SpawnedShock[i] != null)
            {
                SpawnedShock[i].Recycle();
            }
        }
        SpawnedShock.Clear();
        for (int i = 0; i < SpawnedTrailRen.Count; ++i)
        {
            if (SpawnedTrailRen[i] != null)
            {
                SpawnedTrailRen[i].Recycle();
            }
        }
        SpawnedTrailRen.Clear();

        StopCoroutine("IdleRay");
        StartCoroutine("IdleRay", TimeToResetRay);

    }


    public void Invisible()
    {
        // enabled = false;
        OnDisable();
        animator.SetBool("Launch", false);
        for (int i = 0; i < SpawnedTrailRen.Count; ++i)
        {
            if (SpawnedTrailRen[i] != null)
                SpawnedTrailRen[i].Recycle();
        }
        SpawnedTrailRen.Clear();
        for (int i = 0; i < SpawnedShock.Count; ++i)
        {

            if (SpawnedShock[i] != null)
            {
                SpawnedShock[i].Recycle();
            }
        }
        SpawnedShock.Clear();
        if (played)
        {
            audio.Stop();
            played = false;
        }
    }


    public void Visible()
    {
        if (gameObject.activeSelf)
            StartCoroutine("StartLaunchRay");
    }
}
