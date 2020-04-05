using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Colorful;

[RequireComponent(typeof(Animator))]
public class ArcadeMenuPanel : MonoBehaviour
{
    public float TimeGliching = 0.5f;
    public float ValueGlitching = 1;
    public bool blockItemPanel;

    public Animator animator
    {
        get
        {
            if (anim == null)
                anim = GetComponent<Animator>();
            return anim;
        }
    }

    public Arcade_ItemPanel itemPanel
    {
        get
        {
            if (item == null)
                item = GetComponentInChildren<Arcade_ItemPanel>();
            return item;
        }
    }

    public Arcade_LevelPanel levelPanel
    {
        get
        {
            if (level == null)
                level = GetComponentInChildren<Arcade_LevelPanel>();
            return level;
        }
    }

    Animator anim;
    Arcade_ItemPanel item;
    Arcade_LevelPanel level;

    public void Init()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ShowItemPanel()
    {
        if (blockItemPanel)
        {
            blockItemPanel = false;
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);

        ChangueGlitching();
        levelPanel.Hide();
        itemPanel.Show();
    }

    public void ShowLevelPanel()
    {
        Debug.Log("Show level Panel");
        EventSystem.current.SetSelectedGameObject(null);

        ChangueGlitching();
        itemPanel.Hide();
        levelPanel.Show();
    }

    public void StartOut()
    {
        itemPanel.Hide();
        levelPanel.Hide();
        animator.SetTrigger("Out");
    }

    public void ChangueBackMenu()
    {
        ManagerMenuUI.Instance.ChangueToMenuMenu();
    }

    #region GlitchEffect:

    public Glitch glitch;

    public void ChangueGlitchValue(float value)
    {
        if (value > 0)
            glitch.enabled = true;
        else
            glitch.enabled = false;
    }

    public void ChangueGlitching()
    {
        StartCoroutine(ChangueGlitchTime(TimeGliching, ValueGlitching));
    }

    public IEnumerator ChangeWithGlitch()
    {
        yield return StartCoroutine(ChangueGlitchTime(TimeGliching, ValueGlitching));
    }

    public IEnumerator ChangueGlitchTime(float time, float value)
    {
        ChangueGlitchValue(value);
        yield return new WaitForSeconds(time);
        ChangueGlitchValue(0);

    }

    public AnalogTV glitchTV;

    public void SetAnalogTV(float value)
    {
        if (value > 0)
            glitchTV.enabled = true;
        else
            glitchTV.enabled = false;
    }

    #endregion
}
