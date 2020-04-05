using UnityEngine;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    /// <summary>
    /// Whether this object should persist between scenes
    /// </summary>
    public bool persistantBetweenScenes;

    /// <summary>
    /// The instance.
    /// </summary>
    private static T _instance = null;

    private static bool applicationIsQuitting;

    public static bool IsInstanceNull()
    {
        return _instance == null;
    }


    /// <summary>
    /// Checks whether an instance exists or not.
    /// </summary>
    /// <returns><c>true</c>, if this was instanced, <c>false</c> otherwise.</returns>
    public static bool InstanceExists()
    {
        return _instance != null;
    }

    /// <summary>
    /// Lazy instantiation of this behaviour.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            // If there are no instances
            if (_instance == null && !applicationIsQuitting)
            {
                // Try to find any instance in the game.
                T[] instances = GameObject.FindObjectsOfType<T>();

                // If there are no instances, create one.
                if (instances.Length == 0)
                {
                    Type type = typeof(T);
                    string typename = type.ToString();

                    Debug.LogWarning("No instance of " + typename + ", creating a temporary one.");
                    _instance = new GameObject("Temp_" + typename, type).GetComponent<T>();

                    // If the instance is still null, there was a fatal error.
                    if (_instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typename);
                        return null;
                    }
                }
                else
                {
                    _instance = instances[0];

                    // Destroy all other remaining instances
                    for (int i = 1; i != instances.Length; ++i)
                        DestroyImmediate(instances[i].gameObject);
                }

                // Initialize it
                _instance.Init();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
        // If there isn't an instance, pick us
        if (_instance == null)
        {
            _instance = this as T;
            _instance.Init();
        }
        // If there is an instance (and it isn't us), destroy us
        else if (_instance != null && _instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    /// <summary>
    /// Inits this instance.
    /// </summary>
    /// <remarks>
    /// Please use this instead of Awake.
    /// </remarks>
    protected virtual void Init()
    {
        if (persistantBetweenScenes)
            DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Raises the application quit event.
    /// </summary>
    private void OnApplicationQuit()
    {
        _instance = null;
        applicationIsQuitting = true;
    }

}
