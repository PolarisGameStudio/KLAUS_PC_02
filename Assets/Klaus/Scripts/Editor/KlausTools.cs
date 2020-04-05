using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;

public class KlausTools : EditorWindow
{
    #region Window Handler:

    static KlausTools m_klausWindow;

    [MenuItem("Utils/Klaus/Show Window")]
    public static void ShowWindow()
    {
        if (m_klausWindow == null)
        {
            m_klausWindow = EditorWindow.GetWindow<KlausTools>();
        }

        m_klausWindow.Show();
    }

    #endregion

    /*
    protected void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    protected void OnDisable()
    {

        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    void OnSceneGUI()
    {
        
    }
    */

    // is called at 10 frames per second to give the inspector a chance to update.
    void OnInspectorUpdate()
    {
        if (EditorWindow.mouseOverWindow)
            EditorWindow.mouseOverWindow.Focus();

        // Call Repaint on OnInspectorUpdate as it repaints the windows
        // less times as if it was OnGUI/Update
        Repaint();
    }

    void OnGUI()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            GUILayout.Label("This buttons doesn't work when you are playing the game.");
            return;
        }

        if (IsBlockOperations())
        {
            GUILayout.Label("This buttons doesn't work when you are playing the game.");

            return;
        }

        if (GUILayout.Button("Fix Camera Trigger Boundaries"))
        {
            Selection.activeGameObject = null;
            BlockOperations(true);
            OpenAllScenes(FixBoundaries);
            BlockOperations(false);
            Selection.activeGameObject = null;
        }
        if (GUILayout.Button("Revert HUD Camera"))
        {
            Selection.activeGameObject = null;
            BlockOperations(true);
            OpenAllScenes(RevertHUD_Canvas);
            BlockOperations(false);
            Selection.activeGameObject = null;
        }

        if (GUILayout.Button("Revert Prefab"))
        {
            bool isDone = PrefabUtility.ReconnectToLastPrefab(Selection.activeGameObject);
            Debug.Log(isDone + " : " + Selection.activeGameObject.name);

        }
        if (GUILayout.Button("Fix Video Scene"))
        {
            BlockOperations(true);
            OpenSelectedScenes(FixSceneVideo);
            BlockOperations(false);
        }
        if (GUILayout.Button("Delete Save File"))
        {
            if (File.Exists(SaveManager.SaveFileFullPath))
            {
                File.Delete(SaveManager.SaveFileFullPath);
                Debug.Log("Deleted: " + SaveManager.SaveFileFullPath);
            }
            else
            {
                Debug.Log("Can't find: " + SaveManager.SaveFileFullPath);
            }
        }
    }

    #region Helper Fucntions:

    bool m_runningOperation = false;

    bool IsBlockOperations()
    {
        return m_runningOperation;
    }

    void BlockOperations(bool value)
    {
        Assert.AreNotEqual(value, m_runningOperation, "You are trying to do a nother operation.");
        m_runningOperation = value;
        AssetDatabase.SaveAssets();
    }


    void OpenAllScenes(Action<Scene> operation)
    {
        var scenesBuild = EditorBuildSettings.scenes;

        foreach (EditorBuildSettingsScene sceneName in scenesBuild)
        {
            if (!sceneName.enabled)
                continue;

            var currentScene = EditorSceneManager.OpenScene(sceneName.path, OpenSceneMode.Single);

            Debug.Log(" Scene Opened: " + currentScene.name);
            if (!currentScene.IsValid())
            {
                Debug.LogWarning("The scene " + currentScene.name + " is not valid.");
                continue;
            }
            Assert.IsTrue(currentScene.isLoaded, "The scene " + currentScene.name + " to modified is not loaded");
            //Perfom option in here
            operation.Invoke(currentScene);
            //

            if (currentScene.isDirty) //TODO: Check why isDirty doesn't change
                EditorSceneManager.SaveScene(currentScene);

        }
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Debug.Log("Finish the task.");
    }

    void OpenSelectedScenes(Action<Scene> operation)
    {
        var sceneObjects = Selection.GetFiltered<SceneAsset>(SelectionMode.Assets);
        foreach (var sceneName in sceneObjects)
        {
            var newPath = AssetDatabase.GetAssetPath(sceneName);
            var currentScene = EditorSceneManager.OpenScene(newPath, OpenSceneMode.Single);
            Debug.Log(" Scene Opened: " + currentScene.name);
            if (!currentScene.IsValid())
            {
                Debug.LogWarning("The scene " + currentScene.name + " is not valid.");
                continue;
            }
            Assert.IsTrue(currentScene.isLoaded, "The scene " + currentScene.name + " to modified is not loaded");
            //Invoke
            operation.Invoke(currentScene);
            //
            if (currentScene.isDirty) //TODO: Check why isDirty doesn't change
                EditorSceneManager.SaveScene(currentScene);
        }
        Debug.Log("Finish the task.");
    }
    #endregion


    #region Fix Functions:

    void RevertHUD_Canvas(Scene scene)
    {
        bool isDirty = false;
        //Perfom option in here
        var hudCanvas = GameObject.Find("HUD_Canvas");
        if (hudCanvas != null)
        {
            bool isDone = PrefabUtility.ReconnectToLastPrefab(hudCanvas);
            isDirty = true;

        }

        if (isDirty)
            EditorSceneManager.MarkSceneDirty(scene);
    }

    void FixBoundaries(Scene scene)
    {
        bool isDirty = false;
        //Perfom option in here
        var boundariesScripts = GameObject.FindObjectsOfType<Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerBoundaries>();
        foreach (Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerBoundaries boundaries in boundariesScripts)
        {
            isDirty = true;
            boundaries.SetAsStartingBoundaries = true;
        }
        if (isDirty)
            EditorSceneManager.MarkSceneDirty(scene);
    }

    #endregion

    #region ChangeSceneVideo:
    void FixSceneVideo(Scene scene)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        AS_MixerCollectOUT soundScript = FindObjectOfType<AS_MixerCollectOUT>();
        if (soundScript != null)
        {
            var cameraMixer = Camera.main.gameObject.AddComponent<AS_MixerCollectOUT>();
            cameraMixer.mainMixer = soundScript.mainMixer;
        }

        foreach (var GO in allObjects)
        {
            if (GO.transform.parent == null)
            {
                if (GO != Camera.main.gameObject)
                {
                    DestroyImmediate(GO);
                }
            }
        }

        foreach (var GO in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (GO.name == "Plane")
            {
                DestroyImmediate(GO);
                break;
            }
        }


        var vPlayerScene = Camera.main.GetComponent<VideoPlayerScene>();
        var clipNames = vPlayerScene.videoName.Split('.');
        vPlayerScene.videoName = clipNames[0];


        GameObject videoPlayerGO = new GameObject("Vide Player");
        var videoP = videoPlayerGO.AddComponent<VideoPlayer>();
        videoP.targetCamera = Camera.main;
        videoP.playOnAwake = false;
        videoP.renderMode = VideoRenderMode.CameraNearPlane;

        vPlayerScene.m_videoPlayer = videoP;
        EditorSceneManager.MarkSceneDirty(scene);
    }
    #endregion
}
