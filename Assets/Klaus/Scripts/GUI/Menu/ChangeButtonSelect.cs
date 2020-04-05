using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ChangeButtonSelect : MonoBehaviour
{

    public GameObject firstSelected;
    public EventSystem evenSys;

    public ButtonSelectMenu textToChangeColor;
    public SpecialButtonSelect specialTextToChangeColor;

    public Color newColor;
    protected Color currentColor;

    public string InputToBack;
    bool isSelected = false;

    public BackButtonMenu backController;
    public Action BackCallback;
    public void ResetSelectFirst()
    {
        evenSys.SetSelectedGameObject(gameObject);
        if (textToChangeColor) textToChangeColor.NormalText = currentColor;
        if (specialTextToChangeColor) specialTextToChangeColor.NormalText = currentColor;
        isSelected = false;
        backController.enabled = true;
    }

    public void SelectFirstButton()
    {

        evenSys.SetSelectedGameObject(firstSelected);

        if (textToChangeColor) currentColor = textToChangeColor.NormalText;
        if (textToChangeColor) textToChangeColor.NormalText = newColor;
        if (textToChangeColor) textToChangeColor.SetNormalColor();

        if (specialTextToChangeColor) currentColor = specialTextToChangeColor.NormalText;
        if (specialTextToChangeColor) specialTextToChangeColor.NormalText = newColor * ColorWorldManager.Instance.getColorScene();
        if (specialTextToChangeColor) specialTextToChangeColor.SetNormalColor();

        isSelected = true;
        backController.enabled = false;

    }

    void Update()
    {
     
        if (isSelected && backController.IsPressed())
        {
            if (BackCallback != null)
                BackCallback();
            ResetSelectFirst();
        }
        
    }
}
