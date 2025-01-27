using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnermyStats : CharacterStats
{
    private ItemDrop myDropSystem;
    private Enermy enermy;


    [Header("�ȼ�")]
    public int level;
    [Range(0f, 1f)]
    [SerializeField] private float levelModifierpercentage;

    [Header("�����")]
    [SerializeField] private int baseMoneyDropAmount;
    
    protected override void Die()
    {   
        base.Die();
        
        enermy.Die();
        PlayerManager.instance.AddMoney(GetCalculatedMoney());
        myDropSystem.BurstItems();
        StartCoroutine(IE_SelfDestoryAfter(4f));

    }
    private IEnumerator IE_SelfDestoryAfter(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(this.gameObject);
    }
    public override void KillEntity()
    {
        base.KillEntity();
    }
    private void GrowUpByLevel()
    {
        GrowUp(maxHealth);
        GrowUp(damage);
        GrowUp(armor);
        GrowUp(antiMagic);
    }
    private void GrowUp(Stat _stat)
    {
        int growPower= Mathf.RoundToInt(_stat.GetValue()*level*levelModifierpercentage);
        _stat.AddModifier(growPower);
    }
    public override void Start()
    {
        GrowUpByLevel();
        base.Start();
        enermy=GetComponent<Enermy>();
        myDropSystem = GetComponent<ItemDrop>();

    }
    public override void Update()
    {
        base .Update();
    }
    private int GetCalculatedMoney()
    {   
        //TODO:��ϵȼ����һ�����Ľ��
        return Mathf.RoundToInt(baseMoneyDropAmount + (baseMoneyDropAmount * levelModifierpercentage * level));
    }
}
