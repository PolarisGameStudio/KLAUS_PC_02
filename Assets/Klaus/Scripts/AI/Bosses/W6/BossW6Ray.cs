using UnityEngine;

public class BossW6Ray : MonoBehaviour
{
    public GameObject warningRay, attackRay;

    public void Show(bool warning = true)
    {
        warningRay.SetActive(warning);
        attackRay.SetActive(!warning);
    }

    public void Hide()
    {
        warningRay.SetActive(false);
        attackRay.SetActive(false);
    }
}
