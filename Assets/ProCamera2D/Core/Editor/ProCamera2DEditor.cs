using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    [CustomEditor(typeof(ProCamera2D))]
    public class ProCamera2DEditor : Editor
    {
        GUIContent _tooltip;

        ReorderableList _targetsList;

        List<ProCamera2DTriggerBoundaries> _triggerBoundaries;
        int _currentTriggerBoundaries;

        List<ProCamera2DTriggerInfluence> _triggerInfluence;
        int _currentTriggerInfluence;

        List<ProCamera2DTriggerZoom> _triggerZoom;
        int _currentTriggerZoom;

        List<ProCamera2DCinematicFocusTarget> _cinematicFocusTargets;
        int _currentCinematicFocusTarget;

        string hAxis = "";
        string vAxis = "";

        void OnEnable()
        {
            var proCamera2D = (ProCamera2D)target;

            if (proCamera2D.GameCamera == null)
                proCamera2D.GameCamera = proCamera2D.GetComponent<Camera>();

            #if PC2D_TK2D_SUPPORT
            if (proCamera2D.Tk2dCam == null)
                proCamera2D.Tk2dCam = proCamera2D.GetComponent<tk2dCamera>();
            #endif

            // Show correct axis name
            switch (proCamera2D.Axis)
            {
                case MovementAxis.XY:
                    hAxis = "X";
                    vAxis = "Y";
                    break;

                case MovementAxis.XZ:
                    hAxis = "X";
                    vAxis = "Z";
                    break;

                case MovementAxis.YZ:
                    hAxis = "Y";
                    vAxis = "Z";
                    break;
            }

            // Get total TriggerBoundaries
            _triggerBoundaries = FindObjectsOfType<ProCamera2DTriggerBoundaries>().ToList();

            // Get total TriggerInfluence
            _triggerInfluence = FindObjectsOfType<ProCamera2DTriggerInfluence>().ToList();

            // Get total TriggerZoom
            _triggerZoom = FindObjectsOfType<ProCamera2DTriggerZoom>().ToList();

            // Get total CinematicFocusTargets
            _cinematicFocusTargets = FindObjectsOfType<ProCamera2DCinematicFocusTarget>().ToList();

            // Targets List
            _targetsList = new ReorderableList(serializedObject, serializedObject.FindProperty("CameraTargets"), false, false, true, true);

            _targetsList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                var element = _targetsList.serializedProperty.GetArrayElementAtIndex(index);

                #if UNITY_5
                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, 65, 10), new GUIContent("Transform", "The target transform"), EditorStyles.boldLabel);
                #else
				EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, 65, 10), new GUIContent("Transform", "The target transform"));
                #endif
                EditorGUI.PropertyField(new Rect(
                        rect.x + 65,
                        rect.y,
                        80,
                        EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("TargetTransform"), GUIContent.none);

                #if UNITY_5
                EditorGUI.PrefixLabel(new Rect(rect.x + 160, rect.y, 65, 10), new GUIContent("Offset", "Offset the camera position relative to this target"), EditorStyles.boldLabel);
                #else
				EditorGUI.PrefixLabel(new Rect(rect.x + 160, rect.y, 65, 10), new GUIContent("Offset", "Offset the camera position relative to this target"));
                #endif
                EditorGUI.PropertyField(new Rect(
                        rect.x + 200,
                        rect.y,
                        rect.width - 200,
                        EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("TargetOffset"), GUIContent.none);

                #if UNITY_5
                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 25, 65, 10), new GUIContent("Influence" + hAxis, "How much does this target horizontal position influences the camera position?"), EditorStyles.boldLabel);
                #else
                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 25, 65, 10), new GUIContent("Influence" + hAxis, "How much does this target horizontal position influences the camera position?"));
                #endif
                EditorGUI.PropertyField(new Rect(
                        rect.x + 80,
                        rect.y + 25,
                        rect.width - 80,
                        EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("TargetInfluenceH"), GUIContent.none);

                #if UNITY_5
                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 40, 65, 10), new GUIContent("Influence" + vAxis, "How much does this target vertical position influences the camera position?"), EditorStyles.boldLabel);
                #else
                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 40, 65, 10), new GUIContent("Influence" + vAxis, "How much does this target vertical position influences the camera position?"));
                #endif
                EditorGUI.PropertyField(new Rect(
                        rect.x + 80,
                        rect.y + 40,
                        rect.width - 80,
                        EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("TargetInfluenceV"), GUIContent.none);
            };

            _targetsList.elementHeight = 80;
            _targetsList.headerHeight = 0;
            _targetsList.draggable = true;
        }

        public override void OnInspectorGUI()
        {
            var proCamera2D = (ProCamera2D)target;

            serializedObject.Update();


            EditorGUILayout.Space();

            // Draw User Guide link
            if (ProCamera2DEditorResources.UserGuideIcon != null)
            {
                var rect = GUILayoutUtility.GetRect(0f, 0f);
                rect.width = ProCamera2DEditorResources.UserGuideIcon.width;
                rect.height = ProCamera2DEditorResources.UserGuideIcon.height;
                if (GUI.Button(new Rect(15, rect.y, 32, 32), new GUIContent(ProCamera2DEditorResources.UserGuideIcon, "User Guide")))
                {
                    Application.OpenURL("http://www.procamera2d.com/user-guide/");
                }
            }

            // Draw header
            if (ProCamera2DEditorResources.InspectorHeader != null)
            {
                var rect = GUILayoutUtility.GetRect(0f, 0f);
                rect.x += 37;
                rect.width = ProCamera2DEditorResources.InspectorHeader.width;
                rect.height = ProCamera2DEditorResources.InspectorHeader.height;
                GUILayout.Space(rect.height);
                GUI.DrawTexture(rect, ProCamera2DEditorResources.InspectorHeader);
            }

            EditorGUILayout.Space();


            // Assign game camera
            if (proCamera2D.GameCamera == null)
                proCamera2D.GameCamera = proCamera2D.GetComponent<Camera>();


            // Targets Drop Area
            Event evt = Event.current;
            Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            var style = new GUIStyle("box");
            if (EditorGUIUtility.isProSkin)
                style.normal.textColor = Color.white;
            GUI.Box(drop_area, "\nDROP CAMERA TARGETS HERE", style);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object dragged_object in DragAndDrop.objectReferences)
                        {
                            var newCameraTarget = new CameraTarget
                            {
                                TargetTransform = ((GameObject)dragged_object).transform,
                                TargetInfluence = 1f
                            };
							
                            proCamera2D.CameraTargets.Add(newCameraTarget);
                            EditorUtility.SetDirty(proCamera2D);
                        }
                    }
                    break;
            }

            EditorGUILayout.Space();




            // Remove empty targets
            for (int i = 0; i < proCamera2D.CameraTargets.Count; i++)
            {
                if (proCamera2D.CameraTargets[i].TargetTransform == null)
                {
                    proCamera2D.CameraTargets.RemoveAt(i);
                }
            }




            // Targets List
            _targetsList.DoLayoutList();
            AddSpace();





            // Center target on start
            _tooltip = new GUIContent("Center target on start", "Should the camera move instantly to the target on game start?");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CenterTargetOnStart"), _tooltip);

            if (proCamera2D.CenterTargetOnStart && proCamera2D.UseGeometryBoundaries)
                EditorGUILayout.HelpBox("Centering on target at start while using geometry boundaries might cause the camera to get stuck.", MessageType.Warning, true);

            AddSpace();
            




            // Axis
            _tooltip = new GUIContent("Axis", "Choose the axis in which the camera should move.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Axis"), _tooltip);

            AddSpace();




            // UpdateType
            _tooltip = new GUIContent("Update Type", "LateUpdate: Non physics based game\nFixedUpdate: Physics based game");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateType"), _tooltip);

            AddSpace();




            // Show correct axis name
            switch (proCamera2D.Axis)
            {
                case MovementAxis.XY:
                    hAxis = "X";
                    vAxis = "Y";
                    break;

                case MovementAxis.XZ:
                    hAxis = "X";
                    vAxis = "Z";
                    break;

                case MovementAxis.YZ:
                    hAxis = "Y";
                    vAxis = "Z";
                    break;
            }




            // Follow horizontal
            _tooltip = new GUIContent("Follow " + hAxis, "Should the camera move on the horizontal axis?");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FollowHorizontal"), _tooltip);
            if (proCamera2D.FollowHorizontal)
            {
                EditorGUI.indentLevel = 1;

                // Follow smoothness
                _tooltip = new GUIContent("Follow Smoothness", "How long it takes the camera to reach the target horizontal position.");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HorizontalFollowSmoothness"), _tooltip);

                if (proCamera2D.HorizontalFollowSmoothness < 0f)
                    proCamera2D.HorizontalFollowSmoothness = 0f;

                // Limit camera distance to target
                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Limit Cam Distance", "Prevent the camera target from getting too far. Use this if you have a high follow smoothness and your targets are getting out of the screen.");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitHorizontalCameraDistance"), _tooltip);

                if (proCamera2D.LimitHorizontalCameraDistance)
                {
                    _tooltip = new GUIContent(" ", "The percentage of the screen at which the camera will be forced to move");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHorizontalTargetDistance"), _tooltip);

                    if (proCamera2D.MaxHorizontalTargetDistance < .1f)
                        proCamera2D.MaxHorizontalTargetDistance = .1f;
                    else if (proCamera2D.MaxHorizontalTargetDistance > 1f)
                        proCamera2D.MaxHorizontalTargetDistance = 1f;
                }

                EditorGUILayout.EndHorizontal();

                // Limit speed
                EditorGUILayout.BeginHorizontal();

                _tooltip = new GUIContent("Limit Speed", "Limit how fast the camera moves per second on the horizontal axis");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitMaxHorizontalSpeed"), _tooltip);

                if (proCamera2D.LimitMaxHorizontalSpeed)
                {
                    _tooltip = new GUIContent(" ", "Limit how fast the camera moves per second on the horizontal axis");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHorizontalSpeed"), _tooltip);

                    if (proCamera2D.MaxHorizontalSpeed <= 0)
                        proCamera2D.MaxHorizontalSpeed = 0.01f;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel = 0;
                AddSpace();
            }




            // Follow vertical
            _tooltip = new GUIContent("Follow " + vAxis, "Should the camera move on the vertical axis?");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FollowVertical"), _tooltip);

            if (proCamera2D.FollowVertical)
            {
                EditorGUI.indentLevel = 1;

                // Follow smoothness
                _tooltip = new GUIContent("Follow Smoothness", "How long it takes the camera to reach the target vertical position.");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("VerticalFollowSmoothness"), _tooltip);

                if (proCamera2D.VerticalFollowSmoothness < 0f)
                    proCamera2D.VerticalFollowSmoothness = 0f;

                // Limit camera distance to target    
                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Limit Cam Distance", "Prevent the camera target from getting too far. Use this if you have a high follow smoothness and your targets are getting out of the screen.");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitVerticalCameraDistance"), _tooltip);

                if (proCamera2D.LimitVerticalCameraDistance)
                {
                    _tooltip = new GUIContent(" ", "The percentage of the screen at which the camera will be forced to move");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxVerticalTargetDistance"), _tooltip);

                    if (proCamera2D.MaxVerticalTargetDistance < .1f)
                        proCamera2D.MaxVerticalTargetDistance = .1f;
                    else if (proCamera2D.MaxVerticalTargetDistance > 1f)
                        proCamera2D.MaxVerticalTargetDistance = 1f;
                }

                EditorGUILayout.EndHorizontal();

                // Limit camera speed
                EditorGUILayout.BeginHorizontal();

                _tooltip = new GUIContent("Limit Speed", "Limit how fast the camera moves per second on the vertical axis");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitMaxVerticalSpeed"), _tooltip);

                if (proCamera2D.LimitMaxVerticalSpeed)
                {
                    _tooltip = new GUIContent(" ", "Limit how fast the camera moves per second on the vertical axis");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxVerticalSpeed"), _tooltip);

                    if (proCamera2D.MaxVerticalSpeed <= 0)
                        proCamera2D.MaxVerticalSpeed = 0.01f;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel = 0;
            }

            if (!proCamera2D.FollowHorizontal && !proCamera2D.FollowVertical)
                EditorGUILayout.HelpBox("Camera won't move if it's not following the targets on any axis.", MessageType.Error, true);

            AddSpace();





            // Overall offset
            EditorGUILayout.LabelField("Offset");
            EditorGUI.indentLevel = 1;

            _tooltip = new GUIContent(hAxis, "Horizontal offset");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OverallOffset.x"), _tooltip);

            _tooltip = new GUIContent(vAxis, "Vertical offset");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OverallOffset.y"), _tooltip);

            EditorGUI.indentLevel = 0;

            AddSpace();

            



            // Camera window rect
            _tooltip = new GUIContent("Camera Window", "Inside this Rect the camera won't move. Use to avoid constant camera move with slight changes.");
            EditorGUILayout.LabelField(_tooltip);

            EditorGUI.indentLevel = 1;

            _tooltip = new GUIContent("Width", "Window width");
            EditorGUILayout.Slider(serializedObject.FindProperty("CameraWindowRect.width"), 0f, 1f, _tooltip);

            _tooltip = new GUIContent("Height", "Window height");
            EditorGUILayout.Slider(serializedObject.FindProperty("CameraWindowRect.height"), 0f, 1f, _tooltip);

            _tooltip = new GUIContent(hAxis, "Window horizontal offset");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraWindowRect.x"), _tooltip);

            _tooltip = new GUIContent(vAxis, "Window vertical offset");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraWindowRect.y"), _tooltip);

            EditorGUI.indentLevel = 0;

            AddSpace();







            // Geometry boundaries
            _tooltip = new GUIContent("Use Geometry Boundaries", "Should the camera position be constrained by geometry?");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseGeometryBoundaries"), _tooltip);

            if (proCamera2D.UseGeometryBoundaries)
            {
                EditorGUI.indentLevel = 1;

                _tooltip = new GUIContent("Layer", "Choose which layer contains the (3d) colliders that define the boundaries for the camera");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BoundariesLayerMask"), _tooltip);

                EditorGUI.indentLevel = 0;
            }





            // Numeric boundaries
            _tooltip = new GUIContent("Use Numeric Boundaries", "Should the camera position be constrained by position?");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseNumericBoundaries"), _tooltip);

            if (proCamera2D.UseNumericBoundaries)
            {
                EditorGUI.indentLevel = 1;
                
                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Use Top", "Prevent camera movement beyond this point");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UseTopBoundary"), _tooltip);

                if (proCamera2D.UseTopBoundary)
                {
                    _tooltip = new GUIContent(" ", "Prevent camera movement beyond this point");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("TopBoundary"), _tooltip);
                }

                if (proCamera2D.UseBottomBoundary && proCamera2D.TopBoundary < proCamera2D.BottomBoundary)
                    proCamera2D.TopBoundary = proCamera2D.BottomBoundary;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Use Bottom", "Prevent camera movement beyond this point");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UseBottomBoundary"), _tooltip);

                if (proCamera2D.UseBottomBoundary)
                {
                    _tooltip = new GUIContent(" ", "Prevent camera movement beyond this point");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("BottomBoundary"), _tooltip);
                }

                if (proCamera2D.UseTopBoundary && proCamera2D.BottomBoundary > proCamera2D.TopBoundary)
                    proCamera2D.BottomBoundary = proCamera2D.TopBoundary;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Use Left", "Prevent camera movement beyond this point");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UseLeftBoundary"), _tooltip);

                if (proCamera2D.UseLeftBoundary)
                {
                    _tooltip = new GUIContent(" ", "Prevent camera movement beyond this point");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("LeftBoundary"), _tooltip);
                }

                if (proCamera2D.UseRightBoundary && proCamera2D.LeftBoundary > proCamera2D.RightBoundary)
                    proCamera2D.LeftBoundary = proCamera2D.RightBoundary;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                _tooltip = new GUIContent("Use Right", "Prevent camera movement beyond this point");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UseRightBoundary"), _tooltip);

                if (proCamera2D.UseRightBoundary)
                {
                    _tooltip = new GUIContent(" ", "Prevent camera movement beyond this point");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("RightBoundary"), _tooltip);
                }

                if (proCamera2D.UseLeftBoundary && proCamera2D.RightBoundary < proCamera2D.LeftBoundary)
                    proCamera2D.RightBoundary = proCamera2D.LeftBoundary;

                EditorGUILayout.EndHorizontal();

                if ((proCamera2D.UseTopBoundary && proCamera2D.UseBottomBoundary && proCamera2D.BottomBoundary == proCamera2D.TopBoundary) ||
                    (proCamera2D.UseLeftBoundary && proCamera2D.UseRightBoundary && proCamera2D.LeftBoundary == proCamera2D.RightBoundary))
                    EditorGUILayout.HelpBox("Same axis boundaries can't have the same value!", MessageType.Error, true);
                    
                EditorGUI.indentLevel = 0;
            }

            AddSpace();



            // Divider
            GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            GUILayout.Label("PLUGINS", EditorStyles.boldLabel);




            // Pixel perfect plugin
            EditorGUILayout.BeginHorizontal();

            var pixelPerfect = proCamera2D.GetComponent<ProCamera2DPixelPerfect>();

            EditorGUILayout.LabelField("Pixel Perfect");
            if (pixelPerfect == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    pixelPerfect = proCamera2D.gameObject.AddComponent<ProCamera2DPixelPerfect>();
                    pixelPerfect.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(pixelPerfect);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();



            // Forward focus plugin
            EditorGUILayout.BeginHorizontal();

            var forwardFocus = proCamera2D.GetComponent<ProCamera2DForwardFocus>();

            EditorGUILayout.LabelField("Forward Focus");
            if (forwardFocus == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    forwardFocus = proCamera2D.gameObject.AddComponent<ProCamera2DForwardFocus>();
                    forwardFocus.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(forwardFocus);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();



            // Zoom to fit plugin
            EditorGUILayout.BeginHorizontal();

            var zoomToFit = proCamera2D.GetComponent<ProCamera2DZoomToFitTargets>();

            EditorGUILayout.LabelField("Zoom-To-Fit");
            if (zoomToFit == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    zoomToFit = proCamera2D.gameObject.AddComponent<ProCamera2DZoomToFitTargets>();
                    zoomToFit.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(zoomToFit);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();




            // Speed based zoom plugin
            EditorGUILayout.BeginHorizontal();

            var speedBasedZoom = proCamera2D.GetComponent<ProCamera2DSpeedBasedZoom>();

            EditorGUILayout.LabelField("Speed Based Zoom");
            if (speedBasedZoom == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    speedBasedZoom = proCamera2D.gameObject.AddComponent<ProCamera2DSpeedBasedZoom>();
                    speedBasedZoom.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(speedBasedZoom);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();



            // Parallax plugin
            GUI.enabled = proCamera2D.GameCamera.orthographic;
        
            EditorGUILayout.BeginHorizontal();

            var parallax = proCamera2D.GetComponent<ProCamera2DParallax>();

            EditorGUILayout.LabelField("Parallax");
            if (parallax == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    parallax = proCamera2D.gameObject.AddComponent<ProCamera2DParallax>();
                    parallax.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(parallax);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;



            // Shake plugin
            EditorGUILayout.BeginHorizontal();

            var shake = proCamera2D.GetComponent<ProCamera2DShake>();

            EditorGUILayout.LabelField("Shake");
            if (shake == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    shake = proCamera2D.gameObject.AddComponent<ProCamera2DShake>();
                    shake.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(shake);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();




            // Mouse influence plugin
            EditorGUILayout.BeginHorizontal();

            var mouseInfluence = proCamera2D.GetComponent<ProCamera2DPointerInfluence>();

            EditorGUILayout.LabelField("Pointer Influence");
            if (mouseInfluence == null)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Enable"))
                {
                    mouseInfluence = proCamera2D.gameObject.AddComponent<ProCamera2DPointerInfluence>();
                    mouseInfluence.ProCamera2D = proCamera2D;
                }
            }
            else
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Disable"))
                {
                    if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to remove this plugin?", "Yes", "No"))
                    {
                        DestroyImmediate(mouseInfluence);
                        EditorGUIUtility.ExitGUI();
                    }
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();




            // Divider
            EditorGUILayout.Space();
            GUI.color = Color.white;
            GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            GUILayout.Label("HELPERS", EditorStyles.boldLabel);




            // Boundaries trigger
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Boundaries Trigger (" + _triggerBoundaries.Count + ")");

            if (GUILayout.Button("Add"))
            {
                var newGo = new GameObject("ProCamera2DTriggerBoundaries");
                newGo.transform.localScale = new Vector3(5, 5, 5);
                var boundsTrigger = newGo.AddComponent<ProCamera2DTriggerBoundaries>();
                boundsTrigger.ProCamera2D = proCamera2D;
                _triggerBoundaries.Add(boundsTrigger);
            }

            GUI.enabled = _triggerBoundaries.Count > 0;

            if (GUILayout.Button(">"))
            {
                Selection.activeGameObject = _triggerBoundaries[_currentTriggerBoundaries].gameObject;
                SceneView.FrameLastActiveSceneView();
                EditorGUIUtility.PingObject(_triggerBoundaries[_currentTriggerBoundaries].gameObject);

                Selection.activeGameObject = proCamera2D.gameObject;

                _currentTriggerBoundaries = _currentTriggerBoundaries >= _triggerBoundaries.Count - 1 ? 0 : _currentTriggerBoundaries + 1;
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();




            // Influence trigger
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Influence Trigger (" + _triggerInfluence.Count + ")");

            if (GUILayout.Button("Add"))
            {
                var newGo = new GameObject("ProCamera2DTriggerInfluence");
                newGo.transform.localScale = new Vector3(5, 5, 5);
                var influenceTrigger = newGo.AddComponent<ProCamera2DTriggerInfluence>();
                influenceTrigger.ProCamera2D = proCamera2D;
                _triggerInfluence.Add(influenceTrigger);
            }

            GUI.enabled = _triggerInfluence.Count > 0;

            if (GUILayout.Button(">"))
            {
                Selection.activeGameObject = _triggerInfluence[_currentTriggerInfluence].gameObject;
                SceneView.FrameLastActiveSceneView();
                EditorGUIUtility.PingObject(_triggerInfluence[_currentTriggerInfluence].gameObject);

                Selection.activeGameObject = proCamera2D.gameObject;

                _currentTriggerInfluence = _currentTriggerInfluence >= _triggerInfluence.Count - 1 ? 0 : _currentTriggerInfluence + 1;
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();




            // Zoom trigger
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Zoom Trigger (" + _triggerZoom.Count + ")");

            if (GUILayout.Button("Add"))
            {
                var newGo = new GameObject("ProCamera2DTriggerZoom");
                newGo.transform.localScale = new Vector3(5, 5, 5);
                var zoomTrigger = newGo.AddComponent<ProCamera2DTriggerZoom>();
                zoomTrigger.ProCamera2D = proCamera2D;
                _triggerZoom.Add(zoomTrigger);
            }

            GUI.enabled = _triggerZoom.Count > 0;

            if (GUILayout.Button(">"))
            {
                Selection.activeGameObject = _triggerZoom[_currentTriggerZoom].gameObject;
                SceneView.FrameLastActiveSceneView();
                EditorGUIUtility.PingObject(_triggerZoom[_currentTriggerZoom].gameObject);

                Selection.activeGameObject = proCamera2D.gameObject;

                _currentTriggerZoom = _currentTriggerZoom >= _triggerZoom.Count - 1 ? 0 : _currentTriggerZoom + 1;
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();




            // Cinematic Focus Target
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Cinematic Focus Target (" + _cinematicFocusTargets.Count + ")");

            if (GUILayout.Button("Add"))
            {
                var newGo = new GameObject("ProCamera2DCinematicFocusTarget");
                var cinematicFocusTarget = newGo.AddComponent<ProCamera2DCinematicFocusTarget>();
                cinematicFocusTarget.ProCamera2D = proCamera2D;
                _cinematicFocusTargets.Add(cinematicFocusTarget);
            }

            GUI.enabled = _cinematicFocusTargets.Count > 0;

            if (GUILayout.Button(">"))
            {
                Selection.activeGameObject = _cinematicFocusTargets[_currentCinematicFocusTarget].gameObject;
                SceneView.FrameLastActiveSceneView();
                EditorGUIUtility.PingObject(_cinematicFocusTargets[_currentCinematicFocusTarget].gameObject);

                Selection.activeGameObject = proCamera2D.gameObject;

                _currentCinematicFocusTarget = _currentCinematicFocusTarget >= _cinematicFocusTargets.Count - 1 ? 0 : _currentCinematicFocusTarget + 1;
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();




            serializedObject.ApplyModifiedProperties();
        }

        void AddSpace()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}