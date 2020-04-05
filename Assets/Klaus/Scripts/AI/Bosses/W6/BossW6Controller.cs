using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Colorful;

public class BossW6Controller : StateBehaviour, ICompleteLevel
{
    #region Difficulties

    public enum Direction
    {
        Left,
        Right
    }

    [Serializable]
    public class Difficulty
    {
        [Header("General setup")]
        public States[] attacksSequence;
        public GameObject effectToEnable;
        public Color blinkColor = Color.white;
        public float blinkSpeed;
        public float timeToWakeUp = 3f;
        [Space(1f)]

        [Header("First Attack")]
        public int punches = 1;
        public float timeToLockPunch = 3f;

        [Range(0f, 1f)]
        public float pauseForLock = 0.95f;
        public float minErrorDistance = 0.2f;
        public float timeToRiseHand = 1f;
        public float punchDownMultiplier = 1f;
        public Vector2 firstAttackDelay;
        [Space(1f)]

        [Header("Fist Sweep")]
        public Direction[] sweeps;
        public float timeToLockSweep = 3f;
        public float sweepSpeed = 5f;
        public float timeAfterSweep = 1.5f;
        public Vector2 sweepDelay;
        [Space(1f)]

        [Header("Eyes Laser")]
        public float timeToLockEyesLaser = 3f;
        public float timeToThrowEyesLaser = 3f;
        public float timeAfterEyesLaser = 3f;
        public float eyesMoveSpeed = 6f;
        public Vector2 eyesAttackDelay;
        public EyesPatterns[] attackPatterns;
        [Space(1f)]

        [Header("Mouth Laser")]
        public float timeToLockMouthLaser = 3f;
        public float mouthLockSpeed = 3f;
        public float timeToThrowMouseLaser = 3f;
        public Vector2 mouthAttackDelay;
        [Space(1f)]

        [Header("Electric Balls")]
        public Vector2 electricBallsDelay;
        public int waves = 1;
        public float timeBetweenWaves = 1f;
        public Vector2 electricBallsSpeed = new Vector2(88f, 92f);
        public Vector2 electricBallsMass = new Vector2(3.1f, 3.9f);
        [Space(1f)]

        [Header("Free Fists Attack")]
        public int attacks = 1;
        public float timeToLockAttack = 3f;
        public float timeAfterPunch = 0.25f;
        public float timeBetweenAttacks = 1f;
        public float fistsVerticalSpeed = 1f;
        public float fistsPunchSpeed = 3f;
        public float fistsReturnSpeed = 1f;
        public Vector2 freeFistsDelay;
        [Space(1f)]

        [HideInInspector]
        public int currentAttack;
    }

    #endregion

    #region Setup

    public enum States
    {
        Intro,
        Vulnerable,
        FightStart,
        FirstAttack,
        EyesLaser,
        MouthLaser,
        FreeFists,
        ElectricBalls,
        Victory,
        Cutscene,
        FistsSweep,
        Taunt
    }

    [Header("Game elements")]
    public Difficulty[] difficulties;
    int currentLevel = 0;

    [Header("Other stuff")]
    public Animator gameText;
    public int shakePreset = 1;

    public void OnLevelWasLoaded(int level)
    {
        SaveManager.Instance.SaveCurrentLevel(Application.loadedLevelName);
    }

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

        // Set death time
        DeadState state = target.GetComponentInChildren<DeadState>();
        state.deathTime = timeToRevive;

        electricBall.CreatePool<BulletHandler>(20);

        ManagerAnalytics.MissionStarted(Application.loadedLevelName, false);

