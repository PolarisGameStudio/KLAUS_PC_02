using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arcade_Active : MonoBehaviour
{
    public Button arcadeButton;
    public bool debugArcade = false;
    // Start is called before the first frame update
    void Start()
    {
        if (arcadeButton != null && ((SaveManager.Instance.dataKlaus.isArcadeModeUnlock) || debugArcade))
        {
            arcadeButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
