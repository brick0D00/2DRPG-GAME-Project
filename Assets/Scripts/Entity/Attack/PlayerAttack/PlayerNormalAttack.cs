using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : PlayerAttack
{


    protected override void OnTriggerEnter2D(Collider2D enermy)
    {   
        base.OnTriggerEnter2D(enermy);
        enermy.GetComponent<Entity>()?.Damage(entity.stats.Calculate_ADdamage(), entity.transform.position, AttackLevel.NormalLevelAttack, entity.stats.currentAttackType);
    }
}
