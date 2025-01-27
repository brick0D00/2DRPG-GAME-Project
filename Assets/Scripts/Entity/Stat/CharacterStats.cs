using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffType//Buff����������
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
    protected enum ElementStates/*��ʾ��ʱ������״̬*/
    {
        NormalState, /*��̬*/
        IgnitedState,/*ÿ��ȼ�տ�Ѫ*/
        ChilledState,/*����Ч��*/
        ShockedState/*��������*/
    }

    private EntityFX fx;
    private Entity entity;
    [Header("��Ҫ����")]
    public Stat strength;/*����������������*/
    public Stat intelligence;/*������Ӱ��ħ���˺�*/
    public Stat vitality;/*�����������������ֵ*/


    [Header("��������")]
    public Stat maxHealth;/*�������ֵ*/
    public Stat armor;/*����*/


    [Header("��������")]
    public Stat damage;

    [Header("ħ������")]
    public Stat AP;/*��ǿ*/

    public Stat antiMagic;/*ħ��*/
    [Header("Ԫ���쳣")]

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    //���ּ�����
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
    public AttackType currentAttackType;//��������������ֵ

    public System.Action OnHealthChanged;/*�����¼���ÿ��TakeDamageʱ����*/
    #region Calculate Value
    public virtual int Calculate_ADdamage()
    {   //TODO:��ϼ��㹥����������
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
        //TODO:�����������Ѫ���Լ���������
         TakeDamage(GetFinalDamage(_damage,_attackType));
       
    }
    protected virtual void Die()
    {   
        //TODO��ִ�������ͱ�װ��
        Debug.Log("����");

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
        //TODO:������Լ�������տ۳���Ѫ��
        if (_attackType != AttackType.Ad)
        {
            finalDamage=GetMagicDamage(_damage);
            SetElementAbnormality(_attackType);
        }
        else
        {
            finalDamage = _damage - armor.GetValue();
        }
        

        //���⻤�׹��߳��ֻ�Ѫ�����
        finalDamage = finalDamage < 0 ? 0 : finalDamage;

        return finalDamage;
    }
    public virtual int GetMagicDamage(int _damage)
    {   //TODO:�����ܵ���ħ���˺�
        int finalDamage=_damage-antiMagic.GetValue();
        return finalDamage;

    }
    protected virtual void TakeDamage(int _damamge)
    {   
        //�����Ŀ�Ѫ ����������ui�¼�
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

        //TODO:�����쳣״̬��ͬʱ�����쳣��ʱ��
        if (currentElementState != ElementStates.NormalState)
        {   
            //ֻ����һ���쳣״̬
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
    {   //TODO:�쳣���Խ���,�Լ����������쳣״̬
        //1.չʾ��Ч
        //2.��������

        if (ElementAbnormality_Timer < 0)
        {
            //��ʱ�������Ƴ��쳣״̬
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
            //ȼ�գ�����ÿ���Ѫ
            if (isIgnited) { return; }
            IgniteEffects();

        }
        else if (currentElementState == ElementStates.ChilledState)
        {
            //���᣺���ٷ�����
            if (isChilled)
            {
                return;
            }
            ChillEffects();

        }
        else if (currentElementState==ElementStates.ShockedState)
        {
            //ѣ�Σ���������
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
    {   //TODO: �������

        GameObject newThunder=Instantiate(thunderStrikePrefab,entity.transform);
        TakeAttack(shockedThunderDamage, AttackType.Ad);
    }
    public void BeSmallStrike()
    {   //TODO����С����

        GameObject newSmallThunder=Instantiate(smallThunderStrikePrefab, entity.transform);
        TakeAttack(smallThunderDamage, AttackType.Ad);
    }
    #endregion
    public virtual void HealMyselfWith(int _healValue)
    {
        //TODO:�ظ� _healValueѪ��
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
        //TODO:�������ʱ�����
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
