using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using Luminosity.IO;
using Rewired;

[RequireComponent(typeof(Animator))]
public class StartMenuPanel : MonoBehaviour
{
    public GameObject FirstSelected;
    public EventSystem eventSystem;
    private bool played = false;
    private Animator anim;
    public CanvasGroup currentCanvas;
    InputActionOld InputSubmit = InputActionOld.UI_Submit;

    public Animator animator
    {
        get
        {
            if (anim == null)
                anim = GetComponent<Animator>();

            return anim;
        }
    }

    void Start()
    {
#if UNITY_PS4 && !UNITY_EDITOR
        SonyManager.Instance.SignIn();
#endif
    }

    public void ResetButton()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    public void SelectFirst()
    {
        eventSystem.SetSelectedGameObject(FirstSelected);
    }

    public void StartOut()
    {

       

            Debug.Log("Enter game");
        animator.SetTrigger("Out");
        if (!played)
        {
            GetComponent<AudioSource>().Play();
            played = false;
        }

    }

    public void ChangueNextMenu()
    {
        ManagerMenuUI.Instance.ChangueToMenuMenu();
    }

    public void Update()
    {
        if(currentCanvas.interactable && ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputSubmit)))
        {

            StartOut();
        }
        
    }
}
