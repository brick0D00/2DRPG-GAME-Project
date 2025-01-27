using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{   
    //get private set�����ɶ�����д
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState)
    {   //TODO:״̬���ĳ�ʼ������ֵ�����ó�ʼ��Enter
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
