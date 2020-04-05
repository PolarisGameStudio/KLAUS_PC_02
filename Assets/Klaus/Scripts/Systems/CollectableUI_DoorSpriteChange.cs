using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollectableUI_DoorSpriteChange : MonoBehaviour
{

    public Image imageSprite;
    const string NameFile = "Doors0";
    const string PathResources = "HD/Sprites/Collectables/";
    public Dictionary<string, Sprite> spritesDoor = new Dictionary<string, Sprite>();
    int World;
    char[] chars;

    // Use this for initialization
    void Start()
    {
        string nameLevel = Application.loadedLevelName.Substring(0, 1);
        World = 0;
        if (nameLevel == "W")
        {

            string numberLevel = Application.loadedLevelName.Substring(1, 1);
            World = Convert.ToInt32(numberLevel);
        }
        if (World > 1)
        {

            Sprite[] textures = Resources.LoadAll<Sprite>(PathResources + NameFile + World);

            for (int i = 0; i < textures.Length; ++i)
            {
                spritesDoor.Add(textures[i].name, textures[i]);
            }

        }

    }

    public void LateUpdate()
    {
        if (World > 1)
        {
            chars = imageSprite.sprite.name.ToCharArray();
            chars[7] = Convert.ToChar(World+48);
            string key = new string(chars);
            if (spritesDoor.ContainsKey(key) && imageSprite.sprite.name != key)
                imageSprite.sprite = spritesDoor[key];
        }
    }


}
