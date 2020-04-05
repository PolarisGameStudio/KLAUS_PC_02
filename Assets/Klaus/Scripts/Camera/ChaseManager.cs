using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaseManager : MonoSingleton<ChaseManager>
{

    //        CameraFollow.Instance.currentTarget = TargetEspecialKlaus;
    //      CameraFollow.Instance.dampTimeFarAway = 0.3f;
    //   CameraFollow.Instance.dampTime
    //Si muere fadetoBlack, REloadLevel

    // Resetear todos los parametros

    public float TimeToStart = 3.0f;
    public float minDistToReachPoint = 10;

    public Transform[] Spots;
    static int currentSpot = 0;
    [Tooltip("Entre mas alto la velocidad sera menor.")]
    public float[] DamptTimeSpots;

    static Vector3 PositionToRespawn = Vector3.zero;
    static bool ChaseByDead = false;

    public float TimeTokill = 2.0f;

    bool isChasing = false;

    public MoveState[] Player;
    public ChasinKillKlaus[] KillerKlaus;
    public Vector3[] offSetRespawn;

    public EnterDoorManager Door;
    public PausaMenu_PopUp pausa;

    public bool blockHorizontal = false;
    public float PosInX = 0;
    public bool blockVertical = false;
    public float PosInY = 0;
    public float ZoomDefault = 4.4f;

    public float DeadTimeNoMoveKill = 2.5f;

    protected float currentTimePlayLevel = 0;

    protected override void Init()
    {
        base.Init();

        CameraChase chase = Camera.main.gameObject.GetComponent<CameraChase>();
        if (chase == null)
            chase = Camera.main.gameObject.AddComponent<CameraChase>();

        if (chase != null)
            chase.enabled = false;

        Camera.main.GetComponent<CameraMovement>().enabled = false;
        Door = GameObject.FindObjectOfType<EnterDoorManager>();

        if (ChaseByDead)
        {
            Door.isStartLevel = false;
            Door.canStartAnalitic = false;
            Door.SetLevelCounterTimer(currentTimePlayLevel);
        }
        else
        {
            currentTimePlayLevel = 0;
        }
    }

    void Start()
    {
        ManagerCheckPoint.Instance.callbackKill += KillKlaus;
        Door.callbackEnterDoor += ResetValuesChasin;
        pausa.callbackChangeLevel += ResetValuesChasin;

        if (ChaseByDead)
        {
            for (int i = 0; i < Player.Length; ++i)
            {
                // Player [i].CanMove(false);
                // StartCoroutine("ResetCanMove", DeadTimeNoMoveKill);
                // Player [i]._rigidbody2D.isKinematic = true;
                Player[i]._rigidbody2D.velocity = Vector2.zero;
                Player[i].transform.position = PositionToRespawn + offSetRespawn[i];
                float posX = PosInX;
                float posY = PosInY;
                if (!blockHorizontal)
                    posX = Player[i].transform.position.x;
                if (!blockVertical)
                    posY = Player[i].transform.position.y;

                Camera.main.transform.position = new Vector3(posX, posY, Camera.main.transform.position.z);
                CameraFollow.Instance.MoveCameraToOwnPos();

            }

        }
        for (int i = 0; i < Player.Length; ++i)
        {
            killing.Add(null);
        }
        for (int i = 0; i < KillerKlaus.Length; ++i)
        {
            KillerKlaus[i].Number = i;
        }
    }

    IEnumerator ResetCanMove(float tune)
    {
        while (tune > 0)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                tune -= Time.deltaTime;
                for (int i = 0; i < Player.Length; ++i)
                {
                    Player[i]._rigidbody2D.isKinematic = true;
                    Player[i]._rigidbody2D.velocity = Vector2.zero;
                    Player[i].CanMove(false);
                }
            }
            yield return null;
        }
        for (int i = 0; i < Player.Length; ++i)
        {
            Player[i]._rigidbody2D.isKinematic = false;
            Player[i].CanMove(true);
        }
    }


    protected virtual void FixedUpdate()
    {

        if (!ManagerStop.Stop && !ManagerPause.Pause)
        {
            if (isChasing)
            {
                Vector2 remainDistance = Spots[currentSpot].position - Camera.main.transform.position;

                if (remainDistance.magnitude <= minDistToReachPoint)
                {
                    ChangeSpot();
                }
            }
        }
    }

    public void ResetValuesChasin()
    {
        ChaseByDead = false;
        PositionToRespawn = Vector3.zero;
        currentSpot = 0;
        currentTimePlayLevel = 0;
    }

    public void StartChase()
    {

        if (!isChasing)
        {
            StartCoroutine("CounterDown", TimeToStart);
        }
    }

    IEnumerator CounterDown(float time)
    {
        CharacterManager.Instance.FreezeAll();
        for (int i = 0; i < Player.Length; ++i)
        {
            Player[i].ResetForce();
            Player[i]._rigidbody2D.velocity = Vector2.zero;
            Player[i]._rigidbody2D.isKinematic = true;
        }
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        for (int i = 0; i < Player.Length; ++i)
        {
            Player[i]._rigidbody2D.isKinematic = false;
            Player[i]._rigidbody2D.velocity = Vector2.zero;
        }
        CharacterManager.Instance.UnFreezeAll();


        for (int i = 0; i < KillerKlaus.Length; ++i)
            KillerKlaus[i].canKillCamera = true;
        SetCameraValues();
        isChasing = true;

        float posX = Camera.main.transform.position.x;
        float posY = Camera.main.transform.position.y;
        if (blockHorizontal)
        {
            posX = PosInX;
            CameraZoom.Instance.SetTargetSizeIndme(ZoomDefault);
        }
        if (blockVertical)
        {
            posY = PosInY;
            CameraZoom.Instance.SetTargetSizeIndme(ZoomDefault);

        }
        Camera.main.transform.position = new Vector3(posX, posY, Camera.main.transform.position.z);


    }

    void ChangeSpot()
    {
        currentSpot++;
        if (currentSpot >= Spots.Length)
        {
            FinishChase();
        }
        else
        {
            SetCameraValues();
            isChasing = true;
        }
    }

    public void FinishChase()
    {
        if (isChasing)
        {
            for (int i = 0; i < KillerKlaus.Length; ++i)
                KillerKlaus[i].canKillCamera = false;
            CameraChase.Instance.FinishChase();
            isChasing = false;
            ResetValuesChasin();
        }

    }

    void SetCameraValues()
    {

        CameraChase.Instance.StartChase(Spots[currentSpot], DamptTimeSpots[currentSpot]);
    }

    public void KillKlaus(CheckPoint check)
    {
        if (isChasing)
        {
            isChasing = false;
            PositionToRespawn = check.transform.position;
            currentSpot = check.PositionChase;
            ChaseByDead = true;

            currentTimePlayLevel = Door.TimePlayingLevel;
            CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.0f);
            CameraFade.Instance.m_OnFadeFinish += RestartScene;

        }
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= RestartScene;
    }

    void RestartScene()
    {
        LoadLevelManager.Instance.RestartCurrentLevel();
    }

    List<Coroutine> killing = new List<Coroutine>();

    public void StartKill(int i)
    {
        if (killing[i] != null)
            return;

        //  StopKill(i);
        killing[i] = StartCoroutine("KillerCounter", TimeTokill);
    }

    public void StopKill(int i)
    {
        if (killing[i] == null)
            return;
        StopCoroutine(killing[i]);
        killing[i] = null;
    }

    IEnumerator KillerCounter(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        for (int i = 0; i < Player.Length; ++i)
        {
            killing[i] = null;
            Player[i].Kill();
        }
    }
}
