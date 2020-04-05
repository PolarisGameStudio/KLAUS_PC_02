using UnityEngine;
using System.Collections;

public class PlatformArrowAnim : MonoBehaviour {
    public PlatformMovement move;

    public SpriteRenderer ArrowPos;
    public SpriteRenderer ArrowNeg;

    public Color activeColor;
    public Color nonActiveColor;
    void Awake()
    {
        if (move)
        {
            if (move.isV)
            {
                ArrowPos.transform.rotation = Quaternion.Euler(0, 0, 0);
                ArrowNeg.transform.rotation = Quaternion.Euler(0, 0, -180);

            }
            else
            {
                ArrowPos.transform.rotation = Quaternion.Euler(0, 0, -90);
                ArrowNeg.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }
	// Use this for initialization
	void Start () {
        ResetMove();

	}
	
	// Update is called once per frame
	public void PositiveMove () {
        ArrowNeg.color = nonActiveColor;

        ArrowPos.color = activeColor;

	}

    public void NegativeMove()
    {
        ArrowPos.color = nonActiveColor;
        ArrowNeg.color = activeColor;

    }

    public void ResetMove()
    {
        ArrowPos.color = nonActiveColor;
        ArrowNeg.color = nonActiveColor;
    }
}
