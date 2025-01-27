using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyState 
{
    protected Enermy enermy;
    protected EnermyStateMachine stateMachine;
    public string aniBoolName;

    protected Rigidbody2D rb;

    protected bool triggerCalled;
    protected float stateTimer;
    public EnermyState(Enermy _enermy,EnermyStateMachine _stateMachine,string _aniBoolName)
    {
        this.enermy = _enermy;
        this.stateMachine = _stateMachine;
        this.aniBoolName = _aniBoolName;
    }
    public virtual void Enter()
    {
        rb = enermy.rb;
        triggerCalled = false;
        enermy.anim.SetBool(aniBoolName, true);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enermy.anim.SetBool(aniBoolName, false);
    }
    protected virtual void BattleEnterController()
    {

    }
}
