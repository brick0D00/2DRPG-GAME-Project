using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerDeadState : DeathBringerState
{
    public DeathBringerDeadState(Enermy_DeathBringer _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
