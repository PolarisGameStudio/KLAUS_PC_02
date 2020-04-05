using UnityEngine;
using System.Collections;

public class WallJumpTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WallState wall = other.GetComponent<WallState>();
            if (wall.getLegs() == other)
            {
                wall.SetInWall(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WallState wall = other.GetComponent<WallState>();
            if (wall.getLegs() == other)
            {
                wall.SetInWall(false);
            }
        }
    }
}
