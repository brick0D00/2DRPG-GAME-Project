using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //生成分身
        player.AddInvincibleList(1);
        SkillManager.instance.dash.CreateCloneAfterDash();

        stateTimer = player.dashSetTime;
        player.SetVelocity(player.dashDirection * player.dashSpeed, 0);
    
    }

    public override void Exit()
    {
        player.RemoveInvincibleList(1);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGroundDetected() && player.isWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        if (stateTimer < 0 && !player.isGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        if (stateTimer < 0 && player.isGroundDetected() )
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        player.fx.CreateAfterImage();
    }
}
