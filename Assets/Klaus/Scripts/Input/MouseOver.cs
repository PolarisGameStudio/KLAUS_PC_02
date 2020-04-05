using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour
{
  //  public string name = "PEPE";
    public bool selected = false;
    public bool clicked = false;
    public Slider slider;
    public float sliderValue=0f;
    public bool dropdown = false;


    void OnMouseEnter()
    {
    //    name = "manuel";
        selected = true;
        if (slider != null)
        {
            sliderValue = slider.value;
        }
    }

    void OnMouseExit()
    {
        clicked = false;
        selected = false;
        if (slider!=null)
        {
            slider.enabled = false;
            slider.enabled = true;
        }
    }

    void OnMouseDown()
    {
        clicked = true;

        if (GetComponent<Button>()!=null)
        { 
            GetComponent<Button>().onClick.Invoke();
        }

        else if (GetComponent<Toggle>()!=null)
        {
            if (GetComponent<Toggle>().isOn)
                GetComponent<Toggle>().isOn = false;
            else
                GetComponent<Toggle>().isOn = true;
        }
        
        clicked = true;
        selected = false;
        if (slider != null)
        {
            slider.enabled = false;
            slider.enabled = true;
        }
        
    }

    public void Update()
    {
        if(clicked)
        {
            
            if (slider != null)
            {
                slider.enabled = false;
                slider.enabled = true;
            }

            if (slider != null)
            {
                    if (sliderValue != slider.value)
                {
                    slider.enabled = false;
                    slider.enabled = true;
                }
            }

        }

        
    }
}
