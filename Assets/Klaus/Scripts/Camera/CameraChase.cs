using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraChase : Singleton<CameraChase>
{

    Tween tween;
    bool firstRun = true;

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
    }

    void OnDisable()
    {
        ManagerPause.UnSubscribeOnPauseGame(OnPauseGame);
        ManagerPause.UnSubscribeOnResumeGame(OnResumeGame);
    }

    public void OnPauseGame()
    {
        if (tween != null)
            tween.Pause();
    }

    public void OnResumeGame()
    {
        if (tween != null)
            tween.Play();
    }

    bool isChasing = false;
    GameObject newGameObject;
    BoxCollider2D box;
    float StoreOrtograhicSize = 0;
    public void StartChase(Transform target, float Speed)
    {
        isChasing = true;
        float time = Vector2.Distance(target.position, transform.position) / Speed;

        Vector3 dir = target.position - transform.position;
        if (newGameObject == null)
        {
            newGameObject = new GameObject();
            newGameObject.name = "SuperCollider";
            newGameObject.layer = LayerMask.NameToLayer("Systems");
            newGameObject.transform.parent = transform;
            box = newGameObject.AddComponent<BoxCollider2D>();

        }
        if (dir.normalized.x > 0.5f)
        {
            newGameObject.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height * 0.5f, 10)) - transform.position + Vector3.right * 0.5f;
            box.size = new Vector2(1, 20);
            StoreOrtograhicSize = Camera.main.orthographicSize;

        }
        else if (dir.normalized.x < -0.5f)
        {

            newGameObject.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.5f, 10)) - transform.position + Vector3.left * 0.5f;
            box.size = new Vector2(1, 20);
            StoreOrtograhicSize = Camera.main.orthographicSize;
        }
        else if (dir.normalized.y > 0.5f)
        {

            newGameObject.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height, 10)) - transform.position + Vector3.up * 0.5f;
            box.size = new Vector2(20, 1);
            StoreOrtograhicSize = Camera.main.orthographicSize;
        }
        else if (dir.normalized.y < -0.5f)
        {

            newGameObject.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0, 10)) - transform.position + Vector3.down * 0.5f;
            box.size = new Vector2(20, 1);
            StoreOrtograhicSize = Camera.main.orthographicSize;
        }


        if (tween == null)
        {
            enabled = true;
            CameraZoom.Instance.FinishZoom();
            CameraFollow.Instance.enabled = false;
            Camera.main.GetComponent<CameraMovement>().enabled = false;
            tween = transform.DOMove(target.position, time)
                .SetEase(Ease.Linear)
                .OnUpdate(OnUpdate)
                .OnComplete(OnComplete);
        }
        else
        {
            ChangeTarget(target, time);
        }

    }
    void OnUpdate()
    {
        CameraFollow.Instance.MoveCameraToOwnPos();

        float minus = Camera.main.orthographicSize - StoreOrtograhicSize;
        newGameObject.transform.localPosition += Vector3.right * minus * 1.8f;
        StoreOrtograhicSize = Camera.main.orthographicSize;
    }

    public void FinishChase()
    {
        if (tween != null)
            tween.Kill();
        OnComplete();
    }
    void ChangeTarget(Transform target, float time)
    {
        tween.Kill();
        tween = transform.DOMove(target.position, time)
            .SetEase(Ease.Linear)
            .OnUpdate(OnUpdate)
            .OnComplete(OnComplete);
    }

    void OnComplete()
    {
        isChasing = false;
        CameraFollow.Instance.enabled = true;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
    }
}
