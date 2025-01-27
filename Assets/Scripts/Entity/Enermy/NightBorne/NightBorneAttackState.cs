using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneAttackState : NightBorneState
{
    public NightBorneAttackState(Enermy_NightBorne _enermy, EnermyStateMachine _stateMachine, string _aniBoolName) : base(_enermy, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        base.Update();

        enermy.SetVelocity(0, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        enermy.lastAttackTime = Time.time;
    }
    public void OpenParryWindow()
    {
        enermy.OpenParryWindow(0.4f);
    }
    public void CloseParryWindow()
    {
        enermy.CloseParryWindow();
    }
}
