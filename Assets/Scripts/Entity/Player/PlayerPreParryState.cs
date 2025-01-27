using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreParryState : PlayerState
{
    public PlayerPreParryState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.parryDuration;
        //player.anim.SetBool("Parry", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0,rb.velocity.y);
        if (player.isHit)
        {
            stateMachine.ChangeState(player.parryState);
            return;
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
    
