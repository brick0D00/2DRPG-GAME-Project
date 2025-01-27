using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneRecoverState : NightBorneState
{
    public NightBorneRecoverState(Enermy_NightBorne _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enermy.AddInvincibleList(1);
        enermy.canRecover = false;
        enermy.stats.HealMyselfWith(enermy.stats.Caculate_MaxHealthValue());
        enermy.SetVelocity(0,enermy.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enermy.RemoveInvincibleList(1);

        //½øÈë¿ñ±©
        enermy.sr.color = Color.red;
        enermy.stats.strength.AddModifier(enermy.stats.strength.GetValue() / 2);
        enermy.attackIntervalTime = 0;
    }

    public override void Update()
    {
        base.Update();
    }
}
