using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using Rewired;

//TODO: UNITY5_5 RE-Make this plugin
public class CameraMovement : BasePlugin
{

    const InputActionOld VerticalButton = InputActionOld.Move_Camera_Y;
    const InputActionOld HorizontalButton = InputActionOld.Move_Camera_X;

    bool m_staticCamera = false;
    bool m_isMoving = false;
    bool canTakeInput = true;
    Coroutine m_couStaticCamera;

    public float MaxDistanceToMove = 5.0f;
    public float InfluenceSmoothness = .2f;

    Vector3 originalPos = Vector3.zero;
    Vector3 direction = Vector3.zero;
    Vector3 _influence = Vector3.zero;
    Vector3 _velocity = Vector3.zero;

    ProCamera2D _proCamera = null;

    public float TimeStatic = 1.0f;
    public bool useDontMove = false;

    public ProCamera2D proCamera2D
    {

        get
        {
            if (_proCamera == null)
                _proCamera = GetComponent<ProCamera2D>();

            return _proCamera;
        }
    }
    HUD_BlockCamera _blockCamHUD = null;

    public HUD_BlockCamera blockCamHUD
    {

        get
        {
            if (_blockCamHUD == null)
                _blockCamHUD = GameObject.FindObjectOfType<HUD_BlockCamera>();

            return _blockCamHUD;
        }
    }

    public static bool IsBlockingMove
    {
        get
        {
            if (InputEnum.USE_CONTROL)
            {
                return ReInput.players.GetPlayer(0).GetButton("Click Move Camera");
                // return InputManager.GetButton(InputEnum.GetInputString(InputActionOld.Click_Move_Camera));
            }

            return false;
        }

    }

    #region UnityCallbacks:

    void LateUpdate()
    {
        if (ProCamera2D.UpdateType == UpdateType.LateUpdate)
            LogicPlugin();
    }

    void FixedUpdate()
    {
        if (ProCamera2D.UpdateType == UpdateType.FixedUpdate)
            LogicPlugin();

    }
    /*
    void OnEnable()
    {
        if (blockCamHUD != null)
        {
            blockCamHUD.Hide();
        }

    }
    void OnDisable()
    {
        if (blockCamHUD != null)
        {
            blockCamHUD.Show();
        }
    }*/
    // Update is called once per frame
    void LogicPlugin()
    {
        if (!ManagerPause.Pause)
        {
            CheckInput();
            if (m_isMoving)
            {
                if (Limitation())
                {
                    MoveCamera();
                }
                else
                {
                    if (!m_staticCamera || canTakeInput)
                    {
                        canTakeInput = false;
                        direction = Vector3.zero;
                        _influence = Vector3.zero;
                        if (useDontMove)
                            proCamera2D.enabled = false;

                        if (m_couStaticCamera != null)
                            StopCoroutine(m_couStaticCamera);
                        m_couStaticCamera = StartCoroutine(StaticCamera());
                    }
                }


            }

            if (!proCamera2D.ExclusiveTargetPosition.HasValue)
            {
                if (blockCamHUD != null)
                {
                    blockCamHUD.Hide();
                }
            }
        }
    }

    #endregion

    IEnumerator HideBlock(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        if (blockCamHUD != null)
        {
            blockCamHUD.Hide();
        }

    }
    void CheckInput()
    {
        if (!canTakeInput)
        {
            //Safaty Check for some situation
            if (m_isMoving && !m_staticCamera)
            {
                if (m_couStaticCamera != null)
                    StopCoroutine(m_couStaticCamera);
                m_couStaticCamera = StartCoroutine(StaticCamera());
            }
            return;
        }

        bool canMove = true;

        if (InputEnum.USE_CONTROL)
        {
            //canMove = InputManager.GetButton(InputEnum.GetInputString(InputActionOld.Click_Move_Camera));
            canMove = ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(InputActionOld.Click_Move_Camera));
        }

        // float xInput = InputManager.GetAxis(InputEnum.GetInputString(HorizontalButton));
        float xInput = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(HorizontalButton));
        if (Mathf.Abs(xInput) < 0.4f)
            xInput = 0;
        //  float yInput = InputManager.GetAxis(InputEnum.GetInputString(VerticalButton));
        float yInput = ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(VerticalButton));
        if (Mathf.Abs(yInput) < 0.4f)
            yInput = 0;

        if (canMove
            && (!Mathf.Approximately(yInput, 0.0f)
                || !Mathf.Approximately(xInput, 0.0f)))
        {
            if (proCamera2D.ExclusiveTargetPosition.HasValue)
            {
                if (blockCamHUD != null)
                {
                    blockCamHUD.Show();
                    StopCoroutine("HideBlock");
                    StartCoroutine("HideBlock", 1.5f);
                }
            }
            if (m_couStaticCamera != null)
                StopCoroutine(m_couStaticCamera);
            m_staticCamera = false;
            if (!m_isMoving)
            {
                originalPos = transform.position;
                m_isMoving = true;
            }

            direction = new Vector2(xInput, yInput);
            direction.Normalize();
        }
        else
        {
            if (m_isMoving)
            {
                canTakeInput = false;
                if (m_couStaticCamera != null)
                    StopCoroutine(m_couStaticCamera);
                m_couStaticCamera = StartCoroutine(StaticCamera());
            }
        }
    }

    IEnumerator StaticCamera()
    {
        m_staticCamera = true;
        yield return StartCoroutine(new TimeCallBacks().WaitPause(TimeStatic));
        direction = Vector3.zero;
        _influence = Vector3.zero;
        ProCamera2D.ApplyInfluence(_influence);

        m_isMoving = false;
        originalPos = Vector3.zero;
        canTakeInput = true;
        if (useDontMove)
            proCamera2D.enabled = true;
        m_staticCamera = false;

    }

    bool Limitation()
    {
        return (Vector3.Distance(transform.position, originalPos) <= MaxDistanceToMove);

    }

    void MoveCamera()
    {
        _influence = Vector3.SmoothDamp(_influence, VectorHV(direction.x * MaxDistanceToMove, direction.y * MaxDistanceToMove), ref _velocity, InfluenceSmoothness);
        ProCamera2D.ApplyInfluence(_influence);
    }

    private void Awake()
    {
       
    }

    private void Update()
    {
       
    }
}
