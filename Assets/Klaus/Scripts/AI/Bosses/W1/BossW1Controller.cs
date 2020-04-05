using System;
using System.Collections;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Audio;
using Colorful;

public class BossW1Controller : StateBehaviour
{
    #region Difficulties

    [Serializable]
    public class Difficulty
    {
        [Header("General setup")]
        public States[] attacksSequence;
        public Color hitColor = Color.white;
        public float hitSpeed;
        public float moveSpeed = 1f;
        public float timeToWakeUp = 3f;
        public float rocksFallingRate = 0f;
        [Space(1f)]

        [Header("Charge setup")]
        public int charges = 1;
        public float walkAcceleration = 1f;
        public float timeToTurn = 0.5f;
        public Vector2 walkDelay;
        [Space(1f)]

        [Header("Jump and Smash setup")]
        public int smashes = 1;
        public float timeBetweenJumps;
        public float upSpeed;
        public float downSpeed;
        public Vector2 jumpDelay;
        [Space(1f)]

        [Header("Laugh setup")]
        public float laughTime = 1f;
        public Vector2 laughDelay;
        public ObstaclesGroup[] obstaclesPrefabs;
        [Space(1f)]

        [Header("Platform shake")]
        public float shakeDuration = 2f;
        public float strength = 2f;
        public int vibrato = 10;
        public float delayBeforeShake = 0.25f;
        public float delayAfterShake = 0.25f;
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
        WalkPunch,
        JumpSmash,
        Laugh,
        FightStart,
        Victory,
        ResetPlatform,
        Cutscene
    }
	public GameObject messageMix;
    [Header("Game elements")]
    public Difficulty[] difficulties;
    public float obstaclesFallSpeed;

    [Header("Limits and positions")]
    public Vector2 leftLimitPosition;
    public Vector2 rightLimitPosition;
    public Vector2 obstaclesPosition;
    public float upLimitPosition;

    [Header("Scene elements")]
    public Animator gameText;
    public ManualPlatform platform;
    public Collider2D exitBox;
    public ParticleSystem rocksParticles;

    int currentLevel = 0;

    [Header("Blur")]
    public RadialBlur blur;
    public Vector2 blurLimits = new Vector2(0.01f, 0.1f);
    public float blurSpeed = 1f;
    bool blurEnabled = false;

    [Header("Camera")]
    public Vector2 fightOffset = new Vector2(0, 2f);
    public float timeToTargetK1 = 1.5f;
    public Vector2 inGameZoomSmoothness = new Vector2(1, 1);

    [Header("Other")]
    public LayerMask groundLayer;
    public Action onJump;

    void Start()
    {
        Initialize<States>();
        ChangeState(States.Intro);

        // Find other player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
            if (player.name == "Klaus")
            {
                target = player.transform;
                target.GetComponentInChildren<DeadState>().SuscribeOnStart(OnPlayerDead);
                break;
            }


        foreach (Difficulty difficulty in difficulties)
        {
            foreach (ObstaclesGroup group in difficulty.obstaclesPrefabs)
                group.CreatePool<ObstaclesGroup>(1);
        }

        // Set death time
        DeadState state = target.GetComponentInChildren<DeadState>();
        state.deathTime = timeToRevive;
    }

    public void ResetBoss()
    {
        StopAllCoroutines();
        animator.Rebind();
        ChangeState(States.FightStart, StateTransition.Overwrite);
		StartCoroutine("VolDown", aSource2);
		speedDown = 1f;
		AS_W1Boss.Instance.StopSFX();
    }

    #endregion

    #region Components

    public SpriteRenderer punchSpriteRenderer;
    public Collider2D punchCollider;

    public SpriteRenderer spriteRenderer;

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

    public ResetBossFight resetTrigger
    {
        get
        {
            if (_resetTrigger == null)
                _resetTrigger = GameObject.FindObjectOfType<ResetBossFight>();
            return _resetTrigger;
        }
    }

