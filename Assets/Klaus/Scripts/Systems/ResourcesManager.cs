using UnityEngine;
using System.Collections;

public class ResourcesManager : PersistentSingleton<ResourcesManager>
{
    bool resetAll = false;
    void Start()
    {
        StartCoroutine(ResetAssets());
        DontDestroyOnLoad(this);
    }
    public IEnumerator ResetAssets()
    {
        var unloader = Resources.UnloadUnusedAssets();

        while (!unloader.isDone)
        {
            yield return null;
        }
        System.GC.Collect();

    }
    // This function is called after a new level was loaded
    public void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Loading")
            resetAll = true;
    }
    void LateUpdate()
    {
        if (resetAll)
        {
            resetAll = false;
            StartCoroutine(ResetAssets());
        }
    }

}
