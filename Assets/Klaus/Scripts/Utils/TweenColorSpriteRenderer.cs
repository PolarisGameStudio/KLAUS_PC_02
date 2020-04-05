using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenColorSpriteRenderer : MonoBehaviour
{

    public Color fromColor;
    public Color toColor;
    public float time = 0.5f;

    public int Loop = -1;
    public LoopType typeLoop = LoopType.Yoyo;
    SpriteRenderer _renderer;

    Tweener materialTwen;
    bool firstRun = true;

    public SpriteRenderer renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    static bool initTween = false;


    void Awake()
    {
        if (!initTween)
        {
            DOTween.Init();
            initTween = true;
        }
    }

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

        renderer.material.color = fromColor;
        if (materialTwen == null)
            materialTwen = renderer.DOColor(toColor, time).SetLoops(Loop, typeLoop);

        materialTwen.Play();

    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);

        materialTwen.Pause();
    }

    #region Pause:

    int twennPaused = 0;

    public void OnPauseGame()
    {
        materialTwen.Pause();
        twennPaused = 1;
    }

    public void OnResumeGame()
    {
        if (twennPaused > 0)
        {
            materialTwen.Play();
            twennPaused = 0;
        }
    }

    #endregion
}
