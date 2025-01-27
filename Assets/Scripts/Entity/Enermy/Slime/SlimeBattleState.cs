using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState :SlimeState
{

    private Transform player;

    //׷������
    private int moveDirection;
    public SlimeBattleState(Enermy_Slime _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enermy.battleExitTime;
        //��ȡ׷����ҵ�λ��
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
            //playerλ���ұ�
            moveDirection = 1;
        }
        else if (player.position.x < rb.position.x)
        {
            //playerλ�����
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
                //��⵽���ͬʱ���ڹ�����Χ֮��
                if (ableToAttack())
                {
                    //�ж���ȴ�Ƿ����,���빥��
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
