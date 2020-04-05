using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

#if  UNITY_PS4
using UnityEngine.PS4;
#endif
public class CollectableVideo : MonoBehaviour
{

    public string videoName = "";
    const string MoviePath = "StreamingAssets/Movies";
    public RawImage planePS4;


#if UNITY_STANDALONE || UNITY_EDITOR
    public VideoPlayer video;
#endif
    public RawImage rendererVideo;

    const string pathHD = "/HD/";
#if  UNITY_PS4 && !UNITY_EDITOR
    PS4VideoPlayer videoPlayerPs4;
    UnityEngine.PS4.PS4ImageStream lumaTex;
    UnityEngine.PS4.PS4ImageStream chromaTex;
    PS4VideoPlayer.Looping looping = PS4VideoPlayer.Looping.None;
#endif
    bool isStart = false;

    public bool isDebug = true;
    public float timeDebug = 1.0f;

    public Action VideoEnding;
    public Vector3 RelativePos = new Vector3(0, 0, 1);

    void Awake()
    {

        GameObject hud = GameObject.Find("HUD_Canvas");

        planePS4.transform.parent = hud.transform;
        planePS4.transform.localPosition = Vector3.zero;
        planePS4.transform.SetAsLastSibling();

        rendererVideo.transform.parent = hud.transform;
        rendererVideo.transform.localPosition = Vector3.zero;
        rendererVideo.transform.SetAsLastSibling();


    }
    void Start()
    {
        /*/
        planePS4.transform.localScale = new Vector3(1, 1, 1);
        planePS4.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        planePS4.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        rendererVideo.transform.localScale = new Vector3(1, 1, 1);
        rendererVideo.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        rendererVideo.GetComponent<RectTransform>().offsetMin = Vector2.zero;



        planePS4.gameObject.SetActive(false);
        rendererVideo.gameObject.SetActive(false);
        /*/
    }

    public void StartVideo()
    {

        if (isStart)
            return;


#if UNITY_PS4 && !UNITY_EDITOR
        planePS4.gameObject.SetActive(true);

        videoPlayerPs4 = new PS4VideoPlayer();

        videoPlayerPs4.PerformanceLevel = PS4VideoPlayer.Performance.Optimal;
        videoPlayerPs4.clearFrameOnTerminate = false;
        videoPlayerPs4.demuxVideoBufferSize = 8 * 1024 * 1024;       // defaults to 256k, or 1mb for Optimal
        videoPlayerPs4.videoDecoderType = PS4VideoPlayer.VideoDecoderType.SOFTWARE2;
        videoPlayerPs4.videoMemoryType = PS4VideoPlayer.MemoryType.WC_GARLIC;

        //      video.AudioThreadPriority = 500;

        lumaTex = new UnityEngine.PS4.PS4ImageStream();
        lumaTex.Create(1920, 1080, PS4ImageStream.Type.R8, 0);
        chromaTex = new UnityEngine.PS4.PS4ImageStream();
        chromaTex.Create(1920 / 2, 1080 / 2, PS4ImageStream.Type.R8G8, 0);
        videoPlayerPs4.Init(lumaTex, chromaTex);

        planePS4.material.mainTexture = lumaTex.GetTexture();
        planePS4.material.SetTexture("_CromaTex", chromaTex.GetTexture());

        videoPlayerPs4.Play(MoviePath + pathHD + videoName, looping);

#else

        video.url = MoviePath + pathHD + videoName;

        video.isLooping = false;
        video.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        video.targetMaterialRenderer = GetComponent<Renderer>();
        video.targetMaterialProperty = "_MainTex";

        video.Play();
#endif

        if (isDebug)
        {
            StartCoroutine("FinishVideo", timeDebug);
        }
        isStart = true;
    }
#if UNITY_PS4 && !UNITY_EDITOR

    void OnPreRender()
    {
        if (isStart)
        {
            videoPlayerPs4.Update();
        }
    }
    void Update()
    {
        if (isStart)
        {

            int cropleft, cropright, croptop, cropbottom, width, height;
            cropleft = 0;
            cropright = 0;
            cropbottom = 0;
            croptop = 0;
            width = 1920;
            height = 1080;

            videoPlayerPs4.GetVideoCropValues(out cropleft, out cropright, out croptop, out cropbottom, out width, out height);
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
            planePS4.material.SetVector("_MainTex_ST", new Vector4(scalex, scaley * -1, offx, 1 - offy)); // typically we want to invert the Y on the video because thats how planes UV's are layed out

        }

    }
#endif

    void OnMovieEvent(int eventID)
    {
        if (!isStart)
            return;

#if UNITY_PS4 && !UNITY_EDITOR
        if (isDebug)
            return;
        //   Debug.LogError("script has received FMV event " + eventID);
        if (eventID == 1)
            LoadLevelManager.Instance.ActivateLoadedLevel();
#endif
    }


    IEnumerator FinishVideo(float time)
    {
        yield return StartCoroutine(new TimeCallBacks().WaitPause(time));
        isStart = false;
        if (VideoEnding != null)
            VideoEnding();

        planePS4.gameObject.SetActive(false);
        rendererVideo.gameObject.SetActive(false);
    }


    void LateUpdate()
    {
#if  (UNITY_STANDALONE || UNITY_EDITOR)
        if (!video.isPlaying && isStart && !isDebug)
        {
            isStart = false;
            if (VideoEnding != null)
                VideoEnding();
        }
#endif

    }
}
