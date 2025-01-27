using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerWaitAttackState : DeathBringerState
{
    private Transform player;


    public DeathBringerWaitAttackState(Enermy_DeathBringer _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player=enermy.player;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enermy.isPlayerDetected().distance > enermy.attackDistance)
        {
            {
                stateMachine.ChangeState(enermy.battleState);
                return;
            }
        }
        if (ableToAttack())
        {
            //ÅÐ¶ÏÀäÈ´ÊÇ·ñ½áÊø,½øÈë¹¥»÷
            stateMachine.ChangeState(enermy.attackState);
        }
    }

    public bool ableToAttack()
    {
        return Time.time - enermy.lastAttackTime > enermy.attackIntervalTime;
    }
}
