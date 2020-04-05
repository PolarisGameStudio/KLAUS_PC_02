using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PopupOption : MonoBehaviour
{
    public string titleKey;
    public PopupPanel parentPanel;
    public CanvasGroup canvas;
    public BackButtonMenu back;
    public CanvasGroup optionCanvas;
    public  BoxCollider2D[] colisionsButtons;
    public InGameSubPanel panelOption;

    public MemoriesPanel memoriesPanel;
    GameObject lastSelected;

    public virtual void Show()
    {
        back.enabled = false;

        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;

        parentPanel.Show(titleKey, this);

        if (memoriesPanel != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            memoriesPanel.OnSelectMemory();
        }
    }

    public virtual void Hide()
    {
        back.enabled = true;

        if(colisionsButtons!=null)
        {
            for(int i=0; i<colisionsButtons.Length; i++)
            {
                colisionsButtons[i].enabled = true;

            }
            
        }


        //NO ES AQUI ES EN EL OTRO

            Debug.Log("Selecciono el primero");

        

        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;

        if (memoriesPanel != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        parentPanel.Hide();
        if (optionCanvas != null)
        {
            optionCanvas.interactable = true;
            EventSystem.current.SetSelectedGameObject(lastSelected);
            panelOption.Show();
        }
    }
}
