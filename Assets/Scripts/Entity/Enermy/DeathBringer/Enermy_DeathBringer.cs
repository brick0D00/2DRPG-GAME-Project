using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Enermy_DeathBringer : Enermy
{
    #region State
    public DeathBringerIdleState idleState {  get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerWaitAttackState waitAttackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }

    public DeathBringerCastState castState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }

    #endregion
    //每次攻击之后有几率传送，传送后有几率召唤

    [Header("传送信息")]
    [SerializeField] private List<Transform> teleportPostions;
    [SerializeField] private List<Transform> skeletonBornPostions;
    [Header("召唤攻击信息")]
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject spellAttackPrefab;
    public Vector2 spellOffset;
    public float castStateDuration;
    public float castCoolDown;
    public float castCoolDownTimer;
    public int defaultTeleportChance;
    public int teleportChance;
    public int teleportIncreaseRate;

    public int skeletonCallChance;//召唤骷髅的概率
    public int spellAttackChance;//欧内的手的概率
    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle");
        moveState = new DeathBringerMoveState(this, stateMachine, "Move");
        battleState = new DeathBringerBattleState(this, stateMachine, "Move");
        waitAttackState = new DeathBringerWaitAttackState(this, stateMachine, "Idle");
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack");
        deadState = new DeathBringerDeadState(this, stateMachine, "Die");
        castState = new DeathBringerCastState(this, stateMachine, "Cast");
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport");

        SetUpDefaultDirection(-1);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        castCoolDownTimer-=Time.deltaTime;
    }

    public void TeleportToRandomPostion()
    {
        int randomIndex=Random.Range(0, teleportPostions.Count);
        transform.position = teleportPostions[randomIndex].position;
        FaceToPlayer();
    }
    public void FaceToPlayer()
    {
        //TODO:在传送后依旧朝向玩家

        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x<transform.position.x && facingRight)
        {
            Flip();
        }
    }

    public override void Damage(int damage, Vector2 enermyPostion, AttackLevel attackLevel, AttackType _attackType)
    {
        if (isInvincible) { return; }

        StartCoroutine("SetisAttacked");
        stats.TakeAttack(damage, _attackType);

        if (attackLevel == AttackLevel.SuperLevelAttack && isdead == false)
        {
            fx.CreateCriticalHitFX(enermyPostion, transform.position);
            StartCoroutine("HitKnockBack", enermyPostion);
        }
        else
        {
            fx.StartCoroutine("FlashFX");
            fx.CreateHitFX(transform.position);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public bool CanBossTeleport()
    {   //TODO:尝试使用巫术，逐渐增加几率
        
        if (teleportChance < Random.Range(0, 100))
        {
            //不执行
            teleportChance += teleportIncreaseRate;
            return false;
        }
        else
        {
            //执行
            teleportChance=defaultTeleportChance;
            return true;
        }
    }

    public void ChangeAttackOrTeleport()
    {   
        //TODO:攻击之后攻击或者传送
        if (CanBossTeleport())
        {
            stateMachine.ChangeState(teleportState);
        }
        else
        {
            stateMachine.ChangeState(battleState);
        }
    }
    public void AfterTp_AttackOrCast()
    {   //TODO:TP之后进入召唤攻击或者物理攻击
        if (canCastAttack())
        {
            stateMachine.ChangeState(castState);
        }
        else
        {
            stateMachine.ChangeState(battleState);
        }
    }

    public void CastAttack()
    {   //TODO:判断传送之后是召唤骷髅或者用欧内的手,
        if(skeletonCallChance > Random.Range(0, 100))
        {
            CastAttack_Skeleton();
        }
        else
        {
            CastAttack_SpellAttack();
        }

        castCoolDownTimer = castCoolDown;
    }
    public bool canCastAttack()
    {
        return castCoolDownTimer < 0;
    }
    public void CastAttack_Skeleton()
    {
        Debug.Log(1145);
        //TODO:召唤骷髅
        for(int i = 0; i < skeletonBornPostions.Count; i++)
        {
            Instantiate(skeletonPrefab,skeletonBornPostions[i].position,Quaternion.identity);
        }
    }

    public void CastAttack_SpellAttack()
    {
        //TODO:开始欧内的手攻击
        StartCoroutine(IE_StartSpellAttackwithTime(5, 0.5f));
    }
    public IEnumerator IE_StartSpellAttackwithTime(int _times,float _interval)
    {
        for(int i = 0;i <= _times; i++)
        {
            CreateSpellAttack();
            yield return new WaitForSeconds(_interval);
        }
    }
    public void CreateSpellAttack()
    {
        GameObject newSpellAttack =Instantiate(spellAttackPrefab,new Vector2(player.position.x+spellOffset.x,player.position.y+spellOffset.y),Quaternion.identity);
       
    }
}
