using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Assertions;

public class VideoPlayerScene : MonoBehaviour
{

    public string videoName = "";
    public string NextScene = "";

    public VideoPlayer m_videoPlayer;

    const string VideoPathResources = "Video";

    /// <summary>
    /// If need to wait for the localized key
    /// </summary>
    public bool WaitForKey = false;

    /// <summary>
    /// if use the loading screen after the video
    /// </summary>
    public bool useLoadingScene = true;

    void Start()
    {
        CameraFade.StartAlphaFade(Color.black, true, 0.0f);
        StartCoroutine("StartVideo");
    }

    public IEnumerator StartVideo()
    {
        while (LoadLevelManager.Instance.IsLoading)
        {
            yield return null;
        }

        while (WaitForKey)
            yield return null;


        var asyncRequest = Resources.LoadAsync<VideoClip>(Path.Combine(VideoPathResources, videoName));
        yield return asyncRequest;
        Assert.IsNotNull(asyncRequest.asset);

        if (!useLoadingScene)
            LoadLevelManager.Instance.LoadLevel(NextScene, true);


        var clip = asyncRequest.asset as VideoClip;
        m_videoPlayer.clip = clip;
        m_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        var audioSource = m_videoPlayer.gameObject.AddComponent<AudioSource>();
        m_videoPlayer.SetTargetAudioSource(0, audioSource);
        m_videoPlayer.Prepare();
        yield return null;
        while (!m_videoPlayer.isPrepared)
            yield return null;
        m_videoPlayer.Play();

        yield return null;

        while (m_videoPlayer.isPlaying)
        {
            yield return null;
        }


        if (!useLoadingScene)
            LoadLevelManager.Instance.ActivateLoadedLevel();
        else
            LoadLevelManager.Instance.LoadLevelWithLoadingScene(NextScene, false);
    }
}