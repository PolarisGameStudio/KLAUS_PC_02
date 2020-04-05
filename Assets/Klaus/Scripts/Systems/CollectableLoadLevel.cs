using UnityEngine;
using System.Collections;

public class CollectableLoadLevel : MonoBehaviour
{
    public string sortingLayer;
    public int sortingNumber = 0;
    public SpriteRenderer spriteR;
    [HideInInspector]
    public string sceneToLoad = "";
    public bool PreloadScene = true;
    public void ChangeLevel()
    {
        CameraFade.StartAlphaFade(Color.black, false, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += LoadLevel;
    }

    void LoadLevel()
    {
        CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
        Time.timeScale = 1;
        CameraShake.Instance.StopShake();
        if (PreloadScene)
        {
            LoadLevelManager.Instance.ActivateLoadedLevel();
        }
        else {
            LoadLevelManager.Instance.LoadLevelWithLoadingScene(sceneToLoad, false);
        }
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
    }

    public void ChangeOrdeingLayer()
    {
        spriteR.sortingLayerName = sortingLayer;
        spriteR.sortingOrder = sortingNumber;
    }
}
