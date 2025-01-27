using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeSize
{
    Large, Medium,Small
}
public class Enermy_Slime : Enermy
{
    #region State
    public SlimeIdleState idleState{  get; private set; }
    public SlimeBattleState battleState{ get; private set; }
    public SlimeMoveState moveState{ get; private set; }
    public SlimeAttackState attackState{ get; private set; }
    public SlimeWaitAttackState waitAttackState{ get; private set; }
    public SlimeStunnedState stunnedState{ get; private set; }
    public SlimeStunnedOverState stunnedOverState{ get; private set; }
    public SlimeDeadState deadState{ get; private set; }

    public SlimeSize slimeSize;
    [SerializeField] private Vector2 XRange;
    [SerializeField] private Vector2 YRange;
    [SerializeField] private GameObject slimePrefab;


    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle");
        battleState = new SlimeBattleState(this, stateMachine, "Move");
        moveState = new SlimeMoveState(this, stateMachine, "Move");
        attackState = new SlimeAttackState(this, stateMachine, "Attack");
        waitAttackState = new SlimeWaitAttackState(this, stateMachine, "Idle");
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stun");
        stunnedOverState = new SlimeStunnedOverState(this, stateMachine, "StunnedOver");
        deadState = new SlimeDeadState(this, stateMachine, "Dead");
        SetUpDefaultDirection(-1);
    }
    #endregion
    protected override void Start()
    {
        base.Start();

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
            stateMachine.ChangeState(stunnedState);
            fx.CreateCriticalHitFX(enermyPostion, transform.position);
        }
        else
        {
            //fx.StartCoroutine("FlashFX");
            fx.CreateHitFX(transform.position);
        }
        StartCoroutine("HitKnockBack", enermyPostion);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void SpiltNewSlime() 
    {

        //TODO:分裂出新的史莱姆
        if (slimeSize == SlimeSize.Small) { return; }
        for (int i = 0;i<2; i++)
        {
            GameObject newSlime= Instantiate(slimePrefab,transform.position,Quaternion.identity);
            newSlime.GetComponent<Enermy_Slime>().SetUpNewSlime();
        }
    }

    public void SetUpNewSlime()
    {   
        //TODO：制作出一个弹出效果
        float tempX = Random.Range(XRange.x, XRange.y);
        float tempY = Random.Range(YRange.x, YRange.y);

        isKnocked = true;

        rb.velocity = new Vector2(tempX, tempY);

        StartCoroutine(IE_cancelKnocked(1.5f));
    }
    private IEnumerator IE_cancelKnocked(float _time)
    {
        yield return new WaitForSeconds(_time);
        isKnocked= false;
    }
}
