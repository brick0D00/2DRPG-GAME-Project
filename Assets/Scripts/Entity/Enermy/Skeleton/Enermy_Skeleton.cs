using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Skeleton : Enermy
{
    #region State

    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonWaitAttackState waitAttackState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonRecoverState recoverState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState   = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState   = new SkeletonMoveState(this, stateMachine, "Move");
        battleState = new SkeletonBattleState(this,stateMachine, "Move");
        waitAttackState = new SkeletonWaitAttackState(this, stateMachine, "Idle");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stun");
        recoverState = new SkeletonRecoverState(this,stateMachine, "Recover");
        deadState = new SkeletonDeadState(this, stateMachine, "Die");
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(recoverState);
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

        if (attackLevel == AttackLevel.SuperLevelAttack&&isdead==false)
        {   
            stateMachine.ChangeState(stunnedState);
            fx.CreateCriticalHitFX(enermyPostion,transform.position);
        }
        else
        {
            fx.StartCoroutine("FlashFX");
            fx.CreateHitFX(transform.position);
        }
        StartCoroutine("HitKnockBack", enermyPostion);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
