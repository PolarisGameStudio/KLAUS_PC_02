using UnityEngine;

public class BossW6Animator : MonoBehaviour
{
    public BossW6Controller controller;
    public LayerMask ground;
    public Transform hand1, hand2;

    public static Color laserColor;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f);
            if (hit.collider != null)
                controller.GenerateWaves(hit.point);
        }
    }

    public void LowerPlatform()
    {
        controller.LowerPlatform();
    }

    public void OnPunchTouchedFloor()
    {
        controller.OnPunchTouchedFloor();
    }

    public void OnCurrentAnimationEnded()
    {
        controller.OnCurrentAnimationEnded();
    }

    public void ShowBoss()
    {
        controller.ShowBoss();
    }

    public void ToggleFists()
    {
        controller.ToggleHands();
    }

    public void ToggleHands()
    {
        controller.TogglePlatformHands();
    }

    public void CannonsShot()
    {
        controller.CannonsShot();
    }

    public void TurnOffPlatform()
    {
        controller.TurnOffPlatform();
    }

    public void CutsceneEnded()
    {
        controller.CutsceneEnded();
    }

    public void BossDead()
    {
        controller.BossDead();
    }

    public void ShowEndDialogue()
    {
        controller.ShowEndDialogue();
    }

    public void ShowBossBanner()
    {
        controller.ShowBossBanner();
    }

    public void HideCurrentDialogue()
    {
        controller.HideCurrentDialogue();
    }
}
