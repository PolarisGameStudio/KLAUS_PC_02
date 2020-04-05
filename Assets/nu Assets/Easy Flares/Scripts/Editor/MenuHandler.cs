
using ch.sycoforge.Flares.Editor;
using UnityEditor;

public static class MenuHandler
{
    [MenuItem("Assets/Create/nu/Easy Flare Style")]
    public static void CreateStyle()
    {
        //Show existing window instance. If one doesn't exist, make one.
        FlareWindow window = (FlareWindow)EditorWindow.GetWindow(typeof(FlareWindow), true, "New Easy Flares Style");
        window.Show();
    }

    [MenuItem("Window/Easy Flares/Style Editor")]
    public static void ShowStyleEditor()
    {
        EditorWindow.GetWindow<StyleEditorWindow>("Style Editor");
    }

    [MenuItem("Window/Easy Flares/Flare Preview")]
    public static void ShowPreviewEditor()
    {
        FlarePreviewWindow.ShowPreviewWindow();
    }
}

