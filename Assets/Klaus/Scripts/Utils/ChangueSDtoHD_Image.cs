using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[RequireComponent(typeof(Image))]

public class ChangueSDtoHD_Image : MonoBehaviour
{
    #if UNITY_EDITOR
    public Sprite SD = null;
    public Sprite HD = null;
    #endif
    
    public bool isSpriteSheet = true;
    /*
    public string pathSD;
    public string pathHD;
    public string nameSD;
    public string nameHD;
    // Use this for initialization
    void Awake()
    {

        Sprite sprite = null;
        Image rend = GetComponent<Image>();
        bool rendEnble = rend.enabled;
        rend.enabled = false;
        #if UNITY_PSP2 
        if (string.IsNullOrEmpty(pathSD) || string.IsNullOrEmpty(nameSD))
        {
        Debug.LogWarning("El path o el nombre de: " + gameObject + " es nulo");
        return;

        }
        if (isSpriteSheet) {
        Sprite[] textures = Resources.LoadAll<Sprite>(pathSD);
        string[] names = new string[textures.Length];

        for (int ii = 0; ii < names.Length; ii++)
        {
        names[ii] = textures[ii].name;
        }
        int pos = System.Array.IndexOf(names, nameSD);
        sprite = textures[pos];
        }
        else
        {
        sprite = Resources.Load<Sprite>(pathSD);
        }
        #else
        if (string.IsNullOrEmpty(pathHD) || string.IsNullOrEmpty(nameHD))
        {
            Debug.LogWarning("El path o el nombre de: " + gameObject + " es nulo");
            return;
        }
        if (isSpriteSheet)
        {
            Sprite[] textures = Resources.LoadAll<Sprite>(pathHD);
            string[] names = new string[textures.Length];

            for (int ii = 0; ii < names.Length; ii++)
            {
                names [ii] = textures [ii].name;
            }

            sprite = textures [System.Array.IndexOf(names, nameHD)];
        } else
        {
            sprite = Resources.Load<Sprite>(pathHD);
        }

        #endif
        if (sprite == null)
            Debug.LogWarning("El sprite: " + gameObject + " es nulo");

        rend.sprite = sprite;
        rend.enabled = rendEnble;
    }*/

}
