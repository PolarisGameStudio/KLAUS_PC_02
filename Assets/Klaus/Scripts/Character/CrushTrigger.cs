using UnityEngine;
using System.Collections;

public class CrushTrigger : MonoBehaviour
{
    public CrushState state;
    public CFX_AutoDestructShuriken particlePunch;

    void OnTriggerEnter2D(Collider2D other)
    {
        ICrushObject objecCrus = other.GetComponent<ICrushObject>();
        if (objecCrus != null)
        {
            if (particlePunch != null)
            {
                particlePunch.transform.position = other.transform.position;
                particlePunch.gameObject.SetActive(true);
            }
            objecCrus.Crush(state != null ? state.typeC : TypeCrush.Middle);
        }
        else
        {
            if (other.CompareTag("Resorte"))
            {
                state.FinishDownCrush();
            }
        }
    }
}
