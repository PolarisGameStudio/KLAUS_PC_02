using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DeadState))]
public class Glitch_SpawnDead : MonoBehaviour
{

    public GlitchDeadTimeReset prefabToSpawn;
    protected DeadState dead;
    public float TimeOfDead = -1.0f;
    // Use this for initialization
    void Awake()
    {
        prefabToSpawn.CreatePool(15);
        dead = GetComponent<DeadState>();
    }

    void OnEnable()
    {
        dead.onRespawn += SpawnPrefab;
    }

    void OnDisable()
    {
        dead.onRespawn -= SpawnPrefab;
    }

    public void SpawnPrefab(Vector3 pos)
    {
        GlitchDeadTimeReset spawn =  prefabToSpawn.Spawn(pos);
        spawn.DeadTime = TimeOfDead;
    }

    public void CleanSpawn()
    {
        prefabToSpawn.RecycleAll();
    }
}
