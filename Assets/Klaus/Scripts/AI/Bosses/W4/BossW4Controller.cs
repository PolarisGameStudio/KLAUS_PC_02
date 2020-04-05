using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using Colorful;

public class BossW4Controller : StateBehaviour, ICrushObject, ICompleteLevel
{
    #region Difficulties

    [Serializable]
    public class Difficulty
    {
        [Header("General setup")]
        public float movementSpeed = 1f;
        public States[] attacksSequence;
        public Color hitColor = Color.white;
        public GameObject effectToEnable;
        public float hitSpeed;
        public float timeToWakeUp = 3f;
        [Space(1f)]

        [Header("Electric Bolts setup")]
        public int boltsThrown = 1;
        public float boltsSpeed = 8f;
        public Vector2 boltsDelay;
        [Space(1f)]

        [Header("Dash setup")]
        public int dashBounces = 1;
        public Vector2 dashDelay;
        [Space(1f)]

        [Header("Spread Bullets setup")]
        public int spreadBullets = 8;
        public float spreadSpeed = 8f;
        public float timeBetweenSpreadWaves = 1f;
        public float[] spreadWavesRotations;
        public Vector2 spreadDelay;
        [Space(1f)]

        [Header("Vertical Bullets setup")]
        public int verticalBullets = 8;
        public float verticalBulletsSpeed = 8f;
        public float shootPauseTime = 1f;
        public float timeBetweenWaves = 1f;
        public Vector2 verticalDelay;
        [Space(1f)]

        [Header("Laser setup")]
        public float laserLockTime = 1f;
        public float laserShootTime = 1f;
        public float bigBulletSpeed = 10f;
        public float blurTime = 0.15f;
        public Vector2 laserDelay;
        public float raySpeed = 5f;
        [Space(1f)]

        [HideInInspector]
        public int currentAttack;
    }

    #endregion

    #region Setup

    public enum States
    {
        Idle,
        Intro,
        ElectricBolts,
        Dash,
        SpreadBullets,
        VerticalBullets,
        Laser,
        Vulnerable,
        FightStart,
        Victory,
        Cutscene
    }

    [Header("SFX")]
    public GameObject messageMix;
    public AudioSource sfxAudio;
    public AudioSource sfxAudio2;
    public AudioSource sfxAudio3;
    public AudioSource audioEnding;
    public AudioClip shieldDeathSFX;
    public AudioClip electricBoltSFX;
    public AudioClip spreadBulletSFX;
    public AudioClip bigBulletSFX;
    public AudioClip punchSFX;
    public AudioClip riseSFX;
    public AudioClip rumbleSFX;
    public AudioSource klausAudio;
    public AudioClip klausLaugh;
    public AudioClip klausHacking;
    public AudioClip klausHaked;
    public AudioClip klausMad;
    public AudioClip[] klausDeathSFX;
    public AudioClip[] klausTired;
    public AudioClip[] klausWakes;
    public AudioClip[] klausJA;
    public float speed;
    private bool played = false;
    private bool toggleBulletAS = true;
    [Space(1f)]

    [Header("Game elements")]
    public Difficulty[] difficulties;

    [Header("Limits and positions")]
    public Vector2 minPoint;
    public Vector2 maxPoint;

    int currentLevel = 0;
    bool grounded;
    float groundRadius = 0.09f;

    [Header("Other stuff")]
    public LayerMask groundLayer;
    public Animator gameText;

    [Header("Blur")]
    public RadialBlur blur;
    public Vector2 blurLimits = new Vector2(0.01f, 0.1f);
    public float blurSpeed = 1f;
    bool blurEnabled = false;
    public SpriteRenderer K1SpriteAngry;

    public void OnLevelWasLoaded(int level)
    {
        SaveManager.Instance.SaveCurrentLevel(Application.loadedLevelName);
    }

    void Start()
    {


        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Object"), false);

        // Find other player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
            if (player.name == "K1")
            {
                target = player.transform;
                target.GetComponentInChildren<DeadState>().SuscribeOnStart(OnPlayerDead);
                break;
            }

        // Create pools
        electricBoltPrefab.CreatePool<BulletHandler>(50);
        spreadBulletPrefab.CreatePool<BulletHandler>(50);
        verticalBulletPrefab.CreatePool<BulletHandler>(50);
        bigBulletPrefab.CreatePool<BulletHandler>(5);

        shieldTrigger.onTriggerEnter += CheckForKill;
        lastPosition = cachedRigidbody.position;

        // Set death time
        DeadState state = target.GetComponentInChildren<DeadState>();
        state.deathTime = timeToRevive;

        Initialize<States>();
        ChangeState(States.Intro);

        CallTrophy();
        CounterTimerPlay.Instance.StartTime();
    }

