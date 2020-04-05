using UnityEngine;
using System.Collections;

public class NoMouse : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D cursorTexture2;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;
    void OnMouseEnter()
    {
    //     Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
       //  Cursor.visible = true;
    }
    void OnMouseExit()
    {
    //     Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
    void Start()
    {
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
 /*/
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
          {
         //   Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
         }

         else 
          {
         //     Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
          }
/*/
    }
}