using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState :SlimeState
{

    private Transform player;

    //追击方向
    private int moveDirection;
    public SlimeBattleState(Enermy_Slime _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enermy.battleExitTime;
        //获取追击玩家的位置
        player = enermy.player;

    }
    public override void Update()
    {
        base.Update();

        moveController();
        attackEnterController();
        battleExitController();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public bool ableToAttack()
    {
        return Time.time - enermy.lastAttackTime > enermy.attackIntervalTime;
    }
    public void battleExitController()
    {
        if (stateTimer < 0 || Vector2.Distance(player.position, enermy.transform.position) > enermy.battleExitDistance)
        {

            stateMachine.ChangeState(enermy.idleState);
        }
    }
    public void moveController()
    {
        if (player.position.x > rb.position.x)
        {
            //player位于右边
            moveDirection = 1;
        }
        else if (player.position.x < rb.position.x)
        {
            //player位于左边
            moveDirection = -1;
        }

        if (Vector2.Distance(player.transform.position, rb.position) < enermy.attackDistance || !enermy.isPreGroundDetected())
        {
            if (enermy.facingRight && moveDirection == -1)
            {
                enermy.SetVelocity(enermy.battleMoveSpeed * moveDirection, rb.velocity.y);
            }
            else if (!enermy.facingRight && moveDirection == 1)
            {
                enermy.SetVelocity(enermy.battleMoveSpeed * moveDirection, rb.velocity.y);
            }
            else
            {
                enermy.SetVelocity(0, rb.velocity.y);
            }
        }
        else
        {
            enermy.SetVelocity(enermy.battleMoveSpeed * moveDirection, rb.velocity.y);
        }

    }
    public void attackEnterController()
    {
        if (enermy.isPlayerDetected())
        {

            if (enermy.isPlayerDetected().distance < enermy.attackDistance)
            {
                //检测到玩家同时处于攻击范围之内
                if (ableToAttack())
                {
                    //判断冷却是否结束,进入攻击
                    stateMachine.ChangeState(enermy.attackState);

                }
                else
                {
                    stateMachine.ChangeState(enermy.waitAttackState);
                }
            }
        }
    }
}
