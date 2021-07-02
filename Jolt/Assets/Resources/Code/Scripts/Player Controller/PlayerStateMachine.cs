using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public string LastState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        LastState = CurrentState.ToString();
        Initialize(newState);
    }

    public string GetState()
    {
        return CurrentState.ToString();
    }
}