    public ShieldTrigger trigger
    {
        get
        {
            if (_trigger == null)
                _trigger = GetComponentInChildren<ShieldTrigger>();
            return _trigger;
        }
    }

    public Blink blink
    {
        get
        {
            if (_blink == null)
                _blink = GetComponentInChildren<Blink>();
            return _blink;
        }
    }

    public Collider2D[] colliders
    {
        get
        {
            if (_colliders == null)
                _colliders = GetComponents<Collider2D>();
            return _colliders;
        }
    }

    public ProCamera2DZoomToFitTargets zoomTargets
    {
        get
        {
            if (_zoomTargets == null)
                _zoomTargets = GameObject.FindObjectOfType<ProCamera2DZoomToFitTargets>();
            return _zoomTargets;
        }
    }

    ProCamera2DZoomToFitTargets _zoomTargets;
    Collider2D[] _colliders;
    Blink _blink;
    ShieldTrigger _trigger;
    Transform _groundTransform;
    Rigidbody2D _rigidbody;
    Animator _animator;
    Transform target;
    ResetBossFight _resetTrigger;
    FlipSprite _flipSprite;

    #endregion

    #region Intro

    [Header("Intro stuff")]
    public float timeToFall = 1.5f;
    public float madTime = 1f;
    public float burpTime = 2f;
    public float laughTime = 1.5f;

    StartFightTrigger fightTrigger;
    StartFightTrigger tubeTrigger;
    Vector2 cameraOffset;

	[Header("Audio")]
	public AudioSource aSource1;
	public AudioSource aSource2;
	public AudioSource aSource3;
	public AudioClip[] SFX;
 	public AudioMixerGroup mixerGroup1;
	public AudioMixerGroup mixerGroup2;
	private float speedUp;
	private float speedDown;
	public float vol;
    void Intro_Enter()
    {
        exitBox.enabled = false;
        blackScreen.CrossFadeAlpha(0f, 0, true);
        spriteRenderer.color = Color.clear;
        cachedRigidbody.gravityScale = 0;

        StartFightTrigger[] triggers = GameObject.FindObjectsOfType<StartFightTrigger>();
        if (rocksParticles) rocksParticles.emissionRate = difficulties[0].rocksFallingRate;

        for (int i = 0; i != triggers.Length; ++i)
        {
            if (triggers[i].name == "FloorTrigger")
            {
                fightTrigger = triggers[i];
                fightTrigger.onEnterTrigger += PlayerEnteredRoom;
            }
            else if (triggers[i].name == "FallTrigger")
            {
                tubeTrigger = triggers[i];
                tubeTrigger.onEnterTrigger += PlayerFalling;
            }
        }
    }

    void PlayerFalling()
    {
        tubeTrigger.onEnterTrigger -= PlayerFalling;
        CharacterManager.Instance.FreezeAll();
        target.GetComponentInChildren<MoveState>().ResetForceX();
    }

    void PlayerEnteredRoom()
    {
        fightTrigger.onEnterTrigger -= PlayerEnteredRoom;
        StartCoroutine("IntroSequence");
		messageMix.SendMessage ("FightMix");
        if (AS_W1BossMusik.Instance != null)
            AS_W1BossMusik.Instance.MusikFight();
        CameraSetup();
    }

