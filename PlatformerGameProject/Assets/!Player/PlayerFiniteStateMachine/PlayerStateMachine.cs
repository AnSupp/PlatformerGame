using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//отвечает за установку и смену состояний
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
