using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonState
{   
    public SkeletonIdleState(Enermy_Skeleton _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enermy.SetVelocity(0, rb.velocity.y);
        stateTimer = enermy.idleSetTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enermy.isHit)
        {
            stateMachine.ChangeState(enermy.battleState);
            return;
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enermy.moveState);
        }
    }
}
