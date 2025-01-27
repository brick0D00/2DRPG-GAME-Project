using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_NightBorne : Enermy
{
    #region State
    public NightBorneIdleState idleState {  get; private set; }
    public NightBorneMoveState moveState { get; private set; }
    public NightBorneAttackState attackState { get; private set; }
    public NightBorneWaitAttackState waitAttackState { get; private set; }
    public NightBorneRecoverState recoverState { get; private set; }

    public NightBorneBattleState battleState { get; private set; }
    public NightBorneStunnedState stunnedState { get; private set; }
    public NightBorneDeadState deadState { get; private set; }

    #endregion
    public bool canRecover;

    protected override void Awake()
    {   

        base.Awake();
        idleState = new NightBorneIdleState(this, stateMachine, "Idle");
        moveState = new NightBorneMoveState(this, stateMachine, "Move");
        battleState = new NightBorneBattleState(this, stateMachine, "Move");
        waitAttackState = new NightBorneWaitAttackState(this, stateMachine, "Idle");
        attackState = new NightBorneAttackState(this, stateMachine, "Attack");
        stunnedState = new NightBorneStunnedState(this, stateMachine, "Stun");
        deadState = new NightBorneDeadState(this, stateMachine, "Die");
        recoverState = new NightBorneRecoverState(this, stateMachine, "Recover");
    }
    protected override void Start()
    {
        base.Start();
        canRecover = true;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

    }
    public override void Damage(int damage, Vector2 enermyPostion, AttackLevel attackLevel, AttackType _attackType)
    {
        if (isInvincible) { return; }

        StartCoroutine("SetisAttacked");
        stats.TakeAttack(damage, _attackType);

        if (attackLevel == AttackLevel.SuperLevelAttack && isdead == false)
        {
            if (canRecover)
            {
                stateMachine.ChangeState(stunnedState);
            }
            fx.CreateCriticalHitFX(enermyPostion, transform.position);
        }
        else
        {
            //fx.StartCoroutine("FlashFX");
            fx.CreateHitFX(transform.position);
        }
        StartCoroutine("HitKnockBack", enermyPostion);
        
        //可以回复 并且血量低于50%
        if (canRecover&&stats.currentHealth <= stats.Caculate_MaxHealthValue() / 2)
        {
            Debug.Log(114);
            stateMachine.ChangeState(recoverState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        
    }
}
