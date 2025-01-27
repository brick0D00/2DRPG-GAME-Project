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
    //�������ƽ���Ŀ��֮�䵯�����
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
    private float hitTimer; //���⽣�Ĺ���ʱ��
    private float hitcoolDown;//ÿ�ι����ļ��
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
        //TODO:���ĳ�ʼ��
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

            //TODO:��������Ĺ���ģʽ
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
        //TODO:�������������߼�

        if (isBouncing && TargetsToBounce.Count > 0)
        {
            //���ڵ�����ͬʱ����Ŀ��

            transform.position = Vector2.MoveTowards(transform.position, TargetsToBounce[TargetIndex].position, bounceSpeed * Time.deltaTime);

            //����һ��Ŀ���ƶ�
            if (Vector2.Distance(transform.position, TargetsToBounce[TargetIndex].position) < 0.1f)
            {   
                UseEquipmentPower(transform.position);
                TargetsToBounce[TargetIndex].GetComponent<Enermy>()?.Damage(swordDamage,player.transform.position,AttackLevel.NormalLevelAttack, AttackType.Ad);
                TargetIndex++;

                //���Ƶ�������,�����󷵻�
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (TargetIndex >= TargetsToBounce.Count)
                {

                    //����������ص���һ��
                    TargetIndex = 0;
                }
            }
        }
    }

    


    public void CallSwordReturn()
    {
        //TODO:�ٻ����ع��ֱ�

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        returnSpeed=defaultReturnSpeed;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    public void SwordReturn()
    {
        //TODO:�����˻ع飬���빻���ͻص��������
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
            //����δ��ײ�Ļع��嵽������
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
                //��ȡ���ϵĵ���
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

        //���⴩�̽������⽣��ʱ��ס
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


        //�Ƿ�����ײ��������ײ��ֹͣ��ת,Ƕ������֮��

        canRotate = false;
        cd.enabled = false;

        //����ȫ������
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        //���ڵ�����ʱ��Ͳ�����
        if (isBouncing&&TargetsToBounce.Count>0)
        {
            return;
        }
        //���ĸ�����ý����ڵ�������
        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}
