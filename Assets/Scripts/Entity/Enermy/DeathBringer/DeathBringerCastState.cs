using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerCastState : DeathBringerState
{
    public DeathBringerCastState(Enermy_DeathBringer _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enermy.castStateDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {   
        //TODO:召唤小怪或者巫术攻击
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enermy.battleState);
            return;
        }
    }
}
