using UnityEngine;
using System.Collections;


/// <summary>
/// Class used for keeping a flare emitter at constant distance to a <c>Transform</c>.
/// Note: The position of the flare can not be changed at runtime.
/// </summary>
public class ConstantDistance : MonoBehaviour 
{
    //------------------------------
    // Public Fields
    //------------------------------  

    /// <summary>
    /// The target <c>Transform</c> (usually the main camera or avatar).
    /// </summary>
    public Transform Target;

    /// <summary>
    /// The constant distance to the specified <c>Target</c> in units.
    /// </summary>
    public float Distance = 10;



    //------------------------------
    // Private Fields
    //------------------------------  

    /// <summary>
    /// The flare emitter <c>Transform</c>.
    /// </summary>
    private Transform FlareEmitter;
    private Vector3 initialDirection;
    private Vector3 worldPosition;
    private Vector3 localDirection;
    private Vector3 localPosition;
    private Quaternion lastRotation;

    //------------------------------
    // Methods
    //------------------------------

	public void Start() 
    {
        FlareEmitter = this.transform;

        // Set initial direction from target to emitter
        initialDirection = (FlareEmitter.position - Target.position).normalized;

        CalculateLocals();
        CalculateWorldPosition();

        this.transform.position = worldPosition;

	}

    public void Update()
    {        
        if (!lastRotation.Equals(Target.transform.rotation))
        {
            CalculateLocals();
        }

        CalculateWorldPosition();


        this.transform.position = worldPosition;
        this.lastRotation = Target.transform.rotation;
    }

    private void CalculateLocals()
    {
        // Transform direction to local space
        localDirection = Target.InverseTransformDirection(initialDirection);

        // Offset local position
        localPosition = localDirection * Distance;
    }

    private void CalculateWorldPosition()
    {
        // Transform to world space
        worldPosition = Target.TransformPoint(localPosition);
    }
}
