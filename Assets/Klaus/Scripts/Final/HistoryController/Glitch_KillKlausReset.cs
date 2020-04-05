using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Glitch_KillKlausReset : MonoBehaviour
{
    public DeadState[] KlausDead;

    public static bool DeadByGamePlay = false;
    protected float currentTimePlayLevel = 0;
    bool isReseting = false;

    void Awake()
    {
        if (DeadByGamePlay)
        {
            // Door.isStartLevel = false;
            // Door.canStartAnalitic = false;
            // Door.SetLevelCounterTimer(currentTimePlayLevel);
            DeadByGamePlay = false;
            currentTimePlayLevel = 0;
        }
        else
        {
            currentTimePlayLevel = 0;
        }
    }


    void OnEnable()
    {

        if (KlausDead == null)
        {
            KlausDead = GameObject.FindObjectsOfType<DeadState>();

        }
        for (int i = 0; i < KlausDead.Length; ++i)
        {
            KlausDead[i].onRespawn += OnDead;
        }
    }

    void OnDead(Vector3 pos)
    {
       // Debug.Log("Klaus is dead");
        if (isReseting)
            return;

        isReseting = true;
        DeadByGamePlay = true;

        CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0.0f);
        CameraFade.Instance.m_OnFadeFinish += RestartScene;

        //  currentTimePlayLevel = Door.TimePlayingLevel;
    }
    void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // LoadLevelManager.Instance.RestartCurrentLevel();
    }
    void OnDestroy()
    {
        CameraFade.Instance.m_OnFadeFinish -= RestartScene;
    }

}
