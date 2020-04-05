using UnityEngine;
using System.Collections;

public class PlayerMoveAI : MonoBehaviour
{
    [Range(-1, 1)]
    public float DirecctionX;
    public bool isFast = true;
    public MoveState move;


    void OnEnable()
    {
        move.SetButton(move.RunButtonValue, true);

    }
    void OnDisable()
    {
        move.SetButton(move.RunButtonValue, false);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            if (DirecctionX > 0)
            {
                move.SetMovement(Vector2.right);

            }
            else if (DirecctionX < 0)
            {
                move.SetMovement(Vector2.right * -1);

            }
            else
            {
                move.SetMovement(Vector2.zero);

            }
        }

    }
}
