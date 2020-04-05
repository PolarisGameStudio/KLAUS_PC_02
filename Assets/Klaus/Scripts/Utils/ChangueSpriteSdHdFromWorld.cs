using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]

public class ChangueSpriteSdHdFromWorld : MonoBehaviour
{
#if UNITY_EDITOR
    public Sprite[] SdWorld;
    public Sprite[] HdWorld;
#endif
    public bool isSpriteSheet = false;

    //public string[] pathSdWorld;
    public string[] pathHdWorld;
    // public string[] nameSdWorld;
    public string[] nameHdWorld;

    // Use this for initialization
    void Awake()
    {
        string nameLevel = Application.loadedLevelName.Substring(0, 1);
        int World = 0;
        if (nameLevel == "W")
        {

            string numberLevel = Application.loadedLevelName.Substring(1, 1);
            World = Convert.ToInt32(numberLevel) - 1;
            if (World == 5)
            {
                string newNumberLevel = Application.loadedLevelName.Substring(4, 1);
                int newWorld = Convert.ToInt32(newNumberLevel) - 1;
                World += newWorld;
            }
        }
        Sprite sprite = null;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        bool rendEnble = rend.enabled;
        rend.enabled = false;
        /*
#if UNITY_PSP2
        if (World >= pathSdWorld.Length || World < 0)
            World = 0;

        if (string.IsNullOrEmpty(pathSdWorld[World]) || string.IsNullOrEmpty(nameSdWorld[World]))
        {
            Debug.LogWarning("El path o el nombre de: " + gameObject + " es nulo");
            return;
        }
        if (isSpriteSheet)
        {
            Sprite[] textures = Resources.LoadAll<Sprite>(pathSdWorld[World]);
            string[] names = new string[textures.Length];

            for (int ii = 0; ii < names.Length; ii++)
            {
                names[ii] = textures[ii].name;
            }

            sprite = textures[System.Array.IndexOf(names, nameSdWorld[World])];
        }
        else
        {
            sprite = Resources.Load<Sprite>(pathSdWorld[World]);
        }
#else*/
        if (World >= pathHdWorld.Length || World < 0)
            World = 0;

        if (string.IsNullOrEmpty(pathHdWorld[World]) || string.IsNullOrEmpty(nameHdWorld[World]))
        {
            Debug.LogWarning("El path o el nombre de: " + gameObject + " es nulo");
            return;
        }
        if (isSpriteSheet)
        {
            Sprite[] textures = Resources.LoadAll<Sprite>(pathHdWorld[World]);
            string[] names = new string[textures.Length];

            for (int ii = 0; ii < names.Length; ii++)
            {
                names[ii] = textures[ii].name;
            }

            sprite = textures[System.Array.IndexOf(names, nameHdWorld[World])];
        }
        else
        {
            sprite = Resources.Load<Sprite>(pathHdWorld[World]);
        }
        //#endif
        if (sprite == null)
            Debug.LogWarning("El sprite: " + gameObject + " es nulo");

        rend.sprite = sprite;
        rend.enabled = rendEnble;
    }

}
