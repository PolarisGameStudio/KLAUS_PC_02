using UnityEngine;

public class LoopObject : MonoBehaviour
{
    public Transform firstObject, secondObject;
    public bool repeatInX = true, repeatInY, repeatInZ;
    public bool ignoreTimescale;
    public float speed = 1f;
    public bool pingPong;

    Vector3 value;

    void OnEnable()
    {
        if (firstObject == null || secondObject == null)
        {
            Debug.LogError("LoopObject: " + name + " doesn't have a firstObject or secondObject assigned.");
            enabled = false;
        }

        value = Vector3.zero;
    }

    void Update()
    {
        Vector3 position = transform.position;
        float delta = Time.deltaTime * speed * (ignoreTimescale ? 1 : Time.timeScale);

        if (repeatInX)
        {
            value.x += delta;
            position.x = Evaluate(value.x, firstObject.position.x, secondObject.position.x);
        }

        if (repeatInY)
        {
            value.y += delta;
            position.y = Evaluate(value.y, firstObject.position.y, secondObject.position.y);
        }

        if (repeatInZ)
        {
            value.z += delta;
            position.z = Evaluate(value.z, firstObject.position.z, secondObject.position.z);
        }

        transform.position = position;
    }

    float Evaluate(float value, float min, float max)
    {
        if (pingPong)
            return Mathf.PingPong(value - min, max - min) + min;
        else
            return Mathf.Repeat(value - min, max - min) + min;
    }
}
