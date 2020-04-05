using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour
{

    public Button button;
    public GameObject newGameLabel;

    void Start()
    {
        if (!SaveManager.Instance.dataKlaus.isNewGame)
        {
            button.interactable = true;
            newGameLabel.SetActive(true);
        }
        else
        {
            button.interactable = false;
            newGameLabel.SetActive(false);

        }
    }

    public void LoadLevel()
    {
        SaveManager.Instance.SetHistory();//Fix Luis
        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0);
        CameraFade.Instance.m_OnFadeFinish += ActivateManualty;
    }

    protected void ActivateManualty()
    {
        LoadLevelManager.Instance.LoadLevelWithLoadingScene(SaveManager.Instance.dataKlaus.GetCurrentLevel(), false);

        // LoadLevelManager.Instance.LoadLevelImmediate(SaveManager.Instance.dataKlaus.currentLevel);
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= ActivateManualty;
    }
}
