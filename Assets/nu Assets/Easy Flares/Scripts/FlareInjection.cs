using ch.sycoforge.Flares;
using UnityEngine;


/// <summary>
/// Injects the flare geomtry in the <c>OnGUI()</c> pass for using Easy Flares Pro
/// in Unity's <c>Application.CaptureScreenshot()</c> method.
/// </summary>
public class FlareInjection : MonoBehaviour 
{
    public EasyFlares Flare;

    private bool renderScreenshot;
    private int frames;
    private string path;

    private const int FLAREFRAMES = 2;

    private void Start()
    {
        // Format output path
        path = string.Format("{0}/{1}.png", Application.dataPath, "screenshot");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            renderScreenshot = true;

            //Reset frames
            frames = 0;

            // Enqueue screenshot command
            ScreenCapture.CaptureScreenshot(path, 1);
        }

        if(renderScreenshot)
        {
            frames++;
        }
    }

    private void OnGUI()
    {
        // Is there a render command waiting and are we in repaint state
        if (renderScreenshot && Event.current.type == EventType.Repaint)
        {
            // Inject procedural flare geometry
            Flare.LateUpdate();

            // Reset render command after the specified amount of frames
            renderScreenshot = frames < FLAREFRAMES;
        }
    }
}
