using System;
using UnityEngine;

/* Implementation note
 * A more elegant approach might be to use the generic form
 * StateBehaviour<T> : MonoBehaviour where T : Enum
 * Unfortunately the C# CLR doesn't support enums as type constraints and probably never will. 
 */

[RequireComponent(typeof(StateEngine))]
public class StateBehaviour : MonoBehaviour
{
    public StateEngine stateMachine
    {
        get
        {
            // Guaranteed to be availble thanks to RequireComponent
            if (_stateMachine == null)
                _stateMachine = GetComponent<StateEngine>();

            // This happens when we forget to inherit from StateBehaviour and change it after the script has been added to a game object.
            if (_stateMachine == null)
                throw new Exception("Please make sure StateEngine is also present on any StateBehaviour objects");

            return _stateMachine;
        }
    }

    StateEngine _stateMachine;

    public Enum GetState()
    {
        return stateMachine.GetState();
    }

    protected void Initialize<T>()
    {
        stateMachine.Initialize<T>(this);
    }

    protected virtual void ChangeState(Enum newState)
    {
        stateMachine.ChangeState(newState);
    }

    protected virtual void ChangeState(Enum newState, StateTransition transition)
    {
        stateMachine.ChangeState(newState, transition);
    }
}