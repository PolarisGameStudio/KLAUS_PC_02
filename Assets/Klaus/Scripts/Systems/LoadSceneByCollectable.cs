using UnityEngine;
using System.Collections;

public class LoadSceneByCollectable : MonoBehaviour
{

    public VideoPlayerScene videoPlayer;
    // Use this for initialization
    void Awake()
    {

        if (!string.IsNullOrEmpty(SaveManager.Instance.LevelToLoadVideoCollectable))
            videoPlayer.NextScene = SaveManager.Instance.LevelToLoadVideoCollectable;
        else
            Debug.LogError("Error: Video not load collectables");
    }
}
