using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(EnermyStats))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(ItemDrop))]
public class Enermy : Entity
{
    #region State
    public EnermyStateMachine stateMachine { get; private set; }

    #endregion
    public Transform player { get; private set; }
    [Header("������")]
    [SerializeField] protected Transform preGroundCheck;
    [SerializeField] protected float preGroundCheckDistance;

    [Header("��������")]
    public float battleExitDistance;
    public float battleEnterDistance;

    [Header("��������")]
    public float idleSetTime;
    public float battleExitTime;

    [Header("�˶���ֵ")]
    public float idleMoveSpeed;
    public float battleMoveSpeed;
    protected float defaultIdleMoveSpeed;
    protected float defaultBattleMoveSpeed;

    [Header("��������")]
    public float attackDistance;
    public float attackIntervalTime;
    [HideInInspector] public float lastAttackTime;
    public bool canbeParry;
    public GameObject parryImage;



    [Header("��Ҽ��")]
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float playerDetectDistance;

    public static bool allEnermyCallToFreeze;
    public bool enermyCallToFreeze;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnermyStateMachine();

    }
    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player.transform;
        defaultIdleMoveSpeed = idleMoveSpeed;
        defaultBattleMoveSpeed = battleMoveSpeed;
        //stateMachine�ĳ�ʼ��������ʵ��
    }

    protected override void Update()
    {
        base.Update();
        FreezeController();
        stateMachine.currentState.Update();
    }
    public override void EntitySlowDown(float _slowPercentage, float _slowDuration)
    {
        Debug.Log(_slowDuration);
        idleMoveSpeed *= (1 - _slowPercentage);
        battleMoveSpeed *= (1 - _slowPercentage);
        anim.speed *= (1 - _slowPercentage);

        Invoke("ReturnToDefaultSpeed", _slowDuration);
    }
    protected override void ReturnToDefaultSpeed()
    {
        base.ReturnToDefaultSpeed();

        idleMoveSpeed = defaultIdleMoveSpeed;
        battleMoveSpeed = defaultBattleMoveSpeed;
    }
    #region Detect
    public virtual bool isPreGroundDetected()
    {   //TODO:�����Ƿ񼴽��ȿ�groundDetected���������жϵ���ɱ
        return Physics2D.Raycast(preGroundCheck.position, Vector2.down, preGroundCheckDistance, whatIsGround);
    }
    public virtual RaycastHit2D isPlayerDetected()
    {   //TODO:�����ж��Ƿ��⵽���
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDetectDistance, whatIsPlayer);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(preGroundCheck.position, new Vector3(preGroundCheck.position.x, preGroundCheck.position.y - preGroundCheckDistance));

        //��Ҽ��
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + playerDetectDistance * facingDirection, wallCheck.position.y));

        //������Χ��ʾ
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + attackDistance * facingDirection, wallCheck.position.y));
    }
    #endregion
    #region Parry
    public virtual bool CanbeParry()
    {
        if (canbeParry)
        {
            CloseParryWindow();
            return true;
        }
        return false;
    }
    public virtual void OpenParryWindow(float _duration)
    {
        canbeParry = true;
        parryImage.SetActive(true);

        StartCoroutine(IE_OpenParryWindow(_duration));
    }
    protected virtual IEnumerator IE_OpenParryWindow(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        CloseParryWindow();
    }
    public virtual void CloseParryWindow()
    {
        canbeParry = false;
        parryImage.SetActive(false);
    }
    #endregion
    #region FreezeController
    public override void FreezeController()
    {
        base.FreezeController();
        if (allEnermyCallToFreeze || allEntityCallToFreeze || enermyCallToFreeze)
        {
            Freeze();
        }
        else if (!allEnermyCallToFreeze && !allEntityCallToFreeze && !enermyCallToFreeze)
        {
            Unfreeze();
        }
    }

    public override void Freeze()
    {
        base.Freeze();
    }
    public void SetFreeze(bool _enermyCallToFreeze)
    {
        enermyCallToFreeze = _enermyCallToFreeze;
    }

    public override void Unfreeze()
    {
        base.Unfreeze();
    }

    #endregion
    public override void Damage(int damage, Vector2 enermyPostion, AttackLevel attackLevel, AttackType _attackType)
    {
        base.Damage(damage, enermyPostion, attackLevel, _attackType);
    }
    public override void Die()
    {
        base.Die();
    }
}
