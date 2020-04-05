using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Arcade_ItemPanel : MonoBehaviour
{

    public MouseOver[] mbtns;
    public Collider2D[] Colmbtns;
    public bool[] currentMbts;
    public CanvasGroup canvasGroup;

    public ArcadeMenuPanel menuPanel
    {
        get
        {
            if (menu == null)
                menu = GetComponentInParent<ArcadeMenuPanel>();
            return menu;
        }
    }

    public CanvasGroup canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GetComponent<CanvasGroup>();
            return _canvas;
        }
    }

    public BackButtonMenu back
    {
        get
        {
            if (_back == null)
                _back = GetComponent<BackButtonMenu>();
            return _back;
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

    public ArcadeLevelSelector levelSelector
    {
        get
        {
            if (selector == null)
                selector = GetComponentInChildren<ArcadeLevelSelector>();
            return selector;
        }
    }

    ArcadeLevelSelector selector;
    ArcadeMenuPanel menu;
    CanvasGroup _canvas;
    BackButtonMenu _back;
    Button[] btns;

    

    void Start()
    {
        back.Callback = StartOut;

        currentMbts = new bool[mbtns.Length];

        for (int i = 0; i < currentMbts.Length; i++)
        {
            currentMbts[i] = false;
        }

        
    }

    public void ResetSelectFirst()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectFirstButton()
    {
        foreach (Button button in buttons)
            if (button.interactable)
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                return;
            }

        ResetSelectFirst();
    }

    public void Show()
    {
        ResetSelectFirst();
        canvas.alpha = 1;
        ToggleInteractions(true);


        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = true;
        }

        SelectFirstButton();
     
    }

    public void Hide()
    {
        canvas.alpha = 0;

        for (int i = 0; i < Colmbtns.Length; i++)
        {
            Colmbtns[i].enabled = false;
        }
        ToggleInteractions(false);

     
    }

    public void returntoMain()
    {
        _back.ActivateBack();
    }

    public void ToggleInteractions(bool value)
    {
        canvas.interactable = value;
        back.enabled = value;
    }

    void StartOut()
    {
        menuPanel.StartOut();
    }


    public void Update()
    {

        if (InputEnum.GamePad == "keyboard" && canvasGroup.interactable && canvasGroup.alpha == 1 && mbtns != null)
        {
            for (int i = 0; i < mbtns.Length; i++)
            {

                if (mbtns[i].selected && !currentMbts[i])
                {
                    EventSystem.current.SetSelectedGameObject(mbtns[i].gameObject);
                    currentMbts[i] = true;
                }
                if (!mbtns[i].selected && currentMbts[i])
                {
                    currentMbts[i] = false;
                }


            }

        }
    }

}
