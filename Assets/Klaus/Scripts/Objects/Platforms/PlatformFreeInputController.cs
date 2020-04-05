using UnityEngine;
using System.Collections;
using Luminosity.IO;
using Rewired;

public class PlatformFreeInputController : PlatformInputController
{
    protected InputActionOld Control2 = InputActionOld.Move_Platform_X;

    protected InputActionOld ControlHorizontal = (InputActionOld.Move_Platform_X);
    protected InputActionOld ControlVertical = (InputActionOld.Move_Platform_Y);


    protected override void Awake()
    {
        move = (IMovePlatform)gameObject.GetComponent(typeof(IMovePlatform));
        Control = InputActionOld.Move_Platform_Y;
    }

    protected override void FixedUpdate()
    {
        ControlPad();
    }
    protected override void ControlPad()
    {
        if (!ManagerPause.Pause && ManagerPlatform.CanMovePlatform)
        {
            bool isClick = !InputEnum.USE_CONTROL && ReInput.players.GetPlayer(0).GetButton(InputEnum.GetInputString(ClickControl));
            if (isClick || InputEnum.USE_CONTROL)
            {
                flickHandlerDirection(new Vector2(ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(ControlHorizontal)),
                                                  ReInput.players.GetPlayer(0).GetAxis(InputEnum.GetInputString(ControlVertical))));
            }
            else
            {
                flickHandlerDirection(Vector2.zero);

            }
        }
        else
        {
            flickHandlerDirection(Vector2.zero);
        }
    }
}
