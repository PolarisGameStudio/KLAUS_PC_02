using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Assertions;
using DG.Tweening;
using Rewired;
#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class InputTouchPS4 : MonoBehaviour
{
#if UNITY_PS4 && !UNITY_EDITOR
    public const int playerId = 0;
    private PS4Input.LoggedInUser loggedInUser;
    private PS4Input.ConnectionType connectionType;
#endif
    // Touchpad variables

    private int touchNum, touch0x, touch0y, touch0id, touch1x, touch1y, touch1id;
    int touchResolutionX, touchResolutionY, analogDeadZoneLeft, analogDeadZoneRight;
    private float touchPixelDensity;
    public LayerMask PlatformMask;
    RaycastHit hit;
    //#endif
    public Transform touchPrefab;
    Transform m_touchSpawned;

    public const float ScaleFactorX = 1.25f;
    public const float ScaleFactorY = 1.35f;

    Vector2 storeMouse = Vector2.zero;

    #region target var:
    const string c_targetResources = "GO/TargetGamepad";
    GameObject m_targetPrefab;
    GameObject m_targetSpawned;
    Tweener m_animPosTarget;
    [SerializeField]
    float m_animDurationTarget = .5f;
    bool m_isReleaseSelectButton = false;

    [SerializeField]
    float m_distanceCastControl = 20;
    [SerializeField]
    float m_radiusCastControl = 1;
    [SerializeField]
    LayerMask m_maskCastControl;
    RaycastHit[] m_hitsCastArray = new RaycastHit[4];

    #endregion

    //#if UNITY_PS4
    void Awake()
    {
        m_touchSpawned = touchPrefab.Spawn();
    }
    protected bool BlockInput = false;
    public void Block(bool bloc)
    {
        BlockInput = bloc;
    }

    void MouseInputSelection()
    {

#if UNITY_PS4 && !UNITY_EDITOR
        PS4Input.GetPadControllerInformation(playerId, out touchPixelDensity, out touchResolutionX, out touchResolutionY, out analogDeadZoneLeft, out analogDeadZoneRight, out connectionType);
        PS4Input.GetLastTouchData(playerId, out touchNum, out touch0x, out touch0y, out touch0id, out touch1x, out touch1y, out touch1id);
#endif


        if (
#if UNITY_PS4 && !UNITY_EDITOR
            touchNum > 0

#elif UNITY_PSP2 && !UNITY_EDITOR
            Input.touchCount == 1
#else
Vector2.Distance(storeMouse, Input.mousePosition) > 0.5f
                //Input.GetMouseButton(0)
#endif
                )
        {
#if UNITY_PS4 && !UNITY_EDITOR
            if (touch0x > 0 || touch0y > 0)
            {
#endif
            if (!m_touchSpawned.gameObject.activeSelf)
                m_touchSpawned.gameObject.SetActive(true);

            float xDraw = 0;
            float yDraw = 0;
#if UNITY_PS4 && !UNITY_EDITOR
               // m_touchSpawned.position = Camera.main.ScreenToWorldPoint(new Vector3(touch0x, touchResolutionY - touch0y, 9));
                 xDraw = (1.0f /touchResolutionX) * touch0x;
                xDraw -= 0.5f;
                xDraw *= ScaleFactorX;
                xDraw += 0.5f;
                xDraw *= Screen.width;

                yDraw = (1.0f / touchResolutionY) * touch0y;
                yDraw = 1 - yDraw;
                yDraw -= 0.5f;
                yDraw *= ScaleFactorY;
                yDraw += 0.5f;
                yDraw *= Screen.height;

               // m_touchSpawned.position = (new Vector3(xDraw, -yDraw, 9));
#elif UNITY_PSP2 && !UNITY_EDITOR
                xDraw =  Input.touches[0].position.x;
                yDraw =  Input.touches[0].position.y;
            
#else
            xDraw = Input.mousePosition.x;
            yDraw = Input.mousePosition.y;
#endif
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(xDraw, yDraw, 0));
            m_touchSpawned.position = ray.origin;
            if (Physics.Raycast(m_touchSpawned.position, Vector3.forward, out hit, 100, PlatformMask))
            {
                hit.collider.GetComponent<Input3DSelection>().SelectByInput();
            }
#if UNITY_PS4 && !UNITY_EDITOR
            }
            else if (m_touchSpawned.gameObject.activeSelf)
            {
                m_touchSpawned.gameObject.SetActive(false);
            }
#endif
        }
        else if (m_touchSpawned.gameObject.activeSelf)
        {
            m_touchSpawned.gameObject.SetActive(false);
        }
#if UNITY_PS4 && !UNITY_EDITOR
#else
        storeMouse = Input.mousePosition;
#endif
    }
    #region GamePAD Var:
    Input3DSelection m_currentSelected = null;
    Input3DSelection[] m_platSelections = null;
    bool m_checkReleaseButton = false;

    const float m_maxDistanceToSelect = 9.0f;
    public const float AxisTriggerStart = 0.0f;

    public static float GetValueSelectPlatform()
    {
        return InputEnum.NormalizeTriggerL2(ReInput.players.GetPlayer(0).GetAxisRaw(InputEnum.GetInputString(InputActionOld.Click_Select_Platform)));
    }

    public static bool IsSelectingPlatforms
    {
        get
        {
            Debug.Log("Value select platform "+ GetValueSelectPlatform());
            return GetValueSelectPlatform() > AxisTriggerStart;
        }
    }
    #endregion

    bool IsPlatformInLimits(Input3DSelection trans, Vector2 center)
    {
        //return m_maxDistanceToSelect >= Vector2.Distance(trans.Center, center);
        return trans.SpriteForInput.isVisible;
    }


    void FindAllPlatforms()
    {
        m_platSelections = FindObjectsOfType<Input3DSelection>();
    }


    void SortNearPlatform(Vector3 center)
    {
        Array.Sort(m_platSelections, new SpriteVisibleComparer());

        int firstIndexToClear = Array.FindIndex(m_platSelections, (Input3DSelection obj) => !IsPlatformInLimits(obj, center));
        if (firstIndexToClear > -1)
            Array.Resize(ref m_platSelections, firstIndexToClear);
    }

    void SortSmallestAngle(Vector3 center, Vector2 inputDir)
    {
        if (inputDir != Vector2.zero)
            Array.Sort(m_platSelections, new AngleComparer(center, inputDir));

    }


    /// <summary>
    /// First Task: Select all near platform.
    /// Second Task: Use the input direction and using the center sort by the smallest angle.
    /// Third Recalculate everything with the new selected.
    /// </summary>
    void GamePadInputSelection()
    {
        if (IsSelectingPlatforms)
        {
            m_isReleaseSelectButton = true;

            int selectPosArray = -1;
            //Check if the platform is still in view
            if (m_currentSelected != null && !IsPlatformInLimits(m_currentSelected, Camera.main.transform.position))
            {
                m_currentSelected = null;
                m_platSelections = null;
            }

            if (m_platSelections == null || m_platSelections.Length == 0)//TODO: checker of distance to recalculate this
            {
                FindAllPlatforms();
                //All the visibile platform to the camera
                SortNearPlatform(Camera.main.transform.position);

                if (m_platSelections == null || m_platSelections.Length == 0)
                {
                    return;
                }

                if (m_currentSelected == null)
                {
                    selectPosArray = 0;
                    m_checkReleaseButton = true;
                }
            }


            var dirAxis = new Vector2(ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Move_Select_Platform_X))
          , ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(InputActionOld.Move_Select_Platform_Y)));

            if (!m_checkReleaseButton)
            {
                if (dirAxis.sqrMagnitude > 0.6f)
                {
                    dirAxis.Normalize();
                    // SortSmallestAngle(m_currentSelected.Center, dirAxis);
                    Vector3 dir = new Vector3(dirAxis.x, dirAxis.y, 0).normalized;
                    var centerToRay = m_currentSelected.Center + dir * 2f;
                    Debug.DrawLine(centerToRay, centerToRay + dir * m_distanceCastControl, Color.black, 5);

                    int hitCast = Physics.SphereCastNonAlloc(centerToRay
                                       , m_radiusCastControl
                                       , dir
                                       , m_hitsCastArray
                                       , m_distanceCastControl
                                       , m_maskCastControl
                                                            );
                    // RaycastHit hit;
                    if (hitCast > 0)
                    {
                        Array.Sort(m_hitsCastArray, 0, hitCast, new DistanceComparer());
                        Input3DSelection inputCast = null;
                        for (int i = 0; i < hitCast; ++i)
                        {
                            inputCast = m_hitsCastArray[i].collider.gameObject.GetComponent<Input3DSelection>();

                            if (inputCast != m_currentSelected)
                                break;

                        }

                        for (int i = 0; i < m_platSelections.Length; ++i)
                        {

                            if (inputCast == m_platSelections[i])
                            {
                                if (inputCast != m_currentSelected)
                                {
                                    selectPosArray = i;
                                }
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                if (dirAxis.sqrMagnitude < 0.1f)
                    m_checkReleaseButton = false;
            }

            if (selectPosArray > -1)
            {
                m_checkReleaseButton = true;
                m_currentSelected = m_platSelections[selectPosArray];
            }
            Assert.IsNotNull(m_currentSelected);

            m_currentSelected.SelectByInput();
            ActivePointer(m_currentSelected.Center);
        }
        else if (m_isReleaseSelectButton)
        {
            m_isReleaseSelectButton = false;
            DeactivePointer();
            m_platSelections = null;
        }
    }

    void ActivePointer(Vector2 target)
    {
        if (m_targetPrefab == null)
        {
            m_targetPrefab = Resources.Load<GameObject>(c_targetResources);
            Assert.IsNotNull(m_targetPrefab);
            m_targetSpawned = Instantiate(m_targetPrefab);
        }

        if (!m_targetSpawned.activeSelf)
        {
            m_targetSpawned.transform.position = target;
            m_targetSpawned.SetActive(true);
        }

        //play animation to target Tween
        if (m_animPosTarget != null)
        {
            m_animPosTarget.Kill(false);

        }
        m_animPosTarget = m_targetSpawned.transform.DOMove(target, m_animDurationTarget);
        m_animPosTarget.PlayForward();
    }


    void DeactivePointer()
    {
        if (m_targetSpawned != null)
        {
            m_animPosTarget.Kill(false);
            m_targetSpawned.SetActive(false);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!ManagerPause.Pause && !BlockInput)
        {
            if (InputEnum.GamePad == "keyboard")
            {
                MouseInputSelection();
            }
            else
            {
                if (m_touchSpawned != null && m_touchSpawned.gameObject.activeSelf)
                {
                    m_touchSpawned.gameObject.SetActive(false);
                }
                GamePadInputSelection();
            }
        }
    }

    void OnDestroy()
    {
        if (m_targetSpawned != null)
        {
            Destroy(m_targetSpawned);
        }
    }
}

public class SpriteVisibleComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return Compare(x as Input3DSelection, y as Input3DSelection);
    }

    int Compare(Input3DSelection x, Input3DSelection y)
    {
        return y.SpriteForInput.isVisible.CompareTo(x.SpriteForInput.isVisible);
    }
}

public class DistanceComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return Compare((RaycastHit)x, (RaycastHit)y);
    }

    int Compare(RaycastHit x, RaycastHit y)
    {

        return x.distance.CompareTo(y.distance);
    }
}

public class AngleComparer : IComparer
{
    Vector2 m_center;
    Vector2 m_dir;

    public AngleComparer(Vector2 center, Vector2 dir)
    {
        m_center = center;
        m_dir = dir;
    }

    public int Compare(object x, object y)
    {
        return Compare((RaycastHit)x, (RaycastHit)y);
    }


    int Compare(RaycastHit x, RaycastHit y)
    {
        var xDir = (((Vector2)x.collider.transform.position - m_center)).normalized;
        var yDir = (((Vector2)y.collider.transform.position - m_center)).normalized;

        var xAngle = Vector2.Angle(m_dir, xDir);
        var yAngle = Vector2.Angle(m_dir, yDir);

        return xAngle.CompareTo(yAngle);
    }
}