using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSpriteInAnimation : MonoBehaviour
{
    private SpriteRenderer _renderer;

    public SpriteRenderer renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    public string[] Key;
    public string[] Value;
    Dictionary<string,Sprite> SpritesChange = new Dictionary<string, Sprite>();

    public string Path = "/Sprites/Klaus/KlausSpriteSheet";

    public bool isActive = false;
    // Use this for initialization
    void Awake()
    {
        string formatH = "HD";
        #if UNITY_PSP2 && !(UNITY_EDITOR)
        formatH = "SD";
        #endif
        Sprite[] textures = Resources.LoadAll<Sprite>(formatH + Path + formatH);
        string[] names = new string[textures.Length];

        for (int i = 0; i < Key.Length; ++i)
        {
            for (int ii = 0; ii < textures.Length; ii++)
            {
                if (textures [ii].name == Value [i])
                {
                    SpritesChange.Add(Key [i], textures [ii]);
                    break;
                }
            }
        }
    }
	
    // Update is called once per frame
    void LateUpdate()
    {
	
        if (isActive && SpritesChange.ContainsKey(renderer.sprite.name))
        {
            renderer.sprite = SpritesChange [renderer.sprite.name];
        }


    }
}
