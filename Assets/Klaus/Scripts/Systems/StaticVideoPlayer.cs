using UnityEngine;
using System.Collections;
using UnityEngine.Video;

#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class StaticVideoPlayer : MonoBehaviour
{

    public string videoName = "";
    string MoviePath = "StreamingAssets/Movies";
    string pathHD = "/HD/";

#if UNITY_STANDALONE || UNITY_EDITOR
    public VideoPlayer video;
    public Renderer rendererVideo;
#endif

#if  UNITY_PS4
    PS4VideoPlayer.Looping looping = PS4VideoPlayer.Looping.Continuous;
    PS4VideoPlayer video;
    UnityEngine.PS4.PS4ImageStream lumaTex;
    UnityEngine.PS4.PS4ImageStream chromaTex;
#endif

    public Renderer plane;
    bool isStart = false;

    void Start()
    {
        StartVideo();
    }

    public void StartVideo()
    {
        isStart = true;
#if  UNITY_PS4 && !UNITY_EDITOR
        video = new PS4VideoPlayer();

        video.PerformanceLevel = PS4VideoPlayer.Performance.Optimal;
        video.clearFrameOnTerminate = false;
        video.demuxVideoBufferSize = 8 * 1024 * 1024;		// defaults to 256k, or 1mb for Optimal
        video.videoDecoderType = PS4VideoPlayer.VideoDecoderType.SOFTWARE2;
        video.videoMemoryType = PS4VideoPlayer.MemoryType.WC_GARLIC;

        //		video.AudioThreadPriority = 500;

        lumaTex = new UnityEngine.PS4.PS4ImageStream();
        lumaTex.Create(1920, 1080, PS4ImageStream.Type.R8, 0);
        chromaTex = new UnityEngine.PS4.PS4ImageStream();
        chromaTex.Create(1920 / 2, 1080 / 2, PS4ImageStream.Type.R8G8, 0);
        video.Init(lumaTex, chromaTex);


        plane.material.mainTexture = lumaTex.GetTexture();
        plane.material.SetTexture("_CromaTex", chromaTex.GetTexture());

        video.Play(MoviePath + pathHD + videoName, looping);

#else


        video.url = MoviePath + pathHD + videoName;

        video.isLooping = false;
        video.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        video.targetMaterialRenderer = GetComponent<Renderer>();
        video.targetMaterialProperty = "_MainTex";

        video.Play();
#endif
    }
#if UNITY_PS4 && !UNITY_EDITOR

    void OnPreRender()
    {
        if (!isStart)
            return;
        video.Update();
    }
    void Update()
    {
        if (!isStart)
            return;
        if (plane != null)
        {
            int cropleft, cropright, croptop, cropbottom, width, height;
            video.GetVideoCropValues(out cropleft, out cropright, out croptop, out cropbottom, out width, out height);
            float scalex = 1.0f;
            float scaley = 1.0f;
            float offx = 0.0f;
            float offy = 0.0f;
            if ((width > 0) && (height > 0))
            {
                int fullwidth = width + cropleft + cropright;
                scalex = (float)width / (float)fullwidth;
                offx = (float)cropleft / (float)fullwidth;
                int fullheight = height + croptop + cropbottom;
                scaley = (float)height / (float)fullheight;
                offy = (float)croptop / (float)fullheight;
            }
            plane.material.SetVector("_MainTex_ST", new Vector4(scalex, scaley * -1, offx, 1 - offy)); // typically we want to invert the Y on the video because thats how planes UV's are layed out

        }
    }
#endif
    public void SetVolume(float volume)
    {
    }

    void OnMovieEvent(int eventID)
    {
#if UNITY_PS4 && !UNITY_EDITOR
       Debug.LogError("script has received FMV event " + eventID);
#endif
    }
}
