using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerState : EnermyState
{
    new protected Enermy_DeathBringer enermy;
    public DeathBringerState(Enermy_DeathBringer _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
        this.enermy = _enermy;
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        base.Update();
        BattleEnterController();
    }

    public override void Exit()
    {
        base.Exit();
    }
    #region BattleEnterController
    protected override void BattleEnterController()
    {
        base.BattleEnterController();
        if (enermy.isPlayerDetected() && CheckState() && DistanceShortEnough())
        {
            stateMachine.ChangeState(enermy.battleState);
            return;
        }
    }
    private bool CheckState()
    {   //TODO:�����ڹ����Լ�ѣ�κ�����״̬

        return stateMachine.currentState != enermy.attackState && stateMachine.currentState != enermy.deadState&&stateMachine.currentState!=enermy.waitAttackState&&stateMachine.currentState!=enermy.castState;
    }
    private bool DistanceShortEnough()
    {
        return Vector2.Distance(enermy.player.position, enermy.transform.position) < enermy.battleEnterDistance;
    }
    #endregion

}
