using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffType//Buff提升的种类
{
    Strength,
    Intelligence,
    Vitality,
    AntiMagic,
    Aromr,
    Damage,
    MaxHealth,
    Ap,
}
public enum AttackType
{
    Ad,
    fireAttack,
    iceAttack,
    lightingAttack
}
public class CharacterStats : MonoBehaviour
{   
    protected enum ElementStates/*表示此时所处的状态*/
    {
        NormalState, /*常态*/
        IgnitedState,/*每秒燃烧扣血*/
        ChilledState,/*易碎效果*/
        ShockedState/*降低力量*/
    }

    private EntityFX fx;
    private Entity entity;
    [Header("主要属性")]
    public Stat strength;/*力量：提升攻击力*/
    public Stat intelligence;/*智力：影响魔法伤害*/
    public Stat vitality;/*活力：提升最大生命值*/


    [Header("防御属性")]
    public Stat maxHealth;/*最大生命值*/
    public Stat armor;/*护甲*/


    [Header("进攻属性")]
    public Stat damage;

    [Header("魔法属性")]
    public Stat AP;/*法强*/

    public Stat antiMagic;/*魔抗*/
    [Header("元素异常")]

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    //各种减增益
    protected int igniteDamage=5;


    protected int chilledLoseArmor=-10;
    protected float chilledSlowPercentage = .3f;


    protected int shockedLoseStrength=-10;
    protected int shockedThunderDamage = 10;
    protected int smallThunderDamage = 5;
    [SerializeField]public GameObject thunderStrikePrefab;
    [SerializeField]public GameObject smallThunderStrikePrefab;



    public int currentHealth;

    public float ElementAbnormality_Timer;
    protected float ElementAbnormality_Duration=5f;
    protected ElementStates currentElementState;
    public AttackType currentAttackType;//给攻击附上属性值

    public System.Action OnHealthChanged;/*创建事件，每次TakeDamage时触发*/
    #region Calculate Value
    public virtual int Calculate_ADdamage()
    {   //TODO:结合计算攻击力和力量
        return damage.GetValue()+strength.GetValue();
    }
    public virtual int Calculate_Magicdamage(int _basedamage)
    {
        return _basedamage+AP.GetValue()+intelligence.GetValue();
    }
    public virtual int Caculate_MaxHealthValue()
    {
        return maxHealth.GetValue()+vitality.GetValue();
    }

    #endregion
    private void Awake()
    {
        fx = GetComponent<EntityFX>();
        entity=GetComponent<Entity>();
    }
    public virtual  void Start()
    {   
          
        currentHealth = Caculate_MaxHealthValue();
        currentElementState = ElementStates.NormalState;
        SetCurrentAttackType(AttackType.Ad);
    }
    public virtual void Update()
    {   
        
        ElementAbnormality_Timer -= Time.deltaTime;
        ElementStateController();
        if (currentHealth <= 0)
        {
            if (entity.isdead) { return; }
            Die();
        }
    }
    #region TakeAttack&Damage
    public virtual void TakeAttack(int _damage,AttackType _attackType)
    {
        //TODO:单纯处理减少血量以及触发死亡
         TakeDamage(GetFinalDamage(_damage,_attackType));
       
    }
    protected virtual void Die()
    {   
        //TODO：执行死亡和爆装备
        Debug.Log("死了");

    }
    public virtual void KillEntity()
    {
        if (!entity.isdead)
        {
            Die();
        }
    }
    protected virtual  int GetFinalDamage(int _damage, AttackType _attackType)
    {
        int finalDamage = 0;
        //TODO:结合属性计算出最终扣除的血量
        if (_attackType != AttackType.Ad)
        {
            finalDamage=GetMagicDamage(_damage);
            SetElementAbnormality(_attackType);
        }
        else
        {
            finalDamage = _damage - armor.GetValue();
        }
        

        //避免护甲过高出现回血的情况
        finalDamage = finalDamage < 0 ? 0 : finalDamage;

        return finalDamage;
    }
    public virtual int GetMagicDamage(int _damage)
    {   //TODO:计算受到的魔法伤害
        int finalDamage=_damage-antiMagic.GetValue();
        return finalDamage;

    }
    protected virtual void TakeDamage(int _damamge)
    {   
        //单纯的扣血 并触发更改ui事件
        currentHealth -= _damamge;
        OnHealthChanged();
        
    }
    protected virtual void TakeIgnitedDamage()
    {
        TakeDamage(igniteDamage-antiMagic.GetValue());
    }
    #endregion
    #region ElementsEffects
    protected virtual void SetElementAbnormality(AttackType _attackType)
    {

        //TODO:设置异常状态，同时重置异常计时器
        if (currentElementState != ElementStates.NormalState)
        {   
            //只能有一种异常状态
            return;
        }
        ElementAbnormality_Timer = ElementAbnormality_Duration;
        if (_attackType==AttackType.fireAttack) {
            currentElementState = ElementStates.IgnitedState;
        }
        else if (_attackType == AttackType.iceAttack)
        {
            currentElementState = ElementStates.ChilledState;
        }
        else if (_attackType == AttackType.lightingAttack)
        {
            currentElementState = ElementStates.ShockedState;
        }
    }
    protected virtual void ElementStateController()
    {   //TODO:异常属性结束,以及根据所处异常状态
        //1.展示特效
        //2.更改属性

        if (ElementAbnormality_Timer < 0)
        {
            //计时结束，移除异常状态
            if (currentElementState == ElementStates.ChilledState)
            {
                armor.RemoveModifier(chilledLoseArmor);
                isChilled = false;
            }
            if (currentElementState == ElementStates.ShockedState)
            {
                strength.RemoveModifier(shockedLoseStrength);
                isShocked = false;
            }
            if (currentElementState == ElementStates.IgnitedState)
            {
                CancelInvoke("TakeIgnitedDamage");
                isIgnited = false;
            }
            currentElementState = ElementStates.NormalState;

        }

        if (currentElementState==ElementStates.IgnitedState)
        {
            //燃烧：灼烧每秒扣血
            if (isIgnited) { return; }
            IgniteEffects();

        }
        else if (currentElementState == ElementStates.ChilledState)
        {
            //冻结：减少防御力
            if (isChilled)
            {
                return;
            }
            ChillEffects();

        }
        else if (currentElementState==ElementStates.ShockedState)
        {
            //眩晕：减少力量
            if (isShocked)
            {
                return;
            }
            ShockedEffects();
        }



    }
    private void ShockedEffects()
    {
        isShocked = true;
        Invoke("BeThunderStriked",ElementAbnormality_Duration);
        fx.ShockedFXForSeconds(ElementAbnormality_Duration);
        strength.AddModifier(shockedLoseStrength);
    }

