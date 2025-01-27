using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{   
    //get private set做到可读不可写
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState)
    {   //TODO:状态机的初始化，赋值并调用初始的Enter
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
