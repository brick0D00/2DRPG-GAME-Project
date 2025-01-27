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


    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("攻击数值")]

    //public int damage;
    public bool isHit;
    private float isHitDuration = 0.01f;

    [Header("击退属性值")]
    [SerializeField] protected Vector2 knockBackForce;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    [Header("缓速")]
    public static bool allEntityCallToFreeze;
    public bool isFreezing;

    [Header("死亡")]
    public bool isdead;

    public System.Action OnFlipped;//用于订阅的事件

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
    {   //TODO:对地面进行镭射检测返回是否着地

        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    public virtual bool isWallDetected()
    {   //TODO:检测是否面部碰到墙体
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {   //TODO:画出测试线

        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    #endregion
    #region SetVelocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {   //TODO:设置移动速度,同时自动转向

        if (isKnocked || isFreezing) { return; }//被击退以及冻结无法移动 

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
            OnFlipped();//事件
        }

    }
    public virtual void FlipController(float _x)
    {   //TODO:对翻转进行封装控制

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
    {   //TODO:和弹反相关确定是否被击打

        isHit = true;
        yield return new WaitForSeconds(isHitDuration);
        isHit = false;
    }
    public virtual int GetHitDir(Vector2 enermyPostion)
    {
        return transform.position.x - enermyPostion.x > 0 ? 1 : -1;
    }
    protected virtual IEnumerator HitKnockBack(Vector2 enermyPostion)
    {   //TODO:使用携程在击退一段时间（knockBackDuration）后再将击退设置为false

        isKnocked = true;

        //求出击退方向
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
        //TODO:移除列表如果空了就取消无敌
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
        //TODO:让物体变得透明
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
        //TODO:设置基础朝向，因为有些图像初朝向不同
        facingDirection = _direction;
        if (facingDirection == -1)
        {
            facingRight = false;
        }
    }

}
