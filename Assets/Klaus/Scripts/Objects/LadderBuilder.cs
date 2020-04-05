using UnityEngine;
using System.Collections;

public class LadderBuilder : MonoBehaviour
{

    public float yDistance = 0.3f;
    public float yOffSetBaseTrigger = 0.1f;
    public Transform prefabLadder;
    public int NumberLadder = 1;

    public BoxCollider2D Trigger;

    public float fixValue15 = 0.3f;
    void Awake()
    {
        Transform baseLadder = prefabLadder.Spawn();
        baseLadder.parent = transform;
        baseLadder.localPosition = Vector3.zero;

        for (int i = 1; i < NumberLadder; ++i)
        {
            Transform newLadder = prefabLadder.Spawn();
            newLadder.parent = baseLadder;
            newLadder.localPosition = Vector3.up * -1 * 0.3f;
            baseLadder = newLadder;
        }
        float fixValue = 0.1f;
        if (NumberLadder < 10)
        {
            fixValue = 0.1f;
        }
        else if (NumberLadder < 15)
        {
            fixValue = 0.2f;
        }
        else
        {
            fixValue = fixValue15;
        }
        Trigger.transform.localPosition = new Vector3(0, -1 * yOffSetBaseTrigger * (NumberLadder - 1) + fixValue, 0);
        Trigger.size = new Vector2(Trigger.size.x, yDistance * NumberLadder + 0.2f);
        //   Trigger.transform.localPosition = new Vector3(0, -1 * yOffSetBaseTrigger * (NumberLadder - 1) + 0.1f, 0);
        //   Trigger.size = new Vector2(Trigger.size.x, yDistance*NumberLadder+0.1f);
    }
}
