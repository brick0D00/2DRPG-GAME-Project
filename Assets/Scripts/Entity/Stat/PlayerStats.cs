using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    protected override void Die()
    {   
        //TODO:��ɫ����,ͬʱ��װ��
        base.Die();
        PlayerManager.instance.player.Die();
        LoseMoney();
        GetComponent<PlayerItemDrop>().BurstItems();
    }
    public override void KillEntity()
    {
        base.KillEntity();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void TakeAttack(int _damage, AttackType _attackType)
    {
        ItemData_Equipment myAromr=Inventory.instance.GetWantedEquipment(EquipmentType.Aromr);
        if (myAromr != null)
        {
            myAromr.ApplyItemEffectsForDefend();
        }
        

        base.TakeAttack(_damage, _attackType);
    }
    private void LoseMoney()
    {   
        //TODO:���˵�Ǯ
        int moneyToLose = Mathf.RoundToInt(PlayerManager.instance.currency / 5);
        PlayerManager.instance.AddMoney(-moneyToLose);
    }
}
