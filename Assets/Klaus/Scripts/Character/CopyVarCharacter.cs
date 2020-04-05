using UnityEngine;
using System.Collections;

public class CopyVarCharacter : MonoBehaviour {
#if UNITY_EDITOR
    public bool CopyValuesFromKlaus = true;
    public MoveStateKlaus CopyMove;
    public MoveStateK1 PasteMove;

    public Rigidbody2D CopyRigidbody;
    public Rigidbody2D PasteRigidbody;
#endif
}
