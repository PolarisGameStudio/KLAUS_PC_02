using UnityEngine;

public class NoiseAnimator : MonoBehaviour
{
    public float rate = 0.2f;
    float timer = 0;

    void OnEnable()
    {
        timer = rate;
    }

    void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0)
        {
            AnimateNoise();
            timer = rate;
        }
    }

    void AnimateNoise()
    {
        Vector3 scale = transform.localScale;

        int random = Random.Range(1, 4);

        if (random == 1)
        {
            scale.x = -scale.x;
        }
        else if (random == 2)
        {
            scale.y = -scale.y;
        }
        else
        {
            scale.x = -scale.x;
            scale.y = -scale.y;
        }

        transform.localScale = scale;
    }
}
