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

            //��ҵĽ�ռλӦ���ǿյ�
        if (Input.GetKeyDown(KeyCode.Mouse1)&&SwordCheck()&&SkillManager.instance.throwSword.isSwordUnlocked)
        {   
            //TODO:�Ҽ���׼��������
            stateMachine.ChangeState(player.aimSwordState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {   
            //TODO:�������
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
            //TODO:�ո���Ծ
            stateMachine.ChangeState(player.jumpState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //TODO:��Q����
            SkillManager.instance.parry.TryToUseSkill();
            return;
        }
        
    }
    private bool SwordCheck()
    {   
        //TODO:����ʡ�Դ���ֻͨ���Ҽ��Ϳ���ִ��

        if (player.sword == null)
        {
            return true;
        }
        player.sword.GetComponent<SwordController>().CallSwordReturn();
        return false;
    }
}
