using UnityEngine;
using System.Collections;


public class SetRandomPosition : MonoBehaviour
{

    public bool useX = true;
    public float minX = 0;
    public float maxX = 17;
    public bool useY = false;
    public float minY = 0;
    public float maxY = 17;

    public void SetPositionRandom()
    {
        float newX = transform.localPosition.x;
        if (useX)
            newX = Random.Range(minX, maxX);
        float newY = transform.localPosition.y;
        if (useY)
            newY = Random.Range(minY, maxY);

        transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
         
    }


}
