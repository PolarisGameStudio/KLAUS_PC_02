using UnityEngine;

public class BossW4AnimationControl : MonoBehaviour
{
    public BossW4Controller controller;

    public void StartShake()
    {
        CameraShake.Instance.StartShake();
        controller.EnableBlur();
    }

    public void StopShake()
    {
        CameraShake.Instance.StopShake();
        controller.DisableBlur();
    }

    public void ShowOutroDialogue(int index)
    {
        controller.ShowOutroDialogue(index);
    }

    public void RunToCPU()
    {
        controller.RunToCPU();
    }
}