    void OnApplicationQuit()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Object"), false);
    }

    public void ResetBoss()
    {
        StopAllCoroutines();
        animator.Rebind();
        ChangeState(States.FightStart, StateTransition.Overwrite);
        StartCoroutine("VolDown", sfxAudio2);
    }

    #endregion

    #region Components

    public SpriteRenderer spriteRenderer;

    public Blink blink
    {
        get
        {
            if (_blink == null)
                _blink = GetComponentInChildren<Blink>();
            return _blink;
        }
    }

    public Transform electricBoltPoint
    {
        get
        {
            if (_electricBoltPoint == null)
                _electricBoltPoint = transform.Find("ShootPoints/ElectricBoltPoint");
            return _electricBoltPoint;
        }
    }

    public Transform verticalBulletPoint
    {
        get
        {
            if (_verticalBulletPoint == null)
                _verticalBulletPoint = transform.Find("ShootPoints/VerticalBulletPoint");
            return _verticalBulletPoint;
        }
    }

    public Transform groundTransform
    {
        get
        {
            if (_groundTransform == null)
                _groundTransform = transform.Find("Ground");
            return _groundTransform;
        }
    }

    public Rigidbody2D cachedRigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();
            return _rigidbody;
        }
    }

    public Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }

    public FlipSprite flipSprite
    {
        get
        {
            if (_flipSprite == null)
                _flipSprite = GetComponentInChildren<FlipSprite>();
            return _flipSprite;
        }
    }

    public ShieldTrigger shieldTrigger
    {
        get
        {
            if (_shieldTrigger == null)
                _shieldTrigger = GetComponentInChildren<ShieldTrigger>();
            return _shieldTrigger;
        }
    }

    public ResetBossFight resetTrigger
    {
        get
        {
            if (_resetTrigger == null)
                _resetTrigger = GameObject.FindObjectOfType<ResetBossFight>();
            return _resetTrigger;
        }
    }

    public SpriteRenderer overlay
    {
        get
        {
            if (_overlay == null)
                _overlay = transform.Find("Overlay").GetComponent<SpriteRenderer>();
            return _overlay;
        }
    }

    Blink _blink;
    Transform _electricBoltPoint;
    Transform _verticalBulletPoint;
    Transform _rafaga;
    Transform _groundTransform;
    Rigidbody2D _rigidbody;
    Animator _animator;
    Transform target;
    ShieldTrigger _shieldTrigger;
    ResetBossFight _resetTrigger;
    FlipSprite _flipSprite;
    SpriteRenderer _overlay;

    #endregion

    #region Intro

    [Header("Intro Setup")]

    public TweenTextShow[] dialogues;
    public float[] timeBetweenDialogues;

    StartFightTrigger fightTrigger;

    void Intro_Enter()
    {
        ResetFight();
        animator.SetTrigger("OnIntro");
        blackScreen.CrossFadeAlpha(0, 0, true);

        fightTrigger = GameObject.FindObjectOfType<StartFightTrigger>();
        fightTrigger.onEnterTrigger += PlayerFallingOnTube;
        fightTrigger.onExitTrigger += PlayerEnteredRoom;
    }

    void PlayerFallingOnTube()
    {
        fightTrigger.onEnterTrigger -= PlayerFallingOnTube;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Object"), true);
    }

    void PlayerEnteredRoom()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Object"), false);
        CharacterManager.Instance.FreezeAll();

        fightTrigger.onExitTrigger -= PlayerEnteredRoom;
        StartCoroutine("IntroSequence");
        resetTrigger.AddFightCheckpoint(target.GetComponent<Collider2D>());

        //Baja Musik Clones
        messageMix.SendMessage("FightMix");
        if (AS_W4BossMusik.Instance != null)
        {
            AS_W4BossMusik.Instance.MusikClonesDown();
        }
        if (AS_W4BossMusik.Instance != null && !played)
        {
            played = true;
            AS_W4BossMusik.Instance.MusikFight();
        }
    }

    IEnumerator IntroSequence()
    {
        target.GetComponentInChildren<FlipSprite>().FlipIfCanFlip(Vector3.right);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1f));

        // Show dialogues
        for (int i = 0; i != dialogues.Length; ++i)
        {
            dialogues[i].InitText();
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeBetweenDialogues[i]));
        }

        // Hide dialogues
        yield return new WaitForSeconds(1f);
        for (int i = 0; i != dialogues.Length; ++i)
        {
            dialogues[i].HideText();
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.1f));
        }

        // Make Klaus mad
        yield return new WaitForSeconds(0.2f);
        sfxAudio.clip = riseSFX;
        sfxAudio.volume = 0.4f;
        sfxAudio.Play();
        animator.SetTrigger("IntroEnded");
        klausAudio.clip = klausLaugh;
        klausAudio.spatialBlend = 0;
        klausAudio.PlayDelayed(1.5f);
        sfxAudio2.clip = rumbleSFX;
        sfxAudio2.volume = 0;
        sfxAudio2.loop = true;
        klausAudio.spatialBlend = 0;
        sfxAudio2.Play();
        StartCoroutine("VolUp", sfxAudio2);
        yield return new WaitForSeconds(3f);

        // Move down here

        // Show text
        gameText.SetTrigger("Show");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1.5f));

        // Start fight
        ChangeState(States.FightStart);
    }

    IEnumerator Intro_Exit()
    {
        yield return StartCoroutine(MoveTowards(new Vector2(maxPoint.x, minPoint.y), difficulties[currentLevel].movementSpeed));
    }

    #endregion

    #region Fight Start

    void ResetFight()
    {
        ElectricBoltsReset();
        DashReset();
        SpreadBulletsReset();
        VerticalBulletsReset();
        LaserReset();
        VulnerableReset();

        CameraShake.Instance.StopShake();

        DisableBlur();
        blur.Strength = 0;

        blink.Stop();
        spriteRenderer.color = Color.white;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Object"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Boss"), false);
    }

    IEnumerator FightStart_Enter()
    {
        // Locate the player on an actual site
        cachedRigidbody.position = new Vector2(maxPoint.x, minPoint.y);

        // Reset everything
        ResetFight();

        // Reset other stuff
        currentLevel = 0;

        foreach (Difficulty difficulty in difficulties)
        {
            difficulty.currentAttack = 0;
            if (difficulty.effectToEnable)
                difficulty.effectToEnable.SetActive(false);
        }

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.1f));
        blackScreen.CrossFadeAlpha(0f, timeToRevive / controlOverBlack, true);
        StartCoroutine("StartSequence");

        //MusikFight
        /*if (AS_W4BossMusik.Instance != null && !played)
        {
            played = true;
            AS_W4BossMusik.Instance.MusikFight();
        }*/
    }

    IEnumerator StartSequence()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1f));
        PlayCurrentAttack();
    }

    void FightStart_Exit()
    {
        CharacterManager.Instance.UnFreezeAll();
    }

    #endregion

    #region Electric Bolts

    [Header("Electric Bolts")]
    public BulletHandler electricBoltPrefab;

    void ElectricBoltsReset()
    {
        electricBoltPrefab.RecycleAll<BulletHandler>();
    }

    IEnumerator ElectricBolts_Enter()
    {
        yield return StartCoroutine(MoveTowards(new Vector2(maxPoint.x, minPoint.y), difficulties[currentLevel].movementSpeed));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].boltsDelay.x));

        animator.SetBool("ElectricBolt", true);
        StartCoroutine("ElectricBoltsLogic");
    }

    IEnumerator ElectricBoltsLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];
        klausAudio.clip = klausJA[UnityEngine.Random.Range(0, klausJA.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        // Throw each bolts wave
        for (int i = 0; i != difficulty.boltsThrown; ++i)
        {
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.5f));

            // Throw bolt
            sfxAudio.clip = electricBoltSFX;
            sfxAudio.volume = 1;
            sfxAudio.Play();
            BulletHandler bullet = electricBoltPrefab.Spawn<BulletHandler>(electricBoltPoint, Vector3.zero, Quaternion.identity);
            bullet.maxSpeed = difficulty.boltsSpeed;
            bullet.SetDirection(new BulletInfo(bullet.transform.right, 60f));
        }

        ToNextAttack();
    }

    IEnumerator ElectricBolts_Exit()
    {
        animator.SetBool("ElectricBolt", false);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].boltsDelay.y));
    }

    #endregion

    #region Dash

    void DashReset()
    {
        flipSprite.FlipIfCanFlip(Vector2.right);
    }

    IEnumerator Dash_Enter()
    {
        yield return StartCoroutine(MoveTowards(new Vector2(maxPoint.x, minPoint.y), difficulties[currentLevel].movementSpeed));

        animator.SetBool("Dash", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].dashDelay.x));

        StartCoroutine("DashLogic");
    }

    IEnumerator DashLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];

        animator.SetTrigger("DashLoaded");
        for (int i = 0; i != difficulty.dashBounces; ++i)
        {
            yield return StartCoroutine(MoveTowards(minPoint, difficulty.movementSpeed, restrictY: true));
            flipSprite.FlipIfCanFlip(-Vector2.right);
            yield return StartCoroutine(MoveTowards(new Vector2(maxPoint.x, minPoint.y), difficulty.movementSpeed, restrictY: true));
            flipSprite.FlipIfCanFlip(Vector2.right);
        }

        ToNextAttack();
    }

    IEnumerator Dash_Exit()
    {
        animator.SetBool("Dash", false);
        flipSprite.FlipIfCanFlip(Vector2.right);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].dashDelay.y));
    }

    #endregion

    #region Spread Bullets

    [Header("Spread Bullets")]
    public BulletHandler spreadBulletPrefab;

    void SpreadBulletsReset()
    {
        spreadBulletPrefab.RecycleAll<BulletHandler>();
    }

    IEnumerator SpreadBullets_Enter()
    {
        yield return StartCoroutine(MoveTowards(new Vector2(Mathf.Lerp(minPoint.x, maxPoint.x, 0.5f), maxPoint.y), difficulties[currentLevel].movementSpeed));

        Difficulty difficulty = difficulties[currentLevel];

        /* sfxAudio.clip = spreadBulletSFX;
         sfxAudio.volume = 1;
         sfxAudio.Play();

         if (currentLevel > 1)
         {
             Debug.Log("entro al 2");
             sfxAudio2.clip = spreadBulletSFX;
             sfxAudio2.volume = 1;
             sfxAudio2.loop = false;
             klausAudio.spatialBlend = 1;
             sfxAudio2.PlayDelayed(1);
         }
         if (currentLevel > 2)
         {
             Debug.Log("entro al 3");
             sfxAudio3.clip = spreadBulletSFX;
             sfxAudio3.volume = 1;
             sfxAudio3.PlayDelayed(2);
         }*/
        klausAudio.clip = klausLaugh;
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        animator.SetBool("SpreadAttack", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].spreadDelay.x));
        StartCoroutine("SpreadBulletsLogic");
    }

    IEnumerator SpreadBulletsLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];

        // Make each spread bullets wave
        animator.SetTrigger("LaunchSpreadAttack");
        for (int i = 0; i != difficulty.spreadWavesRotations.Length; ++i)
        {
            // Calculate the angle of each spread
            float initialAngle = difficulty.spreadWavesRotations[i];
            float angleOffset = 360f / (float)difficulty.spreadBullets;

            for (int j = 0; j != difficulty.spreadBullets; ++j)
            {
                BulletHandler bullet = spreadBulletPrefab.Spawn(transform.position, Quaternion.identity);
                bullet.transform.right = Quaternion.Euler(0, 0, initialAngle + (float)j * angleOffset) * transform.right;
                bullet.maxSpeed = difficulty.spreadSpeed;
                bullet.SetDirection(new BulletInfo(bullet.transform.right, 50f));

                if (toggleBulletAS)
                {
                    sfxAudio2.clip = spreadBulletSFX;
                    sfxAudio2.volume = 1;
                    sfxAudio2.loop = false;
                    klausAudio.spatialBlend = 1;
                    sfxAudio2.Play();
                }
                else
                {
                    sfxAudio3.clip = spreadBulletSFX;
                    sfxAudio3.volume = 1;
                    sfxAudio3.loop = false;
                    klausAudio.spatialBlend = 1;
                    sfxAudio3.Play();
                }
                toggleBulletAS = !toggleBulletAS;
            }

            // Wait for some time
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulty.timeBetweenSpreadWaves));
        }

        ToNextAttack();
    }

    IEnumerator SpreadBullets_Exit()
    {
        animator.SetBool("SpreadAttack", false);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].spreadDelay.x));
    }

    #endregion

    #region Vertical Bullets

    [Header("Vertical Bullets")]
    public BulletHandler verticalBulletPrefab;

    void VerticalBulletsReset()
    {
        verticalBulletPrefab.RecycleAll<BulletHandler>();
    }

    IEnumerator VerticalBullets_Enter()
    {
        Vector2 position = maxPoint;
        yield return StartCoroutine(MoveTowards(position, difficulties[currentLevel].movementSpeed));

        animator.SetBool("VerticalAttack", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].verticalDelay.x));
        StartCoroutine("VerticalBulletsLogic");
    }

    IEnumerator VerticalBulletsLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];
        float chunkSize = (maxPoint.x - minPoint.x) / ((float)difficulty.verticalBullets);

        klausAudio.clip = klausJA[UnityEngine.Random.Range(0, klausJA.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        // Going left
        for (int i = 0; i != difficulty.verticalBullets; ++i)
        {
            // Go to shooting position
            Vector2 position = cachedRigidbody.position;
            position.x -= chunkSize;
            yield return StartCoroutine(MoveTowards(position, difficulties[currentLevel].movementSpeed));

            // Shoot and make shoot pause
            sfxAudio.clip = electricBoltSFX;
            sfxAudio.volume = 1;
            sfxAudio.Play();
            animator.SetTrigger("LaunchVerticalAttack");
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.15f));

            BulletHandler bullet = verticalBulletPrefab.Spawn<BulletHandler>(verticalBulletPoint, Vector3.zero, Quaternion.identity);
            bullet.maxSpeed = difficulty.verticalBulletsSpeed;
            bullet.SetDirection(new BulletInfo(bullet.transform.right, 60f));

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulty.shootPauseTime));
        }

        // Wait between the wave
        yield return StartCoroutine(MoveTowards(new Vector2(minPoint.x, cachedRigidbody.position.y), difficulties[currentLevel].movementSpeed));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulty.timeBetweenWaves));

        klausAudio.clip = klausJA[UnityEngine.Random.Range(0, klausJA.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        // Going right
        for (int i = 0; i != difficulty.verticalBullets; ++i)
        {
            // Go to shooting position
            Vector2 position = cachedRigidbody.position;
            position.x += chunkSize;
            yield return StartCoroutine(MoveTowards(position, difficulties[currentLevel].movementSpeed));

            // Shoot and make shoot pause
            sfxAudio.clip = electricBoltSFX;
            sfxAudio.volume = 1;
            sfxAudio.Play();
            animator.SetTrigger("LaunchVerticalAttack");
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.15f));

            BulletHandler bullet = verticalBulletPrefab.Spawn<BulletHandler>(verticalBulletPoint, Vector3.zero, Quaternion.identity);
            bullet.SetDirection(new BulletInfo(bullet.transform.right, 60f));

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulty.shootPauseTime));
        }

        yield return StartCoroutine(MoveTowards(new Vector2(maxPoint.x, cachedRigidbody.position.y), difficulties[currentLevel].movementSpeed));
        ToNextAttack();
    }

    IEnumerator VerticalBullets_Exit()
    {
        animator.SetBool("VerticalAttack", false);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].verticalDelay.y));
    }

    #endregion

    #region Laser

    [Header("Final Attack")]
    public BulletHandler bigBulletPrefab;
    public Transform[] shootPositions;
    public GameObject[] loadEffects;
    public GameObject[] onActivationEffects;
    public GameObject[] beforeReleaseEffects;
    public GameObject transitionEffect;
    public GameObject[] onReleaseEffects;

    void LaserReset()
    {
        overlay.color = new Color(0, 0, 0, 0);
        bigBulletPrefab.RecycleAll<BulletHandler>();

        foreach (GameObject effect in loadEffects)
            effect.SetActive(false);

        foreach (GameObject effect in onActivationEffects)
            effect.SetActive(false);

        foreach (GameObject effect in beforeReleaseEffects)
            effect.SetActive(false);

        transitionEffect.SetActive(false);

        foreach (GameObject effect in onReleaseEffects)
            effect.SetActive(false);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ObjectCharacter"), LayerMask.NameToLayer("Boss"), true);
        flipSprite.FlipIfCanFlip(Vector2.right);
    }

    IEnumerator Laser_Enter()
    {
        yield return StartCoroutine(MoveTowards(new Vector2(Mathf.Lerp(minPoint.x, maxPoint.x, 0.5f), cachedRigidbody.position.y), difficulties[currentLevel].movementSpeed));
        yield return StartCoroutine(MoveTowards(new Vector2(cachedRigidbody.position.x, minPoint.y), difficulties[currentLevel].movementSpeed));
        klausAudio.clip = klausMad;
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        sfxAudio.clip = bigBulletSFX;
        sfxAudio.volume = 1;
        sfxAudio.Play();
        animator.SetBool("Laser", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].laserDelay.x));
        StartCoroutine("LaserLogic");
    }

    IEnumerator LaserLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];
        float transitionWaitTime = 0.55f;
        float totalTime = difficulty.laserLockTime - transitionWaitTime;

        // Start creating energy bullets
        BulletHandler[] bullets = new BulletHandler[shootPositions.Length];
        for (int i = 0; i != shootPositions.Length; ++i)
        {
            bullets[i] = bigBulletPrefab.Spawn<BulletHandler>(shootPositions[i], Vector3.zero, Quaternion.identity);
        }

        // Turn off the background light
        StartCoroutine(TweenColor(new Color(0, 0, 0, 0.5f), totalTime * 0.9f));
        CameraShake.Instance.StartShake();

        // Turn on the light shield, the lights-in-ground effect, and the electric ground effect
        foreach (GameObject effect in loadEffects)
            effect.SetActive(true);

        // First 5%: Turn on some lightning animations
        float chunkTime = (totalTime * 0.1f) / ((float)onActivationEffects.Length);
        foreach (GameObject effect in onActivationEffects)
        {
            effect.SetActive(true);
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(chunkTime));
        }

        // Wait a little
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(totalTime * 0.6f));

        // Just before release, turn on some light effects
        chunkTime = (totalTime * 0.2f) / ((float)beforeReleaseEffects.Length);
        foreach (GameObject effect in beforeReleaseEffects)
        {
            effect.SetActive(true);
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(chunkTime));
        }

        // Wait a little more
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(totalTime * 0.1f));

        // Turn on the transition effect and turn on the background light
        transitionEffect.SetActive(true);
        StartCoroutine(TweenColor(new Color(0, 0, 0, 0), 0.1f));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(transitionWaitTime));

        // LAUNCH ATTACK!
        CameraShake.Instance.StopShake();

        // Disable the light shield, the lights-in-ground effect, and the electric ground effect
        foreach (GameObject effect in loadEffects)
            effect.SetActive(false);

        // Turn off the light effects
        foreach (GameObject effect in beforeReleaseEffects)
            effect.SetActive(false);

        // Play the animation
        animator.SetTrigger("LaunchLaser");

        // Launch bullets!
        for (int i = 0; i != shootPositions.Length; ++i)
        {
            bullets[i].maxSpeed = difficulty.bigBulletSpeed;
            bullets[i].GetComponent<Animator>().SetTrigger("Launch");
            bullets[i].SetDirection(new BulletInfo(bullets[i].transform.right, 60f));
        }

        // Blur and shake for a moment
        StartCoroutine("BlurForTime", difficulty.blurTime);

        // Enable the electricity bolts
        foreach (GameObject effect in onReleaseEffects)
            effect.SetActive(true);

        // Stop animation
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.75f));
        animator.SetBool("Laser", false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ObjectCharacter"), LayerMask.NameToLayer("Boss"), false);

        // Turn off the remaining effects
        transitionEffect.SetActive(false);
        foreach (GameObject effect in onReleaseEffects)
            effect.SetActive(false);

        foreach (GameObject effect in onActivationEffects)
            effect.SetActive(false);

        // Make vulnerable
        ChangeState(States.Vulnerable);
    }

    IEnumerator TweenColor(Color color, float time)
    {
        Color initialColor = overlay.color;
        float timer = time;

        while (timer >= 0)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                timer -= Time.deltaTime;
                overlay.color = Color.Lerp(color, initialColor, timer / time);
            }

            yield return null;
        }
    }

    void Laser_Exit()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ObjectCharacter"), LayerMask.NameToLayer("Boss"), true);
        flipSprite.FlipIfCanFlip(Vector2.right);
        animator.SetBool("Laser", false);
    }

    #endregion

    #region Vulnerable

    [Header("Vulnerable")]
    public float vulnerableTime;
    public float punchForce;
    public float decayRate;
    public GameObject vulnerableEffect;

    bool vulnerable;

    void VulnerableReset()
    {
        shieldTrigger.gameObject.SetActive(true);
        vulnerableEffect.SetActive(false);
        vulnerable = false;

        target.GetComponent<CrushState>().LaunchPunchAction -= LastPunchLaunched;
    }

    void Vulnerable_Enter()
    {
        shieldTrigger.gameObject.SetActive(false);
        vulnerableEffect.SetActive(true);
        klausAudio.clip = klausTired[UnityEngine.Random.Range(0, klausTired.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.Play();
        StartCoroutine("VulnerableTimer");

        animator.SetBool("Vulnerable", true);

        // Si es la ultima dificultad, prevengo aqui recibir un golpe (no eres vulnerable realmente
        if (currentLevel == difficulties.Length - 1)
        {
            target.GetComponent<CrushState>().LaunchPunchAction += LastPunchLaunched;
        }
        else
        {
            vulnerable = true;
        }
    }

    public void Crush(TypeCrush type = TypeCrush.Middle)
    {
        if (!vulnerable)
            return;

        vulnerable = false;
        StopCoroutine("VulnerableTimer");
        StartCoroutine("TakeHit");
    }

    IEnumerator TakeHit()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Boss"), true);
        sfxAudio.clip = punchSFX;
        sfxAudio.volume = 1;
        sfxAudio.Play();
        klausAudio.clip = klausDeathSFX[UnityEngine.Random.Range(0, klausDeathSFX.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.Play();

        animator.SetTrigger("Hitted");

        // Go to next difficulty level
        if (difficulties[currentLevel].effectToEnable)
            difficulties[currentLevel].effectToEnable.SetActive(true);
        float timeToWakeUp = difficulties[currentLevel].timeToWakeUp;
        ++currentLevel;

        if (currentLevel >= difficulties.Length)
        {
            // WE SHOULD NEVER ENTER HERE
            Debug.LogWarning("WE SHOULD HAVE NEVER ENTER HERE");
            LastPunchLaunched();
        }
        else
        {
            blink.Play(difficulties[currentLevel].hitSpeed, Color.white, difficulties[currentLevel].hitColor);

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToWakeUp));

            difficulties[currentLevel].currentAttack = 0;
            PlayCurrentAttack();
        }
    }

    IEnumerator VulnerableTimer()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(vulnerableTime));
        ToNextAttack();
    }

    IEnumerator Vulnerable_Exit()
    {
        vulnerable = false;
        target.GetComponent<CrushState>().LaunchPunchAction -= LastPunchLaunched;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1.5f));

        sfxAudio2.clip = rumbleSFX;
        sfxAudio2.volume = 0;
        sfxAudio2.loop = true;
        klausAudio.clip = klausWakes[UnityEngine.Random.Range(0, klausWakes.Length)];
        klausAudio.spatialBlend = 0;
        klausAudio.PlayDelayed(1f);
        sfxAudio2.Play();
        StartCoroutine("VolUp", sfxAudio2);
        animator.SetBool("Vulnerable", false);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.3f));
        shieldTrigger.gameObject.SetActive(true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Boss"), false);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1f));

        vulnerableEffect.SetActive(false);
    }

    #endregion

    #region Victory

    [Header("Death setup")]
    public UnityEngine.UI.Image blackScreen;
    public float timeToRevive = 0.8f;

    [Range(2f, 10f)]
    public float controlOverBlack = 2f;

    void Victory_Enter()
    {
        Debug.Log("YEAH I WIN");
        blackScreen.CrossFadeAlpha(1f, timeToRevive / controlOverBlack, true);
    }

    void OnPlayerDead()
    {
        StopAllCoroutines();
        ChangeState(States.Victory, StateTransition.Overwrite);
    }

    #endregion

    #region Cutscene

    [Header("Cutscene")]
    public BulletHandler finalBulletPrefab;
    public TweenTextShow[] K1Dialogues;
    public float[] K1TimeBewteenDialogues;
    public float timeForKlausAttack;
    public float timeAfterKlausAttack;
    public TweenTextShow[] KlausDialogues;
    public Vector2 cpuPosition;
    public float hackTime = 4f;
    public Animator cpu;
    public string nextScene;
    public GameObject K1;

    bool inFrontOfCpu;

    public void LastPunchLaunched()
    {
        blink.Stop();
        blink.SetColor(Color.white);
        currentLevel = difficulties.Length - 1;
        ChangeState(States.Cutscene, StateTransition.Overwrite);
    }

    IEnumerator Cutscene_Enter()
    {
        audioEnding.PlayDelayed(0.01f);
        DeadState state = target.GetComponentInChildren<DeadState>();
        state.blockRespawn = true;
        state.UnSuscribeOnStart(OnPlayerDead);
        CharacterManager.Instance.FreezeAll();

        // Flip if needed
        if (target.position.x < cachedRigidbody.position.x)
            target.GetComponentInChildren<FlipSprite>().FlipIfCanFlip(Vector3.right);
        else
            target.GetComponentInChildren<FlipSprite>().FlipIfCanFlip(-Vector3.right);

        // Show K1 dialogues

        
        Animator K1Animator = K1.GetComponent(typeof(Animator)) as Animator;
        SpriteRenderer K1Sprite = K1.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        K1Animator.enabled = false;
        K1Sprite.sprite = K1SpriteAngry.sprite;
        

        for (int i = 0; i != K1Dialogues.Length; ++i)
        {
            K1Dialogues[i].InitText();
            if (i == 0)
                CameraShake.Instance.StartShake();
            //VolDownMusik
            if (AS_W4BossMusik.Instance != null)
            {
                AS_W4BossMusik.Instance.MusikMuere();
            }
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(K1TimeBewteenDialogues[i]));

            if (i == 0)
                CameraShake.Instance.StopShake();
        }

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeForKlausAttack));
        K1Animator.enabled = true;

        // Klaus GETS UP AND ATTACKS!!
        animator.SetBool("FinalAttack", true);
        animator.SetBool("Vulnerable", false);

        Blink overlayBlink = overlay.GetComponent<Blink>();
        overlayBlink.Play(5f, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5f));

        foreach (GameObject effect in beforeReleaseEffects)
            effect.SetActive(true);

        foreach (GameObject effect in onReleaseEffects)
            effect.SetActive(true);

        //MusikClonesUp
        messageMix.SendMessage("NormalMix");
        if (AS_W4BossMusik.Instance != null)
        {
            AS_W4BossMusik.Instance.MusikRevive();
        }
        BulletHandler bullet = finalBulletPrefab.Spawn<BulletHandler>(cachedRigidbody.position, Quaternion.identity);
        bullet.transform.right = Vector3.Normalize(target.position - transform.position);
        bullet.SetDirection(new BulletInfo(bullet.transform.right, 60f));

        // Hide dialogues
        for (int i = 0; i != K1Dialogues.Length; ++i)
        {
            K1Dialogues[i].HideText();
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.1f));
        }

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.5f));

        // Now Klaus waits until it exits its trance
        overlayBlink.Stop();
        overlayBlink.SetColor(new Color(0, 0, 0, 0));

        foreach (GameObject effect in onReleaseEffects)
            effect.SetActive(false);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeAfterKlausAttack * 0.1f));

        foreach (GameObject effect in beforeReleaseEffects)
            effect.SetActive(false);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeAfterKlausAttack * 0.9f));

        // He gets up and sees K1
        animator.SetTrigger("WakeUp");
        KlausDialogues[0].HideText();

        Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
            collider.enabled = false;

        target.GetComponentInChildren<Rigidbody2D>().isKinematic = true;

        DynamicCameraManager.Instance.RemoveEspecialTargetForK1();
        DynamicCameraManager.Instance.ChangueEspecialTargetForK1(transform, 0.5f, 7.5f, 0.5f);

        while (!inFrontOfCpu)
            yield return null;

        // Run to the CPU
        animator.SetFloat("SpeedX", 5f);
        yield return StartCoroutine(MoveTowards(cpuPosition, 5f, restrictY: true));
        animator.SetFloat("SpeedX", 0f);
        ShowOutroDialogue(7);

        //lUIS COunter
        float timer = CounterTimerPlay.Instance.EndTime();
        SaveManager.Instance.AddPlayTime(timer);
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName, false, timer, 0, true);

        // Hack the CPU
        LoadLevelManager.Instance.LoadLevel(SaveManager.Instance.isComingFromArcade ? "PrincipalMenu" : nextScene, true);
        animator.SetBool("TurnAround", true);
        animator.SetBool("isCoding", true);
        cpu.SetBool("Hacking", true);
        klausAudio.clip = klausHacking;
        klausAudio.loop = true;
        klausAudio.spatialBlend = 1;
        klausAudio.volume = 0.6f;
        klausAudio.Play();

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(hackTime));

        // CPU hacked
        klausAudio.Stop();
        klausAudio.loop = false;
        sfxAudio.clip = klausHaked;
        sfxAudio.spatialBlend = 1;
        sfxAudio.volume = 0.6f;
        sfxAudio.Play();
        cpu.SetBool("HackingTrue", true);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(2f));
        ShowOutroDialogue(8);

        // Load level
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(2f));
        LoadLevelManager.Instance.ActivateLoadedLevel();

        while (true)
            yield return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "RunningTrigger")
        {
            ShowOutroDialogue(6);
            DynamicCameraManager.Instance.RemoveEspecialTargetForK1();
            DynamicCameraManager.Instance.ChangueEspecialTargetForK1(transform, 0.5f, 5f, 0.5f);
        }
    }

    public void ShowOutroDialogue(int index)
    {
        KlausDialogues[index].InitText();

    }

    public void RunToCPU()
    {
        inFrontOfCpu = true;
    }

    #endregion

    #region Utilities

    Collider2D[] aux = new Collider2D[5];
    Vector2 lastPosition;

    void Update()
    {
        Vector2 difference = cachedRigidbody.position - lastPosition;
        animator.SetFloat("VelocityX", difference.x);
        animator.SetFloat("VelocityY", difference.y);
        lastPosition = cachedRigidbody.position;
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircleNonAlloc(groundTransform.position, groundRadius, aux, groundLayer) > 0;
        animator.SetBool("InGround", grounded);
    }

    void CheckForKill(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<DeadState>().typeOfDead = DeadType.Ray;
            other.GetComponent<MoveState>().Kill();
            if (sfxAudio.clip != shieldDeathSFX)
            {
                sfxAudio.clip = shieldDeathSFX;
                sfxAudio.volume = 1;
                sfxAudio.Play();
            }
        }
        else
        {
            KillObject obj = other.GetComponent<KillObject>();
            if (obj != null)
                obj.Kill();
        }
    }

    void ToNextAttack()
    {
        if (++difficulties[currentLevel].currentAttack == difficulties[currentLevel].attacksSequence.Length)
            difficulties[currentLevel].currentAttack = 0;

        PlayCurrentAttack();
    }

    void PlayCurrentAttack()
    {
        ChangeState(difficulties[currentLevel].attacksSequence[difficulties[currentLevel].currentAttack]);
    }

    IEnumerator MoveTowards(Vector2 position, float speed, bool restrictX = false, bool restrictY = false)
    {
        while ((!restrictX && cachedRigidbody.position.x != position.x) || (!restrictY && cachedRigidbody.position.y != position.y))
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                Vector2 newPosition = cachedRigidbody.position;
                float delta = speed * Time.deltaTime;

                if (!restrictX)
                    newPosition.x = Mathf.MoveTowards(newPosition.x, position.x, delta);
                if (!restrictY)
                    newPosition.y = Mathf.MoveTowards(newPosition.y, position.y, delta);

                cachedRigidbody.position = newPosition;
            }

            yield return null;
        }
    }

    IEnumerator Impulse(Vector3 direction, float force, float decayRate, bool restrictX = false, bool restrictY = false)
    {
        while (force > 0)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                Vector2 newPosition = cachedRigidbody.position;
                force -= decayRate * Time.deltaTime;

                if (!restrictX)
                    newPosition.x += direction.x * force * Time.deltaTime;
                if (!restrictY)
                    newPosition.y += direction.y * force * Time.deltaTime;

                cachedRigidbody.position = newPosition;
            }

            yield return null;
        }
    }

    public void EnableBlur()
    {
        blurEnabled = true;
        StopCoroutine("UpdateBlur");
        StartCoroutine("UpdateBlur");
    }

    public void DisableBlur()
    {
        blurEnabled = false;
    }

    IEnumerator BlurForTime(float time)
    {
        EnableBlur();
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        DisableBlur();
    }

    IEnumerator UpdateBlur()
    {
        while (true)
        {
            if (!ManagerPause.Pause)
            {
                // Update blur center according to Klaus position
                Vector2 center = blur.Center;
                center.x = Mathf.InverseLerp(minPoint.x, maxPoint.x, cachedRigidbody.position.x);
                center.y = Mathf.InverseLerp(minPoint.y, maxPoint.y, cachedRigidbody.position.y);
                blur.Center = center;

                // Update blur intensity
                if (blurEnabled)
                {
                    if (blur.Strength < blurLimits.x)
                        blur.Strength = Mathf.MoveTowards(blur.Strength, blurLimits.x, Time.deltaTime * blurSpeed);
                    else
                        blur.Strength = Mathf.PingPong(blur.Strength + Time.deltaTime * blurSpeed - blurLimits.x, blurLimits.y - blurLimits.x) + blurLimits.x;
                }
                else
                {
                    if (!Mathf.Approximately(blur.Strength, 0f))
                    {
                        blur.Strength = Mathf.MoveTowards(blur.Strength, 0f, Time.deltaTime * blurSpeed);
                    }
                    else
                    {
                        blur.Strength = 0f;
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    #endregion

    #region Audio Dinamics

    IEnumerator VolUp(AudioSource audio)
    {
        for (float i = 0; i <= 1f; i += 0.7f * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;

        }
        StartCoroutine("VolDown", sfxAudio2);
    }

    IEnumerator VolDown(AudioSource audio)
    {
        for (float i = 1; i >= 0f; i -= (0.3f) * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;

        }
    }

    Action completeSceneCallback;
    Action completeLevelCallback;
    public void CallTrophy()
    {
        CompleteScene_Trophy[] trophy = GameObject.FindObjectsOfType<CompleteScene_Trophy>();
        for (int i = 0; i < trophy.Length; ++i)
        {
            if (trophy[i] != null)
            {
                trophy[i].OnRegister(this);
            }
        }
    }

    public void CompleteScene()
    {
        if (completeSceneCallback != null)
            completeSceneCallback();
    }

    public void CompleteLevel()
    {
        if (completeLevelCallback != null)
            completeLevelCallback();
    }

    public void RegisterCompleteLevel(Action callback)
    {
        completeLevelCallback += callback;
    }

    public void UnRegisterCompleteLevel(Action callback)
    {
        completeLevelCallback -= callback;
    }

    public void RegisterCompleteScene(Action callback)
    {
        completeSceneCallback += callback;
    }

    public void UnRegisterCompleteScene(Action callback)
    {
        completeSceneCallback -= callback;
    }

    #endregion
}
