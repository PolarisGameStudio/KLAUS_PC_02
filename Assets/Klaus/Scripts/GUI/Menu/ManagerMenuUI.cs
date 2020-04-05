using UnityEngine;
using System.Collections;

public class ManagerMenuUI : Singleton<ManagerMenuUI>
{
    public Animator StartMenu;
    public Animator MenuMenu;
    public Animator InMenu;
    public Animator ArcadeMenu;

    public Arcade_ButtonSelectMenu[] worldButtons;

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    RectTransform _rectTransform;

    public Vector2 GetSize()
    {
        return rectTransform.sizeDelta;
    }

    public void ChangueToStartMenu()
    {
        StartMenu.SetTrigger("In");

    }

    IEnumerator Start()
    {
        yield return null;
        CameraFade.StartAlphaFade(Color.black, true, 0.0f);

        if (SaveManager.Instance.dataKlaus != null && SaveManager.Instance.isComingFromArcade && !SaveManager.Instance.comingFromMemoryMode)
        {
            ArcadeMenuPanel menuPanel = ArcadeMenu.GetComponent<ArcadeMenuPanel>();

            string lastArcadeLevel = SaveManager.Instance.lastArcadeLevel;
            int world = int.Parse(lastArcadeLevel[1].ToString()) - 1;

            // Entra al mundo
            worldButtons[world].SelectWorldInmediatly();

            // Highlight al nivel
            Arcade_ButtonSelect[] levels = ArcadeMenu.GetComponentsInChildren<Arcade_ButtonSelect>();

            for (int i = 0; i != levels.Length; ++i)
                if (levels[i].sceneName == lastArcadeLevel)
                {
                    menuPanel.levelPanel.SelectButton(i);
                    levels[i].SetHighlighted();
                    break;
                }

            yield return null;

            menuPanel.blockItemPanel = true;
            ChangueToArcadeMenu();
        }
        else
        {
            ChangueToStartMenu();
        }
    }

    public void UnloadStartMenu()
    {
        StartMenu.SetTrigger("Out");

    }

    public void ChangueToMenuMenu()
    {
        MenuMenu.SetTrigger("In");

    }

    public void UnloadMenuMenu()
    {
        MenuMenu.SetTrigger("Out");

    }

    public void ChangueToInMenu()
    {
        InMenu.SetTrigger("In");

    }

    public void ChangueToArcadeMenu()
    {
        ArcadeMenu.SetTrigger("In");
    }


}
