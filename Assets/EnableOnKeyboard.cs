using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnKeyboard : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputEnum.GamePad.ToString() == "keyboard" && !sprite.enabled)
        {
            sprite.enabled = true;
            text.SetActive(true);
        }

        if(InputEnum.GamePad.ToString() != "keyboard" && sprite.enabled)
        {
            sprite.enabled = false;
            text.SetActive(false);
        }
    }
}
