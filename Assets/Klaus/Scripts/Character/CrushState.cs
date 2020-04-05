//
// CrushState.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Luminosity.IO;

public class CrushState : FSMState, IButtonState
{
    public Animator anim;
    public MoveState moveState;
    public FlipSprite m_Flip;
    public GameObject lowHitSFX;
    public GameObject highHitSFX;
    public GameObject middleHitSFX;
    public GameObject lowHitRumbleSFX;
    Rigidbody2D _rig2D = null;

    public Collider2D puno;
    public SpriteRenderer punoRenderer;

    public Rigidbody2D rigidBody2D
    {

        get
        {

            if (_rig2D == null)
            {
                _rig2D = GetComponent<Rigidbody2D>();
            }

            return _rig2D;
        }
    }

    [SerializeField]
    InputActionOld accionButton = InputActionOld.Action;
    [SerializeField]
    InputActionOld VerticalAxis = InputActionOld.Movement_Y;

    protected Dictionary<string, float> buttonMapAxis = new Dictionary<string, float>();
    protected bool crushInput = false;
    protected float directionAxis = 0;

    protected float inputOffset = 0.5f;

    public float TimeToCrush = 0.4f;
    public float TimeToCrushUp = 0.8f;
    public float TimeToCrushDown = 0.5f;
    protected float store_GravityScale = 0;
    public float GravityScaleDown = 4.5f;

    public float ForceUp = 100.0f;
    bool useForceUp = false;
    bool isDown = false;
    [HideInInspector]
    public bool canPuchUp = true;
    [HideInInspector]
    public bool timerPuchUp = true;
    public float TimeToPushUp = 1.0f;
    public TimerVarHelper timerVar;

    public AnimatorFxController FX;

    public Action LaunchPunchAction;

    public AudioSource audio
    {
        get
        {
            if (_audio == null)
                _audio = GetComponent<AudioSource>();
            return _audio;
        }
    }

    private AudioSource _audio;
    public float ShakeTime = 0.5f;
    public int ShakePreset = 0;

    [HideInInspector]
    public TypeCrush typeC = TypeCrush.Middle;

    CrushInRaw_Trophy _crush_Trophy;
    public CrushInRaw_Trophy crush_Trophy
    {
        get
        {
            if (_crush_Trophy == null)
            {
                _crush_Trophy = GameObject.FindObjectOfType<CrushInRaw_Trophy>();
            }
            return _crush_Trophy;
        }
    }
    public void SetButton(InputActionOld button, bool value)
    {
    }

    public void SetButton(InputActionOld button, float value)
    {
        if (VerticalAxis == button)
        {
            directionAxis = value;
        }
    }

    public void SetButtonUp(InputActionOld button, bool value)
    {
    }

    public void SetButtonDown(InputActionOld button, bool value)
    {
        if (accionButton == button)
        {
            crushInput = value;
        }
    }


    public bool isCrushing
    {
        get
        {
            return crushInput;
        }
    }



    protected override void Awake()
    {
        base.Awake();
        stateID = StateID.Crush;
        AddTransition(Transition.CrushToMove, StateID.Move);
        AddTransition(Transition.MoveToDead, StateID.Dead);
        lowHitSFX.CreatePool(1);
        highHitSFX.CreatePool(1);
        middleHitSFX.CreatePool(1);
        lowHitRumbleSFX.CreatePool(1);
    }

    public bool isLookingToMe(Transform pos)
    {
        if (pos != null)
        {
            Vector3 dir = pos.position - transform.position;
            if (m_Flip.isFacingRight)
            {
                if (dir.normalized.x >= 0)
                    return true;

            }
            else
            {
                if (dir.normalized.x <= 0)
                    return true;
            }
        }
        return false;
    }

    void SetTimerPushUp()
    {
        //        Debug.Log("Set true");
        timerPuchUp = true;
    }

    public bool CanCrush()
    {
        if (directionAxis > inputOffset)//arriba
        {
            if (canPuchUp)
                return true;
        }
        else if (directionAxis < -1 * inputOffset)//abajo
        {
            return true;

        }
        else//middle
        {
            return true;

        }
        return false;
    }

    public override void DoBeforeEntering()
    {
 
        // SI hay suscriptores a este evento, NO lanzar ataque
        if (LaunchPunchAction != null)
        {
            LaunchPunchAction();
            fsm.PerformTransition(Transition.CrushToMove);
            return;
        }

        //rigidbody2D.isKinematic = true;
        rigidBody2D.velocity = Vector2.zero;
        store_GravityScale = rigidBody2D.gravityScale;
        rigidBody2D.gravityScale = 0;
        anim.SetBool("isCrushing", true);
        anim.SetTrigger("CrushTrigger");

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);

        anim.SetFloat("CrushDir", directionAxis);

