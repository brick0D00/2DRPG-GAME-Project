using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerMoveState : DeathBringerState
{
    public DeathBringerMoveState(Enermy_DeathBringer _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enermy.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (enermy.isHit)
        {
            stateMachine.ChangeState(enermy.battleState);
            return;
        }

        enermy.SetVelocity(enermy.facingDirection * enermy.idleMoveSpeed, rb.velocity.y);

        if (enermy.isWallDetected() || (!enermy.isPreGroundDetected() && enermy.isGroundDetected()))
        {
            enermy.Flip();
            stateMachine.ChangeState(enermy.idleState);
        }

    }
}
