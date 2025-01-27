using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{   
    #region State
    //���Զ�״̬����ʱ���ʵ��ǲ��ܸ���
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerGroundedState groundedState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerPreParryState preParryState { get; private set; }
    public PlayerParryState parryState { get; private set; }
    public PlayerParryAttackState parryAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }

    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    
    #endregion
    [Header("�˶���ֵ")]
    public float moveSpeed;
    public float jumpForce;
    public bool isWall;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("�����ֵ")]
    public float dashSpeed;
    public float dashSetTime;
    //���������ɫ���ҹ���ʱ�޷���������facingDirection����
    public float dashDirection { get; private set; }
    private float defaultDashSpeed;

    [Header("ʱ������")]
    public float comboResetTime;
    public float parryDuration;

    [Header("��������")]
    public bool isParrying;
    public bool wantToParryAttack;

    public SkillManager skill;
    public GameObject sword;

    public bool isbusy;

    public static bool playerCallToFreeze;/*�����ź�����Ҷ���*/
    

    protected override void Awake()
    {   //TODO:��Start֮ǰ��״̬����ʼ��
        base.Awake();
        stateMachine = new PlayerStateMachine();
        skill = SkillManager.instance;

        idleState  = new PlayerIdleState(this, stateMachine, "Idle");
        moveState  = new PlayerMoveState(this, stateMachine, "Move");
        jumpState  = new PlayerJumpState(this, stateMachine, "Jump");
        airState   = new PlayerAirState(this, stateMachine, "Jump");
        dashState  = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        preParryState = new PlayerPreParryState(this, stateMachine, "PreParry");
        parryState = new PlayerParryState(this, stateMachine, "Parry");
        parryAttackState = new PlayerParryAttackState(this, stateMachine, "ParryAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }


    protected override void Update()
    {
        base.Update();

        FreezeController();

        if (Time.timeScale == 0){return;}

        stateMachine.currentState.Update();

        CheckDashInput();
        CheckRewindTimeInput();
        CheckFlaskInput();
        isWall = isWallDetected();
    }
    public override void EntitySlowDown(float _slowPercentage, float _slowDuration)
    {   
        //TODO��������
        moveSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);
        anim.speed *= (1 - _slowPercentage);

        Invoke("ReturnToDefaultSpeed", _slowDuration);
    }
    protected override void ReturnToDefaultSpeed()
    {
        base.ReturnToDefaultSpeed();

        moveSpeed = defaultMoveSpeed; 
        jumpForce=defaultJumpForce;
        dashSpeed = defaultDashSpeed;

    }
    #region CheckInput
    public void CheckDashInput()
    {   //TODO:��Ⲣȷ�����κ�״̬�¶�����ת�������

        if (Input.GetKeyDown(KeyCode.LeftShift)&&SkillManager.instance.dash.TryToUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");
            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }
    public void CheckRewindTimeInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && SkillManager.instance.rewindTime.TryToUseSkill())
        {

        }
    }

    public void CheckFlaskInput()
    {
        //TODO:�ҩ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.TryToUseFlask();
        }
    }
    #endregion
    public override void Damage(int damage, Vector2 enermyPostion,AttackLevel attackLevel,AttackType _attackType)
    {
        if (isInvincible) { return; }

        StartCoroutine("SetisAttacked");
        fx.StartCoroutine("FlashFX");

        if (ParrySuccess()) { return; }

        stats.TakeAttack(damage,_attackType);
        StartCoroutine("HitKnockBack", enermyPostion);
        if (attackLevel == AttackLevel.SuperLevelAttack)
        {
            fx.CreateCriticalHitFX(enermyPostion, transform.position);
            fx.ScreenShake(fx.shakePower_BeCriticalHitted);
        }
        else
        {
            fx.CreateHitFX(transform.position);
        }
    }
    private bool ParrySuccess()
    {
        return stateMachine.currentState == preParryState || stateMachine.currentState == parryState;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchSword()
    {
        Destroy(sword);
        stateMachine.ChangeState(catchSwordState);
    }
    public virtual IEnumerator BusyFor(float time)
    {   
        //TODO:�������æµ�޷��ƶ�
        isbusy = true;

        yield return new WaitForSeconds(time);
        isbusy = false;
    }
    #region FreezeController
    public override void FreezeController()
    {
        base.FreezeController();
        if (playerCallToFreeze||allEntityCallToFreeze)
        {
            Freeze();
        }
        else if(!allEntityCallToFreeze&&!playerCallToFreeze)
        {
            Unfreeze();
        }
    }

    public override void Freeze()
    {
        base.Freeze();
    }

    public override void Unfreeze()
    {
        base.Unfreeze();
    }

    #endregion
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
