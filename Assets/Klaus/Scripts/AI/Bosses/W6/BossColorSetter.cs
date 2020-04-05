using UnityEngine;

public class BossColorSetter : MonoBehaviour
{
    public bool mantainOriginalAlpha = true;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        UpdateColor();
    }

    void Update()
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        Color color = BossW6Animator.laserColor;

        if (mantainOriginalAlpha)
            color.a = spriteRenderer.color.a;

        spriteRenderer.color = color;
    }
}
