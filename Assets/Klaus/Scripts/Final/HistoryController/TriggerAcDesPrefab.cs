using UnityEngine;
using System.Collections;

public class TriggerAcDesPrefab : TriggerHistory
{
    public GameObject[] Prefabs;

    public bool useRandom = false;

    public Vector2 minMaxTimeToActive = new Vector2(0.3f, 1.0f);
    public Vector2 minMaxTimeToDeactive = new Vector2(0.3f, 1.0f);


    protected override void OnEnterAction(Collider2D other)
    {
        base.OnEnterAction(other);
        if (useRandom)
        {
            for (int i = 0; i < Prefabs.Length; ++i)
            {
                ActiveLogic(Prefabs [i], true);
            }
        } else
        {
            for (int i = 0; i < Prefabs.Length; ++i)
            {
                ActiveLogic(Prefabs [i], false);
            }
        }
    }

    IEnumerator TimeToLogicActive(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        for (int i = 0; i < Prefabs.Length; ++i)
        {
            ActiveLogic(Prefabs [i], true);
        }
    }

    void ActiveLogic(GameObject Prefab, bool useCoroutine)
    {
        if (Prefab.activeSelf)
        {
            Prefab.SetActive(false);
            if (useCoroutine)
            {
                StartCoroutine("TimeToLogicActive", Random.Range(minMaxTimeToActive.x, minMaxTimeToActive.y));
            }
        } else
        {
            Prefab.SetActive(true);
            if (useCoroutine)
            {
                StartCoroutine("TimeToLogicActive", Random.Range(minMaxTimeToDeactive.x, minMaxTimeToDeactive.y));
            }
        }
    }
}
