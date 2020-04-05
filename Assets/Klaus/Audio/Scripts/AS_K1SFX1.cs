using UnityEngine;
using System.Collections;

public class AS_K1SFX1 : MonoBehaviour
{

    public MoveState moveState;
    public DeadState deadState;
    public CrouchState crouchState;
    public ThrowState throwState;
    public CrushState crushState;

    //public AudioClip nextClip;
    //public AudioClip corre;
    public AudioClip[] steps;
    public bool camina = false;
    public bool moving = false;
    public bool corre = false;
    public bool onGround = false;
    public bool rio = false;
    public bool saveGround = false;
    public bool crushB = false;
    public float volumeP;
    public float walkTimer = 0.0f;
    public float walkCooler = 3.0f;
    public GameObject jump;
    public GameObject dead;
    public GameObject crouch;
    public GameObject throws;
    public GameObject crush;
    public GameObject ground;
    public GameObject runLaugh;

    void Start()
    {
        volumeP = GetComponent<AudioSource>().volume;
        /// Suscribete al estado muerte con la funcion spawndead.
        deadState.SuscribeOnStart(SpawnDead);
        moveState.SuscribeOnStart(StartMove);
        moveState.SuscribeOnJump(JumpMove);
        moveState.SuscribeOnEnd(EndMove);
        crouchState.SuscribeOnStart(Crouching);
        throwState.SuscribeOnStart(Throwing);
        //        crushState.SetFunct(Crushing, CrouchingEND);
        throwState.SuscribeOnEnd(Throwing2);

    }

    /// <summary>
    /// Spaw el sonido de muerte
    /// </summary>
    /// 
    void StartMove()
    {
        moving = true;
    }

    void EndMove()
    {
        moving = false;
    }

    void JumpMove(float percent)
    {
        jump.Spawn(transform.position, transform.rotation);
    }

    void SpawnDead()
    {
        dead.Spawn(transform.position, transform.rotation);
    }

    void Crouching()//agacharse
    {
        crouch.Spawn(transform.position, transform.rotation);
    }

    void CrouchingEND()//cuando terminas de golpear
    {
        //no pongo nada
    }

    void Throwing()// aqui me agacho
    {
        crouch.Spawn(transform.position, transform.rotation);
    }

    void Throwing2()//aqui lock lanzo
    {
        throws.Spawn(transform.position, transform.rotation);
    }

    void Crushing()//lanzar el golpe
    {
        crushB = true;
        crush.Spawn(transform.position, transform.rotation);
    }
    // Update is called once per frame
    void LateUpdate()
    {

        //Walktimer value rolloff	
        if (walkTimer > 0.0f)
        {

            walkTimer -= Time.deltaTime;
        }
        if (walkTimer < 0.0f)
        {

            walkTimer = 0.0f;
        }

        //Camina y Corre
        if (moving)
        {
            if (walkTimer == 0.0f)
            {

                //CAMINA
                if (!moveState.canRun && moveState.GetComponent<Rigidbody2D>().velocity.x != 0 && moveState.isGround && moveState.isMovingByControl)
                {
                    camina = true;
                    rio = false;
                    if (!corre)
                    {

                        corre = false;
                        //Si antes no caminaba y ahora si
                        ground.Spawn(transform.position, transform.rotation);
                        walkCooler = 0.35f;
                        walkTimer = walkCooler;
                    }
                }
                else
                {

                    walkCooler = 0.0f;
                    camina = false;
                }
                //CORRE
                if (moveState.canRun && moveState.GetComponent<Rigidbody2D>().velocity.x != 0 && moveState.isGround && moveState.isMovingByControl)
                {
                    camina = false;
                    if (!camina)
                    {

                        corre = true;
                        //Aqui hago sonido de correr
                        ground.Spawn(transform.position, transform.rotation);
                        walkCooler = 0.25f;
                        walkTimer = walkCooler;
                    }


                }
                else
                {

                    walkCooler = 0.0f;
                    corre = false;
                }
            }

        }//END MOVING

        if (moveState.isGround && !onGround)//Sonido de cayo
        {
            if (Mathf.Approximately(moveState.GetComponent<Rigidbody2D>().velocity.x, 0.0f))
            {
                if (moveState.GetComponent<Rigidbody2D>().velocity.y <= 0)//ignoro SFX en las one way platform
                {
                    if (OneWaySingleton.Instance != null)
                    {
                        ground.Spawn(transform.position, transform.rotation);
                        /*
                        if (OneWaySingleton.Instance.isK1OneWay)
                        {
                            ground.Spawn(transform.position, transform.rotation);
                        }*/
                    }
                }

            }
            onGround = true;
        }
        if (!moveState.isGround)
        {
            onGround = false;
            saveGround = false;
        }

        if (corre && Mathf.Approximately(moveState.GetComponent<Rigidbody2D>().velocity.y , 0.0f) && !saveGround && !rio)
        {//no ria cuando salta

            rio = true;
            saveGround = true;
            runLaugh.Spawn(transform.position, transform.rotation);
        }


    }
    //END LATE UPDATE
}
