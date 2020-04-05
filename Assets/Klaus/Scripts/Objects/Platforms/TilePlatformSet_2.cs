using UnityEngine;
using System.Collections;

public class TilePlatformSet_2 : TilePlatformSet
{
    public Transform[] transformSizeTile;
    public float SizeBase = 0.5f;
    public float SizeUp = 0.25f;

    public Transform[] transformOffSetXTile;
    public float offsetBaseX = 0.1f;
    public float offsetUpX = 0.25f;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < transformSizeTile.Length; ++i)
        {
            transformSizeTile[i].localScale = new Vector3(1, 1, 1) * (SizeBase + (Tile - 1) * SizeUp);
        }
        for (int i = 0; i < transformOffSetXTile.Length; ++i)
        {
            int positivo = 1;
            if (transformOffSetXTile[i].localPosition.x < 0)
            {
                positivo = -1;
            }

            transformOffSetXTile[i].localPosition = new Vector3(positivo * (offsetBaseX + ((Tile - 1) * offsetUpX)), transformOffSetXTile[i].localPosition.y, transformOffSetXTile[i].localPosition.z);

            if (Tile == 1)
            {
                transformOffSetXTile[i].gameObject.SetActive(false);
            }
        }
    }
}