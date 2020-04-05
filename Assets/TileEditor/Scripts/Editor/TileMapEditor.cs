using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor
{
    enum State
    {
        Hover,
        BoxSelect

    }

    static Vector3[] rect = new Vector3[4];

    TileMap tileMap;
    FieldInfo undoCallback;
    bool editing;
    Matrix4x4 worldToLocal;

    State state;
    int cursorX;
    int cursorY;
    int cursorClickX;
    int cursorClickY;
    bool deleting;
    int direction;

    //bool updateConnections = true;
    bool wireframeHidden;

    #region Inspector GUI

    public override void OnInspectorGUI()
    {
        //Get tilemap
        if (tileMap == null)
            tileMap = (TileMap)target;

        //Crazy hack to register undo
        if (undoCallback == null)
        {
            undoCallback = typeof(EditorApplication).GetField("undoRedoPerformed", BindingFlags.NonPublic | BindingFlags.Static);
            if (undoCallback != null)
                undoCallback.SetValue(null, new EditorApplication.CallbackFunction(OnUndoRedo));
        }

        //Toggle editing mode
        if (editing)
        {
            if (GUILayout.Button("Stop Editing"))
                editing = false;
            else
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Update All"))
                    UpdateAll();
                if (GUILayout.Button("Clear"))
                    Clear();
                EditorGUILayout.EndHorizontal();
            }
        } else if (GUILayout.Button("Edit TileMap"))
            editing = true;


        //Tile Size
        EditorGUI.BeginChangeCheck();
        var newTileSize = EditorGUILayout.FloatField("Tile Size", tileMap.tileSize);
        if (EditorGUI.EndChangeCheck())
        {
            RecordDeepUndo();
            tileMap.tileSize = newTileSize;
            UpdatePositions();
        }

        //Tile Prefab
        EditorGUI.BeginChangeCheck();
        var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", tileMap.tilePrefab, typeof(Transform), false);
        if (EditorGUI.EndChangeCheck())
        {
            RecordUndo();
            tileMap.tilePrefab = newTilePrefab;
        }
        //Z order
        EditorGUI.BeginChangeCheck();
        var newZOrder = EditorGUILayout.IntField("Z Ordes", tileMap.zOrder);
        if (EditorGUI.EndChangeCheck())
        {
            RecordUndo();
            if (newZOrder < 0)
                newZOrder = 0;
            tileMap.zOrder = newZOrder;
        }
        //Tile Map
        EditorGUI.BeginChangeCheck();
        var newTileSet = (TileSet)EditorGUILayout.ObjectField("Tile Set", tileMap.tileSet, typeof(TileSet), false);
        if (EditorGUI.EndChangeCheck())
        {
            RecordUndo();
            tileMap.tileSet = newTileSet;
        }

        //Tile Prefab selector
        if (tileMap.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            var names = new string[tileMap.tileSet.prefabs.Length + 1];
            var values = new int[names.Length + 1];
            names [0] = tileMap.tilePrefab != null ? tileMap.tilePrefab.name : "";
            values [0] = 0;
            for (int i = 1; i < names.Length; i++)
            {
                names [i] = tileMap.tileSet.prefabs [i - 1] != null ? tileMap.tileSet.prefabs [i - 1].name : "";
                //if (i < 10)
                //	names[i] = i + ". " + names[i];
                values [i] = i;
            }
            var index = EditorGUILayout.IntPopup("Select Tile", 0, names, values);
            if (EditorGUI.EndChangeCheck() && index > 0)
            {
                RecordUndo();
                tileMap.tilePrefab = tileMap.tileSet.prefabs [index - 1];
            }
        }

        //Selecting direction
        EditorGUILayout.BeginHorizontal(GUILayout.Width(60));
        EditorGUILayout.PrefixLabel("Direction");
        EditorGUILayout.BeginVertical(GUILayout.Width(20));
        GUILayout.Space(20);
        if (direction == 3)
            GUILayout.Box("<", GUILayout.Width(20));
        else if (GUILayout.Button("<"))
            direction = 3;
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.Width(20));
        if (direction == 0)
            GUILayout.Box("^", GUILayout.Width(20));
        else if (GUILayout.Button("^"))
            direction = 0;
        if (direction == -1)
            GUILayout.Box("?", GUILayout.Width(20));
        else if (GUILayout.Button("?"))
            direction = -1;
        if (direction == 2)
            GUILayout.Box("v", GUILayout.Width(20));
        else if (GUILayout.Button("v"))
            direction = 2;
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.Width(20));
        GUILayout.Space(20);
        if (direction == 1)
            GUILayout.Box(">", GUILayout.Width(20));
        else if (GUILayout.Button(">"))
            direction = 1;
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        /*
    //Connect diagonals
    EditorGUI.BeginChangeCheck();
    var newConnectDiagonals = EditorGUILayout.Toggle("Connect Diagonals", tileMap.connectDiagonals);
    if (EditorGUI.EndChangeCheck())
    {
        RecordUndo();
        tileMap.connectDiagonals = newConnectDiagonals;
        updateConnections = true;
        SceneView.RepaintAll();
    }

    //Connect diagonals
    if (tileMap.connectDiagonals)
    {
        EditorGUI.BeginChangeCheck();
        var newCutCorners = EditorGUILayout.Toggle("Cut Corners", tileMap.cutCorners);
        if (EditorGUI.EndChangeCheck())
        {
            RecordUndo();
            tileMap.cutCorners = newCutCorners;
            updateConnections = true;
            SceneView.RepaintAll();
        }
    }

    //Draw path tiles
    
    EditorGUI.BeginChangeCheck();
    drawPathMap = EditorGUILayout.Toggle("Draw Path Map", drawPathMap);
    if (EditorGUI.EndChangeCheck())
        SceneView.RepaintAll();*/
    }

    #endregion

    #region Scene GUI

    void OnSceneGUI()
    {
        //Get tilemap
        if (tileMap == null)
            tileMap = (TileMap)target;
        /*
		//Update paths
		if (updateConnections)
		{
			updateConnections = false;
			tileMap.UpdateConnections();
		}*/

        //Toggle editing
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            editing = !editing;
            EditorUtility.SetDirty(target);
        }

        //Toggle selected tile
        /*if (tileMap.tileSet != null)
		{
			if (e.type == EventType.KeyDown)
			{
				var code = (int)e.keyCode - (int)KeyCode.Alpha1;
				if (code >= 0 && code < tileMap.tileSet.prefabs.Length)
				{
					RecordUndo();
					tileMap.tilePrefab = tileMap.tileSet.prefabs[code];
					e.Use();
					return;
				}
			}
		}*/

        //Draw path nodes
        /*
        if (drawPathMap)
        {
            Handles.color = new Color(0, 0, 1, 0.5f);
            foreach (ListValues lis in tileMap.orders)
            {
                foreach (var instance in lis.instances)
                {
                    var tile = instance.GetComponent<PathTile>();
                    if (tile != null)
                    {
                        Handles.DotCap(0, tile.transform.localPosition, Quaternion.identity, tileMap.tileSize / 17);
                        foreach (var other in tile.connections)
                            if (other != null && tile.GetInstanceID() > other.GetInstanceID())
                                Handles.DrawLine(tile.transform.localPosition, other.transform.localPosition);
                    }
                }
            }
        }*/

        if (editing)
        {
            //Hide mesh
            HideWireframe(true);

            //Quit on tool change
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.Q:
                    case KeyCode.W:
                    case KeyCode.E:
                    case KeyCode.R:
                        return;
                }
            }

            //Quit if panning or no camera exists
            if (Tools.current == Tool.View || (e.isMouse && e.button > 1) || Camera.current == null || e.type == EventType.ScrollWheel)
                return;
			
            //Quit if laying out
            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                return;
            }

            //Update matrices
            Handles.matrix = tileMap.transform.localToWorldMatrix;
            worldToLocal = tileMap.transform.worldToLocalMatrix;

            //Draw axes
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(-tileMap.tileSize, 0, 0), new Vector3(tileMap.tileSize, 0, 0));
            Handles.DrawLine(new Vector3(0, -tileMap.tileSize, 0), new Vector3(0, tileMap.tileSize, 0));

            //Update mouse position
            var plane = new Plane(tileMap.transform.forward, tileMap.transform.position);
            var ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y));
            float hit;
            if (!plane.Raycast(ray, out hit))
                return;
            var mousePosition = worldToLocal.MultiplyPoint(ray.GetPoint(hit));
            cursorX = Mathf.RoundToInt(mousePosition.x / tileMap.tileSize);
            cursorY = Mathf.RoundToInt(mousePosition.y / tileMap.tileSize);

            //Update the state and repaint
            state = UpdateState();
            HandleUtility.Repaint();
            e.Use();
        } else
            HideWireframe(false);
    }

    void HideWireframe(bool hide)
    {
        if (wireframeHidden != hide)
        {
            wireframeHidden = hide;
            foreach (var renderer in tileMap.transform.GetComponentsInChildren<Renderer>())
                EditorUtility.SetSelectedWireframeHidden(renderer, hide);
        }
    }

    #endregion

    #region Update state

    State UpdateState()
    {
        switch (state)
        {
        //Hovering
            case State.Hover:
                DrawGrid();
                DrawRect(cursorX, cursorY, 1, 1, Color.blue, new Color(1, 1, 1, 0f));
                if (e.type == EventType.MouseDown && e.button < 2)
                {
                    cursorClickX = cursorX;
                    cursorClickY = cursorY;
                    deleting = e.button > 0;
                    return State.BoxSelect;
                }
                break;

        //Placing
            case State.BoxSelect:

			//Get the drag selection
                var x = Mathf.Min(cursorX, cursorClickX);
                var y = Mathf.Min(cursorY, cursorClickY);
                var sizeX = Mathf.Abs(cursorX - cursorClickX) + 1;
                var sizeY = Mathf.Abs(cursorY - cursorClickY) + 1;
			
			//Draw the drag selection
                DrawRect(x, y, sizeX, sizeY, Color.white, deleting ? new Color(1, 0, 0, 0.2f) : new Color(0, 1, 0, 0.2f));

			//Finish the drag
                if (e.type == EventType.MouseUp && e.button < 2)
                {
                    if (deleting)
                    {
                        if (e.button > 0)
                            SetRect(x, y, sizeX, sizeY, null, direction, tileMap.zOrder);
                    } else if (e.button == 0)
                    {
                        SetRect(x, y, sizeX, sizeY, tileMap.tilePrefab, direction, tileMap.zOrder);
                    }

                    return State.Hover;
                }
                break;
        }
        return state;
    }

    void DrawGrid()
    {
        var gridSize = 5;
        var maxDist = Mathf.Sqrt(Mathf.Pow(gridSize - 1, 2) * 2) * 0.75f;
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                Handles.color = new Color(1, 1, 1, 1 - Mathf.Sqrt(x * x + y * y) / maxDist);
                var p = new Vector3((cursorX + x) * tileMap.tileSize, (cursorY + y) * tileMap.tileSize, 0);
                Handles.DotCap(0, p, Quaternion.identity, HandleUtility.GetHandleSize(p) * 0.02f);
            }
        }
    }

    void DrawRect(int x, int y, int sizeX, int sizeY, Color outline, Color fill)
    {
        Handles.color = Color.white;
        var min = new Vector3(x * tileMap.tileSize - tileMap.tileSize / 2, y * tileMap.tileSize - tileMap.tileSize / 2, 0);
        var max = min + new Vector3(sizeX * tileMap.tileSize, sizeY * tileMap.tileSize, 0);
        rect [0].Set(min.x, min.y, 0);
        rect [1].Set(max.x, min.y, 0);
        rect [2].Set(max.x, max.y, 0);
        rect [3].Set(min.x, max.y, 0);
        Handles.DrawSolidRectangleWithOutline(rect, fill, outline);
    }

    #endregion

    #region Modifying TileMap

    bool UpdateTile(int index, int z)
    {
        //Destroy existing tile
        if ((tileMap.orders [z]).instances [index] != null)
        {
            Undo.DestroyObjectImmediate((tileMap.orders [z]).instances [index].gameObject);

        }

        //Check if prefab is null
        if ((tileMap.orders [z]).prefabs [index] != null)
        {
            //Place the tile
            var instance = (Transform)PrefabUtility.InstantiatePrefab((tileMap.orders [z]).prefabs [index]);
            instance.parent = tileMap.transform;
            instance.localPosition = tileMap.GetPosition(index, z);
            //LUIS PROBAR si hay q poner negativo
            instance.localRotation = Quaternion.Euler(0, 0, (tileMap.orders [z]).directions [index] * 90);
            (tileMap.orders [z]).instances [index] = instance;
            wireframeHidden = false;
            return true;
        } else
        {
            //Remove the tile
            (tileMap.orders [z]).hashes.RemoveAt(index);
            (tileMap.orders [z]).prefabs.RemoveAt(index);
            (tileMap.orders [z]).directions.RemoveAt(index);
            (tileMap.orders [z]).instances.RemoveAt(index);
            return false;
        }
    }

    void UpdatePositions()
    {
        foreach (ListValues lis in tileMap.orders)
        {
            int z = tileMap.orders.IndexOf(lis);

            for (int i = 0; i < lis.hashes.Count; i++)
                if (lis.instances [i] != null)
                    lis.instances [i].localPosition = tileMap.GetPosition(i, z);
        }
    }

    void UpdateAll()
    {

        int x, y;
        foreach (ListValues lis in tileMap.orders)
        {
            int z = tileMap.orders.IndexOf(lis);

            for (int i = 0; i < lis.hashes.Count; i++)
            {
                tileMap.GetPosition(i, z, out x, out y);
                SetTile(x, y, lis.prefabs [i], lis.directions [i], z);
            }
        }
    }

    void Clear()
    {
        RecordDeepUndo();
        int x, y;
        foreach (ListValues lis in tileMap.orders)
        {
            int z = tileMap.orders.IndexOf(lis);
            while (lis.hashes.Count > 0)
            {
                tileMap.GetPosition(0, z, out x, out y);
                SetTile(x, y, null, 0, z);
            }
        }
    }

    bool SetTile(int x, int y, Transform prefab, int direction, int z)
    {
        int count = tileMap.orders.Count - 1;
        int rest = z - count;
        for (int i = 1; i <= rest; i++)
        {
            tileMap.orders.Insert(count + i, new ListValues());
        }
        
        var hash = tileMap.GetHash(x, y);
        var index = (tileMap.orders [z]).hashes.IndexOf(hash);
        if (index >= 0)
        {
            //Replace existing tile

            (tileMap.orders [z]).prefabs [index] = prefab;
            if (direction < 0)
                (tileMap.orders [z]).directions [index] = Random.Range(0, 4);
            else
                (tileMap.orders [z]).directions [index] = direction;
            return UpdateTile(index, z);
        } else if (prefab != null)
        {
            //Create new tile
            index = (tileMap.orders [z]).prefabs.Count;
            (tileMap.orders [z]).hashes.Add(hash);
            (tileMap.orders [z]).prefabs.Add(prefab);
            if (direction < 0)
                (tileMap.orders [z]).directions.Add(Random.Range(0, 4));
            else
                (tileMap.orders [z]).directions.Add(direction);
            (tileMap.orders [z]).instances.Add(null);
            return UpdateTile(index, z);
            
        } else
        {
            
            return false;
        }


    }

    void SetRect(int x, int y, int sizeX, int sizeY, Transform prefab, int direction, int z)
    {
        RecordDeepUndo();
        for (int xx = 0; xx < sizeX; xx++)
            for (int yy = 0; yy < sizeY; yy++)
                SetTile(x + xx, y + yy, prefab, direction, z);
    }

    #endregion

    #region Undo handling

    void OnUndoRedo()
    {
        UpdatePositions();
        //	updateConnections = true;
    }

    void RecordUndo()
    {
        //	updateConnections = true;
        Undo.RecordObject(target, "TileMap Changed");
    }

    void RecordDeepUndo()
    {
        //	updateConnections = true;
#if UNITY_5
        Undo.RegisterFullObjectHierarchyUndo(target, "TileMap Changed");
#elif UNITY_4_3
        Undo.RegisterFullObjectHierarchyUndo(target);
#else
		Undo.RegisterSceneUndo("TileMap Changed");
#endif

    }

    #endregion

    #region Properties

    Event e
    {
        get { return Event.current; }
    }
    /*
	bool drawPathMap
	{
		get { return EditorPrefs.GetBool("TileMapEditor_drawPathMap", true); }
		set { EditorPrefs.SetBool("TileMapEditor_drawPathMap", value); }
	}*/

    #endregion

    #region Menu items

    [MenuItem("GameObject/Create Other/TileMap")]
    static void CreateTileMap()
    {
        var obj = new GameObject("TileMap");
        obj.AddComponent<TileMap>();
    }

    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        var asset = ScriptableObject.CreateInstance<TileSet>();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
            path = "Assets";
        else if (Path.GetExtension(path) != "")
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        else
            path += "/";
		
        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
        Debug.Log(assetPathAndName);
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;
    }

    #endregion
}
