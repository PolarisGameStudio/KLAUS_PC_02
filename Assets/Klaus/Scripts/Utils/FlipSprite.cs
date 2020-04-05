using UnityEngine;
using System.Collections;

public class FlipSprite : MonoBehaviour {
    /// <summary>
    /// if facing right the sprite
    /// </summary>
    public bool facingRight = true;

    public SpriteRenderer sprite;

    public bool isFacingRight
    {
        get
        {
            return facingRight;
        }
    }

    void Awake()
    {
        Vector3 theScale = sprite.transform.localScale;

        if (facingRight) {
            theScale.x = 1;
        }
        else {
            theScale.x = -1;
        }
        sprite.transform.localScale = theScale;

    }

    public void FlipIfCanFlip(Vector2 inputDirection)
    {
        if (inputDirection.x > 0 && !facingRight)
            Flip();
        else if (inputDirection.x < 0 && facingRight)
            Flip();

    }
    /// <summary>
    /// Flip this instance.
    /// </summary>
    protected void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = sprite.transform.localScale;
        theScale.x *= -1;
        sprite.transform.localScale = theScale;
    }

    public static void ApplyFlip(Transform flipObj)
    {
        Vector3 theScale = flipObj.transform.localScale;
        theScale.x *= -1;
        flipObj.transform.localScale = theScale;
    }
}
