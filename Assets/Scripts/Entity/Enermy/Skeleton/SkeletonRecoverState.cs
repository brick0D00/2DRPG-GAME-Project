using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRecoverState : SkeletonState
{
    public SkeletonRecoverState(Enermy_Skeleton _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        enermy.SetVelocity(0, 0);

    }

    public override void Exit()
    {
        base.Exit();
        enermy.SetVelocity(0, enermy.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
    }
}

