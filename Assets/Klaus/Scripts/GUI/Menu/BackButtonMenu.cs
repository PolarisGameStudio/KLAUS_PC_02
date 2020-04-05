using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.IO;
using Steamworks;
using Luminosity.IO;
using Rewired;

public class BackButtonMenu : MonoBehaviour
{
    public Action Callback;
    [SerializeField]
    InputActionOld InputBack = InputActionOld.UI_Cancel;
    public Button buttonInteractive;
    public GameObject firstOption;
    public Button[] buttonsInteractive;
    public Slider[] slidersInteractive;
    public Toggle[] togglesInteractive;
    public Dropdown[] dropdownHide;
    public EventSystem evenSys;
    public CanvasGroup canvasGroup;
    public bool isMapping = false;



    public List<InputActionOld> OtherInput = new List<InputActionOld>();

    public AudioSource audioSource
    {
        get
        {
            if (_audioSource == null)
                _audioSource = GameObject.Find("AS_ButtonBack").GetComponent<AudioSource>();
            return _audioSource;
        }
    }

    AudioSource _audioSource;

    public void activateMapping()
    {
        isMapping = true;
    }

    public void deactivateMapping()
    {
        isMapping = false;
    }
    void Update()
    {
        if (!isMapping)
        {
            if (buttonInteractive != null)
            {
                if (IsPressed() && buttonInteractive.interactable)
                {

                    if (!object.ReferenceEquals(Callback, null))
                        Callback();
                    audioSource.Play();


                }

                if (dropdownHide != null)
                {
                    if (IsPressed())
                    {
                        Debug.Log("BACK FROM CONTROL");
                        if (canvasGroup != null)
                        {
                            canvasGroup.interactable = true;
                        }

                        if (dropdownHide != null)
                        {
                            for (int i = 0; i < dropdownHide.Length; i++)
                            {
                                if(dropdownHide[i]!=null)
                                { 
                                    dropdownHide[i].Hide();
                                    dropdownHide[i].interactable = false;
                                }
                            }
                        }

                        for (int i = 0; i < buttonsInteractive.Length; i++)
                        {
                            if (!buttonsInteractive[i].interactable)
                                buttonsInteractive[i].interactable = true;
                        }

                        for (int i = 0; i < slidersInteractive.Length; i++)
                        {
                            if (!slidersInteractive[i].interactable)
                                slidersInteractive[i].interactable = true;
                        }

                        for (int i = 0; i < togglesInteractive.Length; i++)
                        {
                            if (!togglesInteractive[i].interactable)
                                togglesInteractive[i].interactable = true;
                        }

                        evenSys.SetSelectedGameObject(firstOption);

                    }
                }
            }

            else
            {
                if (IsPressed())
                {

                    if (!object.ReferenceEquals(Callback, null))
                        Callback();
                    audioSource.Play();

                }
            }
        }
  
    }

    public bool IsPressed()
    {
        var value = ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(InputBack));



        if (OtherInput.Count > 0)
        {
            foreach (var input in OtherInput)
            {
                value = value || ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(input));

            }

        }

        return value;
    }

    public void ActivateBack()
    {
        Debug.Log("BACK ACTIVATED");
        if (buttonInteractive != null)
        {
            if (buttonInteractive.interactable)
            {

                if (!object.ReferenceEquals(Callback, null))
                    Callback();
                audioSource.Play();
            }

            if (dropdownHide != null)
            {

                    Debug.Log("BACK FROM CONTROL");
                if (canvasGroup != null)
                {
                    canvasGroup.interactable = true;
                }
                for (int i = 0; i < dropdownHide.Length; i++)
                    {
                        dropdownHide[i].Hide();
                        dropdownHide[i].interactable = false;
                    }

                    for (int i = 0; i < buttonsInteractive.Length; i++)
                    {
                        if (!buttonsInteractive[i].interactable)
                            buttonsInteractive[i].interactable = true;
                    }

                    for (int i = 0; i < slidersInteractive.Length; i++)
                    {
                        if (!slidersInteractive[i].interactable)
                            slidersInteractive[i].interactable = true;
                    }

                    for (int i = 0; i < togglesInteractive.Length; i++)
                    {
                        if (!togglesInteractive[i].interactable)
                            togglesInteractive[i].interactable = true;
                    }

                    evenSys.SetSelectedGameObject(firstOption);

                
            }
        }

        else
        {
                if (!object.ReferenceEquals(Callback, null))
                    Callback();
                audioSource.Play();
        }

    }



}
