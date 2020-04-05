using UnityEngine;

public class BossPlatform : PlatformAISingleRute
{
    [HideInInspector]
    public bool hacked;

    public GameObject[] boxes;

    public bool CanMove()
    {
        foreach (GameObject box in boxes)
            if (box.activeSelf)
                return false;

        return true;
    }

    public void SetSpot(int point)
    {
        if (currentPath == point)
            return;
        
        currentPath = point;
        enabled = true;
    }
}
