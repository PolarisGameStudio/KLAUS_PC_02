using Luminosity.IO;

public interface IButtonState
{

	// Use this for initialization
    void SetButton(InputActionOld button, bool value);
    void SetButtonUp(InputActionOld button, bool value);
    void SetButtonDown(InputActionOld button, bool value);
    void SetButton(InputActionOld button, float value);
}