    private void ChillEffects()
    {
        isChilled = true;
        entity.EntitySlowDown(chilledSlowPercentage, ElementAbnormality_Duration);
        fx.ChillFXForSeconds(ElementAbnormality_Duration);
        armor.AddModifier(chilledLoseArmor);
    }

    private void IgniteEffects()
    {
        isIgnited = true;

        InvokeRepeating("TakeIgnitedDamage", 0, 0.3f);
        fx.IgniteFXForSeconds(ElementAbnormality_Duration);
    }
    public bool isInElementStates()
    {
        return isChilled || isIgnited || isShocked;
    }

    #endregion
    #region BeThunderStrike
    public void BeThunderStriked()
    {   //TODO: 被大电劈

        GameObject newThunder=Instantiate(thunderStrikePrefab,entity.transform);
        TakeAttack(shockedThunderDamage, AttackType.Ad);
    }
    public void BeSmallStrike()
    {   //TODO：被小电劈

        GameObject newSmallThunder=Instantiate(smallThunderStrikePrefab, entity.transform);
        TakeAttack(smallThunderDamage, AttackType.Ad);
    }
    #endregion
    public virtual void HealMyselfWith(int _healValue)
    {
        //TODO:回复 _healValue血量
        if (currentHealth+_healValue> Caculate_MaxHealthValue())
        {
            currentHealth = Caculate_MaxHealthValue();
        }
        else
        {
            currentHealth += _healValue;
        }
        OnHealthChanged();
    }
    public virtual void SetCurrentAttackType(AttackType _newAttackType)
    {   
        //TODO:有增益的时候更改
        currentAttackType = _newAttackType;
    }
    public virtual IEnumerator IE_AddBuffFor(BuffType _buff, int _buffPower, float _buffDuration)
    {
        Stat buffedStat = GetWantedStat(_buff);
        
        buffedStat.AddModifier(_buffPower);

        yield return new WaitForSeconds(_buffDuration);

        buffedStat.RemoveModifier(_buffPower);
    }
    public void AddBuffFor(BuffType _buff,int _buffPower,float _buffDuration)
    {
        StartCoroutine(IE_AddBuffFor(_buff,_buffPower,_buffDuration));
    }

    public Stat GetWantedStat(BuffType _buff)
    {
        Stat wantedStat = null;
        switch (_buff)
        {
            case BuffType.Strength:
                wantedStat = strength;
                break;
            case BuffType.Vitality:
                wantedStat = vitality;
                break;
            case BuffType.Intelligence:
                wantedStat = intelligence;
                break;
            case BuffType.AntiMagic:
                wantedStat = antiMagic;
                break;
            case BuffType.Ap:
                wantedStat = AP;
                break;
            case BuffType.MaxHealth:
                wantedStat = maxHealth;
                break;
            case BuffType.Aromr:
                wantedStat = armor;
                break;
            default:
                wantedStat = damage;
                break;
        }

        return wantedStat;
    }
}
