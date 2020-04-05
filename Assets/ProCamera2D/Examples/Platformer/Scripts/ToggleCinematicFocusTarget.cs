using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace Com.LuisPedroFonseca.ProCamera2D.Platformer
{
    public class ToggleCinematicFocusTarget : MonoBehaviour
    {
        public ProCamera2DCinematicFocusTarget Target;

        void OnGUI()
        {
            if (GUI.Button(new Rect(5, 5, 180, 30), (Target.IsActive ? "Disable" : "Enable") + " Cinematic Focus"))
            {
                if (Target.IsActive)
                    Target.Disable();
                else
                    Target.Enable();
            }
        }
    }
}