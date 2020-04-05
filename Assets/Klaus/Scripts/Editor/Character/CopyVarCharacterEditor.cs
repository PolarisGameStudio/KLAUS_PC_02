using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CopyVarCharacter))]
public class CopyVarCharacterEditor : Editor
{
    CopyVarCharacter changuer;
    void Awake()
    {
        changuer = ((CopyVarCharacter)target);

    }
    void OnEnable()
    {
        changuer.CopyMove = GameObject.FindObjectOfType<MoveStateKlaus>();
        changuer.PasteMove = GameObject.FindObjectOfType<MoveStateK1>();
        if (changuer.CopyMove != null && changuer.PasteMove != null)
        {
            changuer.CopyRigidbody = changuer.CopyMove.GetComponent<Rigidbody2D>();
            changuer.PasteRigidbody = changuer.PasteMove.GetComponent<Rigidbody2D>();
        }
    }
    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
            serializedObject.Update();
            GUI.changed = false;
            changuer.CopyValuesFromKlaus = EditorGUILayout.Toggle("Copy Values from Klaus??", changuer.CopyValuesFromKlaus);
            if (changuer.CopyValuesFromKlaus)
            {
                if (changuer.CopyMove != null && changuer.PasteMove != null)
                {
                    changuer.PasteMove.maxSpeedX = changuer.CopyMove.maxSpeedX;
                    changuer.PasteMove.speedX = changuer.CopyMove.speedX;
                    changuer.PasteMove.percentPlatformMoveOposDir = changuer.CopyMove.percentPlatformMoveOposDir;
                    changuer.PasteMove.percentPlatformMoveSameDir = changuer.CopyMove.percentPlatformMoveSameDir;
                    changuer.PasteMove.jumpPercentOfForce = changuer.CopyMove.jumpPercentOfForce;
                    changuer.PasteMove.TimeToResetJumpingVar = changuer.CopyMove.TimeToResetJumpingVar;
                    changuer.PasteMove.timeForHalfJump = changuer.CopyMove.timeForHalfJump;
                    changuer.PasteMove.percentOfJumpForHalfJump = changuer.CopyMove.percentOfJumpForHalfJump;
                    changuer.PasteMove.RoceAirX = changuer.CopyMove.RoceAirX;
                    changuer.PasteMove.TimeAfterImpulse = changuer.CopyMove.TimeAfterImpulse;
                    changuer.PasteMove.PercentLessToSpeed = changuer.CopyMove.PercentLessToSpeed;
                    changuer.PasteMove.ForceImpulse = changuer.CopyMove.ForceImpulse;
                    changuer.PasteMove.percentOfDoubleJump = changuer.CopyMove.percentOfDoubleJump;

                    if (changuer.PasteRigidbody == null || changuer.CopyRigidbody == null)
                    {
                        changuer.CopyRigidbody = changuer.CopyMove.GetComponent<Rigidbody2D>();
                        changuer.PasteRigidbody = changuer.PasteMove.GetComponent<Rigidbody2D>();
                    }
                    changuer.PasteRigidbody.gravityScale = changuer.CopyRigidbody.gravityScale;

                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(changuer.gameObject);

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
