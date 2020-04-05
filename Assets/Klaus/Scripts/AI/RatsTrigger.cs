using UnityEngine;

public class RatsTrigger : MonoBehaviour
{
    public BoolCircle2DOverlap[] circles;
    public LayerMask enterMask, exitMask;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (BoolCircle2DOverlap circle in circles)
                circle.whatIsCollide = enterMask;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (BoolCircle2DOverlap circle in circles)
                circle.whatIsCollide = exitMask;
        }
    }
}
