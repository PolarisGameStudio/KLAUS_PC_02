using UnityEngine;

public class Blink : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool fadeInOut;

    float timer, speed;
    Color initialColor, endColor;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        float value = Mathf.PingPong(timer * speed, 1f);
        SetColor(Color.Lerp(endColor, initialColor, fadeInOut ? value : Mathf.Round(value)));
        timer += Time.deltaTime * Time.timeScale;
    }

    public virtual void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public virtual Color GetColor()
    {
        return spriteRenderer.color;
    }

    public void Play(float speed, Color initialColor, Color endColor)
    {
        this.speed = speed;
        this.initialColor = initialColor;
        this.endColor = endColor;

        enabled = true;
    }

    public void Stop()
    {
        enabled = false;
    }
}
