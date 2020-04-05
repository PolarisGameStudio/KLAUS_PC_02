using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MemoriesPanel : MonoBehaviour
{
    public Button[] memories;

    void OnEnable()
    {
        SaveManager.onGameLoaded += OnGameLoaded;
        OnGameLoaded();
    }

    void OnDisable()
    {
        SaveManager.onGameLoaded -= OnGameLoaded;
    }

    void OnGameLoaded()
    {
        for (int i = 0; i != memories.Length; ++i)
            memories[i].interactable = CollectablesManager.isCollectableFull("W" + (i + 1));

        for (int i = 0; i != memories.Length; ++i)
        {
            if (!memories[i].interactable)
                continue;

            Navigation navigation = memories[i].navigation;

            for (int j = i - 1; j >= 0; --j)
            {
                if (memories[j].interactable)
                {
                    navigation.selectOnUp = memories[j];
                    break;
                }
            }

            for (int j = i + 1; j < memories.Length; ++j)
            {
                if (memories[j].interactable)
                {
                    navigation.selectOnDown = memories[j];
                    break;
                }
            }

            memories[i].navigation = navigation;
        }
    }
    string m_levelToLoad = "";
    public void LoadMemory(string name)
    {
        SaveManager.Instance.SetHistory();
        SaveManager.Instance.comingFromHistoryArcadeMode = true;
        SaveManager.Instance.comingFromMemoryMode = true;
        //SaveManager.Instance.dataKlaus.SetCurrentLevel(name);
        m_levelToLoad = name;
        CameraFade.StartAlphaFade(Color.black, false, 0.2f, 0.2f);
        CameraFade.Instance.m_OnFadeFinish += LoadLevel;
    }

    void OnDestroy()
    {
        if (CameraFade.InstanceExists())
            CameraFade.Instance.m_OnFadeFinish -= LoadLevel;
    }

    void LoadLevel()
    {
        LoadLevelManager.Instance.LoadLevelWithLoadingScene(m_levelToLoad, false);
    }

    public void OnSelectMemory()
    {
        for (int i = 0; i != memories.Length; ++i)
            if (memories[i].interactable)
            {
                EventSystem.current.SetSelectedGameObject(memories[i].gameObject);
                return;
            }

        EventSystem.current.SetSelectedGameObject(null);
    }
}
