using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceSpriteGamePad : MonoBehaviour {

    SpriteRenderer currentSprite;
    public Sprite spriteKeyboard;
    public Sprite spriteXbox;

    // Use this for initialization
    void Start () {

        currentSprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    // Update is called once per frame
    void Update()
    {
        if ((InputEnum.GamePad.ToString() == "xbox 360" || InputEnum.GamePad.ToString() == "wireless controller") && currentSprite.sprite!=spriteXbox)
        {
            currentSprite.sprite = spriteXbox;
        }

        if (InputEnum.GamePad.ToString() == "keyboard" && currentSprite.sprite != spriteKeyboard)
        {
            currentSprite.sprite = spriteKeyboard;
        }
    }
}