        //Defino al direccion del golpe:
        if (directionAxis > inputOffset && canPuchUp)//arriba
        {
            typeC = TypeCrush.Upper;
            useForceUp = true;
            canPuchUp = false;
            timerPuchUp = false;
            moveState.SetIsNONWhenJump();
            StartCoroutine("StartCrush", TimeToCrushUp);
            StartCoroutine(timerVar.StartTime(SetTimerPushUp, TimeToPushUp + TimeToCrushUp));
            highHitSFX.Spawn(transform.position, transform.rotation);
        }
        else if (directionAxis < -1 * inputOffset)//abajo
        {
            typeC = TypeCrush.Down;
            isDown = true;
            moveState.RemoveLayerToWhatisGround("Obstacle");
            StartCoroutine("StartCrush", TimeToCrushDown);
            lowHitSFX.Spawn(transform.position, transform.rotation);
        }
        else//middle
        {
            typeC = TypeCrush.Middle;
            puno.enabled = true;
            punoRenderer.enabled = true;

            StartCoroutine("StartCrush", TimeToCrush);
            anim.SetFloat("CrushDir", 0);
            middleHitSFX.Spawn(transform.position, transform.rotation);
        }

    }

    public override void DoBeforeLeaving()
    {
        StopCoroutine("StartCrush");

        //rigidbody2D.isKinematic = false;
        anim.SetBool("isCrushing", false);
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        rigidBody2D.gravityScale = store_GravityScale;
        useForceUp = false;
        isDown = false;
        moveState.AddLayerToWhatisGround("Obstacle");

        puno.enabled = false;
        punoRenderer.enabled = false;
        punoRenderer.color = new Color(punoRenderer.color.r, punoRenderer.color.g, punoRenderer.color.b, 0);
        moveState.checkGround();
        moveState.CheckVelocityOffGround();
    }

    protected override void FixedUpdateChild()
    {

        if (resume)
        {

            float speedPaltformX = 0;
            float speedPaltformY = rigidBody2D.velocity.y;

            if (moveState.getOnPlatform() != null && typeC == TypeCrush.Middle)
            {
                speedPaltformX = moveState.getOnPlatform().velocity.x;
                speedPaltformY = moveState.getOnPlatform().velocity.y;
            }
            rigidBody2D.velocity = new Vector2(Mathf.Clamp(speedPaltformX, -moveState.MaxSpeed.x, moveState.MaxSpeed.x), Mathf.Clamp(speedPaltformY, -moveState.MaxSpeed.y, moveState.MaxSpeed.y));

            moveState.checkGround();
            if (moveState.isGround)
                moveState.CheckHeadKill();
        }
    }

    protected override void UpdateChild()
    {
        if (resume)
        {
            if (useForceUp)
            {
                puno.enabled = true;
                punoRenderer.enabled = true;
                useForceUp = false;
                rigidBody2D.gravityScale = 0;
                rigidBody2D.velocity = Vector2.zero;
                rigidBody2D.AddForce(ForceUp * transform.up, ForceMode2D.Impulse);
            }
            else if (isDown)
            {
                //Compruebo la caida hasta que toque piso
            }
            else
            {
                rigidBody2D.gravityScale = 0;
            }
        }
    }

    public ParticleSystem particlePunchground;
    public ParticleSystem colorParticleGround_1;
    public ParticleSystem colorParticleGround_2;

    IEnumerator StartCrush(float time)
    {
        rigidBody2D.gravityScale = 0;
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        if (isDown)
        {
            puno.enabled = true;
            punoRenderer.enabled = true;
        }
        int numDestroyObject = 0;
        if (SaveManager.Instance.dataKlaus != null)
            numDestroyObject = SaveManager.Instance.dataKlaus.destroy_object;

        while (isDown)
        {
            if (resume)
            {
                rigidBody2D.gravityScale = GravityScaleDown;
                bool aux = !moveState.checkGround();
                if (isDown && aux == false)
                {
                    lowHitRumbleSFX.Spawn(transform.position, transform.rotation);

                    if (CameraShake.Instance != null)
                    {
                        CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);
                    }
                    //Agrego efecto de camera
                    FX.EffectJump();
                    FX.EffectChangeFast();
                    if (particlePunchground != null)
                    {
                        var main = colorParticleGround_1.main;
                        var main2 = colorParticleGround_2.main;

                        main.startColor = ColorWorldManager.Instance.getColorScene();
                        main2.startColor = ColorWorldManager.Instance.getColorScene();

                        if (particlePunchground.gameObject.activeSelf)
                        {
                            particlePunchground.gameObject.SetActive(false);
                        }
                        particlePunchground.gameObject.SetActive(true);
                    }
                    isDown = aux;
                    moveState.AddLayerToWhatisGround("Obstacle");


                    int auxNumDestroy = 0;
                    if (SaveManager.Instance.dataKlaus != null)
                        auxNumDestroy = SaveManager.Instance.dataKlaus.destroy_object;

                    //Calback con la resta
                    if (crush_Trophy != null)
                        crush_Trophy.OnGetDestroy(auxNumDestroy - numDestroyObject);
                }

            }
            yield return null;
        }
        fsm.PerformTransition(Transition.CrushToMove);
    }

    public void FinishDownCrush()
    {
        if (isDown)
        {
            StopCoroutine("StartCrush");
            lowHitRumbleSFX.Spawn(transform.position, transform.rotation);
            if (CameraShake.Instance != null)
                CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);
            isDown = false;
            moveState.AddLayerToWhatisGround("Obstacle");
            fsm.PerformTransition(Transition.CrushToMove);
        }

    }

    #region Pause:

    bool resume = true;
    Vector2 velPause = Vector3.zero;

    public void OnPauseGame()
    {
        resume = false;
        velPause = rigidBody2D.velocity;
        rigidBody2D.velocity = Vector2.zero;
        anim.speed = 0;
        rigidBody2D.isKinematic = true;
    }

    public void OnResumeGame()
    {
        resume = true;
        rigidBody2D.isKinematic = false;

        rigidBody2D.velocity = velPause;
        anim.speed = 1;
    }

    #endregion
}
