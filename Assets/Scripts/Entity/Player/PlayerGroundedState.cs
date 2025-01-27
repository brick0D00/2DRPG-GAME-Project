using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
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

        if(Input.GetKeyDown(KeyCode.Z)&&SkillManager.instance.blackHole.IsSkillReady()) 
        {
            stateMachine.ChangeState(player.blackHoleState);   
        }

            //玩家的剑占位应该是空的
        if (Input.GetKeyDown(KeyCode.Mouse1)&&SwordCheck()&&SkillManager.instance.throwSword.isSwordUnlocked)
        {   
            //TODO:右键瞄准将剑丢出
            stateMachine.ChangeState(player.aimSwordState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {   
            //TODO:左键攻击
            stateMachine.ChangeState(player.primaryAttack);
            return;
        }
        if (!player.isGroundDetected())
        {   
            stateMachine.ChangeState(player.airState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&player.isGroundDetected())
        {   
            //TODO:空格跳跃
            stateMachine.ChangeState(player.jumpState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //TODO:按Q弹反
            SkillManager.instance.parry.TryToUseSkill();
            return;
        }
        
    }
    private bool SwordCheck()
    {   
        //TODO:做到省略代码只通过右键就可以执行

        if (player.sword == null)
        {
            return true;
        }
        player.sword.GetComponent<SwordController>().CallSwordReturn();
        return false;
    }
}
