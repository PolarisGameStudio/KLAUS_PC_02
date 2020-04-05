using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Colorful;

[RequireComponent(typeof(Animator))]
public class MenuMenuPanel : MonoBehaviour
{

    public BoxCollider2D[] buttonsCollision;


    public Animator animator
    {
        get
        {
            if (anim == null)
                anim = GetComponent<Animator>();
            return anim;
        }
    }

    public BackgroundSelect bgSelectItem
    {
        get
        {
            if (bg == null)
                bg = GetComponentInChildren<BackgroundSelect>();
            return bg;
        }
    }

    public Button[] buttons
    {
        get
        {
            if (btns == null)
            {
                btns = GetComponentsInChildren<Button>();
                System.Array.Sort<Button>(
                    btns,
                    delegate (Button x, Button y)
                    {
                        return x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex());
                    });
            }
            return btns;
        }
    }

    public MouseOver[] mousebuttons
    {
        get
        {
            if (mbtns == null)
            {
                mbtns = GetComponentsInChildren<MouseOver>();
                System.Array.Sort<MouseOver>(
                    mbtns,
                    delegate (MouseOver x, MouseOver y)
                    {
                        return x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex());
                    });
            }
            return mbtns;
        }

    }

        Animator anim;
    BackgroundSelect bg;
    Button[] btns;
    MouseOver[] mbtns;

    void Start()
    {
        GetComponentInChildren<BackButtonMenu>().Callback = BackMenu;
    }

    void BackMenu()
    {
        Reset();
        StartCoroutine(BackMenuCou());
    }
    IEnumerator BackMenuCou()
    {
        yield return null;
        bgSelectItem.Out();
        StartOut2();
    }

    public void Show()
    {
        GameObject.Find("AS_GlitchAmbient").GetComponent<AudioSource>().Stop();
        GameObject.Find("AS_GlitchAmbient2").GetComponent<AudioSource>().Stop();

        
        for(int i=0; i<buttonsCollision.Length; i++)
        {
            buttonsCollision[i].enabled = true;

        }

        //        back.enabled = true;

    }
    public void Hide()
    {
        for (int i = 0; i < buttonsCollision.Length; i++)
        {
            buttonsCollision[i].enabled = true;

        }

        //        back.enabled = false;
    }

    public void Reset()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Init()
    {
        bgSelectItem.Reset();
        Reset();
    }

    public void SelectFirstButton()
    {
        foreach (Button button in buttons)
            if (button.interactable)
            {
               //  Debug.Log("Seleccion");
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                return;
            }

        Reset();
    }

    public void ExitGame()
    {
        MonoBehaviour[] scripts = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        Application.Quit();
    }

    public void FixedUpdate()
    {
        if (InputEnum.GamePad == "keyboard")
        {
            foreach (MouseOver mousebutton in mousebuttons)
            if (mousebutton.selected)
            {
              //  Debug.Log("Seleccion");
                EventSystem.current.SetSelectedGameObject(mousebutton.gameObject);
                return;
            }
        }

    }

    public void StartOut2()
    {
        animator.SetTrigger("Out2");
    }

    public void StartOutArcade()
    {
        EventSystem.current.SetSelectedGameObject(null);
        animator.SetTrigger("Out3");
    }

    public void StartOut()
    {
        animator.SetTrigger("Out");
    }

    public void ChangueNextMenu()
    {
        ManagerMenuUI.Instance.ChangueToInMenu();
    }

    public void ChangueBackMenu()
    {
        ManagerMenuUI.Instance.ChangueToStartMenu();
    }

    public void ChangueArcadeMenu()
    {
        ManagerMenuUI.Instance.ChangueToArcadeMenu();
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

    #endregion
}
