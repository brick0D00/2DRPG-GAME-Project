using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private ThrowSword_Skill swordSkill;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Animator anim;
    private Player player;

    private int swordDamage;
    private bool isReturning;
    private bool canRotate=true;
    [SerializeField] private float returnSpeed;
    private float defaultReturnSpeed;

    #region sword Info
    //用来控制剑在目标之间弹跳打击
    [Header("Bounce Info")]
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> TargetsToBounce = new List<Transform>();
    private int TargetIndex;
    [SerializeField]private float bounceSpeed;

    [Header("Pierce Info")]
    private int amountOfPierce;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isSpinning;
    private bool callStop;
    private float hitTimer; //链锯剑的攻击时间
    private float hitcoolDown;//每次攻击的间隔
    private bool isFirstEnermy;
    private float spinDirection;

    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
        player = PlayerManager.instance.player;
        swordSkill = SkillManager.instance.throwSword;
        defaultReturnSpeed=returnSpeed;
    }
    #region SetUp Sword
    public void SetUpSword(Vector2 _dir, float _gravityScale, int _swordDamage)
    {
        //TODO:剑的初始化
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        swordDamage = _swordDamage;

        if (amountOfPierce <= 0)
        {
            anim.SetBool("Rotate", true);
        }
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
    }
    public void SetBouceSword(bool _isBouncing, int _amountOfBounce)
    {
        amountOfBounce = _amountOfBounce;
        isBouncing = _isBouncing;
    }
    public void SetPierceSowrd(int _amountOfPierce)
    {
        amountOfPierce = _amountOfPierce;
    }

    public void SetSpinSword(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitcoolDown = _hitCoolDown;
    }
    #endregion

    private void Update()
    {   
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        if (isReturning)
        {   
            Debug.Log(returnSpeed);
            SwordReturn();
        }
        BounceLogic();

        if (isSpinning)
        {   

            //TODO:类似链锯的攻击模式
            if(Vector2.Distance(transform.position,player.transform.position) > maxTravelDistance&&!callStop)
            {
                SpinStuck();
            }
            if (callStop)
            {
                spinTimer-=Time.deltaTime;
                hitTimer-=Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position,new Vector2(transform.position.x+spinDirection,transform.position.y),1.5f*Time.deltaTime);
                if(spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
                if(hitTimer < 0) 
                {
                    hitTimer = hitcoolDown;

                    Collider2D[] enermyAround = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D hit in enermyAround)
                    {
                        if (hit.GetComponent<Enermy>()!= null)
                        {
                            //UseEquipmentPower(hit.transform.position);
                            hit.GetComponent<Enermy>().EntitySlowDown(swordSkill.slowPercent, swordSkill.slowPercent);
                            hit.GetComponent<Enermy>().Damage(swordDamage, player.transform.position, AttackLevel.NormalLevelAttack, AttackType.Ad);
                        }
                        
                    }
                }

            }
        }

    }

    private void UseEquipmentPower(Vector3 _enermyPostion)
    {
        ItemData_Equipment wantedEquipement = Inventory.instance.GetWantedEquipment(EquipmentType.Jewelry);
        if(wantedEquipement != null)
        {
            wantedEquipement.ApplyItemEffectsForThrowSword(_enermyPostion);
        }
        wantedEquipement = Inventory.instance.GetWantedEquipment(EquipmentType.Weapon);
        if(wantedEquipement != null)
        {
            wantedEquipement.ApplyItemEffectsForThrowSword(_enermyPostion);
        }
    }
    

    private void SpinStuck()
    {
        if (isFirstEnermy)
        {
            return;
        }
        isFirstEnermy = true;
        callStop = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {   
        //TODO:弹跳剑的运行逻辑

        if (isBouncing && TargetsToBounce.Count > 0)
        {
            //正在弹跳，同时纯在目标

            transform.position = Vector2.MoveTowards(transform.position, TargetsToBounce[TargetIndex].position, bounceSpeed * Time.deltaTime);

            //向下一个目标移动
            if (Vector2.Distance(transform.position, TargetsToBounce[TargetIndex].position) < 0.1f)
            {   
                UseEquipmentPower(transform.position);
                TargetsToBounce[TargetIndex].GetComponent<Enermy>()?.Damage(swordDamage,player.transform.position,AttackLevel.NormalLevelAttack, AttackType.Ad);
                TargetIndex++;

                //限制弹跳次数,结束后返回
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (TargetIndex >= TargetsToBounce.Count)
                {

                    //超出数量后回到第一个
                    TargetIndex = 0;
                }
            }
        }
    }

    


    public void CallSwordReturn()
    {
        //TODO:召唤剑回归手边

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        returnSpeed=defaultReturnSpeed;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    public void SwordReturn()
    {
        //TODO:剑向人回归，距离够近就回到手上清空
        isFirstEnermy = false;
        returnSpeed += (0.1f*Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position,returnSpeed);
        if (Vector2.Distance(player.transform.position, transform.position) < 1)
        {
            player.CatchSword();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            //避免未碰撞的回归会插到人身上
            return;
        }
        if (collision.GetComponent<Enermy>() != null)
        {
            UseEquipmentPower(collision.transform.position);
            if (SkillManager.instance.throwSword.isSlowDownUnlocked)
            {
                collision.GetComponent<Enermy>().EntitySlowDown(swordSkill.slowPercent, swordSkill.slowDuration);
            }
            collision.GetComponent<Enermy>().Damage(swordDamage, player.transform.position, AttackLevel.NormalLevelAttack, AttackType.Ad);
        }
        
        
        GetTargetsForBouncing(collision);

        StuckInto(collision);

    }

    private void GetTargetsForBouncing(Collider2D collision)
    {
        if (collision.GetComponent<Enermy>() != null)
        {
            if (isBouncing && TargetsToBounce.Count <= 0)
            {
                //获取边上的敌人
                Collider2D[] enermyAround = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D hit in enermyAround)
                {
                    if (hit.GetComponent<Enermy>() != null)
                    {
                        TargetsToBounce.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {   

        //避免穿刺剑和链锯剑的时候卡住
        if (amountOfPierce > 0 && collision.GetComponent<Enermy>() != null)
        {   
            amountOfPierce--;
            return;
        }

        if (isSpinning)
        {
            SpinStuck();
            return;
        }


        //是否发生碰撞，发生碰撞就停止旋转,嵌入物体之中

        canRotate = false;
        cd.enabled = false;

        //刚体全部冻结
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        //还在弹跳的时候就不返回
        if (isBouncing&&TargetsToBounce.Count>0)
        {
            return;
        }
        //更改父类好让剑插在敌人身上
        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}
