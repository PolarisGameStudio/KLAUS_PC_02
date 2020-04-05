using UnityEngine;
using System.Collections;

public class TilePlatformSet : MonoBehaviour
{

    [Range(1, 3)]
    public int Tile = 1;
    public Animator anim;
    public RuntimeAnimatorController[] animatorTiles = new RuntimeAnimatorController[3];

    public BoxCollider2D[] boxesTile;
    public float TileSizeX = 1;

    // Use this for initialization
    protected virtual void Awake()
    {
        anim.runtimeAnimatorController = animatorTiles[Tile - 1];
        for (int i = 0; i < boxesTile.Length; ++i)
        {
            boxesTile[i].size = new Vector2(TileSizeX * Tile, boxesTile[i].size.y);
        }
    }


}
