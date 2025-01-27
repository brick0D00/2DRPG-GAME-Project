using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    protected override void OnTriggerEnter2D(Collider2D enermy)
    {
        //TODO:ִ��������Ч
        ItemData_Equipment myWeapon = Inventory.instance.GetWantedEquipment(EquipmentType.Weapon);
        if (myWeapon != null)
        {
            myWeapon.ApplyItemEffectsForAttack(enermy.transform.position);
        }
    }
}
