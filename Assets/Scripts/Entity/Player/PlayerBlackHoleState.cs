using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flytime=0.25f;
    private bool skillUsed;
    private float playerDefaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flytime;
        playerDefaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = playerDefaultGravity;

    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {   
            //·ÉÆðÀ´
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, 0);
            if (!skillUsed)
            {
                SkillManager.instance.blackHole.UseSkill();
                AudioManager.instance.PlaySFX(12, null);
                skillUsed = true;
            }

        }
        if (SkillManager.instance.blackHole.isBlackholeFinished() == true)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
