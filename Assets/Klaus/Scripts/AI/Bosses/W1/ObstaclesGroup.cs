using System.Collections;
using UnityEngine;

public class ObstaclesGroup : MonoBehaviour
{
    public ObstaclesDestroyer[] obstacles;
    public float delay;
    public float minGravity = 0.8f;
    public float maxGravity = 2f;

    public BossW1Controller boss {
        get {
            if (_boss == null)
                _boss = GameObject.FindObjectOfType<BossW1Controller>();
            return _boss;
        }
    }

    BossW1Controller _boss;

    void OnEnable()
    {
        StopAllCoroutines();

        foreach (ObstaclesDestroyer obstacle in obstacles)
            obstacle.gameObject.SetActive(false);
    }

    IEnumerator ThrowObstacles()
    {
        yield return null;

        foreach (ObstaclesDestroyer obstacle in obstacles)
        {
            obstacle.gameObject.SetActive(true);

            // Set falling speed from sky
            if (obstacle.cachedRigidbody != null) obstacle.cachedRigidbody.gravityScale = boss != null && boss.obstaclesFallSpeed != 0 ? boss.obstaclesFallSpeed : minGravity;

            yield return StartCoroutine(new TimeCallBacks().WaitPause(delay));
        }

        StartCoroutine("CheckForRecycling");
    }

    public void TurnOnObstacles()
    {
        StopCoroutine("CheckForRecycling");
        StartCoroutine("ThrowObstacles");
    }

    public void ToggleObjects(bool value)
    {
        foreach (ObstaclesDestroyer obstacle in obstacles)
        {
            // The object falls faster when falling off the platform
            if (!value && obstacle.cachedRigidbody != null) obstacle.cachedRigidbody.gravityScale = maxGravity;
            obstacle.Toggle(value);
        }
    }

    IEnumerator CheckForRecycling()
    {
        while (true)
        {
            bool someoneEnabled = false;
            foreach (ObstaclesDestroyer obstacle in obstacles)
                if (obstacle.gameObject.activeSelf)
                {
                    someoneEnabled = true;
                    break;
                }

            if (!someoneEnabled)
            {
                this.Recycle<ObstaclesGroup>();
                yield break;
            }

            yield return null;
        }
    }
}
