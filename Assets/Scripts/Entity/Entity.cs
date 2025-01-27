using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackLevel
{
    NormalLevelAttack,
    SuperLevelAttack
}
public class Entity : MonoBehaviour
{
    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }

    public CharacterStats stats { get; private set; }
    #endregion

    public bool isInvincible;
    [SerializeField] private List<int> invincibleList;
    public bool facingRight = true;
    public int facingDirection = 1;


    [Header("��ײ���")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("������ֵ")]

    //public int damage;
    public bool isHit;
    private float isHitDuration = 0.01f;

    [Header("��������ֵ")]
    [SerializeField] protected Vector2 knockBackForce;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    [Header("����")]
    public static bool allEntityCallToFreeze;
    public bool isFreezing;

    [Header("����")]
    public bool isdead;

    public System.Action OnFlipped;//���ڶ��ĵ��¼�

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponentInChildren<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }
    #region Collision
    public virtual bool isGroundDetected()
    {   //TODO:�Ե�����������ⷵ���Ƿ��ŵ�

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    public virtual bool isWallDetected()
    {   //TODO:����Ƿ��沿����ǽ��
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {   //TODO:����������

        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    #endregion
    #region SetVelocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {   //TODO:�����ƶ��ٶ�,ͬʱ�Զ�ת��

        if (isKnocked || isFreezing) { return; }//�������Լ������޷��ƶ� 

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (OnFlipped != null)
        {
            OnFlipped();//�¼�
        }

    }
    public virtual void FlipController(float _x)
    {   //TODO:�Է�ת���з�װ����

        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
    #region Hit
    public virtual void Damage(int damage, Vector2 enermyPostion, AttackLevel attackLevel, AttackType attackType)
    {

        StartCoroutine("SetisAttacked");
        stats.TakeAttack(damage, attackType);
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack", enermyPostion);
        fx.CreateHitFX(transform.position);
    }
    public virtual IEnumerator SetisAttacked()
    {   //TODO:�͵������ȷ���Ƿ񱻻���

        isHit = true;
        yield return new WaitForSeconds(isHitDuration);
        isHit = false;
    }
    public virtual int GetHitDir(Vector2 enermyPostion)
    {
        return transform.position.x - enermyPostion.x > 0 ? 1 : -1;
    }
    protected virtual IEnumerator HitKnockBack(Vector2 enermyPostion)
    {   //TODO:ʹ��Я���ڻ���һ��ʱ�䣨knockBackDuration�����ٽ���������Ϊfalse

        isKnocked = true;

        //������˷���
        int hitDir = GetHitDir(enermyPostion);

        rb.velocity = new Vector2(knockBackForce.x * hitDir, knockBackForce.y);

        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
        rb.velocity = new Vector2(0, 0);

    }
    #endregion
    #region FreezeController
    public virtual void FreezeController()
    {

    }
    public virtual void Freeze()
    {
        anim.speed = 0;
        isFreezing = true;
        rb.velocity = new Vector2(0, 0);
    }
    public virtual void Unfreeze()
    {
        anim.speed = 1;
        isFreezing = false;
    }
    #endregion
    public virtual void AddInvincibleList(int _modifier)
    {
        invincibleList.Add(_modifier);
        isInvincible = true;

    }
    public virtual void RemoveInvincibleList(int _modifier)
    {
        //TODO:�Ƴ��б�������˾�ȡ���޵�
        invincibleList.Remove(_modifier);
        if (invincibleList.Count <= 0)
        {
            isInvincible = false;
        }
    }

    public virtual void EntitySlowDown(float _slowPercentage, float _slowDuration)
    {

    }
    protected virtual void ReturnToDefaultSpeed()
    {
        anim.speed = 1;
    }
    public virtual void MakeItTransparent(bool _transparent)
    {
        //TODO:��������͸��
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
    public virtual void Die()
    {
        isdead = true;
    }

    protected virtual void SetUpDefaultDirection(int _direction)
    {   
        //TODO:���û���������Ϊ��Щͼ�������ͬ
        facingDirection = _direction;
        if (facingDirection == -1)
        {
            facingRight = false;
        }
    }

}
