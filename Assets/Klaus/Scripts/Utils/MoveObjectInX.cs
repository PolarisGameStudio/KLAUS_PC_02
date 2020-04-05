using UnityEngine;
using System.Collections;

public class MoveObjectInX : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float minX = -10;
    public float maxX = 10f;

    protected Vector3 StartPos = Vector3.zero;

    void OnEnable()
    {
        StartPos = transform.position;
    }

    void OnDisable()
    {
        transform.position = StartPos;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));

        if (transform.position.x > maxX)
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
    }

}
