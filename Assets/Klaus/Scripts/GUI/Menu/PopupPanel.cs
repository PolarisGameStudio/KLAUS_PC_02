using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SmartLocalization.Editor;
using System.IO;
using Steamworks;
using Luminosity.IO;

public class PopupPanel : MonoBehaviour
{

    public CanvasGroup canvas;
    public EventSystem evenSys;
    protected GameObject CurrentSelect;
    public LocalizedText Title;
    protected PopupOption popup;
    public bool ControlMenu = false;
    public BoxCollider2D[] boxes;
    public Set_Button_Text_TUT[] tutoriales;
    public GameObject optionObject;


    public BackButtonMenu back;

    void Start()
    {
        back.Callback = BackMenu;
    }

    void BackMenu()
    {
        popup.Hide();

    }

    public void Show(string text, PopupOption pop)
    {

        if (canvas.alpha <= 0)
        {

            popup = pop;

            Title.UpdateKey(text);

            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            canvas.interactable = true;
            CurrentSelect = evenSys.currentSelectedGameObject;
            evenSys.SetSelectedGameObject(Title.gameObject);
            back.enabled = true;
        }
    }

    public void Hide()
    {
        if(ControlMenu)
        {
            

            if(boxes!=null)
            { 
                for(int i=0; i<boxes.Length;i++)
                {

                    boxes[i].enabled = false;
                }
            }

            Debug.Log("Trato de Salvar");
            /*/string saveFolder = PathUtility.GetInputSaveFolder(0);
            if (!System.IO.Directory.Exists(saveFolder))
                System.IO.Directory.CreateDirectory(saveFolder);


            InputSaverXML saver = new InputSaverXML(saveFolder + "/input_config.xml");
            InputManager.Save(saver);/*/

            if(tutoriales!=null)
            {
                for(int i=0; i<tutoriales.Length; i++)
                {
                    tutoriales[i].UpdateInputKey();

                }
            }

            if (optionObject != null)
            {
                optionObject.SetActive(true);
            }
         

        }

        evenSys.SetSelectedGameObject(CurrentSelect);
        CurrentSelect = null;

        popup = null;
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        back.enabled = false;
    }

}
