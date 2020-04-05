using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleSelection : MonoBehaviour {

    public Button[] buttons;
    public Slider[] sliders;
    public Toggle[] toggles;
    public Button button;
    public Dropdown dropdown;
    // Use this for initialization
    void Start () {
       
    }

    void DeactivateButtons()
    {
        for(int i=0; i<buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].interactable = false;
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].interactable = false;
        }

    }
	
	// Update is called once per frame
	void Update () {

		if(!buttons[0].interactable && !dropdown.IsActive())
        {
            for (int i = 0; i < buttons.Length; i++)
            {
               buttons[i].interactable = true;
            }

            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].interactable = true;
            }

            for (int i = 0; i < toggles.Length; i++)
            {
                toggles[i].interactable = true;
            }

        }
       

	}
}
