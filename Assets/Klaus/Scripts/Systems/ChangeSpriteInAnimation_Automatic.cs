using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeSpriteInAnimation_Automatic : MonoBehaviour
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

    Dictionary<string, Sprite> SpritesChange = new Dictionary<string, Sprite>();

    public string Path = "/Sprites/Klaus/KLAUSBLANCO";

    public bool isActive = false;
    // Use this for initialization
    void Start()
    {
        if (isActive)
        {
            string formatH = "HD";
#if UNITY_PSP2 && !(UNITY_EDITOR)
        formatH = "SD";
#endif
            Sprite[] textures = Resources.LoadAll<Sprite>(formatH + Path + formatH);
            string[] names = new string[textures.Length];

            for (int ii = 0; ii < textures.Length; ii++)
            {
                SpritesChange.Add(textures[ii].name, textures[ii]);

            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (isActive && SpritesChange.ContainsKey(renderer.sprite.name))
        {
            renderer.sprite = SpritesChange[renderer.sprite.name];
        }
    }
}
