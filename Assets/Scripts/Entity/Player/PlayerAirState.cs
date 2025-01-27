using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{   //ָ����������ڿ��е�ʱ��
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
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
        
        if (player.isWallDetected()&&xInput!=0)
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (player.isGroundDetected()) 
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
