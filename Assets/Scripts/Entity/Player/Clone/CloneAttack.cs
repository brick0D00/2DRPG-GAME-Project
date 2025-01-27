using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAttack : Attack
{
    protected override void Awake()
    {
        entity = PlayerManager.instance.player;
    }

    protected override void OnTriggerEnter2D(Collider2D enermy)
    {
        enermy.GetComponent<Entity>()?.Damage(entity.stats.Calculate_ADdamage(), transform.position, AttackLevel.NormalLevelAttack, AttackType.Ad);
    }
}
