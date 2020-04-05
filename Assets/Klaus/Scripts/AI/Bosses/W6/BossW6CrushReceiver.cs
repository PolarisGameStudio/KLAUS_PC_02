using UnityEngine;

public class BossW6CrushReceiver : MonoBehaviour, ICrushObject
{
    public BossW6Controller controller
    {
        get
        {
            if (_controller == null)
                _controller = GameObject.FindObjectOfType<BossW6Controller>();
            return _controller;
        }
    }

    BossW6Controller _controller;

    public void Crush(TypeCrush type = TypeCrush.Middle)
    {
        controller.Crush();
    }
}
