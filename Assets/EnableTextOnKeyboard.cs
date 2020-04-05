using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableTextOnKeyboard : MonoBehaviour
{
    public TextMeshPro text;
     float min = 0;
     float max = 0;
    // Start is called before the first frame update
    void Start()
    {
        min = text.fontSizeMin;
        max = text.fontSizeMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputEnum.GamePad.ToString() == "keyboard" && text.fontSizeMin == 0)
        {
            text.fontSizeMin = min;
            text.fontSizeMax = max;
        }

        if (InputEnum.GamePad.ToString() != "keyboard" && text.fontSizeMin != 0)
        {
            text.fontSizeMin = 0;
            text.fontSizeMax = 0;
        }
    }
}
