using UnityEngine;

public class BossHack : IHack
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
    public BossPlatform platform;

    public override void HackedSystem()
    {
        controller.SystemHacked(platform);
        platform.SetSpot(1);
    }
}
