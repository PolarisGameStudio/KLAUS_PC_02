using UnityEngine;
using System.Collections;

public class TreadmillPlatform : MonoBehaviour
{

    public SurfaceEffector2D effector;
    public Transform Sprite;

    bool isRight = true;
    bool firstRun = true;

    void Start()
    {

        if (effector.speed < 0)
            InvertSprite(false);

        ManagerPause.SubscribeOnPauseGame(OnPauseGame);
        ManagerPause.SubscribeOnResumeGame(OnResumeGame);
        firstRun = false;

        if (transform.parent != null)
        {
            CurrentPlatform platform = transform.parent.GetComponentInChildren<CurrentPlatform>();
            if (platform != null)
                platform.transform.position += Vector3.up * 0.26f;
        }
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
        effector.enabled = false;
    }

    public void OnResumeGame()
    {
        effector.enabled = true;
    }
    public void InvertSprite(bool isRigthAux)
    {
        if ((isRight && !isRigthAux)
            || (!isRight && isRigthAux))
        {
            Sprite.localScale = new Vector3(Sprite.localScale.x * -1, Sprite.localScale.y, Sprite.localScale.z);
        }
        isRight = isRigthAux;
    }
}
