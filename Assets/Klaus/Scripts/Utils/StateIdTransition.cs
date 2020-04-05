//
// StateIdTransition.cs
//
// Author:
//       Luis Alejandro Vieira <lavz24@gmail.com>
//
// Copyright (c) 2014 
//

/// <summary>
/// Place the labels for the Transitions in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum Transition
{
    NullTransition,
    // Use this transition to represent a non-existing transition in your system
    MoveToDead,
    Move_Ladder,
    Dead_WallJump,
    Move_WallJump,
    DeadToMove,
    MoveToCode,
    MoveToCrush,
    CodeToMove,
    CrushToMove,
    MoveToEnter,
    MoveToCrouch,
    CrouchToMove,
    CrouchToDead,
    ThrowToMove,
    MoveToThrow,
    PlayToPause,
    PlayToCompleted,
    PlayToUncompleted,
    AI_IdleToPersue,
    AI_PersueToIdle,
    AI_IdleToDead,
    AI_PersueToDead,
    AI_DeadToIdle,
    AI_DeadToPersue,
    AI_FollowToDead,
    AI_FollowToIdle,
    AI_FollowToPersue
}

/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum StateID
{
    NullStateID,
    // Use this ID to represent a non-existing State in your system
    Move,
    Dead,
    EnterDoor,
    Code,
    Crouch,
    Throw,
    Crush,
    Play,
    Pause,
    Completed,
    UnCompleted,
    Ladder,
    WallJump,
    AI_Idle,
    AI_Persue,
    AI_Dead,
    AI_BlowUp,
    AI_Shoot,
    AI_Follow
}