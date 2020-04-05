using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreadmillBuilder : MonoBehaviour
{
    public Transform Pivot;
    public Transform LeftMill;
    public Transform RigthMill;
    public Transform CenterMill;

    public int numOfCenterMill = 0;
    public float offSetX = 0.1f;

    public SurfaceEffector2D effector;

    List<Transform> CenterMillSp = new List<Transform>();
    Transform RigthMillSp;
    Transform LeftMillSp;

    bool isRight = true;
    [Header("Pausa")]
    public float timeToReactiveMill = 0.25f;

    Transform SpawnMill(Transform mill, Vector3 offset)
    {
        Transform left = mill.Spawn();
        left.transform.parent = transform;
        left.transform.localPosition = Vector3.zero;
        left.transform.localPosition = offset;
        return left;
    }
    const float factorspeed = 0.015f;
    // Use this for initialization
    void Awake()
    {

        Pivot.localScale = new Vector3(Pivot.localScale.x + numOfCenterMill + 1, Pivot.localScale.y, Pivot.localScale.z);

        Vector3 offsetVec = new Vector3(offSetX, 0, 0);
        LeftMillSp = SpawnMill(LeftMill, offsetVec);
        LeftMillSp.GetComponent<Animator>().speed = Mathf.Abs(effector.speed) * factorspeed;
        for (int i = 0; i < numOfCenterMill; ++i)
        {
            CenterMillSp.Add(SpawnMill(CenterMill, offsetVec * (i * 2 + 3)));
            CenterMillSp[i].GetComponent<Animator>().speed = Mathf.Abs(effector.speed) * factorspeed;
        }
        RigthMillSp = SpawnMill(RigthMill, offsetVec * (numOfCenterMill * 2 + 3));
        RigthMillSp.GetComponent<Animator>().speed = Mathf.Abs(effector.speed) * factorspeed;

        if (effector.speed < 0)
            InvertSprite(false);
    }
    bool firstRun = true;

    void Start()
    {
        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;


    }
    void OnEnable()
    {
        if (!firstRun)
        {
            ManagerPause.SubscribeOnPauseGame(OnPauseGame);
            ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        }
        storeSpeed = effector.speed;
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    float storeSpeed = 0;
    public void OnPauseGame()
    {
        CancelInvoke("SetEffector");
        //  effector.enabled = false;
        effector.speed = 0;
    }
    public void OnResumeGame()
    {
        Invoke("SetEffector", timeToReactiveMill);
    }
    void SetEffector()
    {
        //  effector.enabled = true;
        effector.speed = storeSpeed;
    }
    public void InvertSprite(bool isRigthAux)
    {
        if ((isRight && !isRigthAux)
            || (!isRight && isRigthAux))
        {
            Vector3 aux = LeftMillSp.localPosition;
            LeftMillSp.localPosition = RigthMillSp.localPosition;
            RigthMillSp.localPosition = aux;
            LeftMillSp.localScale = new Vector3(LeftMillSp.localScale.x * -1, LeftMillSp.localScale.y, LeftMillSp.localScale.z);
            RigthMillSp.localScale = new Vector3(RigthMillSp.localScale.x * -1, RigthMillSp.localScale.y, RigthMillSp.localScale.z);

            for (int i = 0; i < CenterMillSp.Count; ++i)
            {
                CenterMillSp[i].localScale = new Vector3(CenterMillSp[i].localScale.x * -1, CenterMillSp[i].localScale.y, CenterMillSp[i].localScale.z);
            }

        }
        isRight = isRigthAux;

    }
}
