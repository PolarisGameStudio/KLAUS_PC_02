using UnityEngine;
using System.Collections;
using Luminosity.IO;
using Rewired;

public class PressButtonToSkipScene : MonoBehaviour
{
    [SerializeField]
    InputActionOld ButtonToPres = InputActionOld.UI_Submit;

    public string NextScene = "MenuPrincipal";
    bool isLoading = false;
    public LoadScene loadScene;
    // Update is called once per frame
    void Update()
    {

        if (!isLoading && ReInput.players.GetPlayer(0).GetButtonDown(InputEnum.GetInputString(ButtonToPres)))
        {
            if (loadScene != null)
                loadScene.enabled = false;
            isLoading = true;
            LoadLevelManager.Instance.LoadLevel(NextScene, false);
        }
    }
}