        CallTrophy();
        CounterTimerPlay.Instance.StartTime();

    }

    public void ResetBoss()
    {
        animator.Rebind();
        AS_W6BossAnimation.Instance.ResetSFX();
        ChangeState(States.FightStart, StateTransition.Overwrite);
    }

    #endregion

    #region Components

    public Difficulty currentDifficulty
    {
        get
        {
            return difficulties[currentLevel];
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

    public ResetBossFight resetTrigger
    {
        get
        {
            if (_resetTrigger == null)
                _resetTrigger = GameObject.FindObjectOfType<ResetBossFight>();
            return _resetTrigger;
        }
    }

    public PlatformAI platformAI
    {
        get
        {
            if (_platformAI == null)
                _platformAI = GameObject.FindObjectOfType<PlatformAISingleRute>();
            return _platformAI;
        }
    }

    public PlatformController platform
    {
        get
        {
            if (_platform == null)
                _platform = platformAI.GetComponentInChildren<PlatformController>();
            return _platform;
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

    Animator _animator;
    Blink _blink;
    Transform target;
    ResetBossFight _resetTrigger;
    PlatformController _platform;
    PlatformAI _platformAI;

    #endregion

    #region Intro

    [Header("Intro")]
    public float timeToStartIntro = 3f;

    void Intro_Enter()
    {
        ResetGame();

        animator.SetTrigger("ToIntro");
        blackScreen.CrossFadeAlpha(0f, 0, true);
        blink.SetColor(new Color(0, 0, 0, 0));
    }

    void PlayerOnPlatform()
    {
        platform.playerOnPlatform -= PlayerOnPlatform;
        platformAI.enabled = true;
        platformAI.GetComponentInChildren<Animator>().enabled = true;
        resetTrigger.AddFightCheckpoint(target.GetComponent<Collider2D>());
        StartCoroutine("IntroSequence");
    }

    public void StartIntro()
    {
        CharacterManager.Instance.UnFreezeAll();
        StartCoroutine("IntroSequence");
    }

    public void ShowBoss()
    {
        blink.SetColor(Color.white);
        //StartCoroutine(TweenColor(Color.white, 0.1f));
    }

    IEnumerator IntroSequence()
    {
        resetTrigger.AddFightCheckpoint(target.GetComponent<Collider2D>());

        CharacterManager.Instance.FreezeAll();
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToStartIntro));

        // START ANIMATION
        currentAnimationEnded = false;
        animator.SetTrigger("StartGame");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(2f));
        audioS1.clip = ActivateSFX;
        audioS1.PlayDelayed(2.8f);

        // Show text
        gameText.SetTrigger("Show");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(1.5f));

        while (!currentAnimationEnded)
            yield return null;

        // Start fight
        ChangeState(States.FightStart);
    }

    #endregion

    #region Fight Start

    void FightStart_Enter()
    {
        ResetGame();
        StartCoroutine("StartSequence");
    }

    void ResetGame()
    {
        CameraShake.Instance.StopShake();
        DisableBlur();
        blur.Strength = 0f;

        ResetCPUAccess();

        // Reset attack's variables
        FirstAttackReset();
        MouthLaserReset();
        FreeFistsReset();
        VulnerableReset();
        EyesLaserReset();
        ElectricBallsReset();
        CutsceneReset();
        FistsSweepReset();

        // Reset difficulties
        currentLevel = 0;
        foreach (Difficulty difficulty in difficulties)
        {
            difficulty.currentAttack = 0;
            if (difficulty.effectToEnable)
                difficulty.effectToEnable.SetActive(false);
        }

        // Remove black screen and blinking
        blink.Stop();
        blink.SetColor(Color.white);
        blackScreen.CrossFadeAlpha(0f, timeToRevive / controlOverBlack, true);

        // Stop all coroutines
        StopAllCoroutines();
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

    #region First Attack

    [Header("First Attack")]
    public Animator punchWarning;
    public Vector2 xLimits;
    public Transform leftLimit, rightLimit, leftDoorPosition, rightDoorPosition;
    public GameObject[] firstAttackTriggers;

    bool punchInGround;

    void FirstAttackReset()
    {
        foreach (GameObject collider in firstAttackTriggers)
            collider.SetActive(false);
        punchWarning.gameObject.SetActive(false);
    }

    IEnumerator FirstAttack_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.firstAttackDelay.x));
        StartCoroutine("FirstAttackLogic");
    }

    IEnumerator FirstAttackLogic()
    {
        OpenCPUAccess();

        for (int i = 0; i != currentDifficulty.punches; ++i)
        {
            // Wait to lock on Klaus
            animator.SetTrigger("PunchAttack");
            float timer = currentDifficulty.timeToLockPunch * currentDifficulty.pauseForLock;
            float waitTime = currentDifficulty.timeToLockPunch - timer;

            punchWarning.gameObject.SetActive(true);
            punchWarning.speed = 2.15f / currentDifficulty.timeToLockPunch;

            while (timer >= 0)
            {
                if (!ManagerPause.Pause)
                {
                    animator.SetFloat("KlausPosition", Mathf.InverseLerp(xLimits.x, xLimits.y, target.position.x) * 2f - 1f);

                    Vector3 position = leftLimit.position;
                    position.x = target.position.x;

                    if (position.x < leftLimit.position.x)
                    {
                        position = leftDoorPosition.position;
                    }
                    else if (position.x > rightLimit.position.x)
                    {
                        position = rightDoorPosition.position;
                    }

                    punchWarning.transform.position = position;

                    timer -= Time.deltaTime;
                }
                yield return null;
            }

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(waitTime));

            // Move hand down to the point
            foreach (GameObject collider in firstAttackTriggers)
                collider.SetActive(true);

            animator.SetFloat("PunchDownMultiplier", currentDifficulty.punchDownMultiplier);
            animator.SetTrigger("Attack");

            // Wait until hand has touched ground
            punchInGround = false;
            currentAnimationEnded = false;

            while (!punchInGround)
                yield return null;

            CameraShake.Instance.StartShakeBy(0.25f, shakePreset);
            audioRumble.PlayDelayed(0.25f);

            foreach (GameObject collider in firstAttackTriggers)
                collider.SetActive(false);
            animator.SetFloat("PunchDownMultiplier", 1f);
            punchWarning.gameObject.SetActive(false);

            while (!currentAnimationEnded)
                yield return null;

            // Make a pause before the next attack
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeToRiseHand));
        }

        ToNextAttack();
    }

    public void OnPunchTouchedFloor()
    {
        punchInGround = true;
    }

    IEnumerator FirstAttack_Exit()
    {
        punchWarning.gameObject.SetActive(false);
        CloseCPUAccess();
        StopCoroutine("FirstAttackLogic");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.firstAttackDelay.y));
    }

    #endregion

    #region Fists Sweep

    [Header("Fists sweep")]
    public float sweepBlinkMaxSpeed = 5f;
    public Transform leftSweepHand, rightSweepHand;
    public Transform leftInitialSweep, rightInitialSweep;
    public Transform leftEndSweep, rightEndSweep;
    public Animator leftSweepAnimator, rightSweepAnimator;
    public GameObject[] sweepTriggers;

    void FistsSweepReset()
    {
        leftSweepHand.gameObject.SetActive(false);
        rightSweepHand.gameObject.SetActive(false);
        leftSweepAnimator.gameObject.SetActive(false);
        rightSweepAnimator.gameObject.SetActive(false);

        foreach (GameObject collider in sweepTriggers)
            collider.SetActive(false);
    }

    IEnumerator FistsSweep_Enter()
    {
        FistsSweepReset();
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.sweepDelay.x));
        StartCoroutine("FistsSweepLogic");
    }

    IEnumerator FistsSweepLogic()
    {
        for (int i = 0; i != currentDifficulty.sweeps.Length; ++i)
        {
            // Throw the punch down
            currentAnimationEnded = false;
            float klausPosition = currentDifficulty.sweeps[i] == Direction.Left ? -1f : 1f;
            foreach (GameObject collider in sweepTriggers)
                collider.SetActive(true);
            animator.SetFloat("KlausPosition", klausPosition);
            animator.SetTrigger("FistSweep");

            // Wait until the animation signals the start of the attack
            while (!currentAnimationEnded)
                yield return null;

            CameraShake.Instance.StartShakeBy(0.25f, shakePreset);
            audioRumble.PlayDelayed(0.25f);
            foreach (GameObject collider in sweepTriggers)
                collider.SetActive(false);
            Transform activeHand = klausPosition < 0 ? rightSweepHand : leftSweepHand;
            Animator handAnimator = klausPosition < 0 ? rightSweepAnimator : leftSweepAnimator;
            Vector2 startPoint = klausPosition < 0 ? rightInitialSweep.position : leftInitialSweep.position;
            Vector2 endPoint = klausPosition < 0 ? rightEndSweep.position : leftEndSweep.position;

            activeHand.position = startPoint;
            activeHand.gameObject.SetActive(true);
            handAnimator.gameObject.SetActive(true);
            handAnimator.speed = 2.15f / currentDifficulty.timeToLockSweep;

            // Prepare to launch the attack
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeToLockSweep));
            handAnimator.gameObject.SetActive(false);

            // Throw the side punch
            //AS_Sweep1B6.Instance.PlaySweep();
            //AS_Sweep2B6.Instance.PlaySweep();
            foreach (GameObject collider in sweepTriggers)
                collider.SetActive(true);
            yield return StartCoroutine(MoveTowards(activeHand, endPoint, currentDifficulty.sweepSpeed));
            yield return StartCoroutine(MoveTowards(activeHand, startPoint, currentDifficulty.sweepSpeed));
            foreach (GameObject collider in sweepTriggers)
                collider.SetActive(false);

            // Wait until the animation signals the end of the animation
            activeHand.gameObject.SetActive(false);
            handAnimator.gameObject.SetActive(false);
            currentAnimationEnded = false;
            animator.SetTrigger("Attack");
            while (!currentAnimationEnded)
                yield return null;

            // Make a pause before the next attack
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeAfterSweep));
        }

        ToNextAttack();
    }

    IEnumerator FistsSweep_Exit()
    {
        FistsSweepReset();
        StopCoroutine("FistsSweepLogic");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.sweepDelay.y));
    }

    #endregion

    #region Eyes Laser

    public enum EyesPatterns
    {
        CornerToCorner,
        XShaped
    }

    [Header("Eyes Laser")]
    public float eyesBlinkMaxSpeed = 5f;
    public BossW6Ray[] eyesLasers;
    public Transform leftPoint, rightPoint;

    void EyesLaserReset()
    {
        foreach (BossW6Ray box in eyesLasers)
            box.Hide();
    }

    IEnumerator EyesLaser_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.eyesAttackDelay.x));

        currentAnimationEnded = false;
        animator.SetBool("EyesLaserAttack", true);
        AS_W6BossAnimation.Instance.EyeLaserSFX();

        while (!currentAnimationEnded)
            yield return null;

        StartCoroutine("EyesLaserLogic");
    }

    IEnumerator EyesLaserLogic()
    {
        float timer = 0;

        for (int i = 0; i != currentDifficulty.attackPatterns.Length; ++i)
        {
            // Setup the rays
            Func<Transform, Vector3> callback = null;
            switch (currentDifficulty.attackPatterns[i])
            {
                case EyesPatterns.CornerToCorner:

                    bool closeToLeft = Mathf.Abs(target.position.x - leftPoint.position.x) < Mathf.Abs(target.position.x - rightPoint.position.x);

                    if (closeToLeft)
                    {
                        eyesLasers[0].transform.right = leftPoint.position - eyesLasers[0].transform.position;
                        eyesLasers[1].transform.right = leftPoint.position - eyesLasers[1].transform.position;
                        callback = ToRight;
                    }
                    else
                    {
                        eyesLasers[0].transform.right = rightPoint.position - eyesLasers[0].transform.position;
                        eyesLasers[1].transform.right = rightPoint.position - eyesLasers[1].transform.position;
                        callback = ToLeft;
                    }
                    break;

                case EyesPatterns.XShaped:
                    eyesLasers[0].transform.right = leftPoint.position - eyesLasers[0].transform.position;
                    eyesLasers[1].transform.right = rightPoint.position - eyesLasers[1].transform.position;
                    callback = ToXPattern;
                    AS_W6BossAnimation.Instance.EyeLaser2SFX();
                    break;
            }

            // Charge the attack
            animator.SetTrigger("Charging");
            foreach (BossW6Ray ray in eyesLasers)
                ray.Show(true);

            yield return StartCoroutine(UpdateAnimatorSpeed(currentDifficulty.timeToLockEyesLaser, eyesBlinkMaxSpeed));

            // Throw ray
            animator.speed = 1f;
            animator.SetBool("Attacking", true);
            timer = currentDifficulty.timeToThrowEyesLaser;
            CameraShake.Instance.StartShake(shakePreset);
            audioRumble.PlayDelayed(0f);

            foreach (BossW6Ray ray in eyesLasers)
                ray.Show(false);

            while (timer >= 0)
            {
                if (!ManagerPause.Pause)
                {
                    foreach (BossW6Ray box in eyesLasers)
                    {
                        Vector3 targetRight = callback != null ? callback(box.transform) : Vector3.down;
                        box.transform.right = Vector3.MoveTowards(box.transform.right, targetRight, currentDifficulty.eyesMoveSpeed * Time.deltaTime);
                    }

                    timer -= Time.deltaTime;
                }
                yield return null;
            }

            // Stop attack
            CameraShake.Instance.StopShake();
            animator.SetBool("Attacking", false);
            foreach (BossW6Ray ray in eyesLasers)
                ray.Hide();

            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeAfterEyesLaser));
        }

        ToNextAttack();
    }

    Vector3 ToRight(Transform box)
    {
        return Vector3.Lerp(leftPoint.position, rightPoint.position, 0.9f) - box.position;
    }

    Vector3 ToLeft(Transform box)
    {
        return Vector3.Lerp(leftPoint.position, rightPoint.position, 0.1f) - box.position;
    }

    Vector3 ToXPattern(Transform box)
    {
        return Vector3.down;
    }

    IEnumerator EyesLaser_Exit()
    {
        StopCoroutine("EyesLaserLogic");
        EyesLaserReset();

        currentAnimationEnded = false;
        animator.SetBool("EyesLaserAttack", false);

        while (!currentAnimationEnded)
            yield return null;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.eyesAttackDelay.y));
    }

    #endregion

    #region Mouth Laser

    [Header("Mouth Laser")]
    public BossW6Ray mouthLaser;
    public float mouthBlinkMaxSpeed = 5f;

    void MouthLaserReset()
    {
        mouthLaser.Hide();
    }

    IEnumerator MouthLaser_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.mouthAttackDelay.x));

        currentAnimationEnded = false;
        animator.SetTrigger("MouthLaserAttack");

        while (!currentAnimationEnded)
            yield return null;

        StartCoroutine("MouthLaserLogic");
    }

    IEnumerator MouthLaserLogic()
    {
        mouthLaser.Show(true);

        // Lock on Klaus by rotating the ray
        float timer = currentDifficulty.timeToLockMouthLaser;

        while (timer >= 0)
        {
            if (!ManagerPause.Pause)
            {
                timer -= Time.deltaTime;

                Vector3 targetPosition = target.position;
                targetPosition.x = Mathf.Clamp(targetPosition.x, leftPoint.position.x, rightPoint.position.x);

                Vector3 targetRight = targetPosition - mouthLaser.transform.position;
                mouthLaser.transform.right = Vector3.MoveTowards(mouthLaser.transform.right, targetRight, currentDifficulty.mouthLockSpeed * Time.deltaTime);
                animator.speed = Mathf.Lerp(mouthBlinkMaxSpeed, 1f, Mathf.InverseLerp(0, currentDifficulty.timeToLockMouthLaser, timer));
            }
            yield return null;
        }

        // Throw ray
        animator.speed = 1f;
        animator.SetBool("Attacking", true);
        CameraShake.Instance.StartShake(shakePreset);
        mouthLaser.Show(false);

        
        audioRumble.PlayDelayed(0f);
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeToThrowMouseLaser));
        CameraShake.Instance.StopShake();
        mouthLaser.Hide();

        currentAnimationEnded = false;
        animator.SetBool("Attacking", false);
        while (!currentAnimationEnded)
            yield return null;

        MouthLaserReset();
        ToNextAttack();
    }

    IEnumerator MouthLaser_Exit()
    {
       
        StopCoroutine("MouthLaserLogic");
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.mouthAttackDelay.y));
    }

    #endregion

    #region Electric Balls Attack

    [Header("Electric Balls")]
    public BulletHandler electricBall;
    public Transform[] spawnPositions;

    void ElectricBallsReset()
    {
        electricBall.RecycleAll<BulletHandler>();
    }

    IEnumerator ElectricBalls_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.electricBallsDelay.x));

        currentAnimationEnded = false;
        animator.SetBool("BackCannonsAttack", true);
        AS_W6BossAnimation.Instance.CannonsSFX();
        while (!currentAnimationEnded)
            yield return null;

        StartCoroutine("ElectricBallsLogic");
    }

    IEnumerator ElectricBallsLogic()
    {
        for (int x = 0; x != currentDifficulty.waves; ++x)
        {
            // Set the attack order and wait until it is fullfilled
            currentAnimationEnded = false;
            animator.SetTrigger("Attack");

            while (!currentAnimationEnded)
                yield return null;

            // Wait to throw next wave
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeBetweenWaves));
        }

        ToNextAttack();
    }

    public void CannonsShot()
    {
        float mass = UnityEngine.Random.Range(currentDifficulty.electricBallsMass.x, currentDifficulty.electricBallsMass.y);
        float speed = UnityEngine.Random.Range(currentDifficulty.electricBallsSpeed.x, currentDifficulty.electricBallsSpeed.y);

        BulletHandler[] bullets = new BulletHandler[spawnPositions.Length];
        for (int i = 0; i != bullets.Length; ++i)
        {
            bullets[i] = electricBall.Spawn<BulletHandler>(spawnPositions[i].position, spawnPositions[i].rotation);
            bullets[i].maxSpeed = speed;
            (bullets[i] as CannonHandler).SetMass(mass);
            bullets[i].SetDirection(new BulletInfo(bullets[i].transform.right, 60f));
        }
    }

    IEnumerator ElectricBalls_Exit()
    {
        StopCoroutine("ElectricBallsLogic");

        currentAnimationEnded = false;
        animator.SetBool("BackCannonsAttack", false);
        while (!currentAnimationEnded)
            yield return null;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.electricBallsDelay.y));
    }

    #endregion

    #region Free Fists Attack

    [Header("Free Fists")]
    public Transform leftHand, rightHand;
    public Transform leftHandClone, rightHandClone;
    public float yOffset;
    public Transform leftInitialLocation, rightInitialLocation;
    public Vector2 punchPositions;
    public Animator leftHandAnimator, rightHandAnimator;

    Collider2D leftHandCollider, rightHandCollider;
    Vector3 leftInitPos, rightInitPos;
    float leftInitAngle, rightInitAngle;

    void FreeFistsReset()
    {
        ToggleHands(false);
        leftHandAnimator.gameObject.SetActive(false);
        rightHandAnimator.gameObject.SetActive(false);
    }

    IEnumerator FreeFists_Enter()
    {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.freeFistsDelay.x));

        // Wait for the intro animation
        animator.SetBool("FistsAttack", true);
        //AS_W6BossAnimation.Instance.LooseHandsSFX();
        while (!leftHand.gameObject.activeSelf && !rightHand.gameObject.activeSelf)
            yield return null;

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.5f));

        rightInitPos = rightHand.position;
        rightInitAngle = rightHand.eulerAngles.z;

        leftInitPos = leftHand.position;
        leftInitAngle = leftHand.eulerAngles.z;

        StartCoroutine("FreeFistsLogic");
    }

    IEnumerator FreeFistsLogic()
    {
        yield return StartCoroutine(MoveToOriginalPosition(currentDifficulty.fistsReturnSpeed));

        for (int i = 0; i != currentDifficulty.attacks; ++i)
        {
            leftHandAnimator.gameObject.SetActive(false);
            rightHandAnimator.gameObject.SetActive(false);

            // Wait to lock on Klaus
            float timer = currentDifficulty.timeToLockAttack;

            // Select the active hand
            Transform activeHand = transform.position.x < target.position.x ? leftHand : rightHand;
            Animator handAnimator = activeHand == leftHand ? leftHandAnimator : rightHandAnimator;

            handAnimator.gameObject.SetActive(true);
            leftHandAnimator.speed = rightHandAnimator.speed = 2.15f / currentDifficulty.timeToLockAttack;

            while (timer >= 0)
            {
                if (!ManagerPause.Pause)
                {
                    // Calculate the target in Y
                    Vector3 targetPosition = activeHand.position;
                    targetPosition.y = target.position.y + yOffset;

                    // Move the hand
                    activeHand.position = Vector3.MoveTowards(activeHand.position, targetPosition, currentDifficulty.fistsVerticalSpeed * Time.deltaTime);

                    // Update the timer
                    timer -= Time.deltaTime;
                }
                yield return null;
            }

            // Calculate the end point of the punch
            Vector3 startPosition = activeHand.position;
            Vector3 endPosition = startPosition;
            endPosition.x = activeHand == leftHand ? punchPositions.x : punchPositions.y;

            // Move the hand in X towards Klaus
            leftHandAnimator.gameObject.SetActive(false);
            rightHandAnimator.gameObject.SetActive(false);

            leftHandCollider.enabled = activeHand == leftHand;
            rightHandCollider.enabled = activeHand == rightHand;
            yield return StartCoroutine(MoveTowards(activeHand, endPosition, currentDifficulty.fistsPunchSpeed));

            // Pause after punch
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeAfterPunch));

            leftHandCollider.enabled = rightHandCollider.enabled = false;
            yield return StartCoroutine(MoveToOriginalPosition(currentDifficulty.fistsReturnSpeed));

            // Make a pause before the next attack
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.timeBetweenAttacks));
        }

        ToNextAttack();
    }

    IEnumerator MoveToOriginalPosition(float speed)
    {
        while (leftHand.position != leftInitialLocation.position || leftHand.eulerAngles != leftInitialLocation.eulerAngles || rightHand.position != rightInitialLocation.position || rightHand.eulerAngles != rightInitialLocation.eulerAngles)
        {
            if (!ManagerPause.Pause)
            {
                float delta = speed * Time.deltaTime;

                leftHand.position = Vector3.MoveTowards(leftHand.position, leftInitialLocation.position, delta);

                Vector3 angles = leftHand.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, leftInitialLocation.eulerAngles.z, delta);
                leftHand.eulerAngles = angles;

                rightHand.position = Vector3.MoveTowards(rightHand.position, rightInitialLocation.position, delta);

                angles = rightHand.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, rightInitialLocation.eulerAngles.z, delta);
                rightHand.eulerAngles = angles;
            }
            yield return null;
        }
    }

    IEnumerator FreeFists_Exit()
    {
        StopCoroutine("FreeFistsLogic");

        // Stop warning animations
        if (leftHandAnimator)
            leftHandAnimator.gameObject.SetActive(false);
        if (rightHandAnimator)
            rightHandAnimator.gameObject.SetActive(false);

        // Wait for the hands to be on the initial position
        float error = 0.1f * 0.1f;
        while (Vector3.SqrMagnitude(leftHand.position - leftInitPos) > error || !Mathf.Approximately(leftHand.eulerAngles.z, leftInitAngle) || Vector3.SqrMagnitude(rightHand.position - rightInitPos) > error || !Mathf.Approximately(rightHand.eulerAngles.z, rightInitAngle))
        {
            if (!ManagerPause.Pause)
            {
                float delta = currentDifficulty.fistsPunchSpeed * Time.deltaTime;

                leftHand.position = Vector3.MoveTowards(leftHand.position, leftInitPos, delta);

                Vector3 angles = leftHand.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, leftInitAngle, delta);
                leftHand.eulerAngles = angles;

                rightHand.position = Vector3.MoveTowards(rightHand.position, rightInitPos, delta);

                angles = rightHand.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, rightInitAngle, delta);
                rightHand.eulerAngles = angles;
            }

            yield return null;
        }

        // Wait for the exit animation
        animator.SetBool("FistsAttack", false);

        while (!leftHandClone.gameObject.activeSelf && !rightHandClone.gameObject.activeSelf)
            yield return null;

        ToggleHands(false);

        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(currentDifficulty.freeFistsDelay.y));
    }

    public void ToggleHands()
    {
        ToggleHands(!leftHand.gameObject.activeSelf);
    }

    public void ToggleHands(bool value)
    {
        leftHand.gameObject.SetActive(value);
        rightHand.gameObject.SetActive(value);

        if (value)
        {
            if (leftHandCollider == null)
                leftHandCollider = leftHand.GetComponentInChildren<PickTrigger>().GetComponent<Collider2D>();
            if (rightHandCollider == null)
                rightHandCollider = rightHand.GetComponentInChildren<PickTrigger>().GetComponent<Collider2D>();

            leftHandCollider.enabled = false;
            rightHandCollider.enabled = false;
        }
    }

    #endregion

    #region Vulnerable

    [Header("Vulnerable")]
    public float vulnerableTime;
    public Transform leftHandPlatform, rightHandPlatform;
    public Transform leftHandPlatformClone, rightHandPlatformClone;

    PlatformInputController leftInputController, rightInputController;
    bool vulnerable;
    BossPlatform currentHackedPlatform;

    public void SystemHacked(BossPlatform platform)
    {
        currentHackedPlatform = platform;
        StopAllCoroutines();

        animator.SetBool("Attacking", false);
        animator.SetBool("BackCannonsAttack", false);
        animator.SetBool("FistsAttack", false);
        animator.SetBool("Vulnerable", false);
        animator.SetBool("EyesLaserAttack", false);
        ChangeState(States.Vulnerable);
    }

    void VulnerableReset()
    {
        TogglePlatformHands(false);
        vulnerable = false;
    }

    IEnumerator Vulnerable_Enter()
    {
        animator.SetBool("Vulnerable", true);
        AS_W6BossAnimation.Instance.LooseHandsSFX();
        while (!leftHandPlatform.gameObject.activeSelf && !rightHandPlatform.gameObject.activeSelf)
            yield return null;

        vulnerable = true;
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(0.5f));

        rightInitPos = rightHandPlatform.position;
        rightInitAngle = rightHandPlatform.eulerAngles.z;

        leftInitPos = leftHandPlatform.position;
        leftInitAngle = leftHandPlatform.eulerAngles.z;

        StartCoroutine("VulnerableTimer");
    }

    public void Crush()
    {
        if (!vulnerable)
            return;

        StopCoroutine("VulnerableTimer");
        StartCoroutine("TakeHit");
    }

    IEnumerator TakeHit()
    {
        currentHackedPlatform.hacked = true;
        vulnerable = false;
        TogglePlatformsControl(false);

        // Go to next difficulty level
        if (currentDifficulty.effectToEnable)
            currentDifficulty.effectToEnable.SetActive(true);

        float timeToWakeUp = currentDifficulty.timeToWakeUp;
        ++currentLevel;

        CameraShake.Instance.StartShakeBy(0.2f, shakePreset);
        audioRumble.PlayDelayed(0.2f);

        if (currentLevel >= difficulties.Length)
        {
            Debug.Log("BOSS DEAD");
            AS_W6BossAnimation.Instance.TakeHitSFX();
            AS_W6BossMusik.Instance.MusikFightDown();
            AS_W6BossMusik.Instance.MusikIntro();
            blink.Stop();
            blink.SetColor(Color.white);
            currentLevel = 0;
            ChangeState(States.Cutscene);
        }
        else
        {
            animator.SetTrigger("Hit");
            AS_W6BossAnimation.Instance.TakeHitSFX();
            blink.Play(currentDifficulty.blinkSpeed, Color.white, currentDifficulty.blinkColor);
            yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(timeToWakeUp));

            currentDifficulty.currentAttack = 0;
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
        TogglePlatformsControl(false);

        // Wait for the hands to be on the initial position
        while (leftHandPlatform.position != leftInitPos || !Mathf.Approximately(leftHandPlatform.eulerAngles.z, leftInitAngle) || rightHandPlatform.position != rightInitPos || !Mathf.Approximately(rightHandPlatform.eulerAngles.z, rightInitAngle))
        {
            if (!ManagerPause.Pause)
            {
                float delta = currentDifficulty.fistsPunchSpeed * Time.deltaTime;

                leftHandPlatform.position = Vector3.MoveTowards(leftHandPlatform.position, leftInitPos, delta);

                Vector3 angles = leftHandPlatform.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, leftInitAngle, delta);
                leftHandPlatform.eulerAngles = angles;

                rightHandPlatform.position = Vector3.MoveTowards(rightHandPlatform.position, rightInitPos, delta);

                angles = rightHandPlatform.eulerAngles;
                angles.z = Mathf.MoveTowardsAngle(angles.z, rightInitAngle, delta);
                rightHandPlatform.eulerAngles = angles;
            }

            yield return null;
        }
        AS_W6BossAnimation.Instance.RecoverHandsSFX();
        animator.SetBool("Vulnerable", false);

        while (!leftHandPlatformClone.gameObject.activeSelf && !rightHandPlatformClone.gameObject.activeSelf)
            yield return null;

        TogglePlatformHands(false);

        vulnerable = false;
    }

    public void TogglePlatformHands()
    {
        TogglePlatformHands(!leftHandPlatform.gameObject.activeSelf);
    }

    public void TogglePlatformHands(bool value)
    {
        leftHandPlatform.gameObject.SetActive(value);
        rightHandPlatform.gameObject.SetActive(value);

        if (value)
        {
            if (leftInputController == null)
                leftInputController = leftHandPlatform.GetComponentInChildren<PlatformInputController>();
            if (rightInputController == null)
                rightInputController = rightHandPlatform.GetComponentInChildren<PlatformInputController>();
        }

        TogglePlatformsControl(value);
    }

    public void TogglePlatformsControl(bool value)
    {
        ManagerPlatform.Instance.DeselectAll();

        if (leftInputController)
        {
            ManagerPlatform.Instance.AddPlatform(leftInputController);
            leftInputController.ToggleControl(value);
        }

        if (rightInputController)
        {
            ManagerPlatform.Instance.AddPlatform(rightInputController);
            rightInputController.ToggleControl(value);
        }
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
    public PlatformAI endPlatform;
    public string nextScene;
    public Color deadColor;
    public GameObject[] deadEffects;
    public TweenTextShow[] dialogues;
    public Animator bossNameText;
    public GameObject blackBorder;

    TextMesh[] dialoguesMeshes
    {
        get
        {
            if (_dialoguesMeshes == null)
            {
                _dialoguesMeshes = new TextMesh[dialogues.Length];
                for (int i = 0; i != _dialoguesMeshes.Length; ++i)
                    _dialoguesMeshes[i] = dialogues[i].GetComponent<TextMesh>();
            }
            return _dialoguesMeshes;
        }
    }

    TextMesh[] _dialoguesMeshes;
    int currentDialogue = -1;
    bool lowerPlatform;

    void CutsceneReset()
    {
        lowerPlatform = false;
        endPlatform.ResetPlatform(0);
        endPlatform.enabled = false;
        endPlatform.gameObject.SetActive(false);
        currentDialogue = -1;

        foreach (GameObject effect in deadEffects)
            effect.SetActive(false);
    }

    public KillFinalBossTrophy trophyKill;

    IEnumerator Cutscene_Enter()
    {
        audioEnding.PlayDelayed(1.0f);
        trophyKill.CompleteTrophy();

        //lUIS COunter
        float timer = CounterTimerPlay.Instance.EndTime();
        SaveManager.Instance.AddPlayTime(timer);
        ManagerAnalytics.MissionCompleted(Application.loadedLevelName,
            false, timer, 0, true);
        LoadLevelManager.Instance.LoadLevel(nextScene, true);
        // Set the animation
        animator.SetBool("Vulnerable", false);
        animator.SetTrigger("Death");

        // Wait until boss wakes up again
        lowerPlatform = false;
        while (!lowerPlatform)
            yield return null;

        // Now start lowering the platform
        endPlatform.gameObject.SetActive(true);
        endPlatform.enabled = true;

        //enable the black border
        if(blackBorder!=null)
        { 
            blackBorder.SetActive(true);
        }

        while (endPlatform.enabled)
            yield return null;

        // When the platform reachs the limit, K1 comes in
        animator.SetTrigger("Hit");
    }

    public void LowerPlatform()
    {
        lowerPlatform = true;
    }

    public void BossDead()
    {
        blink.SetColor(deadColor);

        SpecialBlink spBlink = blink as SpecialBlink;

        for (int i = 0; i != spBlink.renderers.Length; ++i)
            spBlink.renderers[i].sortingLayerName = "MechaBossBack";

        foreach (GameObject effect in deadEffects)
            effect.SetActive(true);

        foreach (Difficulty difficulty in difficulties)
            if (difficulty.effectToEnable != null)
                difficulty.effectToEnable.SetActive(false);
    }

    public void TurnOffPlatform()
    {
        endPlatform.gameObject.SetActive(false);
        target.gameObject.SetActive(false);
    }

    public void CutsceneEnded()
    {
        CompleteScene();

        LoadLevelManager.Instance.ActivateLoadedLevel();
    }

    public void ShowEndDialogue()
    {
        dialogues[++currentDialogue].InitText();

        for (int i = 0; i != currentDialogue; ++i)
            if (dialoguesMeshes[currentDialogue].font == dialoguesMeshes[i].font)
                dialogues[i].gameObject.SetActive(false);

        currentDialogue = Mathf.Clamp(currentDialogue, 0, dialogues.Length);
    }

    public void HideCurrentDialogue()
    {
        dialogues[currentDialogue].gameObject.SetActive(false);
    }

    public void ShowBossBanner()
    {
        bossNameText.SetTrigger("Show");
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
        SaveManager.Instance.dataKlaus.CompleteGame = true;

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

    #region Utilities

    bool currentAnimationEnded;

    public void OnCurrentAnimationEnded()
    {
        currentAnimationEnded = true;
    }

    void Update()
    {
        if (!leftHand.gameObject.activeSelf)
        {
            leftHand.position = leftHandClone.position;
            leftHand.eulerAngles = leftHandClone.eulerAngles;
            rightHand.position = rightHandClone.position;
            rightHand.eulerAngles = rightHandClone.eulerAngles;
        }

        if (!leftHandPlatform.gameObject.activeSelf)
        {
            leftHandPlatform.position = leftHandPlatformClone.position;
            leftHandPlatform.eulerAngles = leftHandPlatformClone.eulerAngles;
            rightHandPlatform.position = rightHandPlatformClone.position;
            rightHandPlatform.eulerAngles = rightHandPlatformClone.eulerAngles;
        }
    }

    IEnumerator TweenColor(Color color, float time)
    {
        float timer = time;
        Color initialColor = blink.GetColor();

        while (timer >= 0)
        {
            if (!ManagerPause.Pause && !ManagerStop.Stop)
            {
                timer -= Time.deltaTime;
                blink.SetColor(Color.Lerp(color, initialColor, timer / time));
            }

            yield return null;
        }
    }

    public void GenerateWaves(Vector3 position)
    {
        //        if (!throwWaves)
        //            return;
        //
        //        handInGround = true;
        //        Vector3 wavePosition = new Vector3(position.x, wavePositionY);
        //
        //        BulletHandler waveRight = wavePrefab.Spawn<BulletHandler>(wavePosition);   // Right
        //        waveRight.trigger.canKillPlayer = wavesKillPlayer;
        //        waveRight.SetDirection(new BulletInfo(waveRight.transform.right, currentDifficulty.wavesLifetime));
        //
        //        BulletHandler waveLeft = wavePrefab.Spawn<BulletHandler>(wavePosition);    // Left
        //        waveLeft.trigger.canKillPlayer = wavesKillPlayer;
        //        waveLeft.transform.localEulerAngles = new Vector3(0, 180f, 0);
        //        waveLeft.SetDirection(new BulletInfo(waveLeft.transform.right, currentDifficulty.wavesLifetime));
        //
        //        CameraShake.Instance.StartShakeBy(0.2f, shakePreset);
    }

    void ToNextAttack()
    {
        float value = UnityEngine.Random.value;

        if (value <= tauntProbability)
        {
            ChangeState(States.Taunt);
            audioTaunt.PlayDelayed(0.1f);
        }
        else
        {

            if (++currentDifficulty.currentAttack == currentDifficulty.attacksSequence.Length)
                currentDifficulty.currentAttack = 0;

            PlayCurrentAttack();
        }
    }

    void PlayCurrentAttack()
    {
        ChangeState(currentDifficulty.attacksSequence[currentDifficulty.currentAttack]);
    }

    IEnumerator MoveTowards(Transform transform, Vector2 position, float speed, bool restrictX = false, bool restrictY = false)
    {
        Vector3 targetPosition = transform.position;
        if (!restrictX)
            targetPosition.x = position.x;
        if (!restrictY)
            targetPosition.y = position.y;

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > 0.005f)
        {
            if (!ManagerPause.Pause)
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator UpdateAnimatorSpeed(float duration, float maxSpeed)
    {
        float timer = duration;
        while (timer >= 0)
        {
            if (!ManagerPause.Pause)
            {
                animator.speed = Mathf.Lerp(maxSpeed, 1f, Mathf.InverseLerp(0, duration, timer));
                timer -= Time.deltaTime;
            }
            yield return null;
        }
    }

    #endregion

    #region Weak Point

    [Header("Weak Point")]
    public CPUTrigger[] cpus;
    public BossPlatform[] platforms;

    void ResetCPUAccess()
    {
        // Reset all the platforms and their boxes
        foreach (BossPlatform platform in platforms)
        {
            platform.ResetPlatform(0);
            platform.hacked = false;

            foreach (GameObject box in platform.boxes)
                box.SetActive(true);
        }

        // Reset all the cpus
        foreach (CPUTrigger cpu in cpus)
            cpu.ResetCPU();
    }

    void OpenCPUAccess()
    {
        // Open all platforms that can still move
        foreach (BossPlatform platform in platforms)
        {
            if (!platform.hacked && platform.CanMove())
                platform.SetSpot(0);
        }

        // Reset all the cpus
        foreach (CPUTrigger cpu in cpus)
            cpu.ResetCPU();
    }

    void CloseCPUAccess()
    {
        // Close the rooms with the platforms
        foreach (BossPlatform platform in platforms)
        {
            if (platform.CanMove())
                platform.SetSpot(1);
        }

        // Reset all the cpus
        foreach (CPUTrigger cpu in cpus)
            cpu.ResetCPU();
    }

    #endregion

    #region Taunt

    [Header("Taunt")]
    [Range(0, 1)]
    public float tauntProbability;

    IEnumerator Taunt_Enter()
    {
        animator.SetFloat("KlausPosition", currentLevel == 0 ? 0 : -1);
        animator.SetTrigger("Taunt");
        audioTaunt.PlayDelayed(0.1f);

        CameraShake.Instance.StartShake(shakePreset);
        audioRumble.PlayDelayed(0f);
        audioTaunt.PlayDelayed(0.1f);
        EnableBlur();

        currentAnimationEnded = false;
        while (!currentAnimationEnded)
            yield return null;

        DisableBlur();
        CameraShake.Instance.StopShake();

        ToNextAttack();
    }

    #endregion

    #region Audio

    [Header("Audio")]
    public AudioSource audioS1;
    public AudioSource audioS2;
    public AudioSource audioS3;
    public AudioSource audioEnding;
    public AudioSource audioTaunt;
    public AudioSource audioRumble;
    public AudioClip ActivateSFX;
    public AudioClip[] SFXS;
    [Header("Audio Properties")]
    public float speed;

    IEnumerator VolUp(AudioSource audio)
    {
        for (float i = 0; i <= 1f; i += speed * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;

        }
    }

    IEnumerator VolDown(AudioSource audio)
    {
        for (float i = 1; i >= 0f; i -= speed * Time.deltaTime)
        {
            audio.volume = i;
            yield return null;
        }
        audio.Stop();
    }

    IEnumerator PanCenterFL(AudioSource audio)
    {
        for (float i = -1f; i < 0; i += 0.6f * Time.deltaTime)
        {
            audio.panStereo = i;
            yield return null;
        }
    }

    IEnumerator PanCenterFR(AudioSource audio)
    {
        for (float i = 1f; i > 0; i -= 0.6f * Time.deltaTime)
        {
            audio.panStereo = i;
            yield return null;
        }
    }

    IEnumerator PanLeft(AudioSource audio)
    {
        for (float i = 0f; i > -1; i -= 0.4f * Time.deltaTime)
        {
            audio.panStereo = i;
            yield return null;
        }
    }

    IEnumerator PanRight(AudioSource audio)
    {
        for (float i = 0f; i < 1; i += 0.4f * Time.deltaTime)
        {
            audio.panStereo = i;
            yield return null;
        }
        audio.panStereo = 0;
    }

    #endregion

    #region Blur

    [Header("Blur")]
    public RadialBlur blur;
    public Vector2 blurLimits = new Vector2(0.01f, 0.1f);
    public float blurSpeed = 1f;
    bool blurEnabled = false;

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
                // The center of the blur is always the same - the head of the boss
                //                Vector2 center = blur.center;
                //                center.x = Mathf.InverseLerp(leftPoint.position.x, rightPoint.position.x, transform.position.x);
                //                center.y = Mathf.InverseLerp(leftPoint.position.y, rightPoint.position.y, transform.position.y);
                //                blur.center = center;

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
}
