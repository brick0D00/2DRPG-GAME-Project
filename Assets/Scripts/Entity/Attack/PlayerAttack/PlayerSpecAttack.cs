using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecAttack : PlayerAttack
{ 
    
    protected override void OnTriggerEnter2D(Collider2D enermy)
    {
        base.OnTriggerEnter2D(enermy);
        enermy.GetComponent<Entity>()?.Damage(entity.stats.damage.GetValue(), entity.transform.position,AttackLevel.SuperLevelAttack, entity.stats.currentAttackType);
    }
}