    IEnumerator IntroSequence()
    {
        // Basic setup
        CharacterManager.Instance.FreezeAll();
        target.GetComponentInChildren<FlipSprite>().FlipIfCanFlip(Vector3.right);
        resetTrigger.AddFightCheckpoint(target.GetComponent<Collider2D>());

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToFall));

        // Make K1 fall
        spriteRenderer.color = Color.black;
        animator.enabled = true;
        animator.SetFloat("CrushDir", -1f);
        animator.SetBool("isCrushing", true);
        cachedRigidbody.gravityScale = difficulties[0].downSpeed;

        while (!grounded) yield return null;
        
		aSource1.Play ();
		vol = aSource2.volume;
		StartCoroutine("VolUp", aSource2);
		speedUp = 1f;
		speedDown = 0.5f;
        CameraShake.Instance.StartShakeBy(0.5f);
        cachedRigidbody.gravityScale = 1;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.25f));
        animator.SetBool("isCrushing", false);
        animator.SetFloat("CrushDir", 0f);

        // Make K1 appear - mad
        animator.SetBool("Mad", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.5f));
        yield return StartCoroutine(TweenColor(Color.white, 0.75f));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(madTime));
        animator.SetBool("Mad", false);

        // Make him burp
        animator.SetBool("Burp", true);
		vol = aSource2.volume;
		StartCoroutine("VolUp", aSource2);
		speedUp = 0.5f;
		speedDown = 0.4f;
        CameraShake.Instance.StartShakeWithDelay(new Vector2(1.5f, 0.5f));
        StartCoroutine(BlurWithDelay(0.5f));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(burpTime));
        CameraShake.Instance.StopShake();
        DisableBlur();
        animator.SetBool("Burp", false);

        // Show text
        gameText.SetTrigger("Show");
        TitleLevelHUD.Instance.SetCompleteName("W1Boss.03", true);

        // Make him laugh
        animator.SetBool("Laugh", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(laughTime));
        animator.SetBool("Laugh", false);

        // Start fight
        yield return new WaitForSeconds(3f);
        ChangeState(States.FightStart);
    }

    IEnumerator TweenColor(Color color, float time)
    {
        float timer = time;
        Color initialColor = spriteRenderer.color;

        while (timer >= 0)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                timer -= Time.deltaTime;
                spriteRenderer.color = Color.Lerp(color, initialColor, timer / time);
            }

            yield return null;
        }
    }

    #endregion

    #region Fight Start

    bool cameraSettedUp;

    void CameraSetup()
    {
        cameraSettedUp = true;

        CameraZoom.Instance.StopZoom();
        zoomTargets.enabled = true;

        // Modify a little bit the offset
        cameraOffset = zoomTargets.ProCamera2D.OverallOffset;
        zoomTargets.ProCamera2D.OverallOffset = fightOffset;

        // Add K1 as a new target
        zoomTargets.ProCamera2D.AddCameraTarget(transform, 1, 0, timeToTargetK1);

        // Modify Klaus's Y influence
        zoomTargets.ProCamera2D.AdjustCameraTargetInfluence(target, 1, 0);
    }

    IEnumerator FightStart_Enter()
    {
        if (!cameraSettedUp)
            CameraSetup();

        zoomTargets.ZoomInSmoothness = inGameZoomSmoothness.x;
        zoomTargets.ZoomOutSmoothness = inGameZoomSmoothness.y;
        
        punchCollider.enabled = punchSpriteRenderer.enabled = false;

        // Locate the player on an actual site
        cachedRigidbody.position = rightLimitPosition;
        cachedRigidbody.isKinematic = false;
        cachedRigidbody.velocity = Vector2.zero;
        cachedRigidbody.gravityScale = 1;

        exitBox.enabled = false;

        // Reset other stuff
        if (hitParticle) hitParticle.SetActive(false);
        platform.ResetPlatform();
        CameraShake.Instance.StopShake();
        ResetBlur();
        flipSprite.FlipIfCanFlip(-Vector2.right);
        trigger.onTriggerStay -= OnSomethingFound;
        executingLogic = false;
        vulnerable = false;

        for (int i = 0; i != colliders.Length; ++i)
            colliders[i].enabled = true;

        // Reset difficulties
        currentLevel = 0;
        platformLevel = 0;
        foreach (Difficulty difficulty in difficulties)
        {
            difficulty.currentAttack = 0;
            foreach (ObstaclesGroup group in difficulty.obstaclesPrefabs)
                group.RecycleAll<ObstaclesGroup>();
        }

        blink.Stop();
        spriteRenderer.color = Color.white;

        if (rocksParticles) rocksParticles.emissionRate = difficulties[currentLevel].rocksFallingRate;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.1f));
        blackScreen.CrossFadeAlpha(0f, timeToRevive / controlOverBlack, true);

        StartCoroutine("StartSequence");
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

    #region WalkPunch

    bool executingLogic;

    IEnumerator WalkPunch_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].walkDelay.x));
        StartCoroutine("WalkPunchLogic");
        executingLogic = true;
        trigger.onTriggerStay += OnSomethingFound;
    }

    IEnumerator WalkPunchLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];

        for (int i = 0; i != difficulty.charges; ++i)
        {
            // Charge
            yield return StartCoroutine("Charge");
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].timeToTurn));
        }

        ToNextAttack();
    }

    IEnumerator Charge()
    {
        bool chargingRight = cachedRigidbody.position.x < target.position.x;
        float speed = difficulties[currentLevel].moveSpeed;
        Vector2 direction = chargingRight ? Vector2.right : -Vector2.right;

        flipSprite.FlipIfCanFlip(chargingRight ? Vector2.right : -Vector2.right);

        while (speed > 0f)
        {
            if (!ManagerPause.Pause)
            {
                animator.SetFloat("SpeedX", speed);
                cachedRigidbody.position += direction * speed * Time.deltaTime;

                if (animator.GetBool("isCrushing"))
                    speed = 0f;
                else if (chargingRight != cachedRigidbody.position.x < target.position.x)
                    speed = Mathf.Max(0f, speed - Time.deltaTime * difficulties[currentLevel].walkAcceleration);
            }

            yield return null;
        }

        animator.SetFloat("SpeedX", 0);
        cachedRigidbody.velocity = Vector2.zero;
    }

    void OnSomethingFound(Collider2D other)
    {
        if (!animator.GetBool("isCrushing") && other.tag.Contains("Player"))
            StartCoroutine(ThrowPunch(Vector2.zero, false));
    }

    IEnumerator WalkPunch_Exit()
    {
        trigger.onTriggerStay -= OnSomethingFound;
        executingLogic = false;

        while (animator.GetBool("isCrushing"))
            yield return null;
        
        animator.SetBool("isCrushing", false);
        animator.SetFloat("CrushDir", 0f);
        punchCollider.enabled = punchSpriteRenderer.enabled = false;
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].walkDelay.y));
    }

    #endregion

    #region JumpSmash

    IEnumerator JumpSmash_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].jumpDelay.x));
        StartCoroutine("JumpSmashLogic");
    }

    IEnumerator JumpSmashLogic()
    {
        Difficulty difficulty = difficulties[currentLevel];
        Vector2 targetPosition = Vector2.zero;

        for (int i = 0; i != difficulty.smashes + 1; ++i)
        {
            // Move K1 up
            animator.SetTrigger("Jump");
            if (onJump != null)
                onJump();
            targetPosition = i == difficulty.smashes ? new Vector3(rightLimitPosition.x, rightLimitPosition.y) : target.position;
            targetPosition.y = upLimitPosition;
            for (int j = 0; j != colliders.Length; ++j)
                colliders[j].enabled = false;

            flipSprite.FlipIfCanFlip(transform.position.x < targetPosition.x ? Vector2.right : -Vector2.right);
            cachedRigidbody.gravityScale = 0;
            yield return StartCoroutine(SlerpTowards(targetPosition, difficulty.upSpeed));

            // Move K1 down
            punchCollider.enabled = punchSpriteRenderer.enabled = true;
            animator.SetFloat("CrushDir", -1f);
            animator.SetBool("isCrushing", true);
            cachedRigidbody.gravityScale = difficulty.downSpeed;
            for (int j = 0; j != colliders.Length; ++j)
                colliders[j].enabled = true;

            while (!grounded)
                yield return null;
			aSource1.Play ();
			vol = aSource2.volume;
			StartCoroutine("VolUp", aSource2);
			speedUp = 0.5f;
			speedDown = 0.5f;
            CameraShake.Instance.StartShakeBy(0.5f);

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.15f));
            animator.SetBool("isCrushing", false);
            animator.SetFloat("CrushDir", 0f);
            punchCollider.enabled = punchSpriteRenderer.enabled = false;

            // Wait after punch
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].timeBetweenJumps));
        }

        ToNextAttack();
    }

    IEnumerator JumpSmash_Exit()
    {
        for (int i = 0; i != colliders.Length; ++i)
            colliders[i].enabled = true;
        cachedRigidbody.gravityScale = 1;
        animator.SetBool("isCrushing", false);
        animator.SetFloat("CrushDir", 0f);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].jumpDelay.y));
    }

    #endregion

    #region Laugh

    [Header("Laugh settings")]
    public GameObject hitParticle;
    public float shakeTimeWhenHitted = 0.2f;
    public float shakeTimeWhenLaughing = 5f;
    public float blurTimeWhenLaughing = 1f;

    ObstaclesGroup currentObstacles;
    bool vulnerable;

    IEnumerator Laugh_Enter()
    {
        if (hitParticle) hitParticle.SetActive(false);
        platformLevel = currentLevel;
        vulnerable = false;

        // If not in the correct position, run to it
        if (!Mathf.Approximately(cachedRigidbody.position.x, rightLimitPosition.x))
            yield return StartCoroutine(MoveTowards(rightLimitPosition, difficulties[currentLevel].moveSpeed));

        flipSprite.FlipIfCanFlip(-Vector2.right);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].laughDelay.x));
        vulnerable = true;
        StartCoroutine("LaughLogic");
    }

    IEnumerator LaughLogic()
    {
        // Open arms
        animator.SetBool("OpenArms", true);

        // Throw objects to screen
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1f));

        Difficulty difficulty = difficulties[currentLevel];
        int index = UnityEngine.Random.Range(0, difficulty.obstaclesPrefabs.Length);

        currentObstacles = difficulty.obstaclesPrefabs[index].Spawn<ObstaclesGroup>(obstaclesPosition);
        currentObstacles.TurnOnObstacles();

        // Laugh
        cachedRigidbody.isKinematic = true;
        animator.SetBool("OpenArms", false);
        animator.SetBool("Laugh", true);
		vol = aSource2.volume;
		StartCoroutine("VolUp", aSource2);
		speedUp = 0.5f;
		speedDown = 0.2f;

        // Shake and blur a little bit
        CameraShake.Instance.StartShakeBy(shakeTimeWhenLaughing);
        StartCoroutine("BlurForTime", blurTimeWhenLaughing);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].laughTime));

        // Exit
        ChangeState(States.ResetPlatform);
    }

    public void Kill()
    {
        if (!vulnerable)
            return;

        vulnerable = false;
        StopCoroutine("LaughLogic");
        StartCoroutine("TakeHit");
		aSource3.clip = SFX[0];
		aSource3.bypassReverbZones = false;
		aSource3.spatialBlend = 1;
		aSource3.outputAudioMixerGroup = mixerGroup1;
		aSource3.Play ();
    }

    IEnumerator TakeHit()
    {
        // Stop animations
        animator.SetBool("OpenArms", false);
        animator.SetBool("Laugh", false);
        CameraShake.Instance.StopShake();
        if (currentObstacles != null)
            currentObstacles.ToggleObjects(false);

        // Shake and blur a little bit
        CameraShake.Instance.StartShakeBy(shakeTimeWhenHitted);
        if (hitParticle) hitParticle.SetActive(true);

        animator.SetBool("isDead", true);
        animator.SetBool("firstIsDead", true);
        yield return null;
        animator.SetBool("firstIsDead", false);

        // Go to next difficulty level
        float timeToWakeUp = difficulties[currentLevel].timeToWakeUp;
        ++currentLevel;

        if (currentLevel >= difficulties.Length)
        {
            Debug.Log("BOSS DEAD");
            blink.Stop();
            spriteRenderer.color = Color.white;
            currentLevel = difficulties.Length - 1;
            if (rocksParticles) rocksParticles.emissionRate = difficulties[0].rocksFallingRate;
            ChangeState(States.Cutscene, StateTransition.Overwrite);
        }
        else
        {
            blink.Play(difficulties[currentLevel].hitSpeed, Color.white, difficulties[currentLevel].hitColor);

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToWakeUp));
            animator.SetBool("isDead", false);

            if (rocksParticles) rocksParticles.emissionRate = difficulties[currentLevel].rocksFallingRate;
            difficulties[currentLevel].currentAttack = difficulties[currentLevel].attacksSequence.Length - 1;
            ChangeState(States.ResetPlatform);
        }
    }

    IEnumerator Laugh_Exit()
    {
        animator.SetBool("OpenArms", false);
        animator.SetBool("Laugh", false);
        CameraShake.Instance.StopShake();
        if (currentObstacles != null)
            currentObstacles.ToggleObjects(false);

        vulnerable = false;
        cachedRigidbody.isKinematic = false;
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[currentLevel].laughDelay.y));
    }

    #endregion

    #region ResetPlatform

    int platformLevel;

    void ResetPlatform_Enter()
    {
        StartCoroutine("ResetPlatformLogic");
    }

    IEnumerator ResetPlatformLogic()
    {
        // Start moving platform
        StartCoroutine("PlatformMovement");
		aSource3.clip = SFX[1];
		aSource3.spatialBlend = 0;
		//aSource3.bypassReverbZones = true;
		aSource3.outputAudioMixerGroup = mixerGroup2;
		aSource3.PlayDelayed (4.5f);

        // Total time before platform moves
        float timeToJump = difficulties[platformLevel].delayBeforeShake + difficulties[platformLevel].delayBeforeShake + difficulties[platformLevel].delayAfterShake;
        float offset = Mathf.Min(difficulties[platformLevel].delayAfterShake, 0.15f);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToJump - offset));

        // Move K1 up
        for (int i = 0; i != colliders.Length; ++i)
            colliders[i].enabled = false;
        animator.SetTrigger("Jump");
        if (onJump != null) onJump();
        cachedRigidbody.gravityScale = 0;

        Vector2 targetPosition = cachedRigidbody.position;
        targetPosition.y = upLimitPosition;
        yield return StartCoroutine(SlerpTowards(targetPosition, 2.5f));

        // Make K1 fall
        for (int i = 0; i != colliders.Length; ++i)
            colliders[i].enabled = true;
        animator.SetBool("isPlanning", true);
        cachedRigidbody.gravityScale = 0.2f;

        while (!grounded)
            yield return null;
        animator.SetBool("isPlanning", false);
		AS_W1Boss.Instance.StopSFX();
        cachedRigidbody.gravityScale = 1;

        ToNextAttack();
    }

    IEnumerator PlatformMovement()
    {
        platform.ToggleAnimations(true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[platformLevel].delayBeforeShake));
        yield return StartCoroutine(platform.Shake(difficulties[platformLevel].shakeDuration, difficulties[platformLevel].strength, difficulties[platformLevel].vibrato));
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(difficulties[platformLevel].delayAfterShake));

        yield return StartCoroutine(platform.MoveDown());
        yield return StartCoroutine(platform.MoveUp());
    }

    void ResetPlatform_Exit()
    {
        for (int i = 0; i != colliders.Length; ++i)
            colliders[i].enabled = true;
        cachedRigidbody.gravityScale = 1;
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
        blackScreen.CrossFadeAlpha(1, timeToRevive / controlOverBlack, true);
    }

    void OnPlayerDead()
    {
        StopAllCoroutines();
        ChangeState(States.Victory, StateTransition.Overwrite);
    }

    #endregion

    #region Cutscene

    [Header("Cutscene")]
    public float delayBeforeZoom;
    public float timeOnGround;
    [Range(0, 1)]
    public float klausInfluence = 0.2f;
    public float zoomInSmoothness = 2f;
    public float zoomOutFactor = 0.25f;

    IEnumerator Cutscene_Enter()
    {
        // Paro a klaus, obstaculos, etc
        if (currentObstacles != null)
            currentObstacles.ToggleObjects(false);
        CharacterManager.Instance.FreezeAll();

        if (AS_W1BossMusik.Instance != null)
            AS_W1BossMusik.Instance.MusikMuere();
        
        // Stop for a bit with K1 on the ground
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(delayBeforeZoom));

        // Look/Zoom at K1
        float timer = timeOnGround;
        float lastZoom = zoomTargets.MaxZoomOutAmount;
        zoomTargets.ProCamera2D.AdjustCameraTargetInfluence(target, klausInfluence, 0f, timeOnGround * 0.95f);
        zoomTargets.MaxZoomOutAmount = zoomTargets.MaxZoomInAmount;
        zoomTargets.ZoomInSmoothness = zoomInSmoothness;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeOnGround));

        // CHAAA!!! K1 IS STILL ALIVE!!
        zoomTargets.ProCamera2D.AdjustCameraTargetInfluence(target, 1f, 0f, zoomOutFactor);
        zoomTargets.MaxZoomOutAmount = lastZoom;
        zoomTargets.ZoomInSmoothness = zoomTargets.ZoomOutSmoothness = zoomOutFactor;

        if (AS_W1Boss.Instance != null)
            AS_W1Boss.Instance.LastLaugh();
        animator.SetBool("isDead", false);
        animator.SetBool("OpenArms", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1f));

        // Laugh
        animator.SetBool("OpenArms", false);
        animator.SetBool("Laugh", true);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(2f));

        // Turn around
        animator.SetBool("Laugh", false);
        flipSprite.FlipIfCanFlip(Vector2.right);

        // Destroy box
        exitBox.enabled = true;
        animator.SetFloat("CrushDir", 0f);
        animator.SetBool("isCrushing", true);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.3f));

        if (exitBox.gameObject.activeSelf)
            exitBox.GetComponent<ICrushObject>().Crush(TypeCrush.Middle);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.7f));

        animator.SetBool("isCrushing", false);
        animator.SetFloat("CrushDir", 0f);
        punchCollider.enabled = punchSpriteRenderer.enabled = false;
		messageMix.SendMessage ("NormalMix");
        if (AS_W1BossMusik.Instance != null)
            AS_W1BossMusik.Instance.MusikRevive();
        	AS_W1BossMusik.Instance.EndFight();

        // Run
        zoomTargets.ProCamera2D.RemoveCameraTarget(transform, 1f);
        yield return StartCoroutine(MoveTowards(rightLimitPosition + Vector2.right * 8f, Mathf.Max(difficulties[currentLevel].moveSpeed, 5f)));

        CharacterManager.Instance.UnFreezeAll();
        target.GetComponentInChildren<DeadState>().UnSuscribeOnStart(OnPlayerDead);
        gameObject.SetActive(false);
    }

    #endregion

    #region Punch

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!animator.GetBool("isCrushing") && other.transform.CompareTag("Player"))
            StartCoroutine(ThrowPunch(other.transform.position - transform.position, true));
    }

    IEnumerator ThrowPunch(Vector2 direction, bool canFlip = false)
    {
        // Flip sprite?
        bool lastLookDirection = flipSprite.facingRight;
        if (canFlip) flipSprite.FlipIfCanFlip(direction);

        // Setup
        animator.SetFloat("CrushDir", 0f);
        animator.SetBool("isCrushing", true);
        punchSpriteRenderer.enabled = true;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.2f));

        punchCollider.enabled = true;
        CameraShake.Instance.StartShakeBy(0.5f);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.8f));

        punchCollider.enabled = punchSpriteRenderer.enabled = false;
        animator.SetBool("isCrushing", false);
        animator.SetFloat("CrushDir", 0f);
        if (canFlip)
            flipSprite.FlipIfCanFlip(lastLookDirection ? Vector2.right : -Vector2.right);
    }

    #endregion

    #region Utilities

    Collider2D[] aux = new Collider2D[5];
    bool grounded;

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircleNonAlloc(groundTransform.position, 0.09f, aux, groundLayer) > 0;
        animator.SetBool("isGround", grounded);
    }

    void CheckForKill(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MoveState>().Kill();
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
        Vector2 targetPosition = cachedRigidbody.position;
        if (!restrictX) targetPosition.x = position.x;
        if (!restrictY) targetPosition.y = position.y;
        
        flipSprite.FlipIfCanFlip(cachedRigidbody.position.x <= targetPosition.x ? Vector2.right : -Vector2.right);
        animator.SetFloat("SpeedX", speed);

        while (Vector2.SqrMagnitude(cachedRigidbody.position - targetPosition) > 0.005f)
        {
            if (!ManagerPause.Pause && !animator.GetBool("isCrushing"))
                cachedRigidbody.position = Vector2.MoveTowards(cachedRigidbody.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }

        animator.SetFloat("SpeedX", 0);
        cachedRigidbody.velocity = Vector2.zero;
    }

    IEnumerator SlerpTowards(Vector2 position, float maxSpeed, bool restrictX = false, bool restrictY = false)
    {
        Vector2 origin = cachedRigidbody.position;

        Vector2 target = position;
        if (restrictX)
            target.x = origin.x;
        if (restrictY)
            target.y = origin.y;

        float delta = 0;
        float speed = maxSpeed;
        float minSpeed = 0.1f;

        while (delta < 1)
        {
            if (!ManagerPause.Pause && !animator.GetBool("isCrushing"))
            {
                delta += speed * Time.deltaTime;
                speed = Mathf.Lerp(maxSpeed, minSpeed, delta);

                cachedRigidbody.position = Vector3.Slerp(origin, target, delta);
            }

            yield return null;
        }

        cachedRigidbody.velocity = Vector2.zero;
    }

    #endregion

    #region Blur

    IEnumerator BlurWithDelay(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        EnableBlur();
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

    public void ResetBlur()
    {
        StopCoroutine("UpdateBlur");
        blurEnabled = false;
        blur.Strength = 0;
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
                center.x = Mathf.InverseLerp(rightLimitPosition.y, leftLimitPosition.x, cachedRigidbody.position.x);
                center.y = Mathf.InverseLerp(upLimitPosition, rightLimitPosition.x, cachedRigidbody.position.y);
                blur.Center = center;

                // Update blur intensity
                if (blurEnabled)
                {
                    if (blur.Strength < blurLimits.x)
                        blur.Strength = Mathf.MoveTowards(blur.Strength, blurLimits.x, Time.deltaTime * blurSpeed);
                    else
                        blur.Strength = Mathf.PingPong(blur.Strength + Time.deltaTime * blurSpeed - blurLimits.x, blurLimits.y - blurLimits.x) + blurLimits.x;
                } else
                {
                    if (!Mathf.Approximately(blur.Strength, 0f))
                    {
                        blur.Strength = Mathf.MoveTowards(blur.Strength, 0f, Time.deltaTime * blurSpeed);
                    } else
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
		for (float i = vol; i <= 1f; i += speedUp * Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
			
		}
		StartCoroutine("VolDown", aSource2);
	}
	
	IEnumerator VolDown(AudioSource audio)
	{
		for (float i = 1; i >= 0f; i -= speedDown * Time.deltaTime)
		{
			audio.volume = i;
			yield return null;
			
		}
	}
	
	#endregion
}
