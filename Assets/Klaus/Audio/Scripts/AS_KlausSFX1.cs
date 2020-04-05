using UnityEngine;
using System.Collections;

public class AS_KlausSFX1 : MonoBehaviour
{

    public MoveState moveState;
    public DeadState deadState;
    //public LadderState ladderState;
    //public AudioClip nextClip;
    //public AudioClip corre;
    public AudioClip[] steps;
    private AudioSource audioRun;
    public bool camina = false;
    public bool moving = false;
    public bool corre = false;
    public bool onGround = false;
    public bool saveGround = false;
    public bool ladder = false;
    public bool plat;
    public int paso;
    public float walkTimer = 0.0f;
    public float walkCooler = 3.0f;
    public float maxVol;
    public GameObject jump;
    public GameObject dead;
    public GameObject ground1;
    public GameObject ground2;
    public GameObject ground3;
    public GameObject groundPlat1;
    public GameObject groundPlat2;
    public GameObject groundPlat3;


    void Start()
    {
        /// Suscribete al estado muerte con la funcion spawndead.
        deadState.SuscribeOnStart(SpawnDead);
        moveState.SuscribeOnStart(StartMove);
        moveState.SuscribeOnJump(JumpMove);
        moveState.SuscribeOnEnd(EndMove);
        audioRun = GetComponent<AudioSource>();
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

    // Update is called once per frame
    void LateUpdate()
    {
        if (moveState.isEnter || moveState.isEnterElevator)//intento parar la respiracion pero no lo logro :(
        {
            EndMove();
        }
        else
        {
            StartMove();
        }
        if (moveState.isInPlatform)
        {
            plat = true;
        }
        if (!moveState.isInPlatform)
        {
            plat = false;
        }
        //Walktimer value rolloff	
        if (walkTimer > 0.0f)
        {

            walkTimer -= Time.deltaTime;
        }
        if (walkTimer < 0.0f)
        {

            walkTimer = 0.0f;
        }

        //Camina, Corre y esta en idle
        if (moving)
        {
            if (walkTimer == 0.0f)
            {

                //CAMINA y Corre
                if (moveState.GetComponent<Rigidbody2D>().velocity.x != 0 && moveState.isGround && moveState.isMovingByControl)
                {
                    if (!moveState.canRun)
                    {
                        camina = true;
                        corre = false;
                        if (audioRun.volume > 0)
                        {
                            audioRun.volume -= 0.5f * Time.deltaTime;
                        }
                        if (!corre)
                        {

                            corre = false;
                            //Si antes no caminaba y ahora si
                            if (paso == 1)
                            {
                                paso = 2;

                                //resp2.Spawn(transform.position, transform.rotation);
                                if (moveState.isInPlatform)
                                {
                                    groundPlat1.Spawn(transform.position, transform.rotation);
                                }
                                if (!moveState.isInPlatform)
                                {
                                    ground1.Spawn(transform.position, transform.rotation);
                                }
                            }
                            else
                            {
                                paso = 1;

                                //resp1.Spawn(transform.position, transform.rotation);
                                if (moveState.isInPlatform)
                                {
                                    groundPlat2.Spawn(transform.position, transform.rotation);
                                }
                                if (!moveState.isInPlatform)
                                {
                                    ground2.Spawn(transform.position, transform.rotation);
                                }
                            }
                            walkCooler = 0.35f;
                            walkTimer = walkCooler;
                        }
                    }
                    else
                    {
                        camina = false;

                        if (audioRun.volume < maxVol)
                        {//Sonido de respiracion
                            audioRun.volume += 0.5f * Time.deltaTime;
                        }
                        if (!camina)
                        {

                            corre = true;
                            /*if(moveState.isEnter || moveState.isEnterElevator)//deja de correr y caminar en las puertas, asensores
							{
								corre = false;
								camina = true;
								Debug.Log ("isEnter");

							}*/
                            //Aqui hago sonido de correr
                            if (paso == 1)
                            {
                                paso = 2;
                                //ground1.Spawn(transform.position, transform.rotation);
                                //resp2.Spawn(transform.position, transform.rotation);
                                if (moveState.isInPlatform)
                                {
                                    groundPlat1.Spawn(transform.position, transform.rotation);
                                }
                                if (!moveState.isInPlatform)
                                {
                                    ground1.Spawn(transform.position, transform.rotation);
                                }
                            }
                            else
                            {
                                paso = 1;
                                //ground2.Spawn(transform.position, transform.rotation);
                                //resp2.Spawn(transform.position, transform.rotation);
                                if (moveState.isInPlatform)
                                {
                                    groundPlat2.Spawn(transform.position, transform.rotation);
                                }
                                if (!moveState.isInPlatform)
                                {
                                    ground2.Spawn(transform.position, transform.rotation);
                                }
                            }
                            walkCooler = 0.25f;
                            walkTimer = walkCooler;
                        }

                    }
                }
                else
                {

                    walkCooler = 0.0f;
                    camina = false;
                    corre = false;
                    if (audioRun.volume > 0)
                    {

                        audioRun.volume -= 0.5f * Time.deltaTime;
                    }

                }
            }

        }
        else//END MOVING
        {
            if (audioRun.volume > 0)
            {
                audioRun.volume -= 0.5f * Time.deltaTime;
            }
        }

        if (moveState.isGround && !onGround)//Sonido de cayo
        {
            if (moveState.GetComponent<Rigidbody2D>().velocity.x == 0)
            {
                if (moveState.isInPlatform)
                {
                    groundPlat3.Spawn(transform.position, transform.rotation);
                   
                }
                if (!moveState.isInPlatform)
                {
                    if (moveState.GetComponent<Rigidbody2D>().velocity.y <= 0)//ignoro SFX en las one way platform
                    {
                        if (OneWaySingleton.Instance != null)
                        {
                            ground3.Spawn(transform.position, transform.rotation);
                           

                            /*if (!OneWaySingleton.Instance.isKlausOneWay)
                            {
                                ground3.Spawn(transform.position, transform.rotation);

                            }*/
                        }
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


    }
    //END LATE UPDATE
}
