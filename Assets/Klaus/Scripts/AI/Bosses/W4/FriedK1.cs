using UnityEngine;

public class FriedK1 : MonoBehaviour
{
    public GameObject[] effects;
    public Color burnedColor;
    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    SpriteRenderer _spriteRenderer;

    public void K1IsFried()
    {
        spriteRenderer.color = burnedColor;

        foreach (GameObject effect in effects)
            effect.SetActive(true);
    }
}
