using UnityEngine;

public class ResetBossFight : CheckPoint
{
    public void AddFightCheckpoint(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersID id = other.GetComponent<PlayerInfo>().playerType;
            if (!isUsed.ContainsKey(id))
                isUsed[id] = false;

            if (!isUsed[id])
            {
                if (sprite) sprite.color = onColor;
                RotateArrow();
                ManagerCheckPoint.Instance.AddPosition(id, this);
                isUsed[id] = true;
            }
        }
    }

    public override void CheckPointUsed()
    {
        switch (Application.loadedLevelName)
        {
            case "W1BossFight":
            case "W1BossFight02":
                BossW1Controller bossW1 = GameObject.FindObjectOfType<BossW1Controller>();
                if (bossW1) bossW1.ResetBoss();
                break;
            case "W4BossFight":
            case "W4BossFight2":
                BossW4Controller bossW4 = GameObject.FindObjectOfType<BossW4Controller>();
                if (bossW4) bossW4.ResetBoss();
                break;
            case "W6L06-1":
                BossW6Controller bossW6 = GameObject.FindObjectOfType<BossW6Controller>();
                if (bossW6) bossW6.ResetBoss();
                break;
        }
    }
}
