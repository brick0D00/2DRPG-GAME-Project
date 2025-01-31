using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeState : EnermyState
{   
    new protected Enermy_Slime enermy;
    public SlimeState(Enermy_Slime _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
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
        return stateMachine.currentState != enermy.attackState && stateMachine.currentState != enermy.stunnedState && stateMachine.currentState != enermy.deadState && stateMachine.currentState!=enermy.waitAttackState;
    }
    private bool DistanceShortEnough()
    {
        return Vector2.Distance(enermy.player.position, enermy.transform.position) < enermy.battleEnterDistance;
    }
    #endregion

}
