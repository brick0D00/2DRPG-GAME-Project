using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
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
    {   //TIPS:这里状态转换后需要return避免继续执行
        base.Update();

        player.SetVelocity(0, rb.velocity.y * 0.8f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        if (player.isGroundDetected() || xInput == -player.facingDirection || !player.isWallDetected()) 
        {
            Debug.Log("exit");
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
